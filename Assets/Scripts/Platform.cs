using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    [HideInInspector] public Vector3 velocity;
    private Vector3 _lastPos;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        velocity = (transform.position - _lastPos)/Time.deltaTime;
        _lastPos = transform.position;
    }
}
