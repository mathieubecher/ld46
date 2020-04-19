using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;


public class Player : MonoBehaviour
{
    
    public float speed = 5;
    public float jumpForce = 10;
    private float _moveX = 0;
    public float _coyoteeTime = 0.1f;
    private float _coyotee = 0;
    
    private bool _onPlatform;
    private Platform _platform;
    [SerializeField] private ColliderTest _testCollide;
    
    private Rigidbody2D _rigidbody;
    private SpriteRenderer _renderer;
    private bool jump;
    private Vector2 _velocity;

    private Vector2 _normalContact;

    private bool _onWall;
    
    
    private bool _onLadder;
    private GameObject _ladder;
    private bool _ladding;
    
    private bool _onFixable;
    private bool _fixing;
    private Fixable _fixable;

    private float _stun;
    private float _gravityScale;

    public float GRAVITY = 50;
    // Start is called before the first frame update
    void Start()
    {
        _gravityScale = GRAVITY;
        _rigidbody = GetComponent<Rigidbody2D>();
        _renderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (_velocity.y >= 0) Debug.Log("ça marche" + _velocity.y );
        */
        //if(_velocity.y < 0) Debug.Log("ça marche plus");
        
        // Stun
        if (_stun > 0)
        {
            _rigidbody.velocity = Vector2.zero;
            _stun -= Time.deltaTime;
            return;
        }

        
        if (!_onPlatform && !_onLadder)
        {
            _coyotee += Time.deltaTime;
        }
        bool move = false;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // INPUT VERTICALE
            Jump();
            move = true;
        }
        
        
        float factorX = 0.03f;
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
        if (!Input.GetKey(KeyCode.Q) && !Input.GetKey(KeyCode.D) && (_onPlatform || _ladding)) _moveX = 0;
        // Update input velocity
        
        _moveX = Mathf.Min(1, Mathf.Max(-1, _moveX));
        if (Input.GetKey(KeyCode.Q)) transform.localScale = new Vector3(-1, transform.localScale.y);
        else if(Input.GetKey(KeyCode.D)) transform.localScale = new Vector3(1, transform.localScale.y);

       
        if (!_onPlatform && !_ladding && (!_onWall || _velocity.y > 0)) _velocity.y -= _gravityScale * Time.deltaTime;
        else if (_onWall) _velocity.y = -2;
        else _velocity.y = 0;
        // calculate velocity
        if(!_ladding)_rigidbody.velocity = new Vector2(_moveX * speed + _velocity.x, _velocity.y);
        
        
        if (_velocity.y <= 0) jump = false; 
        if (jump || _ladding || (gameObject.layer == 11 && _testCollide.platform.Count > 0) || Input.GetKey(KeyCode.S)) gameObject.layer = 11;
        else gameObject.layer = 10;
        
        // Fixing
        if (!move && _onFixable && (_onPlatform || _ladding))
        {
            _rigidbody.velocity = Vector3.zero;
            _velocity = Vector2.zero;
            if (Input.GetKeyDown(KeyCode.E) && !_fixable.isFix) _fixing = true;
            if(_fixing) _fixing = _fixable.Fix();
           
        }
        else _fixing = false;

    }
    
    void FixedUpdate()
    {
        
        transform.rotation = Quaternion.identity;
        float laddingHeight = 0;
        if (Input.GetKey(KeyCode.Z) && _onLadder)
        {
            jump = true;
            laddingHeight += 1;
            if (!_ladding)
            {
                _moveX = 0;
                transform.parent = _ladder.transform.parent;
                
            }
            _ladding = true;
        }

        if (Input.GetKey(KeyCode.S) && _onLadder)
        {
            laddingHeight -= 1;
            if (!_ladding)
            {
                _moveX = 0;
                transform.parent = _ladder.transform.parent;
            }
            _ladding = true;
        }
        if(_ladding)
        {
            _gravityScale = 0;
            _velocity.y = laddingHeight * speed;
            _rigidbody.velocity = new Vector2(_moveX * speed + laddingHeight * (_ladder.transform.rotation * Vector3.up).x * speed,  laddingHeight * (_ladder.transform.rotation * Vector3.up).y * speed);
        }
        

    }
    
    void Jump()
    {
        if(_onPlatform || _coyotee < _coyoteeTime){
            _velocity = _platform.velocity;
            _velocity.y += jumpForce;
            jump = true;
            _onPlatform = false;
            transform.parent = null;
        }
        else if (_onWall)
        {
            Vector2 force = new Vector2((_normalContact.x < 0)? -1 : 1, 1);

            _velocity = new Vector2(force.x * jumpForce/10,force.y * jumpForce);

            _moveX = 0;
            _onWall = false;
            jump = true;
            _onPlatform = false;
        }
    }
    
    
    
    // GESTIONS COLLISION PLATEFORME
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == 9)
        {
            // Test Collision
            foreach (ContactPoint2D point in other.contacts)
            {
                //Debug.DrawLine(point.point, point.point + point.normal*5, Color.blue, 5);
                if (Vector2.Dot(Vector2.up, point.normal) < 0.5f) return;
            }
            
            _velocity.x = 0;
            transform.parent = other.gameObject.transform.parent;
            
            
            _onPlatform = true;
            _platform = other.gameObject.GetComponent<Platform>();
            
        }
        else if (other.gameObject.layer == 8 && !_onPlatform)
        {
            _normalContact = other.contacts[0].normal;
            Debug.DrawLine(other.contacts[0].point, other.contacts[0].point + _normalContact*5, Color.blue, 5);
            _onWall = true;
            jump = false;

        }
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if(other.gameObject == _platform.gameObject && _onPlatform){
            
            if(!_ladding) transform.parent = null;
            else transform.parent = _ladder.transform.parent;
            
            _onPlatform = false;
            _velocity = _platform.velocity;
        }
        else if (other.gameObject.layer == 8)
        {
            _onWall = false;
            if(!jump)_coyotee = 0;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 13)
        {
            _fixable = other.gameObject.GetComponent<Fixable>();
            _onFixable = true;
        }
        else if (other.gameObject.layer == 14)
        {
            _onLadder = true;
            _ladder = other.gameObject;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == 13)
        {
            _onFixable = false;
        }
        else if (other.gameObject.layer == 14)
        {
            if (!_onPlatform) transform.parent = null;
            else transform.parent = _platform.transform.parent;
            _onLadder = false;
            _ladding = false;
            _gravityScale = GRAVITY;
        }
    }


    public void Stun(float time)
    {
        if (_onPlatform || _ladding)
        {
            _stun = time;
        }
    }
}
