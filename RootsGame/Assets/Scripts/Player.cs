using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    [Range(0, 9)]
    private int waterEnergy;

    private bool food = false;

    private int mineral = 0;

    private TileObject actualNode = null;

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

    [SerializeField] private Transform finalPosGameOver;
    [SerializeField] private float gameOverSpeed = 3f;
    [SerializeField] private GameObject panelGameOver,panelWin;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip gameOverAudio, winAudio, finAudio;

    void Start()
    {
        GridManager.instance.brote = Instantiate(GridManager.instance.tallos[0], GridManager.instance.nodes[GridManager.instance.start_node, 0].transform.position + Vector3.up * 0.64f, Quaternion.identity);
        GridManager.instance.nodes[GridManager.instance.start_node, 0] = Instantiate(to_abajo, GridManager.instance.nodes[GridManager.instance.start_node, 0].transform.position, Quaternion.identity).GetComponent<Root>();
        actualNode = GridManager.instance.nodes[GridManager.instance.start_node, 0];
        ((Root)actualNode).birthAnimation();
        this.transform.position = actualNode.transform.position;
    }

    public int getWaterEnergy()
    {
        return waterEnergy;
    }
    public int getMineralPower()
    {
        return mineral;
    }
    public bool getFoodEnergy()
    {
        return food;
    }

    public bool useMineralPower()
    {
        if(mineral > 0)
        {
            mineral--;
            return true;
        }
        else
            return false;
    }

    public void gainFoodEnergy()
    {
        food = true;
    }

    public void gainMineralPower(MineralType mineralType)
    {
        switch (mineralType)
        {
            case MineralType.Calcium:
                mineral++;
                break;
            case MineralType.Sulphur:
                mineral += 2;
                break;
            case MineralType.Magnessium:
                mineral += 3;
                break;
            case MineralType.Potassium:
                mineral += 4;
                break;
            case MineralType.Phosporus:
                mineral += 5;
                break; 
            case MineralType.Nitrogen:
                mineral += 6;
                break;  
        }
    }

    public void useFoodEnergy()
    {
        food = false;
    }

    public void useWaterEnergy(int water = 1)
    {
        if(water < 0) water = 0;
        waterEnergy = waterEnergy - water;
        if (waterEnergy <= 0)
        {
            waterEnergy = 0;
            gameOver();
        }
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
        if(waterEnergy == 0) return false;

        List<TileObject> neighbours = GridManager.instance.GetNeighbours(actualNode);
        bool can = false;
        int i = 0;
        while(!can && i<neighbours.Count)
        {
            if (neighbours[i] == null) can = false;
            else can = neighbours[i].canStep();
            ++i;
        }
        return can;
    }

    private void goToNode(TileObject node, Directions direction)
    {
        if (node == null)
        {
            Debug.Log("null");
            GridManager.instance.virtualCamera.GetComponent<ShakeCamera>().ShakeCameraWrong();
            return;
        }
        else if (!GusanoBehaviour.listaIndices.ContainsValue(GridManager.instance.GetGridIndex(node.transform.position)) && node.onStep())
        {
            int nodeIndex = GridManager.instance.GetGridIndex(node.transform.position);
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
            //GridManager.instance.nodes[GridManager.instance.GetColumn(nodeIndex), GridManager.instance.GetRow(nodeIndex)] = new Root(node.transform.position, auxObj);
            GridManager.instance.nodes[GridManager.instance.GetColumn(nodeIndex), GridManager.instance.GetRow(nodeIndex)] = Instantiate(auxObj, GridManager.instance.GetGridCellCenter(nodeIndex), Quaternion.identity).GetComponent<Root>();
            actualNode.nextTileObject = GridManager.instance.nodes[GridManager.instance.GetColumn(nodeIndex), GridManager.instance.GetRow(nodeIndex)];
            ((Root)actualNode).growAnimation(direction);

            useWaterEnergy();
            actualNode = actualNode.nextTileObject;

            if (!canMove())
            {
                Debug.Log("cant move");
                gameOver();
            }
        }
        else if (GusanoBehaviour.listaIndices.ContainsValue(GridManager.instance.GetGridIndex(node.transform.position))){
            Debug.Log("PAtat2 "+ GridManager.instance.GetGridIndex(node.transform.position));
            Debug.Log(GusanoBehaviour.listaIndices);
            GridManager.instance.player.useWaterEnergy(9);
            GridManager.instance.virtualCamera.GetComponent<ShakeCamera>().ShakeCameraWrong();
        }
    }

    void gameOver()
    {
        GridManager.instance.setGameOver();
        Debug.Log("gameOver");
        GridManager.instance.virtualCamera.GetComponent<ShakeCamera>().ShakeCameraWrong(false);
        StartCoroutine(gameOverCoroutine(panelGameOver));
    }
    private IEnumerator gameOverCoroutine(GameObject panel)
    {
        yield return new WaitForSeconds(1f);
        GridManager.instance.virtualCamera.GetComponent<ShakeCamera>().StopShaking();
        FindObjectOfType<GestureDetector>().enabled = false;
        Vector3 direction = (finalPosGameOver.position - transform.position).normalized;
        audioSource.PlayOneShot(gameOverAudio);
        yield return new WaitForSeconds(2f);
        while(Vector2.Distance(transform.position,finalPosGameOver.position) > 1f)
        {
            //Debug.Log(Vector2.Distance(transform.position, finalPosGameOver.position));
            transform.position += direction * gameOverSpeed * Time.deltaTime;
            yield return null;
        }
        audioSource.PlayOneShot(finAudio);
        yield return new WaitForSeconds(3f);
        panel.SetActive(true);
    }

    public void Win()
    {
        GridManager.instance.virtualCamera.GetComponent<ShakeCamera>().ShakeCameraCorrect(false);
        audioSource.PlayOneShot(winAudio);
        StartCoroutine(gameOverCoroutine(panelWin));
    }

    public void nextNode(Directions direction)
    {
        if (direction == Directions.None) return;

        if(direction == Directions.Up || direction == Directions.RightUp || direction == Directions.LeftUp && !useMineralPower())
        {
            GridManager.instance.virtualCamera.GetComponent<ShakeCamera>().ShakeCameraWrong();
            return;
        }

        this.transform.position = actualNode.transform.position;
        actualNode.nextTileObject = GridManager.instance.GetNeighbour(actualNode, direction);
        goToNode(actualNode.nextTileObject, direction);
    }

    public void nextRightNode() => nextNode(Directions.Right);
    public void nextRightDownNode() => nextNode(Directions.RightDown);
    public void nextDownNode() => nextNode(Directions.Down);
    public void nextLeftDownNode() => nextNode(Directions.LeftDown);
    public void nextLeftNode() => nextNode(Directions.Left);
    public void nextUpNode() => nextNode(Directions.Up);
    public void nextRightUpNode() => nextNode(Directions.RightUp);
    public void nextLeftUpNode() => nextNode(Directions.LeftUp);
}
