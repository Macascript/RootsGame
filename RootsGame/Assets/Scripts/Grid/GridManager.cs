using UnityEngine;
using System.Collections;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public class GridManager : MonoBehaviour
{
    private static GridManager s_Instance = null;

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
    private float gridCellSize = 4096;
    public bool showGrid = true;
    private Vector3 origin = new Vector3();
    public TileObject[,] nodes { get; set; }
    public Player player { get; private set; }

    public Vector3 Origin
    {
        get
        {
            return origin;
        }
    }

    private void Awake()
    {
        player = new Player();
        CalculateTiles();
    }

    private void Update(){}

    void CalculateTiles()
    {
        foreach (string line in System.IO.File.ReadLines("Assets/Resources/chunk.txt"))
        {
            numOfColumns = 0;
            for (int j = 0; j < line.Length; j++)
            {
                numOfColumns++;
            }
            numOfRows++;
        }

        nodes = new TileObject[numOfColumns, numOfRows];
        int index = 0;
        Vector3 cellPos = GetGridCellCenter(index);
        TileObject node = new TileObject(cellPos);

        int lineCounter = 0;
        foreach (string line in System.IO.File.ReadLines("Assets/Resources/chunk.txt"))
        {
            Debug.Log("lineCounter - " + lineCounter);
            for (int j = 0; j < line.Length; j++)
            {   
                cellPos = GetGridCellCenter(index);
                Debug.Log("j - " + line[j]);

                switch (line[j])
                {
                    case 'A':
                        node = new Sand(cellPos);
                        break;
                    case 'B':
                        node = new Rock(cellPos);
                        break;
                    case 'C':
                        node = new Water(cellPos, WaterType.Simple);
                        break;
                    case 'D':
                        node = new Water(cellPos, WaterType.Double);
                        break;
                    case 'E':
                        node = new Water(cellPos, WaterType.Max);
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
        cellPosition.y += (gridCellSize / 2.0f);
        return cellPosition;
    }

    public Vector3 GetGridCellPosition(int index)
    {
        int row = GetRow(index);
        int col = GetColumn(index);
        float xPosInGrid = col * gridCellSize;
        float yPosInGrid = row * gridCellSize;
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
            Vector3 startPos = origin + i * cellSize * new Vector3(0.0f, 1.0f, 0.0f);
            Vector3 endPos = startPos + width * new Vector3(1.0f, 0.0f, 0.0f);
            Debug.DrawLine(startPos, endPos, color);
        }

        // Draw the vertial grid lines
        for (int i = 0; i < numCols + 1; i++)
        {
            Vector3 startPos = origin + i * cellSize * new Vector3(1.0f, 0.0f, 0.0f);
            Vector3 endPos = startPos + height * new Vector3(0.0f, 1.0f, 0.0f);
            Debug.DrawLine(startPos, endPos, color);
        }
    }
}