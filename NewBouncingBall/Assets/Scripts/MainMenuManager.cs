using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class MainMenuManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject mainMenuPanel;
    public GameObject levelSelectPanel;

    [Header("Main Menu — Selectable Items")]
    public RectTransform[] mainMenuItems;   // assign in order: [0] Play, [1] Exit

    [Header("Level Select — Selectable Items")]
    public RectTransform[] levelItems;      // assign in order: [0] Level1, [1] Level2, [2] Back

    [Header("Arrow")]
    public RectTransform arrow;             // your arrow UI Image/Sprite
    public Vector2 arrowOffset = new Vector2(-60f, 0f); // position left of each item
    public float arrowMoveSpeed = 15f;      // smooth slide speed

    [Header("Input")]
    public InputActionReference navigateAction; // e.g. UI/Navigate (Vector2)
    public InputActionReference confirmAction;  // e.g. UI/Submit or your arcade button

    enum Menu { Main, LevelSelect }
    Menu currentMenu = Menu.Main;

    int selectedIndex = 0;
    float inputCooldown = 0f;
    const float INPUT_DELAY = 0.2f;

    RectTransform[] CurrentItems => currentMenu == Menu.Main ? mainMenuItems : levelItems;

    // ─────────────────────────────────────────────────────────────────────

    void OnEnable()
    {
        if (navigateAction != null) navigateAction.action.Enable();
        if (confirmAction  != null) confirmAction.action.Enable();
    }

    void OnDisable()
    {
        if (navigateAction != null) navigateAction.action.Disable();
        if (confirmAction  != null) confirmAction.action.Disable();
    }

    void Start()
    {
        ShowMainMenu();
    }

    void Update()
    {
        HandleNavigation();
        MoveArrow();
    }

    // ── Input ─────────────────────────────────────────────────────────────

    void HandleNavigation()
    {
        if (inputCooldown > 0f)
        {
            inputCooldown -= Time.deltaTime;
            return;
        }

        Vector2 nav = navigateAction != null
            ? navigateAction.action.ReadValue<Vector2>()
            : Vector2.zero;

        if (nav.y < -0.5f)       // down
        {
            selectedIndex = (selectedIndex + 1) % CurrentItems.Length;
            inputCooldown = INPUT_DELAY;
        }
        else if (nav.y > 0.5f)  // up
        {
            selectedIndex = (selectedIndex - 1 + CurrentItems.Length) % CurrentItems.Length;
            inputCooldown = INPUT_DELAY;
        }

        if (confirmAction != null && confirmAction.action.WasPressedThisFrame())
            Confirm();
    }

    void Confirm()
    {
        if (currentMenu == Menu.Main)
        {
            switch (selectedIndex)
            {
                case 0: OnPlayClicked(); break;
                case 1: OnExitClicked(); break;
            }
        }
        else
        {
            switch (selectedIndex)
            {
                case 0: OnLevel1Clicked(); break;
                case 1: OnLevel2Clicked(); break;
                case 2: OnBackClicked();   break;
            }
        }
    }

    // ── Arrow — smoothly slides to the selected item ──────────────────────

    void MoveArrow()
    {
        if (arrow == null) return;

        RectTransform[] items = CurrentItems;
        if (items == null || items.Length == 0) return;

        selectedIndex = Mathf.Clamp(selectedIndex, 0, items.Length - 1);

        Vector2 target = items[selectedIndex].anchoredPosition + arrowOffset;
        arrow.anchoredPosition = Vector2.Lerp(
            arrow.anchoredPosition, target, arrowMoveSpeed * Time.deltaTime
        );
    }

    // ── Panel Switching ───────────────────────────────────────────────────

    void ShowMainMenu()
    {
        currentMenu   = Menu.Main;
        selectedIndex = 0;
        mainMenuPanel.SetActive(true);
        levelSelectPanel.SetActive(false);

        // Snap arrow to first item immediately
        if (arrow != null && mainMenuItems.Length > 0)
            arrow.anchoredPosition = mainMenuItems[0].anchoredPosition + arrowOffset;
    }

    void ShowLevelSelect()
    {
        currentMenu   = Menu.LevelSelect;
        selectedIndex = 0;
        mainMenuPanel.SetActive(false);
        levelSelectPanel.SetActive(true);

        // Snap arrow to first item immediately
        if (arrow != null && levelItems.Length > 0)
            arrow.anchoredPosition = levelItems[0].anchoredPosition + arrowOffset;
    }

    // ── Button Actions ────────────────────────────────────────────────────

    public void OnPlayClicked()    => ShowLevelSelect();
    public void OnBackClicked()    => ShowMainMenu();
    public void OnLevel1Clicked()  => SceneManager.LoadScene("FlipThatCan");
    public void OnLevel2Clicked()  => SceneManager.LoadScene("FlipThatCan2");

    public void OnExitClicked()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}