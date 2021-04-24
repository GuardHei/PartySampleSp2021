using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DelayedAction : MonoBehaviour
{
    public UnityEvent delayed;
    
    public void DelayBy(float delayTime)
    {
        StartCoroutine(DelayCoroutine(delayTime));
    }

    private IEnumerator DelayCoroutine(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        delayed.Invoke();
    }
}
