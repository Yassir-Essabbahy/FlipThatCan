using UnityEngine;

public class PipeMove : MonoBehaviour
{
    public float speed = 2.5f;
    public float destroyX = -12f;

    void Update()
    {
        transform.position += Vector3.left * (speed * Time.deltaTime);

        if (transform.position.x < destroyX)
            Destroy(gameObject);
    }
}
