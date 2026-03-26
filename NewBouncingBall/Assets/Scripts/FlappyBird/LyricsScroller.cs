using UnityEngine;
using TMPro;

public class SyncedLyricsScroller : MonoBehaviour
{
    [System.Serializable]
    public class LyricLine
    {
        public float time;     
        [TextArea(1, 3)] public string text;
    }

    [Header("Audio")]
    public AudioSource music;

    [Header("Lyrics (sorted by time)")]
    public LyricLine[] lines;

    [Header("Scroll")]
    public float speed = 220f;     
    public float startX = 900f;    
    public float endX = -900f;    

    TMP_Text tmp;
    RectTransform rect;
    int index = 0;

    void Awake()
    {
        tmp = GetComponent<TMP_Text>();
        rect = GetComponent<RectTransform>();
    }

    void Start()
    {
        if (music == null)
        {
            Debug.LogError("SyncedLyricsScroller: Assign an AudioSource (music).");
            enabled = false;
            return;
        }

        index = 0;
        SetLine(index);
    }

    void Update()
    {
        if (!music.isPlaying) return;

        float t = music.time;

        // Advance line when time passes next timestamp
        while (index + 1 < lines.Length && t >= lines[index + 1].time)
        {
            index++;
            SetLine(index);
        }

        rect.anchoredPosition += Vector2.left * speed * Time.deltaTime;

        if (rect.anchoredPosition.x < endX)
        {
            rect.anchoredPosition = new Vector2(endX, rect.anchoredPosition.y);
        }
    }

    void SetLine(int i)
    {
        if (lines == null || lines.Length == 0) return;

        tmp.text = lines[i].text;
        rect.anchoredPosition = new Vector2(startX, rect.anchoredPosition.y);
    }
}
