using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animation : MonoBehaviour
{
    [SerializeField]
    private List<AnimationPoint> curve;
   
    private int i = 0;
    private float timer = 0;
    private GameManager _manager;
    public float beginTimer = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        _manager = FindObjectOfType<GameManager>();
        i = 1;
        if(curve.Count > 0){
            while (beginTimer > Get(i).duration)
            {
                beginTimer -= Get(i).duration;
                ++i;
            }
        }

        timer = beginTimer;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (curve.Count > 1)
        {
            timer += Time.deltaTime;
            if (timer > Get(i).duration)
            {
                if (Get(i).stun) _manager.Stun();
                ++i;
                if(i%curve.Count == 0) Debug.Log("end");
                while (!Get(i).read) ++i;
                timer = timer-Get(i-1).duration;
            }
            
            if(Get(i).duration>0){
                transform.localPosition = Vector3.Lerp(Get(i - 1).position, Get(i).position, Get(i).curve.Evaluate(timer/Get(i).duration));
                Quaternion local = transform.localRotation;
                if(Mathf.Abs(Get(i).rotation - Get(i-1).rotation)>0.01f) local.eulerAngles = new Vector3(0,0,
                    Get(i - 1).rotation + (Get(i).rotation - Get(i-1).rotation) * Get(i).curve.Evaluate(timer/Get(i).duration));
                transform.localRotation = local;
            }
        }
        
    }

    AnimationPoint Get(int pos)
    {
        return curve[(int)(pos % curve.Count)];
    }
}
