using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoverGrid : MonoBehaviour
{
    [SerializeField] private MoverData[] moverData;

    private GameObject[] parts;

    private void Awake()
    {
        if (moverData.Length > 1)
            parts = new GameObject[moverData.Length-1];
    }

    public void MoveTo(Directions direction)
    {
        for (int i = 0; i < parts.Length; i++)
        {

        }

        switch (direction)
        {
            case Directions.Left:
                //auxObj = to_izquierda;
                break;
            case Directions.Right:
                //auxObj = to_derecha;
                break;
            case Directions.Up:
                //auxObj = to_arriba;
                break;
            case Directions.Down:
                //auxObj = to_abajo;
                break;
            case Directions.LeftDown:
                //auxObj = to_abajo_izquierda;
                break;
            case Directions.RightDown:
                //auxObj = to_abajo_derecha;
                break;
            case Directions.LeftUp:
                //auxObj = to_arriba_izquierda;
                break;
            case Directions.RightUp:
                //auxObj = to_abajo_derecha;
                break;
            default:
                break;
        }
    }
}
