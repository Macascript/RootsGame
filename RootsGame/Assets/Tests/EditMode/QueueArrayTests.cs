using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class QueueArrayTests
{

    [Test]
    public void Add()
    {
        QueueArray<Directions> cola = new QueueArray<Directions>(3);
        cola.Add(Directions.LeftDown);
        cola.Add(Directions.Left);
        cola.Add(Directions.LeftUp);
        Assert.AreEqual(Directions.LeftUp, cola.Get(0));
        Assert.AreEqual(Directions.Left, cola.Get(1));
        Assert.AreEqual(Directions.LeftDown, cola.Get(2));

        cola.Add(Directions.Up);
        Assert.AreEqual(Directions.Up, cola.Get(0));
        Assert.AreEqual(Directions.LeftUp, cola.Get(1));
        Assert.AreEqual(Directions.Left, cola.Get(2));
    }
}
