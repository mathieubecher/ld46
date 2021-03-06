﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderTest : MonoBehaviour
{
    public List<GameObject> platform;

    void Start()
    {
        platform = new List<GameObject>();
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.isTrigger)
        {
            platform.Add(other.gameObject);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (!other.isTrigger)
        {
            platform.Remove(other.gameObject);
        }
    }
}
