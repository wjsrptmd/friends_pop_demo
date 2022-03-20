using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public Vector3 pos;
    public EnumBlockType block_type;
    public Block block;
    public bool isSelected = false;

    public int y = 0;
    public int x = 0;

    public delegate void ClickEnterEvent(Tile tile);
    public static event ClickEnterEvent OnClickEnter;

    public delegate void ClickDownEvent(Tile tile);
    public static event ClickDownEvent OnClickeDown;

    public delegate void ClickUpEvent();
    public static event ClickUpEvent OnClickUp;

    public bool IsBlockLocated()
    {
        if (block == null) return false;

        return pos == block.Position();
    }

    public void MoveObj()
    {
        if (block == null) return;

        Vector3 obj_pos = block.Position();
        if (obj_pos.y < pos.y)
        {
            block.SetPosition(pos);
        }
        else
        {
            Vector3 dir = (pos - obj_pos).normalized;
            float speed = Settings.Instance().move_speed;
            block.Translate(dir * speed * Time.deltaTime);
        }
    }

    private void OnMouseDown()
    {
        OnClickeDown(this);
    }

    private void OnMouseEnter()
    {
        OnClickEnter(this);
    }

    private void OnMouseUp()
    {
        OnClickUp();
    }
}