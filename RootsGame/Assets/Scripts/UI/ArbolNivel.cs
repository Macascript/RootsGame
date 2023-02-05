using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArbolNivel : MonoBehaviour
{
    [SerializeField]
    private Sprite[] arboles;

    [SerializeField]
    private AudioSource m_source;

    private int index = 0;
    private bool levelUp = false;

    private void Update()
    {
        if (GridManager.instance.GetGridIndex(GridManager.instance.player.transform.position) > 161 && levelUp)
        {
            levelUp = true;
            index = 1;
            if(levelUp)
            m_source.Play();
            StartCoroutine(changeLevel());
        }    
        if (GridManager.instance.GetGridIndex(GridManager.instance.player.transform.position) > 363)
        {
            index = 2;
            m_source.Play();
        }      
    }

    private IEnumerator changeLevel()
    {
        //sol
        yield return new WaitForSeconds(5);
        GetComponent<Image>().sprite = arboles[index];
    }
}
