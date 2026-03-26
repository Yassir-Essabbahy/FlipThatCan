using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    public int score = 0;

    [Header("HUD")]
    public TMP_Text hudScoreText;

    void Awake()
    {
        Instance = this;
        score = 0;
        UpdateHUD();
    }

    public void AddPoint()
    {
        score++;
        UpdateHUD();
    }

    void UpdateHUD()
    {
        if (hudScoreText != null)
            hudScoreText.text = score.ToString();
    }
}
