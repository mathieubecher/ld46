using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fixable : MonoBehaviour
{
    public GameObject fix;
    public GameObject broken;
    public GameObject progressbar;
    
    public bool isFix = true;
    public float fixTime;
    private float _time;
    
    public Gradient color;
    public SpriteRenderer progress;

    public float time
    {
        get => _time;
        set => _time = value;
    }

    // Start is called before the first frame update
    void Start()
    {
        broken.SetActive(false);
        _time = fixTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isFix)
        {
            if (_time <= 0)
            {
                progressbar.SetActive(false);
            }
          if(_time > 0) _time -= Time.deltaTime / 8;
          
          progress.transform.parent.localScale = new Vector3(_time/fixTime, 1,1);
          progress.color = color.Evaluate(_time/fixTime);
        }
    }

    public bool Fix()
    {
        if (_time > 0)
        {
            _time += Time.deltaTime;
            if (_time > fixTime)
            {
                broken.SetActive(false);
                fix.SetActive(true);
                isFix = true;
                return false;
            }
        }
        

        return true;

    }

    public void Broke()
    {
        
        broken.SetActive(true);
        fix.SetActive(false);
        isFix = false;
    }
}
