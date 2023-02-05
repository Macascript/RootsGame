using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArbolNivel : MonoBehaviour
{
    [SerializeField]
    private Sprite[] arboles;

    [SerializeField]
    private Sprite sol;

    [SerializeField]
    private GameObject solAnim;

    [SerializeField]
    private AudioSource m_source;

    private int index = 0;
    private bool levelUp = true;
    private bool levelUp2 = true;

    private void Update()
    {
        if (GridManager.instance.GetGridIndex(GridManager.instance.player.transform.position) > 161 && levelUp)
        {
            levelUp = false;
            index = 1;
            m_source.Play();
            StartCoroutine(changeLevel());
        }    
        if (GridManager.instance.GetGridIndex(GridManager.instance.player.transform.position) > 363 && levelUp2)
        {
            levelUp2 = false;
            index = 2;
            m_source.Play();
            StartCoroutine(changeLevel());
        }      
    }

    private IEnumerator changeLevel()
    {
        //sol sin animacion
        //GetComponent<Image>().sprite = sol;
        //ANIMACION SOL
        GameObject solAux = Instantiate(solAnim, this.transform.position, Quaternion.identity);
        yield return new WaitForSeconds(5);
        Destroy(solAux);
        GetComponent<Image>().sprite = arboles[index];
        Destroy(GridManager.instance.brote);
        GridManager.instance.brote = Instantiate(GridManager.instance.tallos[index], GridManager.instance.nodes[4, 0].transform.position + Vector3.up * 0.64f, Quaternion.identity);
    }
}
