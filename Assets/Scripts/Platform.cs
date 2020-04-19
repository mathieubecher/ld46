using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public Vector3 velocity;
    private Vector3 _lastPos;

    private Player _player;

    public float force = 0;
    // Start is called before the first frame update
    void Start()
    {
        _player = FindObjectOfType<Player>();
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_player.transform.position.y - 0.4f < transform.position.y) gameObject.layer = 15;
        else gameObject.layer = 9;
        
        velocity = (transform.position - _lastPos)/Time.deltaTime;
        _lastPos = transform.position;
    }
}
