using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<Fixable> fixables;
    // Start is called before the first frame update
    void Start()
    {
        fixables = new List<Fixable>();
        foreach(Fixable fix in FindObjectsOfType<Fixable>())
        {
            fixables.Add(fix);
            fix.Broke();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
