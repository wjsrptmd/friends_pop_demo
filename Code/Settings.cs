using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour
{
    private float unit_x = 0;
    private float unit_y = 0;

    private static Settings instance;
    public static Settings Instance() { return instance; }

    void Awake()
    {
        if (null == instance)
        {
            instance = this;
        }
    }

    void SetUnitXY(float unit_x, float unit_y)
    {
        this.unit_x = unit_x;
        this.unit_y = unit_y;
    }
}
