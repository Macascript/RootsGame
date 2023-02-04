using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Root : TileObject
{
<<<<<<< HEAD
    public Root(Vector3 pos, GameObject terminacion)
=======
    private Animator anim;
    public Root(Vector3 pos)
>>>>>>> 32b1479a61e92e7b4cefd57b5ca98b0070e7dce5
    {
        m_position = pos;
        Instantiate(terminacion, this.m_position, Quaternion.identity);
    }

    public override bool canStep()
    {
        return false;
    }

    public override bool onStep()
    {
        return false;
    }

    public void birthAnimation()
    {
        if (anim == null)
            anim = GetComponent<Animator>();
        anim.SetTrigger("birth");
    }

    public void growAnimation()
    {
        if (anim == null)
            anim = GetComponent<Animator>();
        anim.SetTrigger("grow");
    }
}
