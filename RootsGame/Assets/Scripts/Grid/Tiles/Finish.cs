using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Finish : TileObject
{
    public override bool canStep()
    {
        return true;
    }

    public override bool onStep()
    {
        GridManager.instance.virtualCamera.GetComponent<ShakeCamera>().ShakeCameraCorrect();
        GridManager.instance.player.Win();
        return true;
    }
}
