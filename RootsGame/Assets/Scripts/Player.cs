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
    private GameObject[] terminaciones;

    void Start()
    {
        GridManager.instance.nodes[4, 0] = new Root(GridManager.instance.nodes[4, 0].m_position, terminaciones[0]);
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

    private void goToNode(TileObject node)
    {
        if (node == null) return;
        else if (node.onStep())
        {
            int nodeIndex = GridManager.instance.GetGridIndex(node.m_position);
            GridManager.instance.nodes[GridManager.instance.GetColumn(nodeIndex), GridManager.instance.GetRow(nodeIndex)] = new Root(node.m_position, terminaciones[0]);
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
        goToNode(actualNode.nextTileObject);
    }
}
