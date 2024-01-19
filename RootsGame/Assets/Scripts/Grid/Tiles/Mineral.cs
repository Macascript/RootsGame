using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Mineral : TileObject
{
    [SerializeField] private MineralType type;

    [SerializeField]
    private AudioSource m_source;

    public override bool canStep()
    {
        return true;
    }

    public override bool onStep()
    {
        m_source.Play();
        GridManager.instance.player.gainMineralPower(type);
        GridManager.instance.virtualCamera.GetComponent<ShakeCamera>().ShakeCameraCorrect();
        Destroy(gameObject);
        return true;
    }
}