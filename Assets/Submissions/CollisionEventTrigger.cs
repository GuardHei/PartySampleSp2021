using UnityEngine;
using UnityEngine.Events;

public class CollisionEventTrigger : MonoBehaviour
{

    public UnityEvent onEnter;
    public UnityEvent onExit;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && enabled)
        {
            onEnter.Invoke();
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && enabled)
        {
            onExit.Invoke();
        }
    }
}