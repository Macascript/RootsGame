using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Root : TileObject
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override bool canStep()
    {
        return false;
    }

    public override bool onStep()
    {
        return false;
    }
}
