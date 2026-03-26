using UnityEngine;

public class VerticalMover : MonoBehaviour
{
    public float distance = 2f;   // how high it goes
    public float speed = 2f;      // how fast

    Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        float y = Mathf.Sin(Time.time * speed) * distance;
        transform.position = startPos + new Vector3(0f, y, 0f);
    }
}
