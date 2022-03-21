using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitMonitor : MonoBehaviour
{
    private void Update()
    {
        if(Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }

#if UNITY_EDITOR
#else
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                Vector2 touch_pos = Camera.main.ScreenToWorldPoint(touch.position);
                if (GetComponent<BoxCollider2D>().OverlapPoint(touch_pos))
                {
                    Application.Quit();
                }
            }
        }
#endif
    }

#if UNITY_EDITOR
    private void OnMouseDown()
    {
        Application.Quit();
        Debug.Log("Quit !!");
    }
#endif
}
