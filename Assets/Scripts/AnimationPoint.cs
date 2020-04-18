using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationPoint : MonoBehaviour
{
    public float duration;

    public Vector3 position;
    public float rotation;
    public bool horaire = true;
    public Vector3 left;
    public Vector2 right;
    
    // Start is called before the first frame update
    void Start()
    {
        position = transform.localPosition + transform.parent.localPosition + transform.parent.transform.parent.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
