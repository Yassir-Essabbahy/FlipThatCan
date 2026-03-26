using UnityEngine;

public class TriggerActivator : MonoBehaviour
{
    public GameObject targetObject;   // The object to activate
    public bool destroyTrigger = true; // Remove trigger after use

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (targetObject != null)
                targetObject.SetActive(true);

            if (destroyTrigger)
                Destroy(gameObject);
        }
    }
}
