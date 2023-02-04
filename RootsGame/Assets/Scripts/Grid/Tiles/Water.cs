using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : TileObject
{
    private WaterType type;

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
        GridManager.instance.player.gainWaterEnergy(type);
        return true;
    }
}
