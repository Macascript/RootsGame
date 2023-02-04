using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PowerUp : TileObject
{
    public PowerUp(Vector3 pos, GameObject randomSand)
    {
        m_position = pos;
        Instantiate(randomSand, this.m_position, Quaternion.identity);
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
