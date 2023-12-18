using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Unity.VisualScripting;

public class GridManager : MonoBehaviour
{
    private static GridManager s_Instance = null;

    [SerializeField]
    public GameObject[] sand;

    [SerializeField]
    private GameObject rock;

    [SerializeField]
    private GameObject[] water;

    [SerializeField]
    private GameObject bug;

    [SerializeField]
    private GameObject food;

    [SerializeField]
    private GameObject powerUp;

    [SerializeField]
    private GameObject[] paredDer;

    [SerializeField]
    private GameObject[] paredIzq;

    [SerializeField]
    private GameObject negro;

    [SerializeField]
    private GameObject[] pradoSup;

    [SerializeField]
    private GameObject[] pradoInf;

    [SerializeField]
    public GameObject virtualCamera;

    [SerializeField]
    private GameObject[] recorridos;

    [SerializeField]
    public GameObject[] tallos;

    [SerializeField]
    public GameObject[] finish;

    public bool isGameOver {get; private set;} = false;

    public void setGameOver(){
        isGameOver = true;
    }

    public static GridManager instance
    {
        get
        {
            if (s_Instance == null)
            {
                s_Instance = FindObjectOfType(typeof(GridManager))
                as GridManager;
                if (s_Instance == null)
                    Debug.Log("Could not locate a GridManager " +
                              "object. \n You have to have exactly " +
                              "one GridManager in the scene.");
            }
            return s_Instance;
        }
    }

    private int numOfRows = 0;
    private int numOfColumns = 0;
    private float gridCellSize = 0.64f;
    public bool showGrid = true;
    private Vector3 origin = new Vector3();
    public TileObject[,] nodes { get; set; }
    public Player player;
    private int index = 0;
    private int lineCounter = 0;
    public GameObject brote;

    public Vector3 Origin
    {
        get
        {
            return origin;
        }
    }

    private void Awake()
    {
        CalculateTiles();
        isGameOver = false;
    }

    private void Update() { }

    void CalculateTiles()
    {
        UnityEngine.Object[] chunks = Resources.LoadAll("chunks/");
        numOfRows = 0;

        TextAsset txt;
        List<string> lines;
        //numOfColumns = lines[1].Length;
        numOfColumns = 0;

        for (int i = 0; i < chunks.Length; i++)
        {
            txt = (TextAsset)Resources.Load(System.String.Concat("chunks/chunk", (i + 1)), typeof(TextAsset));
            lines = new List<string>(txt.text.Split('\n'));
            numOfRows += System.Int32.Parse(lines[0]);
            for (int j = 1; j <= System.Int32.Parse(lines[0]); j++)
            {
                if (lines[j].Length > numOfColumns)
                    numOfColumns = lines[j].Length;
            }
        }

        nodes = new TileObject[numOfColumns, numOfRows];

        for (int a = 0; a < chunks.Length; a++)
        {
            txt = (TextAsset)Resources.Load(System.String.Concat("chunks/chunk", (a + 1)), typeof(TextAsset));
            lines = new List<string>(txt.text.Split('\n'));
            loadLevel(lines);
        }
    }

    void spawnBug(string bugInfo, int actualLevelLineSize)
    {
        string bugPos = bugInfo.Split("):")[0];
        string bugPath = bugInfo.Split("):")[1];

        char[] simpleSeparators = new char[] { '(', ',' };

        int bugPosCol = System.Int32.Parse(bugPos.Split(simpleSeparators, System.StringSplitOptions.RemoveEmptyEntries)[1]) - 1;
        int bugPosRow = System.Int32.Parse(bugPos.Split(simpleSeparators, System.StringSplitOptions.RemoveEmptyEntries)[0]) - 1 + (lineCounter - actualLevelLineSize);

        int bugPathCol;
        int bugPathRow;

        Vector3 cellPosBug = GetGridCellCenter(GetIndexByCoords(bugPosRow, bugPosCol));

        string[] separators = new string[] { "[(", "),(", ")]" , "\0", "\n", "\r" };
        System.String[] bugPathCoords = bugPath.Split(separators, System.StringSplitOptions.RemoveEmptyEntries);

        List<Vector3> recorrido = new List<Vector3>();

        for (int i = 0; i < bugPathCoords.Length; i++)
        {
            separators = new string[] { ",", "\0", " ", "\n", "\r"};
            string[] aux = bugPathCoords[i].Split(separators, System.StringSplitOptions.RemoveEmptyEntries);

            bugPathCol = System.Int32.Parse(aux[1]) - 1;
            bugPathRow = System.Int32.Parse(aux[0]) - 1 + (lineCounter - actualLevelLineSize);
            recorrido.Add(GetGridCellCenter(GetIndexByCoords(bugPathRow, bugPathCol)));
        }
        recorrido.Add(cellPosBug);

        Instantiate(bug, cellPosBug, Quaternion.identity).GetComponent<GusanoBehaviour>().setPath(recorrido);
    }

    void loadLevel(List<string> lines)
    {
        Vector3 cellPos = GetGridCellCenter(index);
        TileObject node = null;

        int randomAux = 0;
        for (int i = 1; i < lines.Count; i++)
        {
            if (i > System.Int32.Parse(lines[0]))
            {
                spawnBug(lines[i], System.Int32.Parse(lines[0]));
            }
            else
            {

                for (int j = 0; j < numOfColumns; j++)
                {
                    //if (j == lines[i].Length) continue;
                    if (j >= lines[i].Length)
                    {
                        nodes[j, lineCounter] = null;
                        index++;
                        continue;
                    }

                    cellPos = GetGridCellCenter(index);

                    if (j == 0)
                    {
                        for (int z = 1; z <= 10; ++z)
                        {
                            Instantiate(negro, new Vector3(cellPos.x - (0.64f * z), cellPos.y, cellPos.z), Quaternion.identity);
                        }
                        int randomRocas = UnityEngine.Random.Range(0, 2);
                        Instantiate(paredIzq[randomRocas], new Vector3(cellPos.x - 0.385f, cellPos.y, cellPos.z), Quaternion.identity);
                        Instantiate(paredDer[(randomRocas + 1) % 2], new Vector3(cellPos.x - 0.255f, cellPos.y, cellPos.z), Quaternion.identity);
                    }

                    if (j == lines[i].Length - 1)
                    {
                        int randomRocas = 0;
                        Vector3 negroPos;
                        for (var (z, contadorNegros) = (j, 1); z <= 10 + numOfColumns; ++z, ++contadorNegros)
                        {
                            negroPos = new Vector3(cellPos.x + (0.64f * contadorNegros), cellPos.y, cellPos.z);
                            Instantiate(negro, negroPos, Quaternion.identity);

                            if(GetRow(GetGridIndex(negroPos)) - 1 >= 0 && nodes[ z + 1 , GetRow(GetGridIndex(negroPos)) - 1] != null)
                            {
                                randomRocas = UnityEngine.Random.Range(0, 2);
                                GameObject paredSup = Instantiate(paredDer[randomRocas], new Vector3(cellPos.x + (0.64f * contadorNegros), cellPos.y + 0.385f, cellPos.z), Quaternion.identity);
                                GameObject paredInf = Instantiate(paredIzq[(randomRocas + 1) % 2], new Vector3(cellPos.x + (0.64f * contadorNegros), cellPos.y + 0.255f, cellPos.z), Quaternion.identity);
                                paredSup.transform.Rotate(new Vector3(0.0f, 0.0f, 90));
                                paredInf.transform.Rotate(new Vector3(0.0f, 0.0f, 90));
                            }
                        }
                        randomRocas = UnityEngine.Random.Range(0, 2);
                        Instantiate(paredDer[randomRocas], new Vector3(cellPos.x + 0.385f, cellPos.y, cellPos.z), Quaternion.identity);
                        Instantiate(paredIzq[(randomRocas + 1) % 2], new Vector3(cellPos.x + 0.255f, cellPos.y, cellPos.z), Quaternion.identity);
                    }

                    if (lineCounter == 0 && j < lines[i].Length)
                    {
                        Instantiate(pradoInf[j % 2], cellPos, Quaternion.identity);
                        Instantiate(pradoSup[j % 2], new Vector3(cellPos.x, cellPos.y + 0.64f, cellPos.z), Quaternion.identity);
                    }

                    randomAux = UnityEngine.Random.Range(0, 3);

                    switch (lines[i][j])
                    {
                        case 'A':
                            //node = new Sand(cellPos, sand[randomAux]);
                            node = Instantiate(sand[randomAux], cellPos, Quaternion.identity).GetComponent<Sand>();
                            break;
                        case 'B':
                            //node = new Rock(cellPos, sand[randomAux], rock);
                            Instantiate(sand[randomAux], cellPos, Quaternion.identity);
                            node = Instantiate(rock, cellPos, Quaternion.identity).GetComponent<Rock>();
                            break;
                        case 'C':
                            //node = new Water(cellPos, WaterType.Simple, sand[randomAux], water[0]);
                            Instantiate(sand[randomAux], cellPos, Quaternion.identity);
                            node = Instantiate(water[0], cellPos, Quaternion.identity).GetComponent<Water>();
                            node.transform.Rotate(new Vector3(0.0f, 0.0f, UnityEngine.Random.Range(0, 4) * 90));
                            break;
                        case 'D':
                            //node = new Water(cellPos, WaterType.Double, sand[randomAux], water[1]);
                            Instantiate(sand[randomAux], cellPos, Quaternion.identity);
                            node = Instantiate(water[1], cellPos, Quaternion.identity).GetComponent<Water>();
                            node.transform.Rotate(new Vector3(0.0f, 0.0f, UnityEngine.Random.Range(0, 4) * 90));
                            break;
                        case 'E':
                            //node = new Water(cellPos, WaterType.Max, sand[randomAux], water[2]);
                            Instantiate(sand[randomAux], cellPos, Quaternion.identity);
                            node = Instantiate(water[2], cellPos, Quaternion.identity).GetComponent<Water>();
                            break;
                        case 'F':
                            //node = new Bug(cellPos, sand[randomAux]);
                            Instantiate(sand[randomAux], cellPos, Quaternion.identity);
                            //node = Instantiate(bug, cellPos, Quaternion.identity).GetComponent<Bug>();
                            break;
                        case 'G':
                            //node = new Food(cellPos, sand[randomAux]);
                            Instantiate(sand[randomAux], cellPos, Quaternion.identity);
                            node = Instantiate(food, cellPos, Quaternion.identity).GetComponent<Food>();
                            break;
                        case 'H':
                            //node = new PowerUp(cellPos, sand[randomAux]);
                            node = Instantiate(sand[randomAux], cellPos, Quaternion.identity).GetComponent<Sand>();
                            //Instantiate(bug, cellPos, Quaternion.identity).GetComponent<GusanoBehaviour>().pointsPrefab = recorridos[gusanitos];
                            //gusanitos++;
                            break;
                        case 'I':
                            node = Instantiate(finish[0], cellPos, Quaternion.identity).GetComponent<Finish>();
                            break;
                        case 'J':
                            node = Instantiate(finish[1], cellPos, Quaternion.identity).GetComponent<Finish>();
                            break;
                        case 'K':
                            node = Instantiate(finish[2], cellPos, Quaternion.identity).GetComponent<Finish>();
                            break;
                        case 'L':
                            node = Instantiate(finish[3], cellPos, Quaternion.identity).GetComponent<Finish>();
                            break;
                        case 'M':
                            node = Instantiate(finish[4], cellPos, Quaternion.identity).GetComponent<Finish>();
                            break;
                        case 'N':
                            node = Instantiate(finish[5], cellPos, Quaternion.identity).GetComponent<Finish>();
                            break;
                        case 'O':
                            node = Instantiate(finish[6], cellPos, Quaternion.identity).GetComponent<Finish>();
                            break;
                        case 'P':
                            node = Instantiate(finish[7], cellPos, Quaternion.identity).GetComponent<Finish>();
                            break;
                        case 'Q':
                            node = Instantiate(finish[8], cellPos, Quaternion.identity).GetComponent<Finish>();
                            break;
                        case 'R':
                            node = Instantiate(finish[9], cellPos, Quaternion.identity).GetComponent<Finish>();
                            break;
                        case 'S':
                            node = Instantiate(finish[10], cellPos, Quaternion.identity).GetComponent<Finish>();
                            break;
                        case 'T':
                            node = Instantiate(finish[11], cellPos, Quaternion.identity).GetComponent<Finish>();
                            break;
                        case 'U':
                            node = Instantiate(finish[12], cellPos, Quaternion.identity).GetComponent<Finish>();
                            break;
                        default:
                            break;
                    }
                    nodes[j, lineCounter] = node;
                    index++;
                    if (GetRow(GetGridIndex(cellPos)) - 1 >= 0 && nodes[j, GetRow(GetGridIndex(cellPos)) - 1] == null)
                    {
                        int randomRocas = UnityEngine.Random.Range(0, 2);
                        GameObject paredSup = Instantiate(paredDer[randomRocas], new Vector3(cellPos.x, cellPos.y + 0.385f, cellPos.z), Quaternion.identity);
                        GameObject paredInf = Instantiate(paredIzq[(randomRocas + 1) % 2], new Vector3(cellPos.x, cellPos.y + 0.255f, cellPos.z), Quaternion.identity);
                        paredSup.transform.Rotate(new Vector3(0.0f, 0.0f, 90));
                        paredInf.transform.Rotate(new Vector3(0.0f, 0.0f, 90));
                    }
                }
                lineCounter++;
            }
        }
    }

    public Vector3 GetGridCellCenter(int index)
    {
        Vector3 cellPosition = GetGridCellPosition(index);
        cellPosition.x += (gridCellSize / 2.0f);
        cellPosition.y -= (gridCellSize / 2.0f);
        return cellPosition;
    }

    public Vector3 GetGridCellPosition(int index)
    {
        int row = GetRow(index);
        int col = GetColumn(index);
        float xPosInGrid = col * gridCellSize;
        float yPosInGrid = -row * gridCellSize;
        return Origin + new Vector3(xPosInGrid, yPosInGrid, 0.0f);
    }

    public int GetGridIndex(Vector3 pos)
    {
        if (!IsInBounds(pos))
        {
            return -1;
        }

        pos -= Origin;
        int col = (int)(pos.x / gridCellSize);
        int row = (int)(-pos.y / gridCellSize);
        return (row * numOfColumns + col);
    }

    public bool IsInBounds(Vector3 pos)
    {
        float width = numOfColumns * gridCellSize;
        float height = numOfRows * gridCellSize;
        return (pos.x >= Origin.x && pos.x <= Origin.x + width &&
        pos.x >= Origin.y - height && pos.y <= Origin.y);
    }

    public int GetRow(int index)
    {
        int row = index / numOfColumns;
        return row;
    }

    public int GetColumn(int index)
    {
        int col = index % numOfColumns;
        return col;
    }

    public int GetIndexByCoords(int row, int column)
    {
        int index = (row * numOfColumns) + column;
        return index;
    }

    public List<TileObject> GetNeighbours(TileObject node)
    {
        //TileObject[] neighbors = {null, null, null, null, null, null, null, null};
        List<TileObject> neighbors = new List<TileObject>();
        Vector3 neighborPos = node.transform.position;
        int neighborIndex = GetGridIndex(neighborPos);
        int row = GetRow(neighborIndex);
        int column = GetColumn(neighborIndex);
        //int counter = 0;
        //Bottom
        int leftNodeRow = row + 1;
        int leftNodeColumn = column;
        neighbors.Add(AssignNeighbour(leftNodeRow, leftNodeColumn));
        //counter++;
        //Top
        leftNodeRow = row - 1;
        leftNodeColumn = column;
        //neighbors.Add(AssignNeighbour(leftNodeRow, leftNodeColumn));
        //counter++;
        //Diagonal Top Right
        leftNodeRow = row - 1;
        leftNodeColumn = column + 1;
        //neighbors.Add(AssignNeighbour(leftNodeRow, leftNodeColumn));
        //counter++;
        //Diagonal Top Left
        leftNodeRow = row - 1;
        leftNodeColumn = column - 1;
        //neighbors.Add(AssignNeighbour(leftNodeRow, leftNodeColumn));
        //counter++;
        //Right
        leftNodeRow = row;
        leftNodeColumn = column + 1;
        neighbors.Add(AssignNeighbour(leftNodeRow, leftNodeColumn));
        //counter++;
        //Left
        leftNodeRow = row;
        leftNodeColumn = column - 1;
        neighbors.Add(AssignNeighbour(leftNodeRow, leftNodeColumn));
        //counter++;
        //Diagonal Bottom Right
        leftNodeRow = row + 1;
        leftNodeColumn = column + 1;
        neighbors.Add(AssignNeighbour(leftNodeRow, leftNodeColumn));
        //counter++;
        //Diagonal Bottom Left
        leftNodeRow = row + 1;
        leftNodeColumn = column - 1;
        neighbors.Add(AssignNeighbour(leftNodeRow, leftNodeColumn));

        return neighbors;
    }

    public TileObject GetNeighbour(TileObject node, Directions direction)
    {
        Vector3 neighborPos = node.transform.position;
        int neighborIndex = GetGridIndex(neighborPos);
        int row = GetRow(neighborIndex);
        int column = GetColumn(neighborIndex);
        TileObject auxNode = null;
        int leftNodeRow;
        int leftNodeColumn;

        switch (direction)
        {
            case Directions.None:
                auxNode = node;
                break;
            case Directions.Left:
                leftNodeRow = row;
                leftNodeColumn = column - 1;
                auxNode = AssignNeighbour(leftNodeRow, leftNodeColumn);
                break;
            case Directions.Right:
                leftNodeRow = row;
                leftNodeColumn = column + 1;
                auxNode = AssignNeighbour(leftNodeRow, leftNodeColumn);
                break;
            case Directions.Up:
                leftNodeRow = row - 1;
                leftNodeColumn = column;
                auxNode = AssignNeighbour(leftNodeRow, leftNodeColumn);
                break;
            case Directions.Down:
                leftNodeRow = row + 1;
                leftNodeColumn = column;
                auxNode = AssignNeighbour(leftNodeRow, leftNodeColumn);
                break;
            case Directions.LeftDown:
                leftNodeRow = row + 1;
                leftNodeColumn = column - 1;
                auxNode = AssignNeighbour(leftNodeRow, leftNodeColumn);
                break;
            case Directions.RightDown:
                leftNodeRow = row + 1;
                leftNodeColumn = column + 1;
                auxNode = AssignNeighbour(leftNodeRow, leftNodeColumn);
                break;
            case Directions.LeftUp:
                leftNodeRow = row - 1;
                leftNodeColumn = column - 1;
                auxNode = AssignNeighbour(leftNodeRow, leftNodeColumn);
                break;
            case Directions.RightUp:
                leftNodeRow = row - 1;
                leftNodeColumn = column + 1;
                auxNode = AssignNeighbour(leftNodeRow, leftNodeColumn);
                break;
            default:
                break;
        }
        return auxNode;
    }

    //TileObject[] AssignNeighbourArray(int row, int column)
    //{
    //    int counter = 0;
    //    TileObject[] neighbors = { };
    //    if (row != -1 && column != -1 && row < numOfRows && column < numOfColumns)
    //    {
    //        TileObject nodeToAdd = nodes[column, row];
    //        neighbors[counter] = nodeToAdd;
    //        counter++;
    //    }
    //    return neighbors;
    //}

    TileObject AssignNeighbour(int row, int column)
    {
        TileObject neighbour = null;
        if (row != -1 && column != -1 && row < numOfRows && column < numOfColumns)
        {
            neighbour = nodes[column, row];
        }
        return neighbour;
    }

    void OnDrawGizmos()
    {
        if (showGrid)
        {
            DebugDrawGrid(transform.position, numOfRows, numOfColumns,
            gridCellSize, Color.blue);
        }

        Gizmos.DrawSphere(transform.position, 0.5f);
    }

    public void DebugDrawGrid(Vector3 origin, int numRows, int numCols, float cellSize, Color color)
    {
        float width = (numCols * cellSize);
        float height = (numRows * cellSize);

        // Draw the horizontal grid lines
        for (int i = 0; i < numRows + 1; i++)
        {
            Vector3 startPos = origin - i * cellSize * new Vector3(0.0f, 1.0f, 0.0f);
            Vector3 endPos = startPos + width * new Vector3(1.0f, 0.0f, 0.0f);
            Debug.DrawLine(startPos, endPos, color);
        }

        // Draw the vertial grid lines
        for (int i = 0; i < numCols + 1; i++)
        {
            Vector3 startPos = origin + i * cellSize * new Vector3(1.0f, 0.0f, 0.0f);
            Vector3 endPos = startPos - height * new Vector3(0.0f, 1.0f, 0.0f);
            Debug.DrawLine(startPos, endPos, color);
        }
    }
}