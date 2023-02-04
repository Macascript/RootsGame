using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarFog : MonoBehaviour
{
    private float yDistance;
    private void Start()
    {
        yDistance = Vector2.Distance(GridManager.instance.nodes[0, 0].m_position, GridManager.instance.nodes[0, 1].m_position);
    }
    public void NotifyLayerChange()
    {
        transform.position -= Vector3.down * yDistance;
    }
}
