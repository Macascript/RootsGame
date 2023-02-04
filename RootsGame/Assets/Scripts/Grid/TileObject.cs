using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileObject : MonoBehaviour
{
    public Vector3 m_position;
    public TileObject nextTileObject;
    public Animator animator = null;

    public TileObject(Vector3 pos)
    {
        m_position = pos;
    }

    public TileObject(){}

    public virtual bool canStep()
    {
        return true;
    }

    public virtual bool onStep()
    {
        return true;
    }
}
