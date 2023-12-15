using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class MoverData : ScriptableObject
{
    [SerializeField]
    private Sprite to_abajo;
    public Sprite ToAbajo { get { return to_abajo; } }

    [SerializeField]
    private Sprite to_arriba;
    public Sprite ToArriba { get {  return to_arriba; } }

    [SerializeField]
    private Sprite to_izquierda;
    public Sprite ToIzquierda { get {  return to_izquierda; } }

    [SerializeField]
    private Sprite to_derecha;
    public Sprite ToDerecha { get { return to_derecha; } }

    [SerializeField]
    private Sprite to_abajo_derecha;
    public Sprite ToAbajoDerecha { get { return to_abajo_derecha; } }

    [SerializeField]
    private Sprite to_abajo_izquierda;
    public Sprite ToAbajoIzquierda { get { return ToAbajoDerecha; } }

    [SerializeField]
    private Sprite to_arriba_derecha;
    public Sprite ToArribaDerecha { get { return ToArribaDerecha; } }

    [SerializeField]
    private Sprite to_arriba_izquierda;
    public Sprite ToArribaIzquierda { get { return ToArribaDerecha; } }
}
