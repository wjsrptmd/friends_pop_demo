using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingObjManager : MonoBehaviour
{
    private GameObject obj;
    private Stack<GameObject> s;

    private static RingObjManager instance;
    public static RingObjManager Instance() { return instance; }

    void Start()
    {
        s = new Stack<GameObject>();
        obj = Resources.Load("ring") as GameObject;
    }

    void Awake()
    {
        if (null == instance)
        {
            instance = this;
        }
    }

    public void PushObj(GameObject ring_obj)
    {
        ring_obj.SetActive(false);
        s.Push(ring_obj);
    }

    public GameObject PopObj()
    {
        GameObject ring_obj = null;
        if(s.Count == 0)
        {
            ring_obj = Instantiate(obj, new Vector3(0, 0, 0), Quaternion.identity);
            ring_obj.transform.SetParent(this.transform);
        }
        else
        {
            ring_obj = s.Pop();
        }

        return ring_obj;
    }
}
