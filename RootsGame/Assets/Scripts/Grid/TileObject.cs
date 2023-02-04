using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileObject : MonoBehaviour
{
    public Vector3 m_position;
    public TileObject nextTileObject;
    public Animator animator;

    public TileObject(Vector3 pos)
    {
        m_position = pos;
    }

    public TileObject(){}

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual bool onStep()
    {
        return true;
    }
}
