using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Root : TileObject
{
    [SerializeField] private GameObject dust;
    private GameObject actualDust;
    [SerializeField] private GameObject piquitoLeftDownArriba, piquitoLeftDownAbajo,piquitoRightDownArriba,piquitoRightDownAbajo;
    private bool isChiquito = true;

    //public Root(Vector3 pos, GameObject terminacion)
    //{
    //    transform.position = pos;
    //    Instantiate(terminacion, this.transform.position, Quaternion.identity).AddComponent<Root>();
    //}

    [SerializeField]
    private AudioSource m_Source;

    public override bool canStep()
    {
        return false;
    }

    public override bool onStep()
    {
        GridManager.instance.virtualCamera.GetComponent<ShakeCamera>().ShakeCameraWrong();
        m_Source.Play();
        return false;
    }

    public void birthAnimation()
    {
        if (animator == null)
            animator = GetComponent<Animator>();
        animator.SetTrigger("birth");
        instanceDust();
        Invoke("destroyDust", 0.1f);
    }

    public void growAnimation(Directions d)
    {
        if (animator == null)
            animator = GetComponent<Animator>();
        animator.SetInteger("direction", (int)d);
        animator.SetTrigger("grow");
        ((Root)nextTileObject).birthAnimation();
        instanceDust();
        Invoke("destroyDust", 0.1f);
        if (d == Directions.LeftDown)
        {
            Instantiate(piquitoLeftDownArriba,transform.position + Vector3.left * 0.64f,Quaternion.identity); // izquierda
            Instantiate(piquitoLeftDownAbajo,transform.position + Vector3.down * 0.64f, Quaternion.identity); // abajo
        }else if (d == Directions.RightDown)
        {
            Instantiate(piquitoRightDownArriba,transform.position + Vector3.right * 0.64f,Quaternion.identity); //derecha
            Instantiate(piquitoRightDownAbajo,transform.position + Vector3.down * 0.64f,Quaternion.identity); //abajo
        }
        isChiquito = false;
    }

    public void instanceDust()
    {
        actualDust = Instantiate(dust, transform);
    }

    public void destroyDust()
    {
        Destroy(actualDust);
    }

    public bool isChikito()
    {
        return isChiquito;
    }
}
