using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionalFollow : MonoBehaviour
{

    [SerializeField]
    private Transform _transformToFollow;

    // Start is called before the first frame update
    void Start()
    {
        Vector2 at = _transformToFollow.position;
        transform.position = new Vector3(at.x, at.y, transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 at = _transformToFollow.position;
        transform.position = new Vector3(at.x, at.y, transform.position.z);
    }
}
