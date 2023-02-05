using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GusanoBehaviour : MonoBehaviour
{
    [SerializeField] private int tam = 3;
    //[SerializeField] private Transform[] points;
    private int pointsIndex = 0;
    [SerializeField] private float speed = 1f;
    private int sentido = 1;
    //[SerializeField] private Animator anim;
    [SerializeField] private GameObject body, tail;
    private Vector3 initialPos;
    public GameObject pointsPrefab;

    public static Dictionary<GusanoBehaviour,int> listaIndices = new Dictionary<GusanoBehaviour, int>();

    //private GameObject actualBody, actualTail;
    //private TileObject actualHead;

    private int steps = 0;

    private void Start()
    {
        initialPos = transform.position;
        listaIndices[this] = GridManager.instance.GetGridIndex(pointsPrefab.transform.GetChild(pointsIndex).position);
        //points = Instantiate(pointsPrefab, Vector3.zero, Quaternion.identity);
        BeginBehaviour();
    }

    public void BeginBehaviour()
    {
        NextStep();
    }

    private float Duration(Vector3 nextPos)
    {
        return Vector2.Distance(transform.position, nextPos) / (speed > 0f ? speed : 1f);
    }

    private float WhichDirection(Vector3 nextPos)
    {
        Vector2 direction = (nextPos - transform.position).normalized;
        float angle = Vector2.SignedAngle(Vector2.right, direction)+180f;
        //if (angle >= -15f && angle <= 15f)
        //    return Directions.Right;
        //if (angle > 15f && angle <= 75f)
        //    return Directions.RightUp;
        //if (angle > 75f && angle <= 105f)
        //    return Directions.Up;
        //if (angle > 105f && angle <= 165f)
        //    return Directions.LeftUp;
        //if ((angle > 165f && angle <= 180f) || (angle >= -180f && angle <= -165f))
        //    return Directions.Left;
        //if (angle > -165f && angle <= -105f)
        //    return Directions.LeftDown;
        //if (angle > -105f && angle <= -75f)
        //    return Directions.Down;
        //if (angle > -75f && angle <= -15f)
        //    return Directions.RightDown;
        //return Directions.None;

        return angle;
    }

    private IEnumerator DOMove(Vector3 pos)
    {
        yield return null;
    }

    private void NextStep()
    {
        if (steps > 0 && steps < tam-1)
        {
            //actualBody = Instantiate(body, initialPos, Quaternion.identity);
            GusanoBehaviour b = Instantiate(body, initialPos, Quaternion.identity).GetComponent<GusanoBehaviour>();
            b.pointsPrefab = pointsPrefab;
            //b.BeginBehaviour();
        }else if (steps == tam - 1)
        {
            //actualTail = Instantiate(tail, initialPos, Quaternion.identity);
            GusanoBehaviour b = Instantiate(tail, initialPos, Quaternion.identity).GetComponent<GusanoBehaviour>();
            b.pointsPrefab = pointsPrefab;
        }
        int i = GridManager.instance.GetGridIndex(pointsPrefab.transform.GetChild(pointsIndex).position);
        TileObject o = GridManager.instance.nodes[GridManager.instance.GetColumn(i), GridManager.instance.GetRow(i)];
        if (o is Root)
        {
            if (((Root)o).isChikito())
            {
                GridManager.instance.player.useWaterEnergy(9);
                GridManager.instance.virtualCamera.GetComponent<ShakeCamera>().ShakeCameraWrong();
            }
            else
            {
                Debug.Log("OSTIA RAMA");
                sentido *= -1;
                pointsIndex = (pointsIndex + 2 * sentido) % pointsPrefab.transform.childCount;
            }
            
        }
        transform.DOMove(pointsPrefab.transform.GetChild(pointsIndex).position, Duration(pointsPrefab.transform.GetChild(pointsIndex).position)).SetEase(Ease.Linear).OnComplete(NextStep);
        transform.rotation = Quaternion.Euler(0, 0, WhichDirection(pointsPrefab.transform.GetChild(pointsIndex).position));
        //switch (WhichDirection(points[pointsIndex].position))
        //{
        //    case Directions.Right:
        //        anim.SetInteger("index", 1);
        //        break;
        //    case Directions.RightUp:
        //        anim.SetInteger("index", 2);
        //        break;
        //    case Directions.Up:
        //        anim.SetInteger("index", 3);
        //        break;
        //    case Directions.LeftUp:
        //        anim.SetInteger("index", 4);
        //        break;
        //    case Directions.Left:
        //        anim.SetInteger("index", 5);
        //        break;
        //    case Directions.LeftDown:
        //        anim.SetInteger("index", 6);
        //        break;
        //    case Directions.Down:
        //        anim.SetInteger("index", 7);
        //        break;
        //    case Directions.RightDown:
        //        anim.SetInteger("index", 8);
        //        break;
        //    default:
        //        anim.SetInteger("index", 0);
        //        break;
        //}
        listaIndices[this] = GridManager.instance.GetGridIndex(pointsPrefab.transform.GetChild(pointsIndex).position);
        //actualHead = GridManager.instance.nodes[GridManager.instance.GetColumn(thisIndex), GridManager.instance.GetRow(thisIndex)].GetComponent<Bug>();
        //int bodyIndex = GridManager.instance.GetGridIndex(actualBody.transform.position);
        //int tailIndex = GridManager.instance.GetGridIndex(actualTail.transform.position);

        pointsIndex = (pointsIndex + sentido) % pointsPrefab.transform.childCount;
        //Debug.Log("Hemos llegao al punto, siguiente: " + pointsIndex+" y somos "+name);
        steps++;
        // TODO: Decirle al código de jaime que ya no estoy aquí, que estoy allí 
        //GridManager.instance.nodes[GridManager.instance.GetColumn(bodyIndex), GridManager.instance.GetRow(bodyIndex)] = actualTail.GetComponent<Bug>();
        //GridManager.instance.nodes[GridManager.instance.GetColumn(headIndex), GridManager.instance.GetRow(headIndex)] = actualBody.GetComponent<Bug>();
        int futureIndex = GridManager.instance.GetGridIndex(pointsPrefab.transform.GetChild(pointsIndex).position);
        //GridManager.instance.nodes[GridManager.instance.GetColumn(bodyIndex), GridManager.instance.GetRow(tailIndex)] = GridManager.instance.nodes[GridManager.instance.GetColumn(newHeadIndex), GridManager.instance.GetRow(newHeadIndex)];
        //GridManager.instance.nodes[GridManager.instance.GetColumn(thisIndex), GridManager.instance.GetRow(thisIndex)] = Instantiate(GridManager.instance.sand[Random.Range(0,3)], GridManager.instance.nodes[GridManager.instance.GetColumn(thisIndex), GridManager.instance.GetRow(thisIndex)].transform.position, Quaternion.identity).GetComponent<Sand>();
        //GridManager.instance.nodes[GridManager.instance.GetColumn(futureIndex), GridManager.instance.GetRow(futureIndex)] = this.gameObject.GetComponent<Bug>();
    }
}
