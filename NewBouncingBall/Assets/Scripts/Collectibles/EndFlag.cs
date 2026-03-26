using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class EndFlag : MonoBehaviour
{
    public CanvasGroup endPanel;
    public TextMeshProUGUI endText;

    public float fadeSpeed = 1.2f;

    bool triggered;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (triggered) return;

        if (col.CompareTag("Player"))
        {
            triggered = true;

            Rigidbody2D rb = col.GetComponent<Rigidbody2D>();
            if (rb) rb.linearVelocity = Vector2.zero;

            PlayerController2D pc = col.GetComponent<PlayerController2D>();
            if (pc) pc.enabled = false;

            StartCoroutine(EndSequence());
        }
    }

    IEnumerator EndSequence()
    {
        // Fade screen
        while (endPanel.alpha < 1f)
        {
            endPanel.alpha += Time.deltaTime * fadeSpeed;
            yield return null;
        }

        yield return new WaitForSeconds(1f);

        // Fade text
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime;
            endText.alpha = t;
            yield return null;
        }

        yield return new WaitForSeconds(3f);

        // Back to menu (or reload)
        SceneManager.LoadScene(0);
    }
}
