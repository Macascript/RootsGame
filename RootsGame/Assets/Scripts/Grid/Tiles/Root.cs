using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Root : TileObject
{
    private Animator anim;
    public Root(Vector3 pos)
    {
        m_position = pos;
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
