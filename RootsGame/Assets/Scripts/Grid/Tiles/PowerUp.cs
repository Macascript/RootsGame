using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PowerUp : TileObject
{
    public PowerUp(Vector3 pos)
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
