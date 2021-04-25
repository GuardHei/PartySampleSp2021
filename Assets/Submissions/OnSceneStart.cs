using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnSceneStart : MonoBehaviour
{
    public UnityEvent onStart;
    void Start()
    {
        onStart.Invoke();
    }
}
