using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialBlock : Block
{
    public int dy = 0;
    public int dx = 0;
    public int dir = 0;

    private Vector3 ring_max_scale = new Vector3(0.35f, 0.35f, 0);
    private Vector3 ring_min_scale = new Vector3(0, 0, 0);
    private Vector3 d_scale = new Vector3(0.01f, 0.01f, 0);

    private GameObject ring_obj = null;
    private GameObject missile_obj1 = null;
    private GameObject missile_obj2 = null;
    private Color color;

    private bool first_break = false;

    public void Init(int dy, int dx, int dir, Color color)
    {
        break_delay = Settings.Instance().break_delay;
        this.dy = dy;
        this.dx = dx;
        this.dir = dir;
        this.color = color;
        is_break = false;
        first_break = false;
    }

    public override bool IsBreakEnd()
    {
        return ring_obj == null && missile_obj1 == null && missile_obj2 == null;
    }

    bool MoveMissile(GameObject obj, List<List<Tile>> tiles, int sy, int sx, int dy, int dx)
    {
        bool ret = false;
        int n = tiles.Count;
        int m = tiles[0].Count;
        int y = sy;
        int x = sx;

        Vector3 target_pos = tiles[sy][sx].pos;
        while (y >= 0 && y < n && x >= 0 && x  < m)
        {
            if(tiles[y][x].block_type != EnumBlockType.None)
            {
                target_pos = tiles[y][x].pos;
            }

            y += dy;
            x += dx;
        }

        Vector3 cur_diff = obj.transform.position - tiles[sy][sx].pos;
        Vector3 target_diff = target_pos - tiles[sy][sx].pos;

        if(cur_diff.sqrMagnitude < target_diff.sqrMagnitude)
        {
            ret = true;
            obj.transform.Translate(target_diff.normalized * Settings.Instance().missile_speed * Time.deltaTime);
        }

        return ret;
    }

    public override void Break(List<List<Tile>> tiles, int y, int x)
    {
        if(ring_obj == null || missile_obj1 == null || missile_obj2 == null)
        {
            Debug.LogError("obj is null");
            return;
        }

        if (first_break)
        {
            ring_obj.SetActive(true);
            ring_obj.transform.position = tiles[y][x].pos;

            missile_obj1.SetActive(true);
            missile_obj1.transform.position = tiles[y][x].pos;

            missile_obj2.SetActive(true);
            missile_obj2.transform.position = tiles[y][x].pos;

            first_break = false;
        }

        bool isBreaking = false;
        // ¾ÆÁ÷ ÆøÆÄ Áß
        if (ring_obj.transform.localScale.sqrMagnitude < ring_max_scale.sqrMagnitude)
        {
            ring_obj.transform.localScale += d_scale;
            isBreaking = true;
        }
        else
        {
            ring_obj.SetActive(false);
        }

        if(MoveMissile(missile_obj1, tiles, y, x, dy, dx))
        {
            isBreaking = true;
        }
        else
        {
            missile_obj1.SetActive(false);
        }

        if (MoveMissile(missile_obj2, tiles, y, x, -dy, -dx))
        {
            isBreaking = true;
        }
        else
        {
            missile_obj2.SetActive(false);
        }

        if (!isBreaking)
        {
            BreakLine(tiles, y, x, dy, dx);
            BreakLine(tiles, y, x, -dy, -dx);

            RingObjManager.Instance().PushObj(ring_obj);
            ring_obj = null;

            MissileObjManager.Instance().PushObj(missile_obj1, dir);
            missile_obj1 = null;

            MissileObjManager.Instance().PushObj(missile_obj2, dir);
            missile_obj2 = null;
        }
    }

    private void BreakLine(List<List<Tile>> tiles, int sy, int sx, int dy, int dx)
    {
        int y = sy;
        int x = sx;
        int n = tiles.Count;
        int m = tiles[0].Count;

        while (true)
        {
            y += dy;
            x += dx;
            if (y < 0 || y >= n || x < 0 || x >= m) break;
            
            Tile tile = tiles[y][x];
            if (tile.block_type != EnumBlockType.None)
            {
                if(!tile.block.is_break)
                {
                    tile.block.StartBreak();
                }
            }
        }
    }

    public override void StartBreak()
    {
        ring_obj = RingObjManager.Instance().PopObj();
        ring_obj.SetActive(false);
        ring_obj.GetComponent<SpriteRenderer>().color = color;
        ring_obj.transform.localScale = ring_min_scale;

        missile_obj1 = MissileObjManager.Instance().PopObj(dir);
        missile_obj1.SetActive(false);
        missile_obj1.GetComponent<SpriteRenderer>().color = color;

        missile_obj2 = MissileObjManager.Instance().PopObj(dir);
        missile_obj2.SetActive(false);
        missile_obj2.GetComponent<SpriteRenderer>().color = color;

        is_break = true;
        first_break = true;
    }

    public override EnumBlockType NextBlockType()
    {
        return EnumBlockType.Break;
    }

    public override bool CanMoveBlock()
    {
        return true;
    }

    public override bool IsSpecialBlock()
    {
        return true;
    }
}
