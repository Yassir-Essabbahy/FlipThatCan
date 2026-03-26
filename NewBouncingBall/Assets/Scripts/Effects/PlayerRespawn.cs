using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    Vector3 startPos;

    void Start()
    {
        startPos = transform.position; 
    }

    public void Die()
    {
        transform.position = startPos;
        GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
    }
}
