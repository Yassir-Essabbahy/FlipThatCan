using UnityEngine;

public class ToggleFlip : MonoBehaviour
{
    public float flipSpeed = 12f;
    public float overshoot = 20f;

    float targetAngle = 0f;
    float currentAngle = 0f;

    void Update()
    {
        currentAngle = Mathf.LerpAngle(currentAngle, targetAngle, Time.deltaTime * flipSpeed);
        transform.rotation = Quaternion.Euler(0, 0, currentAngle);
    }

    public void DoFlip()
    {
        targetAngle += 180f;

        // little cartoon overshoot
        currentAngle = targetAngle - overshoot;
    }
}
