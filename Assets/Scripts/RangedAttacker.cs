using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAttacker : MonoBehaviour
{
    public float lifespan; // how long until this object is deleted.
    public bool alignsWithTargetHorizontally = true; // if this attacker will align horizontally with its target
    public bool alignsWithTargetVertically = false; // if this attacker will align vertically with its target
    public bool facesTarget = false; // if this attacker will be made to face its target
    public bool rotatesInstantly = true; // if this attacker will instantly face its target (if enabled) 

    public float RotationSpeed // when rotation is finished is about when the object is destroyed
    {
        get
        {
            if (rotatesInstantly)
            {
                return 3000;
            }
            else
            {
                return 5 / lifespan;
            }
        }
    }

    private float _age;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Ranged Attacker Created.");
        _age = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (_age > lifespan)
        {
            Debug.Log("Ranged Attacker Destroyed.");
            Destroy(this.gameObject);
        }

        _age += Time.deltaTime;
    }

    /**
     * Works for sprites that are top down (standing up straight, vertical, etc.).
     * The speed is used for rotation. If we wanted things to rotate, this function would have to be moved
     * to an update function. If we care not about rotation, and just want to instantly face the target,
     * use a big constant for speed.
     * For horizontal sprites try:
     * float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
     * @source https://answers.unity.com/questions/650460/rotating-a-2d-sprite-to-face-a-target-on-a-single.html
     */
    public void faceTarget(GameObject target, float rotationSpeed = 3000)
    {
        Transform targetTransform = target.transform;
        Vector3 vectorToTarget = targetTransform.position - transform.position;
        float angle = (Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg) + 180; // - 90;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * rotationSpeed);
    }
}