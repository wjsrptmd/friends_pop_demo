using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    private EnumBlockType type = EnumBlockType.None;
    private int y = 0;
    private int x = 0;

    public delegate void ClickEnterEvent(Block block);
    public static event ClickEnterEvent OnEnter;

    public delegate void ClickExitEvent(Block block);
    public static event ClickExitEvent OnDrag;

    public virtual EnumBlockType GetType()
    {
        return type;
    }

    public virtual void SetType(EnumBlockType type)
    {
        this.type = type;
    }

    public virtual void SetPosition(int y, int x)
    {
        this.y = y;
        this.x = x;
    }

    public virtual int GetX()
    {
        return x;
    }

    public virtual int GetY()
    {
        return y;
    }

    public virtual bool CanMove()
    {
        return type != EnumBlockType.None;
    }

    private void OnMouseDrag()
    {
        OnDrag(this);
    }

    private void OnMouseEnter()
    {
        OnEnter(this);
    }
}
