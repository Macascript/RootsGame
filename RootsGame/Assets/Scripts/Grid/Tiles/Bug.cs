using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bug : TileObject
{
    //public Bug(Vector3 pos, GameObject randomSand)
    //{
    //    transform.position = pos;
    //    Instantiate(randomSand, this.transform.position, Quaternion.identity);
    //}

    public override bool canStep()
    {
        return false;
    }

    public override bool onStep()
    {
        GridManager.instance.player.useWaterEnergy(9);
        return false;
    }
}
