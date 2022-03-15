using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockManager : MonoBehaviour
{
    public GameObject apeach;
    public GameObject muzi;
    public GameObject neo;
    public GameObject ryan;

    List<Stack<GameObject>> block_obj_stack;

    public void Init()
    {
        block_obj_stack = new List<Stack<GameObject>>();
    }

    public void PushBlock(GameObject obj, EnumBlockType type)
    {
        int idx = EnumClass.EnumBlockToInt(type);

        while(block_obj_stack.Count <= idx)
        {
            block_obj_stack.Add(new Stack<GameObject>());
        }

        obj.SetActive(false);
        block_obj_stack[idx].Push(obj);
    }

    public GameObject PopBlock(EnumBlockType type)
    {
        int idx = EnumClass.EnumBlockToInt(type);

        if(block_obj_stack.Count <= idx || block_obj_stack[idx].Count == 0)
        {
            GameObject newObj = Instantiate(GetBlockObj(type), new Vector3(0, 0, 0), Quaternion.identity);
            newObj.transform.SetParent(this.transform);            
            newObj.AddComponent<Block>();
            newObj.GetComponent<Block>().SetType(type);
            newObj.AddComponent<CircleCollider2D>();
            newObj.GetComponent<CircleCollider2D>().isTrigger = true;
            return newObj;
        }

        GameObject obj = block_obj_stack[idx].Pop();
        obj.SetActive(true);
        return obj;
    }

    private GameObject GetBlockObj(EnumBlockType type)
    {
        GameObject obj = null;
        switch(type)
        {
            case EnumBlockType.Apeach:
                obj = apeach;
                break;
            case EnumBlockType.Muzi:
                obj = muzi;
                break;
            case EnumBlockType.Neo:
                obj = neo;
                break;
            case EnumBlockType.Ryan:
                obj = ryan;
                break;
            default:
                break;
        }

        return obj;
    }
}
