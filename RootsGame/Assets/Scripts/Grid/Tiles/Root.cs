using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Root : TileObject
{
    [SerializeField] private GameObject dust;
    private GameObject actualDust;

    public Root(Vector3 pos, GameObject terminacion)
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
        if (animator == null)
            animator = GetComponent<Animator>();
        animator.SetTrigger("birth");
    }

    public void growAnimation(Directions d)
    {
        if (animator == null)
            animator = GetComponent<Animator>();
        animator.SetTrigger("grow");
        ((Root)nextTileObject).birthAnimation();
    }

    public void instanceDust()
    {
        actualDust = Instantiate(dust, transform);
    }

    public void destroyDust()
    {
        Destroy(actualDust);
    }
}
