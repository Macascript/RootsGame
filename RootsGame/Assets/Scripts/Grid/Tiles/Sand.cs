using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sand : TileObject
{
    public Sand(Vector3 pos)
    {
        m_position = pos;
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
