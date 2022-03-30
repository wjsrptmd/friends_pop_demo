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

    public bool IsBlockLocated()
    {
        if (block == null) return false;

        return pos == block.Position();
    }

    public void MoveBlock()
    {
        if (block == null) return;

        Vector3 block_pos = block.Position();
        if (block_pos.y < pos.y)
        {
            block.SetPosition(pos);
        }
        else
        {
            Vector3 dir = (pos - block_pos).normalized;
            float speed = Settings.Instance().move_speed;
            block.Translate(dir * speed * Time.deltaTime);
        }
    }

#if UNITY_EDITOR
    public delegate void ClickEnterEvent(Tile tile);
    public static event ClickEnterEvent OnClickEnter;

    public delegate void ClickDownEvent(Tile tile);
    public static event ClickDownEvent OnClickeDown;

    public delegate void ClickUpEvent();
    public static event ClickUpEvent OnClickUp;

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
#endif
}
