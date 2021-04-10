using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAttacker : MonoBehaviour
{
    public float lifespan; // how long until this object is deleted.
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
}