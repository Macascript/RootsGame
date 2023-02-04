using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : TileObject
{
    public Rock(Vector3 pos, GameObject randomSand, GameObject rock)
    {
        m_position = pos;
        Instantiate(randomSand, this.m_position, Quaternion.identity);
        Instantiate(rock, this.m_position, Quaternion.identity);
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
