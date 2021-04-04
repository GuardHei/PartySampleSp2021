using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineTutorial : MonoBehaviour {

    public float delay = .5f;
    public float rotateSpeed = 90;
    public float time = 1f;
    
    private bool _hit;
    private Coroutine _startedCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space)) {
            if (_startedCoroutine != null) {
                StopCoroutine(_startedCoroutine);
            }
            
            _startedCoroutine = StartCoroutine(delayAndRotateRoutine());
        }
    }

    IEnumerator delayAndRotateRoutine() {
        yield return new WaitForSeconds(delay);
        float remainingTime = time;
        while (remainingTime > 0) {
            transform.Rotate(0, 0, rotateSpeed * Time.deltaTime);
            remainingTime -= Time.deltaTime;
            Debug.Log(Time.frameCount);
            yield return null;
        }

        _startedCoroutine = null;
    }
}