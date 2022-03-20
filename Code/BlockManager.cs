using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockManager : MonoBehaviour
{
    private GameObject apeach;
    private GameObject muzi;
    private GameObject neo;
    private GameObject ryan;
    private GameObject breakBlock;
    private GameObject empty;

    private GameObject special_block1;
    private GameObject special_block2;
    private GameObject special_block3;

    List<Stack<Block>> block_stack;
    List<Stack<SpecialBlock>> special_block_stack;

    private int[] dy;
    private int[] dx;

    public void Init(int[] dy, int[] dx)
    {
        block_stack = new List<Stack<Block>>();
        special_block_stack = new List<Stack<SpecialBlock>>();
        this.dy = dy;
        this.dx = dx;

        apeach = Resources.Load("apeach") as GameObject;
        muzi = Resources.Load("muzi") as GameObject;
        neo = Resources.Load("neo1") as GameObject;
        ryan = Resources.Load("ryan") as GameObject;
        breakBlock = Resources.Load("breakBlock") as GameObject;
        empty = Resources.Load("empty") as GameObject;

        special_block1 = Resources.Load("special_block1") as GameObject;
        special_block2 = Resources.Load("special_block2") as GameObject;
        special_block3 = Resources.Load("special_block3") as GameObject;
    }

    public void PushBlock(Block block, EnumBlockType type)
    {
        if(block.IsSpecialBlock())
        {
            PushSpecialBlock((SpecialBlock)block);
        }
        else
        {
            int idx = EnumClass.EnumBlockToInt(type);

            while (block_stack.Count <= idx)
            {
                block_stack.Add(new Stack<Block>());
            }

            block.SetActive(false);
            block_stack[idx].Push(block);
        }
    }

    public Block PopBlock(EnumBlockType type)
    {
        int idx = EnumClass.EnumBlockToInt(type);

        Block block = null;
        if(block_stack.Count <= idx || block_stack[idx].Count == 0)
        {
            GameObject newObj = Instantiate(GetBlockObj(type), new Vector3(0, 0, 0), Quaternion.identity);
            newObj.transform.SetParent(this.transform);
            block = CreateNewBlock(type);
            block.SetObj(newObj);
        }
        else
        {
            block = block_stack[idx].Pop();
        }

        block.Init();
        block.SetActive(true);
        return block;
    }

    private void PushSpecialBlock(SpecialBlock block)
    {
        block.SetActive(false);

        while(special_block_stack.Count <= block.dir)
        {
            special_block_stack.Add(new Stack<SpecialBlock>());
        }

        special_block_stack[block.dir].Push(block);
    }

    public SpecialBlock PopSpecialBlock(EnumBlockType block_type, int i, int j, int dir)
    {
        SpecialBlock block = null;

        if (special_block_stack.Count <= dir || special_block_stack[dir].Count == 0)
        {
            GameObject newObj = Instantiate(GetSpecialBlockObj(dir), new Vector3(0, 0, 0), Quaternion.identity);
            newObj.transform.SetParent(this.transform);
            block = gameObject.AddComponent<SpecialBlock>();
            block.SetObj(newObj);
        }
        else
        {
            block = special_block_stack[dir].Pop();
        }

        GameObject obj = block.GetObj();
        Color color = Util.GetColor(block_type);
        obj.GetComponent<SpriteRenderer>().color = color;
        block.Init(dy[dir], dx[dir], dir, color);
        block.SetActive(true);

        return block;
    }

    private GameObject GetSpecialBlockObj(int dir)
    {
        GameObject obj = null;
        switch (dir)
        {
            case 0:
                obj = special_block1;
                break;
            case 1:
                obj = special_block2;
                break;
            case 2:
                obj = special_block3;
                break;
            default:
                break;
        }
        return obj;
    }

    void SettColorSpecialBlockObj(GameObject obj, EnumBlockType block_type)
    {
        Color color = new Color();
        switch (block_type)
        {
            case EnumBlockType.Apeach:
                ColorUtility.TryParseHtmlString("#F4C1C0", out color);
                break;
            case EnumBlockType.Muzi:
                ColorUtility.TryParseHtmlString("#FED300", out color);
                break;
            case EnumBlockType.Neo:
                ColorUtility.TryParseHtmlString("#7392B2", out color);
                break;
            case EnumBlockType.Ryan:
                ColorUtility.TryParseHtmlString("#DB9A27", out color);
                break;
            default:
                break;
        }
        obj.GetComponent<SpriteRenderer>().color = color;
    }

    private Block CreateNewBlock(EnumBlockType type)
    {
        Block ret = null;

        switch (type)
        {
            case EnumBlockType.Apeach:
            case EnumBlockType.Muzi:
            case EnumBlockType.Neo:
            case EnumBlockType.Ryan:
                ret = gameObject.AddComponent<Block>();
                break;
            case EnumBlockType.Break:
                ret = gameObject.AddComponent<BreakBlock>();
                break;
            case EnumBlockType.Empty:
                ret = gameObject.AddComponent<Block>();
                break;
            default:
                break;
        }

        return ret;
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
            case EnumBlockType.Break:
                obj = breakBlock;
                break;
            case EnumBlockType.Empty:
                obj = empty;
                break;
            default:
                break;
        }

        return obj;
    }
}
