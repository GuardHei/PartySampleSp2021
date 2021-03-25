using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnfocusedTurret : MonoBehaviour
{
    
    // * TEMPLATE *

    // ------------------------------------- REFERENCES ------------------------------------
    
    public GameObject _bulletPrefab;
    
    // ------------------------------------- PROPERTIES ------------------------------------
    
    public float _fireCooldown = 0.4f;
    public float _turnDuration = 1.0f;
    public float _turnDegrees = 0.0f;
    
    private float _timer = 0.0f;
    private Quaternion _counterClockEndpoint;
    private Quaternion _clockEndpoint;
    
    // -------------------------------------- METHODS --------------------------------------

    public void _FireWeapon(Quaternion rotation)
    {
        Instantiate(_bulletPrefab);
        _bulletPrefab.transform.position = transform.position;
        _bulletPrefab.transform.rotation = rotation;
    }
    
    // ----------------------------------- INITIALIZATION ----------------------------------

    protected void Start()
    {
        Vector3 angles = transform.rotation.eulerAngles;
        _counterClockEndpoint = transform.rotation;
        _clockEndpoint = Quaternion.AngleAxis(angles.z - _turnDegrees, Vector3.forward);
    }

    // --------------------------------------- UPDATE --------------------------------------
    
    protected void Update()
    {
        if (_timer <= 0)
        {
            _FireWeapon(transform.rotation);
            _timer = _fireCooldown;
        }
        else
        {
            _timer -= Time.deltaTime;
        }
        transform.rotation = Quaternion.Lerp(
            _counterClockEndpoint, _clockEndpoint, (Time.time % _turnDuration) / _turnDuration);
    }
    
    // -------------------------------------- CASTING --------------------------------------
}

// * TEMPLATE *

// ------------------------------------- REFERENCES ------------------------------------
// ------------------------------------- PROPERTIES ------------------------------------
// -------------------------------------- METHODS --------------------------------------
// ----------------------------------- INITIALIZATION ----------------------------------
// --------------------------------------- UPDATE --------------------------------------
// -------------------------------------- CASTING --------------------------------------