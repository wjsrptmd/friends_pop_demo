using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour
{
    public float offset_create_new_block = 0.5f;
    public float offset_start_x = -2.2f;
    public float offset_start_y = -1.8f;
    public float move_speed = 10f;
    public int switch_delay = 20;
    public int break_delay = 20;
    public float missile_speed = 10f;

    private static Settings instance;
    public static Settings Instance() { return instance; }

    void Awake()
    {
        if (null == instance)
        {
            instance = this;
        }
    }
}
