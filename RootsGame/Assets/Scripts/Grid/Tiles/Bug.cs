using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bug : TileObject
{
    public Bug(Vector3 pos)
    {
        m_position = pos;
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
