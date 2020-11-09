using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor;
using UnityEngine;

public class SetupGrid : MonoBehaviour
{
    public GameObject Hole;
    public float HoleSize;
    public float Gap;
    public int height;
    public int width;
    public List<GameObject> AllHoles { get; private set; }
    public static SetupGrid instance;

    private void Awake()
    {
        instance = this;
        AllHoles = new List<GameObject>();
    }


    public void DrawBoard()
    {
        GameObject temp_hole;

        for (float y = 0; y < height; y++)
        {
            for (float x = 0; x < width; x++)
            {
                temp_hole = Instantiate(this.Hole, new Vector3(x * HoleSize, y * HoleSize, 0), Quaternion.identity);
                temp_hole.transform.parent = gameObject.transform;
                temp_hole.AddComponent<Hole>();
                temp_hole.GetComponent<Hole>().SetXY(x, y);
                AllHoles.Add(temp_hole);
            }
        }
    }
}
