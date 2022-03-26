using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileObjManager : MonoBehaviour
{
    private GameObject missile_1;
    private GameObject missile_2;
    private GameObject missile_3;
    private List<Stack<GameObject>> s;

    private static MissileObjManager instance;
    public static MissileObjManager Instance() { return instance; }

    void Start()
    {
        s = new List<Stack<GameObject>>();
        missile_1 = Util.CreateObjForPng("missile1", new Vector3(0.08f, 0.08f, 0));
        missile_2 = Util.CreateObjForPng("missile2", new Vector3(0.08f, 0.08f, 0));
        missile_3 = Util.CreateObjForPng("missile3", new Vector3(0.08f, 0.08f, 0));

        missile_1.GetComponent<SpriteRenderer>().sortingOrder = 1;
        missile_2.GetComponent<SpriteRenderer>().sortingOrder = 1;
        missile_3.GetComponent<SpriteRenderer>().sortingOrder = 1;
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
