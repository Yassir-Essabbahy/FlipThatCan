using UnityEngine;

public class BumpFloat : MonoBehaviour
{
    public float amplitude = 0.08f;  // how high it moves
    public float speed = 3f;         // how fast

    Vector3 startPos;
    float t;

    void Start()
    {
        startPos = transform.localPosition;
    }

    void Update()
    {
        t += Time.deltaTime * speed;
        float y = Mathf.Sin(t) * amplitude;
        transform.localPosition = startPos + new Vector3(0, y, 0);
    }
}
