using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : TileObject
{
    public Water(Vector3 pos, WaterType wType, GameObject randomSand, GameObject water)
    {
        m_position = pos;
        type = wType;
        Instantiate(randomSand, this.m_position, Quaternion.identity);
        Instantiate(water, this.m_position, Quaternion.identity);
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
