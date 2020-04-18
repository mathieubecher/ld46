using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocusPoint : MonoBehaviour
{
    public GameObject focus;

    void FixedUpdate()
    {
        transform.position = focus.transform.position;
    }
}