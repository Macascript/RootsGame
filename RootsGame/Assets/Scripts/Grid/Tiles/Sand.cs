using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sand : TileObject
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
        return true;
    }

    public override bool onStep()
    {
        return true;
    }
}
