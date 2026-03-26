using UnityEngine;
using UnityEngine.SceneManagement;

public class FlappyPlayer : MonoBehaviour
{
    public float flapForce = 6.5f;
    public bool isAlive = true;

    [Header("Sound")]
    public AudioSource audioSource;
    public AudioClip canCrushClip;
    [Range(0f, 1f)] public float crushVolume = 1f;

    Rigidbody2D rb;
    ToggleFlip flipper;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        flipper = GetComponent<ToggleFlip>();

        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (!isAlive) return;

        bool flap = Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0);

        if (flap)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
            rb.AddForce(Vector2.up * flapForce, ForceMode2D.Impulse);

            if (flipper != null) flipper.DoFlip();

            // play crush sound every click
            if (audioSource != null && canCrushClip != null)
                audioSource.PlayOneShot(canCrushClip, crushVolume);
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (!isAlive) return;

        if (col.collider.CompareTag("Pipe") || col.collider.CompareTag("Ground"))
            Die();
    }

   void Die()
{
    isAlive = false;

    int currentScore = 0;
    if (ScoreManager.Instance != null) currentScore = ScoreManager.Instance.score;

    if (GameManager.Instance != null)
        GameManager.Instance.GameOver(currentScore);
}

}
