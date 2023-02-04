using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : TileObject
{
    public Water(Vector3 pos, WaterType wType)
    {
        m_position = pos;
        type = wType;
    }

    private WaterType type;

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
