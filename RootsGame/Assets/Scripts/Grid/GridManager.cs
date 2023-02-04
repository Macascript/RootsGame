using UnityEngine;
using System.Collections;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;
using System.Collections.Generic;

public class GridManager : MonoBehaviour
{
    private static GridManager s_Instance = null;

    [SerializeField]
    private GameObject[] sand;

    [SerializeField]
    private GameObject rock;

    [SerializeField]
    private GameObject[] water;

    [SerializeField]
    private GameObject[] water_sin;

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
    }

    private void Update(){}

    void CalculateTiles()
    {
        TextAsset txt = (TextAsset)Resources.Load("chunk", typeof(TextAsset));
        List<string> lines = new List<string>(txt.text.Split('\n'));

        TextAsset txt2 = (TextAsset)Resources.Load("chunk2", typeof(TextAsset));
        List<string> lines2 = new List<string>(txt2.text.Split('\n'));

        TextAsset txt3 = (TextAsset)Resources.Load("chunk3", typeof(TextAsset));
        List<string> lines3 = new List<string>(txt3.text.Split('\n'));

        TextAsset txt4 = (TextAsset)Resources.Load("chunk4", typeof(TextAsset));
        List<string> lines4 = new List<string>(txt4.text.Split('\n'));

        numOfRows = lines.Count + lines2.Count + lines3.Count + lines4.Count;
        numOfColumns = lines[0].Length;
        if (lines[0][lines[0].Length - 1] == '\0') numOfColumns--;

        nodes = new TileObject[numOfColumns, numOfRows];

        loadLevel(lines);
        loadLevel(lines2);
        loadLevel(lines3);
        loadLevel(lines4);
    }

    void loadLevel(List<string> lines)
    {
        Vector3 cellPos = GetGridCellCenter(index);
        TileObject node = new TileObject(cellPos);

        int randomAux = 0;
        for (int i = 0; i < lines.Count; i++)
        {
            //Debug.Log("lineCounter - " + lineCounter);
            for (int j = 0; j < lines[i].Length; j++)
            {
                if (lines[i][j] == '\0') continue;

                cellPos = GetGridCellCenter(index);
                //Debug.Log("j - " + line[j]);
                randomAux = Random.Range(0, 3);

                switch (lines[i][j])
                {
                    case 'A':
                        node = new Sand(cellPos, sand[randomAux]);
                        break;
                    case 'B':
                        node = new Rock(cellPos, sand[randomAux], rock);
                        break;
                    case 'C':
                        node = new Water(cellPos, WaterType.Simple, sand[randomAux], water[0]);
                        break;
                    case 'D':
                        node = new Water(cellPos, WaterType.Double, sand[randomAux], water[1]);
                        break;
                    case 'E':
                        node = new Water(cellPos, WaterType.Max, sand[randomAux], water[2]);
                        break;
                    case 'F':
                        node = new Bug(cellPos);
                        break;
                    case 'G':
                        node = new Food(cellPos);
                        break;
                    case 'H':
                        node = new PowerUp(cellPos);
                        break;
                    default:
                        break;
                }
                nodes[j, lineCounter] = node;
                index++;
            }
            lineCounter++;
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
        int row = (int)(pos.y / gridCellSize);
        return (row * numOfColumns + col);
    }

    public bool IsInBounds(Vector3 pos)
    {
        float width = numOfColumns * gridCellSize;
        float height = numOfRows * gridCellSize;
        return (pos.x >= Origin.x && pos.x <= Origin.x + width &&
        pos.x <= Origin.y + height && pos.y >= Origin.y);
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

    public void GetNeighbours(TileObject node, TileObject[] neighbors)
    {
        Vector3 neighborPos = node.m_position;
        int neighborIndex = GetGridIndex(neighborPos);
        int row = GetRow(neighborIndex);
        int column = GetColumn(neighborIndex);
        //Bottom
        int leftNodeRow = row - 1;
        int leftNodeColumn = column;
        AssignNeighbour(leftNodeRow, leftNodeColumn, neighbors);
        //Top
        leftNodeRow = row + 1;
        leftNodeColumn = column;
        AssignNeighbour(leftNodeRow, leftNodeColumn, neighbors);
        //Diagonal Top Right
        leftNodeRow = row + 1;
        leftNodeColumn = column + 1;
        AssignNeighbour(leftNodeRow, leftNodeColumn, neighbors);
        //Diagonal Top Left
        leftNodeRow = row + 1;
        leftNodeColumn = column - 1;
        AssignNeighbour(leftNodeRow, leftNodeColumn, neighbors);
        //Right
        leftNodeRow = row;
        leftNodeColumn = column + 1;
        AssignNeighbour(leftNodeRow, leftNodeColumn, neighbors);
        //Left
        leftNodeRow = row;
        leftNodeColumn = column - 1;
        AssignNeighbour(leftNodeRow, leftNodeColumn, neighbors);
        //Diagonal Bottom Right
        leftNodeRow = row - 1;
        leftNodeColumn = column + 1;
        AssignNeighbour(leftNodeRow, leftNodeColumn, neighbors);
        //Diagonal Bottom Left
        leftNodeRow = row - 1;
        leftNodeColumn = column - 1;
        AssignNeighbour(leftNodeRow, leftNodeColumn, neighbors);
    }

    public TileObject GetNeighbour(TileObject node, Directions direction)
    {
        Vector3 neighborPos = node.m_position;
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
                AssignNeighbour(leftNodeRow, leftNodeColumn, auxNode);
                break;
            case Directions.Right:
                leftNodeRow = row;
                leftNodeColumn = column + 1;
                AssignNeighbour(leftNodeRow, leftNodeColumn, auxNode);
                break;
            case Directions.Up:
                leftNodeRow = row + 1;
                leftNodeColumn = column;
                AssignNeighbour(leftNodeRow, leftNodeColumn, auxNode);
                break;
            case Directions.Down:
                leftNodeRow = row - 1;
                leftNodeColumn = column;
                AssignNeighbour(leftNodeRow, leftNodeColumn, auxNode);
                break;
            case Directions.LeftDown:
                leftNodeRow = row - 1;
                leftNodeColumn = column - 1;
                AssignNeighbour(leftNodeRow, leftNodeColumn, auxNode);
                break;
            case Directions.RightDown:
                leftNodeRow = row - 1;
                leftNodeColumn = column + 1;
                AssignNeighbour(leftNodeRow, leftNodeColumn, auxNode);
                break;
            case Directions.LeftUp:
                leftNodeRow = row + 1;
                leftNodeColumn = column - 1;
                AssignNeighbour(leftNodeRow, leftNodeColumn, auxNode);
                break;
            case Directions.RightUp:
                leftNodeRow = row + 1;
                leftNodeColumn = column + 1;
                AssignNeighbour(leftNodeRow, leftNodeColumn, auxNode);
                break;
            default:
                break;
        }
        return auxNode;
    }

    void AssignNeighbour(int row, int column, TileObject[] neighbors)
    {
        int counter = 0;
        if (row != -1 && column != -1 && row < numOfRows && column < numOfColumns)
        {
            TileObject nodeToAdd = nodes[row, column];
            neighbors[counter] = nodeToAdd;
            counter++;
        }
    }

    void AssignNeighbour(int row, int column, TileObject neighbour)
    {
        if (row != -1 && column != -1 && row < numOfRows && column < numOfColumns)
        {
            neighbour = nodes[row, column];
        }
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