using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sand : TileObject
{
    //public Sand(Vector3 pos, GameObject randomSand)
    //{
    //    transform.position = pos;
    //    Instantiate(randomSand, this.transform.position, Quaternion.identity);
    //}

    public override bool canStep()
    {
        return true;
    }

    public override bool onStep()
    {
        return true;
    }
}
