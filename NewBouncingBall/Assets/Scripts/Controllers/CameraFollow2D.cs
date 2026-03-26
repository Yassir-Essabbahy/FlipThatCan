using UnityEngine;

public class CameraFollow2D : MonoBehaviour
{
    public Transform target;

    [Header("Follow")]
    public Vector3 offset = new Vector3(0, 0, -10);
    public float followSmooth = 8f;

    [Header("Idle Bob")]
    public float bobAmplitude = 0.08f; // how strong
    public float bobSpeed = 6f;        // how fast

    float timer;

    void LateUpdate()
    {
        if (!target) return;

        timer += Time.deltaTime * bobSpeed;

        float bobY = Mathf.Sin(timer) * bobAmplitude;

        Vector3 desired = target.position + offset + new Vector3(0, bobY, 0);
        transform.position = Vector3.Lerp(transform.position, desired, followSmooth * Time.deltaTime);
    }
}
