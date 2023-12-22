using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueueArray<T>
{
    public readonly int size;
    private T[] queue;
    public QueueArray(int size)
    {
        if (size <= 0)
            throw new MadException("No se puede declarar una QueueArray con un tamaño menor que 1");

        this.size = size;
        this.queue = new T[size];
    }

    public T Get(int index)
    {
        if (index >= size)
            throw new MadException("Se ha intentado acceder al índice " + index + " en una cola de tamaño " + size);
        if (index < 0)
            throw new MadException("Se ha intentado acceder a un índice negativo: " + index);

        return queue[index];
    }

    /// <summary>
    /// Desplaza todos los elementos de la cola uno a la derecha metiendo así el item al inicio.
    /// El elemento que esté al final saldrá de la cola.
    /// </summary>
    /// <param name="item">Elemento a meter en la cola</param>
    public void Add(T item)
    {
        Debug.Log(item);
        T aux = queue[0];
        T aux2 = queue[Mathf.Min(2,size-1)];
        Debug.Log(this);
        queue[0] = item;
        for (int i = 1; i < size; i++)
        {
            Debug.Log(this);
            if (i % 2 == 0)
                aux = queue[i];
            else
                aux2 = queue[i];

            queue[i] = i % 2 == 0 ? aux2 : aux;
        }
        Debug.Log(this);
    }

    public override string ToString()
    {
        string result = "[";
        
        for (int i = 0; i < size; i++)
        {
            result += queue[i];
            if (i < size - 1)
                result += ", ";
        }
        result += "]";
        return result;
    }
}
