using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fixable : MonoBehaviour
{
    public GameObject fix;
    public GameObject broken;
    public GameObject progress;
    
    public bool isFix = true;
    public float fixTime;
    private float _time;
    // Start is called before the first frame update
    void Start()
    {
        broken.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isFix)
        {
          
          if(_time > 0) _time -= Time.deltaTime / 10;
          
          progress.transform.localScale = new Vector3(_time/fixTime, 1,1);
        }
    }

    public bool Fix()
    {
        _time += Time.deltaTime;
        if (_time > fixTime)
        {
            broken.SetActive(false);
            fix.SetActive(true);
            isFix = true;
            return false;
        }

        return true;

    }

    public void Broke()
    {
        
        broken.SetActive(true);
        fix.SetActive(false);
        isFix = false;
        _time = fixTime;
    }
}
