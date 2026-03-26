using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu1 : MonoBehaviour
{
    public AudioSource clickSound;

    public void PlayGame()
    {
        if (clickSound != null)
            clickSound.Play();

        Invoke(nameof(LoadNext), 0.15f);
    }

    void LoadNext()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
