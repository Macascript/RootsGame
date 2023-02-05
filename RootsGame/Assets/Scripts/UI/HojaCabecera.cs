using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HojaCabecera : MonoBehaviour
{
    void Start()
    {
        GetComponent<Image>().fillAmount = GridManager.instance.player.getWaterEnergy() / 9.0f;
    }

    void Update()
    {
        GetComponent<Image>().fillAmount = GridManager.instance.player.getWaterEnergy() / 9.0f;
    }
}
