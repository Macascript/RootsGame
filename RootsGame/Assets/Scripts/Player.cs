using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    [Range(0, 9)]
    private int waterEnergy;

    private TileObject actualNode;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(waterEnergy > 9) waterEnergy = 9;
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
        }
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
            useWaterEnergy();
            actualNode = node;
            if (!canMove()) gameOver();
        }
    }

    void gameOver()
    {

    }

    public void nextNode(Directions direction)
    {
        if (direction == Directions.None) return;

        actualNode.nextTileObject = GridManager.instance.GetNeighbour(actualNode, direction);
        goToNode(actualNode.nextTileObject);
    }
}
