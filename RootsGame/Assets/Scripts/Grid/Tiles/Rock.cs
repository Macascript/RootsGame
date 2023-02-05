using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : TileObject
{
    //public Rock(Vector3 pos, GameObject randomSand, GameObject rock)
    //{
    //    transform.position = pos;
    //    Instantiate(randomSand, this.transform.position, Quaternion.identity);
    //    Instantiate(rock, this.transform.position, Quaternion.identity);
    //}

    public override bool canStep()
    {
        return false;
    }

    public override bool onStep()
    {
        if (GridManager.instance.player.getFoodEnergy())
        {
            GridManager.instance.player.useFoodEnergy();
            GridManager.instance.virtualCamera.GetComponent<ShakeCamera>().ShakeCameraCorrect();
            Destroy(gameObject);
            return true;
        }
        else
        {
            GridManager.instance.virtualCamera.GetComponent<ShakeCamera>().ShakeCameraWrong();
            return false;
        }
    }
}
