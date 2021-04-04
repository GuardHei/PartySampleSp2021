using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipSensor : MonoBehaviour
{
    public GameObject sensor;
    private Transform point1;
    private Transform point2;
    private Vector3 axis;
    private bool point1hit;
    private bool point2hit;
    private Vector3 direction;
    private Vector2 direction2d;
    private Vector2 perp;
    // Start is called before the first frame update
    void Start()
    {
        point1hit = false;
        point2hit = false;
        point1 = GetComponent<StupidAIMovement>().pathStart;
        point2 = GetComponent<StupidAIMovement>().pathEnd;
        direction = point1.position - point2.position;
        direction2d = direction;
        perp = Vector2.Perpendicular(direction2d);
        axis = perp;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position == point1.position && point1hit == false)
        {
            // flip the cone
            point1hit = true;
            point2hit = false;
            sensor.GetComponent<Transform>().RotateAround(transform.position, axis, 180);
        }
        if (transform.position == point2.position && point2hit == false)
        {
            // flip the cone
            point2hit = true;
            point1hit = false;
            sensor.GetComponent<Transform>().RotateAround(transform.position, axis, 180);
        }

    }
}
