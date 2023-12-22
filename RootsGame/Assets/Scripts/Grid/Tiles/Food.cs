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
    [SerializeField]
    private AudioSource m_source;

    public override bool canStep()
    {
        return true;
    }

    public override bool onStep()
    {
        m_source.Play();
        GridManager.instance.player.gainFoodEnergy();
        GridManager.instance.virtualCamera.GetComponent<ShakeCamera>().ShakeCameraCorrect();
        Destroy(gameObject);
        return true;
    }
}
