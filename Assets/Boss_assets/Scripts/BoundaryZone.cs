using UnityEngine;

public class BoundaryZone : MonoBehaviour
{
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.position = transform.position; 
        }
    }
}
