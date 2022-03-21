using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileObjManager : MonoBehaviour
{
    public GameObject missile_1;
    public GameObject missile_2;
    public GameObject missile_3;
    private List<Stack<GameObject>> s;

    private static MissileObjManager instance;
    public static MissileObjManager Instance() { return instance; }

    void Start()
    {
        s = new List<Stack<GameObject>>();
        missile_1 = Resources.Load("missile_1") as GameObject;
        missile_2 = Resources.Load("missile_2") as GameObject;
        missile_3 = Resources.Load("missile_3") as GameObject;
    }

    void Awake()
    {
        if (null == instance)
        {
            instance = this;
        }
    }

    public void PushObj(GameObject ring_obj, int idx)
    {
        ring_obj.SetActive(false);
        while(s.Count <= idx)
        {
            s.Add(new Stack<GameObject>());
        }
        s[idx].Push(ring_obj);
    }

    public GameObject PopObj(int idx)
    {
        GameObject obj = null;
        if (s.Count <= idx || s[idx].Count == 0)
        {
            obj = Instantiate(GetObj(idx), new Vector3(0, 0, 0), Quaternion.identity);
            obj.transform.SetParent(this.transform);
        }
        else
        {
            obj = s[idx].Pop();
        }

        return obj;
    }

    private GameObject GetObj(int idx)
    {
        GameObject obj = null;
        switch (idx)
        {
            case 0:
                obj = missile_1;
                break;
            case 1:
                obj = missile_2;
                break;
            case 2:
                obj = missile_3;
                break;
            default:
                break;
        }

        return obj;
    }
}
