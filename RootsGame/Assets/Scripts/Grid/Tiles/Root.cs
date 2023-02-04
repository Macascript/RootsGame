using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Root : TileObject
{
    public Root(Vector3 pos, GameObject terminacion)
    {
        m_position = pos;
        Instantiate(terminacion, this.m_position, Quaternion.identity);
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
