using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 5;
    public float jumpForce = 10;
    private float _moveX = 0;
    
    private bool _onPlatform;
    private Platform _platform;
    [SerializeField] private ColliderTest _testCollide;
    
    private Rigidbody2D _rigidbody;
    private SpriteRenderer _renderer;
    private bool jump;
    private float _velocityX = 0;

    private Vector2 _normalContact;

    private bool _onWall;

    private bool _onFixable;
    private bool _fixing;
    private Fixable _fixable;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _renderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        bool move = false;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // INPUT VERTICALE
            Jump();
            move = true;
        }
        
        
        float factorX = 0.02f;
        if (_onPlatform)
        {
            factorX = 0.3f;
           

        }

        // INPUT HORIZONTALE
        if (Input.GetKey(KeyCode.Q))
        {
            _moveX -= factorX;
            move = true;
        }

        if (Input.GetKey(KeyCode.D))
        {
            _moveX += factorX;
            move = true;
        }
        if (!Input.GetKey(KeyCode.Q) && !Input.GetKey(KeyCode.D) && _onPlatform) _moveX = 0;
        // Update input velocity
        _moveX = Mathf.Min(1, Mathf.Max(-1, _moveX));
        if (Input.GetKey(KeyCode.Q)) transform.localScale = new Vector3(-1, transform.localScale.y);
        else if(Input.GetKey(KeyCode.D)) transform.localScale = new Vector3(1, transform.localScale.y);
        
        // calculate velocity
        _rigidbody.velocity = new Vector2(_moveX * speed + _velocityX, (_onWall)?-2:_rigidbody.velocity.y);

        if (_rigidbody.velocity.y < 0) jump = false; 
        if (jump || (gameObject.layer == 11 && _testCollide.platform.Count > 0)) gameObject.layer = 11;
        else gameObject.layer = 10;
        
        // Fixing
        if (!move && _onFixable)
        {
            Debug.Log("Fix");
            if (Input.GetKeyDown(KeyCode.E) && !_fixable.isFix) _fixing = true;
            if(_fixing) _fixing = _fixable.Fix();
        }
        else _fixing = false;
    }

    void FixedUpdate()
    {
        transform.rotation = Quaternion.identity;
    }
    
    void Jump()
    {
        if(_onPlatform){
            _rigidbody.AddForce(new Vector2(0, jumpForce + _platform.velocity.y),ForceMode2D.Impulse);
            _velocityX = _platform.velocity.x;
            
            jump = true;
            _onPlatform = false;
            transform.parent = null;
        }
        else if (_onWall)
        {
            Vector2 force = new Vector2((_normalContact.x < 0)? -1 : 1, 1).normalized;
            
            _rigidbody.AddForce(force * jumpForce,ForceMode2D.Impulse);
            _velocityX = force.x * jumpForce/5;
            _moveX = 0;
            _onWall = false;
            //jump = true;
        }
    }
    
    
    
    // GESTIONS COLLISION PLATEFORME
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == 9 || other.gameObject.layer == 12)
        {
            // Test Collision
            foreach (ContactPoint2D point in other.contacts)
            {
                //Debug.DrawLine(point.point, point.point + point.normal*5, Color.blue, 5);
                if (Vector2.Dot(Vector2.up, point.normal) < 0.5f) return;
            }
            
            _velocityX = 0;
            transform.parent = other.gameObject.transform.parent;
            
            if(_platform != null) _platform.gameObject.layer = 9;
            
            _onPlatform = true;
            _platform = other.gameObject.GetComponent<Platform>();
            _platform.gameObject.layer = 12;
        }
        else if (other.gameObject.layer == 8 && !_onPlatform)
        {
            _normalContact = other.contacts[0].normal;
            Debug.DrawLine(other.contacts[0].point, other.contacts[0].point + _normalContact*5, Color.blue, 5);
            _onWall = true;

        }
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if(other.gameObject == _platform.gameObject){
            _onPlatform = false;
            transform.parent = null;
        }
        else if (other.gameObject.layer == 8)
        {
            _onWall = false;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 13)
        {
            _fixable = other.gameObject.GetComponent<Fixable>();
            _onFixable = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == 13)
        {
            _onFixable = false;
        }
    }
}
