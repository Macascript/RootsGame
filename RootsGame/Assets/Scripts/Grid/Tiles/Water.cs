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
        GameObject waterObj = Instantiate(water, this.m_position, Quaternion.identity);
        waterObj.transform.Rotate(new Vector3(0.0f,0.0f, Random.Range(0, 4) * 90));
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
