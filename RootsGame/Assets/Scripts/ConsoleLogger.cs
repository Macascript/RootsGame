using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ConsoleLogger : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI output;

    private int logCount;
    public void Log(string msg)
    {
        Debug.Log(msg);
        output.text = msg+" "+logCount;
        logCount++;
    }
}
