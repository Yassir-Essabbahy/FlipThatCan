using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    [Header("Panel")]
    public GameObject pausePanel;

    [Header("Selectable Items")]
    public RectTransform[] pauseItems; // [0] Resume, [1] Main Menu

    [Header("Arrow")]
    public RectTransform arrow;
    public Vector2 arrowOffset = new Vector2(-60f, 0f);
    public float arrowMoveSpeed = 15f;

    [Header("Input")]
    public InputActionReference pauseAction;    // e.g. your arcade pause/start button
    public InputActionReference navigateAction;
    public InputActionReference confirmAction;

    [Header("Scene Names")]
    public string mainMenuScene = "MainMenu";

    bool isPaused = false;
    int selectedIndex = 0;
    float inputCooldown = 0f;
    const float INPUT_DELAY = 0.2f;

    // ─────────────────────────────────────────────────────────────────────

    void OnEnable()
    {
        if (pauseAction    != null) pauseAction.action.Enable();
        if (navigateAction != null) navigateAction.action.Enable();
        if (confirmAction  != null) confirmAction.action.Enable();
    }

    void OnDisable()
    {
        if (pauseAction    != null) pauseAction.action.Disable();
        if (navigateAction != null) navigateAction.action.Disable();
        if (confirmAction  != null) confirmAction.action.Disable();
    }

    void Start()
    {
        pausePanel.SetActive(false);
    }

    void Update()
    {
        if (pauseAction != null && pauseAction.action.WasPressedThisFrame())
        {
            if (isPaused) Resume();
            else          Pause();
        }

        if (!isPaused) return;

        HandleNavigation();
        MoveArrow();
    }

    // ── Pause / Resume ────────────────────────────────────────────────────

    void Pause()
    {
        isPaused      = true;
        selectedIndex = 0;
        pausePanel.SetActive(true);
        Time.timeScale = 0f;

        // Snap arrow to first item
        if (arrow != null && pauseItems.Length > 0)
            arrow.anchoredPosition = pauseItems[0].anchoredPosition + arrowOffset;
    }

    void Resume()
    {
        isPaused = false;
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
    }

    // ── Input ─────────────────────────────────────────────────────────────

    void HandleNavigation()
    {
        if (inputCooldown > 0f)
        {
            inputCooldown -= Time.unscaledDeltaTime; // unscaled because time is stopped
            return;
        }

        Vector2 nav = navigateAction != null
            ? navigateAction.action.ReadValue<Vector2>()
            : Vector2.zero;

        if (nav.y < -0.5f)
        {
            selectedIndex = (selectedIndex + 1) % pauseItems.Length;
            inputCooldown = INPUT_DELAY;
        }
        else if (nav.y > 0.5f)
        {
            selectedIndex = (selectedIndex - 1 + pauseItems.Length) % pauseItems.Length;
            inputCooldown = INPUT_DELAY;
        }

        if (confirmAction != null && confirmAction.action.WasPressedThisFrame())
            Confirm();
    }

    void Confirm()
    {
        switch (selectedIndex)
        {
            case 0: Resume();     break;
            case 1: GoMainMenu(); break;
        }
    }

    // ── Arrow ─────────────────────────────────────────────────────────────

    void MoveArrow()
    {
        if (arrow == null || pauseItems.Length == 0) return;

        selectedIndex = Mathf.Clamp(selectedIndex, 0, pauseItems.Length - 1);

        Vector2 target = pauseItems[selectedIndex].anchoredPosition + arrowOffset;

        // Use unscaledDeltaTime so arrow still animates while game is paused
        arrow.anchoredPosition = Vector2.Lerp(
            arrow.anchoredPosition, target, arrowMoveSpeed * Time.unscaledDeltaTime
        );
    }

    // ── Actions ───────────────────────────────────────────────────────────

    void GoMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuScene);
    }
}