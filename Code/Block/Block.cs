using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    protected GameObject obj;
    protected int break_delay = 0;
    protected int break_count = 0;
    public bool is_break = false;

    public GameObject GetObj()
    {
        return obj;
    }

    public virtual void Init()
    {
        break_delay = Settings.Instance().break_delay;
        break_count = break_delay;
        is_break = false;
    }

    public virtual bool IsBreakEnd()
    {
        return break_count >= break_delay;
    }

    public virtual void Break()
    {   
        break_count++;
    }

    public virtual void Break(List<List<Tile>> tiles, int y, int x)
    {
        ;
    }

    public virtual void StartBreak()
    {
        is_break = true;
        break_count = 0;
    }

    public virtual bool CanChangeNextBlock()
    {
        return is_break && IsBreakEnd();
    }

    public virtual EnumBlockType NextBlockType()
    {
        return EnumBlockType.Break;
    }

    public virtual bool CanMoveBlock()
    {
        return true;
    }

    public virtual bool IsSpecialBlock()
    {
        return false;
    }

    public void SetActive(bool value)
    {
        obj.SetActive(value);
    }

    public void SetObj(GameObject obj)
    {
        this.obj = obj;
    }

    public Vector3 Position()
    {
        return obj.transform.position;
    }

    public void SetPosition(Vector3 pos)
    {
        obj.transform.position = pos;
    }

    public void Translate(Vector3 translation)
    {
        obj.transform.Translate(translation);
    }

    public void SetBreakDelay(int delay)
    {
        break_delay = delay;
    }
}