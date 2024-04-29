using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoverGrid : MonoBehaviour
{
    [SerializeField] private MoverData[] moverData;

    private GameObject[] parts;
    private QueueArray<Directions> history;

    // Sacadas de Player
    private TileObject actualNode = null;

    private void Awake()
    {
        if (moverData.Length > 1)
        {
            parts = new GameObject[moverData.Length-1];
            history = new QueueArray<Directions>(parts.Length);
        }
    }

    public void StepTo(Directions direction)
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

    // Sacado de Player
    public bool CanMove()
    {
        List<TileObject> neighbours = GridManager.instance.GetNeighbours(actualNode);
        bool can = false;
        int i = 0;
        while (!can && i < neighbours.Count)
        {
            if (neighbours[i] == null) can = false;
            else can = neighbours[i].canStep();
            ++i;
        }
        return can;
    }

    private void GoToNode(TileObject node, Directions direction)
    {
        if (node == null)
        {
            Debug.Log("null");
            return;
        }
        else if (!GusanoBehaviour.listaIndices.ContainsValue(GridManager.instance.GetGridIndex(node.transform.position)) && node.onStep())
        {
            int nodeIndex = GridManager.instance.GetGridIndex(node.transform.position);

            //partsGridManager.instance.GetGridCellCenter(nodeIndex)

            //GridManager.instance.nodes[GridManager.instance.GetColumn(nodeIndex), GridManager.instance.GetRow(nodeIndex)] = new Root(node.transform.position, auxObj);
            //GridManager.instance.nodes[GridManager.instance.GetColumn(nodeIndex), GridManager.instance.GetRow(nodeIndex)] = Instantiate(auxObj, , Quaternion.identity).GetComponent<Root>();
            actualNode.nextTileObject = GridManager.instance.nodes[GridManager.instance.GetColumn(nodeIndex), GridManager.instance.GetRow(nodeIndex)];
            ((Root)actualNode).growAnimation(direction);

            actualNode = actualNode.nextTileObject;

        }
        else if (GusanoBehaviour.listaIndices.ContainsValue(GridManager.instance.GetGridIndex(node.transform.position)))
        {
            Debug.Log("PAtat2 " + GridManager.instance.GetGridIndex(node.transform.position));
            Debug.Log(GusanoBehaviour.listaIndices);
            GridManager.instance.player.useWaterEnergy(9);
            GridManager.instance.virtualCamera.GetComponent<ShakeCamera>().ShakeCameraWrong();
        }
    }
}
