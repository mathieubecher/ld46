using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllFix : MonoBehaviour
{
    private bool resetBroke = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!resetBroke) ResetBroke();
    }

    void ResetBroke()
    {
        resetBroke = true;
        foreach (var fix in FindObjectsOfType<Fixable>())
        {
            fix.Broke();
            fix.time = fix.fixTime / 2;
        }
    }
}
