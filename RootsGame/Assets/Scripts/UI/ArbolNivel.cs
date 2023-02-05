using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArbolNivel : MonoBehaviour
{
    [SerializeField]
    private Sprite[] arboles;

    private void Update()
    {
        if (GridManager.instance.GetGridIndex(GridManager.instance.player.transform.position) > 161)
            GetComponent<Image>().sprite = arboles[1];
        if (GridManager.instance.GetGridIndex(GridManager.instance.player.transform.position) > 363)
            GetComponent<Image>().sprite = arboles[2];
    }
}
