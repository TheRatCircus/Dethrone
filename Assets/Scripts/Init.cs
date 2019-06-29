using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Init : MonoBehaviour
{
    [RuntimeInitializeOnLoadMethod]
    void PrintDebug()
    {
        Debug.Log("This script ran on init.");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
