using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    [Range(0, 9)]
    private int waterEnergy;

    private TileObject actualNode;

    [SerializeField]
    private GameObject to_abajo;

    [SerializeField]
    private GameObject to_arriba;

    [SerializeField]
    private GameObject to_izquierda;

    [SerializeField]
    private GameObject to_derecha;

    [SerializeField]
    private GameObject to_abajo_derecha;

    [SerializeField]
    private GameObject to_abajo_izquierda;

    [SerializeField]
    private GameObject to_arriba_derecha;

    [SerializeField]
    private GameObject to_arriba_izquierda;

    void Start()
    {
        GridManager.instance.nodes[4, 0] = new Root(GridManager.instance.nodes[4, 0].m_position, to_abajo);
        actualNode = GridManager.instance.nodes[4, 0];
    }

    public int getWaterEnergy()
    {
        return waterEnergy;
    }

    public void useWaterEnergy(int water = 1)
    {
        if(water < 0) water = 0;
        waterEnergy = waterEnergy - water;
        if(waterEnergy < 0) waterEnergy = 0;
    }

    public void gainWaterEnergy(WaterType water)
    {
        switch(water)
        {
            case WaterType.Simple:
                waterEnergy += 2;
                break;
            case WaterType.Double:
                waterEnergy += 4;
                break;
            case WaterType.Max:
                waterEnergy = 9;
                break;
            default:
                break;
        }
        if (waterEnergy > 9) waterEnergy = 9;
    }

    public bool canMove()
    {
        if(waterEnergy <= 0) return false;

        TileObject[] neighbours = {};
        GridManager.instance.GetNeighbours(actualNode, neighbours);
        bool can = false;
        int i = 0;
        while(!can)
        {
            can = neighbours[i].canStep();
        }
        return can;
    }

    private void goToNode(TileObject node, Directions direction)
    {
        if (node == null) return;
        else if (node.onStep())
        {
            int nodeIndex = GridManager.instance.GetGridIndex(node.m_position);
            GameObject auxObj = to_abajo;
            switch (direction)
            {
                case Directions.Left:
                    auxObj = to_izquierda;
                    break;
                case Directions.Right:
                    auxObj = to_derecha;
                    break;
                case Directions.Up:
                    auxObj = to_arriba;
                    break;
                case Directions.Down:
                    auxObj = to_abajo;
                    break;
                case Directions.LeftDown:
                    auxObj = to_abajo_izquierda;
                    break;
                case Directions.RightDown:
                    auxObj = to_abajo_derecha;
                    break;
                case Directions.LeftUp:
                    auxObj = to_arriba_izquierda;
                    break;
                case Directions.RightUp:
                    auxObj = to_abajo_derecha;
                    break;
                default:
                    break;
            }
            GridManager.instance.nodes[GridManager.instance.GetColumn(nodeIndex), GridManager.instance.GetRow(nodeIndex)] = new Root(node.m_position, auxObj);
            actualNode.nextTileObject = GridManager.instance.nodes[GridManager.instance.GetColumn(nodeIndex), GridManager.instance.GetRow(nodeIndex)];
            ((Root)actualNode).growAnimation();

            useWaterEnergy();
            actualNode = actualNode.nextTileObject;

            if (!canMove()) gameOver();
        }
        // else actualNode backwards animation
    }

    void gameOver()
    {
        //TO DO
    }

    public void nextNode(Directions direction)
    {
        if (direction == Directions.None) return;

        actualNode.nextTileObject = GridManager.instance.GetNeighbour(actualNode, direction);
        goToNode(actualNode.nextTileObject, direction);
    }
}
