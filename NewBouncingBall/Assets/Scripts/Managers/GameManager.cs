using UnityEngine;
using UnityEngine.InputSystem;

public class TitleScreenManager : MonoBehaviour
{
    [Header("UI")]
    public GameObject titleCanvas;

    [Header("Input")]
    public InputActionReference startGameAction;

    [Header("Player")]
    public PlayerController2D playerController;
    public Rigidbody2D playerRb;

    [Header("Objects")]
    [Tooltip("These objects are OFF during title screen, then ON when gameplay starts.")]
    public GameObject[] objectsToEnableOnStart;

    [Header("Music")]
    public AudioSource musicSource;
    public AudioClip titleMusic;
    public AudioClip gameplayMusic;
    public bool loopMusic = true;

    bool gameStarted;

    void OnEnable()
    {
        if (startGameAction != null)
            startGameAction.action.Enable();
    }

    void OnDisable()
    {
        if (startGameAction != null)
            startGameAction.action.Disable();
    }

    void Start()
    {
        EnterTitleMode();
    }

    void Update()
    {
        if (gameStarted) return;

        if (startGameAction != null && startGameAction.action.WasPressedThisFrame())
        {
            StartGame();
        }
    }

    void EnterTitleMode()
    {
        gameStarted = false;

        if (titleCanvas != null)
            titleCanvas.SetActive(true);

        if (playerController != null)
            playerController.enabled = false;

        if (playerRb != null)
        {
            playerRb.linearVelocity = Vector2.zero;
            playerRb.angularVelocity = 0f;
            playerRb.simulated = false;
        }

        SetObjectsActive(objectsToEnableOnStart, false);
        PlayMusic(titleMusic);
    }

    public void StartGame()
    {
        gameStarted = true;

        if (titleCanvas != null)
            titleCanvas.SetActive(false);

        if (playerRb != null)
        {
            playerRb.simulated = true;
            playerRb.linearVelocity = Vector2.zero;
            playerRb.angularVelocity = 0f;
        }

        if (playerController != null)
            playerController.enabled = true;

        SetObjectsActive(objectsToEnableOnStart, true);
        PlayMusic(gameplayMusic);
    }

    void SetObjectsActive(GameObject[] objects, bool state)
    {
        if (objects == null) return;

        foreach (GameObject obj in objects)
        {
            if (obj != null)
                obj.SetActive(state);
        }
    }

    void PlayMusic(AudioClip clip)
    {
        if (musicSource == null || clip == null) return;

        if (musicSource.clip == clip && musicSource.isPlaying) return;

        musicSource.loop = loopMusic;
        musicSource.clip = clip;
        musicSource.Play();
    }
}