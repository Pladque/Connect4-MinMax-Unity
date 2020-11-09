using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Hole : MonoBehaviour
{
    [HideInInspector]
    public int X;
    [HideInInspector]
    public int Y;


    public void SetXY(float x, float y)
    {
        X = (int) x;
        Y = (int) y;
    }
}
