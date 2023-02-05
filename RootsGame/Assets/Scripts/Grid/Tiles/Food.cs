using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Food : TileObject
{
    //public Food(Vector3 pos, GameObject randomSand)
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
        GridManager.instance.player.gainFoodEnergy();
        return true;
    }
}
