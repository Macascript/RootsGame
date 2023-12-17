using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarFog : MonoBehaviour
{
    private float yDistance;
    private float xDistance;
    private void Start()
    {
        yDistance = Vector2.Distance(GridManager.instance.nodes[0, 0].transform.position, GridManager.instance.nodes[0, 1].transform.position);
        //xDistance = Vector2.Distance(GridManager.instance.nodes[1, 0].transform.position, GridManager.instance.nodes[0, 0].transform.position);
    }
    public void NotifyLayerChange()
    {
        transform.position -= Vector3.down * yDistance;
        //transform.position -= Vector3.right * xDistance;
    }
}
