using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulsatingColor : MonoBehaviour
{

    void Start()
    {

    }

    void Update()
    {
        Color c = Color.Lerp(Color.red, Colors.OrangeCrayola, Mathf.PingPong(Time.time, 1));
        Renderer[] rens = GetComponentsInChildren<Renderer>();
        foreach (Renderer r in rens)
        {
            r.material.color = c;
        }
    }
}
