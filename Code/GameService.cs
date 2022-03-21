using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameService : MonoBehaviour
{
    private MapManager mapMng;
    private BlockManager blockMng;

    private List<List<Tile>> tiles;
    private List<List<List<int>>> break_count;
    private List<Tile> switch_tiles;
    private List<Tile> top_tiles;

    // 타일 맵은 오른쪽 대각선으로 기울어져 있다.
    private int n = 0; // tile map 세로 크기
    private int m = 0; // tile map 가로 크기

    private int switch_count = 0;
    private int switch_delay = 0;

    private int[] dy = { 1, 0, 1 };
    private int[] dx = { 2, 1, 1 };

    private int dir_count = 3;

    private int random_idx = 0;

    void Start()
    {
        CreateTitle();
        CreateMapManager("MapManager");
        CreateBlockManager("BlockManager");
        CreateSettings("Settings");
        CreateRingObjManager("RingObjManager");
        CreateMissileObjManager("MissileObjManager");
        CreateQuitObj();

        switch_tiles = new List<Tile>();

        tiles = mapMng.CreateTileMap();
        n = tiles.Count;
        m = tiles[0].Count;
        InitPlaceBlocks(tiles);
        InitTopTiles(tiles);

        break_count = new List<List<List<int>>>();
        for (int k = 0; k < dir_count; k++)
        {
            List<List<int>> breaks = new List<List<int>>();
            for (int i = 0; i < n; i++)
            {
                List<int> list = new List<int>();
                for (int j = 0; j < m; j++)
                {
                    list.Add(0);
                }
                breaks.Add(list);
            }
            break_count.Add(breaks);
        }

#if UNITY_EDITOR
        Tile.OnClickEnter += BlockClickEnter;
        Tile.OnClickeDown += BlockClickDown;
        Tile.OnClickUp += BlockClickUp;
#endif
    }

    void Update()
    {
        // 블록을 움직인다.
        if (MoveBlocks() > 0)
        {
            return;
        }

        // 빈 타일을 채운다.
        if (FillEmptyTile() > 0)
        {
            //Debug.Log("FillEmptyTile");
            ClearSwitchBlock();
            return;
        }

        // 스페셜 블록을 폭파 시킨다.
        if (BreakBlock(true) > 0)
        {
            ClearSwitchBlock();
            return;
        }

        // 일반 블록을 폭파 시킨다.
        if (BreakBlock(false) > 0)
        {
            ClearSwitchBlock();
            return;
        }

        // 폭파 후 다음 블록으로 채워준다.        
        if (ChangeToNextBlock() > 0)
        {
            InitBreakCount();
            ClearSwitchBlock();
            return;
        }
        
        // 없앨 수 있는 블록 있는지.
        if (CheckBreakBlocks())
        {
            // 블록 폭파 시작
            StartBreakBlock();
            ClearSwitchBlock();
            return;
        }

#if UNITY_EDITOR
#else
        CheckTouch();
#endif

        if (switch_tiles.Count == 2)
        {
            SwitchTiles();
        }
    }

    void InitPlaceBlocks(List<List<Tile>> tiles)
    {
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < m; j++)
            {
                Tile tile = tiles[i][j];
                if (tile.block_type != EnumBlockType.None)
                {
                    tile.block = blockMng.PopBlock(tile.block_type);
                    tile.block.SetPosition(tile.pos);
                }
            }
        }
    }

    void InitTopTiles(List<List<Tile>> tiles)
    {
        top_tiles = new List<Tile>();

        int y = 0;
        int x = 0;
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < m; j++)
            {
                if (tiles[i][j].block_type == EnumBlockType.None) continue;

                Tile cand_tile = null;
                y = i;
                x = j;
                while(y >= 0 && y < n && x >= 0 && x < m)
                {
                    if (tiles[y][x].block_type != EnumBlockType.None) cand_tile = tiles[y][x];

                    // 세로 방향(위로)
                    y -= dy[0];
                    x -= dx[0];
                }

                if(cand_tile != null)
                {
                    if (!IsTopTile(cand_tile))
                    {
                        top_tiles.Add(cand_tile);
                    }
                }
            }
        }
    }

    bool IsTopTile(Tile tile)
    {
        bool ret = false;
        foreach (Tile top_tile in top_tiles)
        {
            if (top_tile.y == tile.y && top_tile.x == tile.x)
            {
                ret = true;
                break;
            }
        }
        return ret;
    }

    int FillEmptyTile()
    {
        int cnt = 0;

        int ny = 0;
        int nx = 0;
        for(int y = n - 1; y >= 0; y--)
        {
            for(int x = m - 1; x >= 0; x--)
            {
                Tile cur_tile = tiles[y][x];
                if (cur_tile.block_type == EnumBlockType.Empty && !IsTopTile(cur_tile))
                {
                    cnt++;
                    for (int i = 0; i < dir_count; i++)
                    {
                        ny = y - dy[i];
                        nx = x - dx[i];

                        if (ny >= 0 && ny < n && nx >= 0 && nx < m)
                        {
                            Tile next_tile = tiles[ny][nx];
                            if (next_tile.block_type != EnumBlockType.None && next_tile.block.CanMoveBlock())
                            {
                                blockMng.PushBlock(cur_tile.block, cur_tile.block_type);
                                cur_tile.block = next_tile.block;
                                cur_tile.block_type = next_tile.block_type;

                                next_tile.block = blockMng.PopBlock(EnumBlockType.Empty);
                                next_tile.block_type = EnumBlockType.Empty;
                                next_tile.block.SetPosition(next_tile.pos);
                                break;
                            }
                        }
                    }
                }
            }
        }

        foreach(Tile top_tile in top_tiles)
        {
            if (top_tile.block_type == EnumBlockType.Empty)
            {
                cnt++;
                random_idx %= 4;
                random_idx++;
                EnumBlockType type = EnumClass.IntToEnumBlock(random_idx);

                blockMng.PushBlock(top_tile.block, top_tile.block_type);
                top_tile.block = blockMng.PopBlock(type);
                top_tile.block_type = type;
                Vector3 pos = top_tile.pos;
                pos.y += Settings.Instance().offset_create_new_block;
                top_tile.block.SetPosition(pos);
            }
        }
        return cnt;
    }

    int MoveBlocks()
    {
        int cnt = 0;
        foreach(List<Tile> list in tiles)
        {
            foreach(Tile tile in list)
            {
                if (tile.block_type == EnumBlockType.None) continue;

                if(!tile.IsBlockLocated())
                {
                    tile.MoveObj();
                    cnt++;
                }
            }
        }

        return cnt;
    }

    bool SpecialBlockOk(int dir, int y, int x)
    {
        if (break_count[dir][y][x] > 3)
        {
            int ny = y + dy[dir];
            int nx = x + dx[dir];
            if (ny < 0 || ny >= n || nx < 0 || nx >= m) return true;
            if (break_count[dir][y][x] > break_count[dir][ny][nx]) return true;
        }

        return false;
    }

    void StartBreakBlock()
    {
        for(int i = 0; i < n; i++)
        {
            for(int j = 0; j < m; j++)
            {
                Tile tile = tiles[i][j];
                if (tile.block_type == EnumBlockType.None) continue;

                if(tile.block.IsBreakEnd())
                {
                    bool isStartBlock = false;
                    for (int k = 0; k < dir_count; k++)
                    {
                        if (break_count[k][i][j] > 0)
                        {
                            isStartBlock = true;
                        }
                    }

                    if (isStartBlock)
                    {
                        tiles[i][j].block.StartBreak();
                    }
                }
            }
        }
    }

    int BreakBlock(bool isSpecialBlock)
    {
        int cnt = 0;
        for(int i = 0; i < n; i++)
        {
            for(int j = 0; j < m; j++)
            {
                Tile tile = tiles[i][j];
                if (tile.block_type != EnumBlockType.None)
                {
                    Block block = tiles[i][j].block;
                    if (!block.IsBreakEnd())
                    {
                        if (isSpecialBlock)
                        {
                            if (block.IsSpecialBlock())
                            {
                                block.Break(tiles, i, j);
                                cnt++;
                            }
                        }
                        else
                        {
                            block.Break();
                            cnt++;
                        }
                    }
                }
            }
        }

        return cnt;
    }

    int GetMaxCountDir(int y, int x)
    {
        int value = 0;
        int dir = 0;
        for (int k = 0; k < 3; k++)
        {
            if (value < break_count[k][y][x])
            {
                value = break_count[k][y][x];
                dir = k;
            }
        }
        return dir;
    }

    int ChangeToNextBlock()
    {
        int cnt = 0;
        for(int i = 0; i < n; i++)
        {
            for(int j = 0; j < m; j++)
            {
                Tile tile = tiles[i][j];

                if (tile.block_type != EnumBlockType.None &&
                    tile.block.CanChangeNextBlock())
                {
                    EnumBlockType nextType = tile.block.NextBlockType();
                    blockMng.PushBlock(tile.block, tile.block_type);
                    int dir = GetMaxCountDir(i, j);

                    if (SpecialBlockOk(dir, i, j) && !tile.block.IsSpecialBlock())
                    {
                        tile.block = blockMng.PopSpecialBlock(tile.block_type, i, j, dir);
                    }
                    else
                    {
                        tile.block = blockMng.PopBlock(nextType);
                        tile.block_type = nextType;
                    }

                    tile.block.SetPosition(tile.pos);
                    cnt++;
                }
            }
        }

        return cnt;
    }

    bool CheckLineForBreak(int sy, int sx, int dir)
    {
        bool ret = false;
        int cnt = 0;
        int y = sy;
        int x = sx;
        EnumBlockType prev_type = EnumBlockType.None;
        while (y >= 0 && y < n && x >= 0 && x < m)
        {
            Tile tile = tiles[y][x];

            if (tile.block_type == EnumBlockType.None  ||
                tile.block_type == EnumBlockType.Break ||
                tile.block_type == EnumBlockType.Empty ||
                tile.block_type != prev_type)
            {
                cnt = 0;
            }

            cnt++;
            if (cnt >= 3)
            {
                bool isInSelectedTile = false;

                int tmp_y = y;
                int tmp_x = x;
                for (int i = cnt; i >= 1; i--)
                {
                    if(tiles[tmp_y][tmp_x].isSelected)
                    {
                        isInSelectedTile = true;
                        break_count[dir][tmp_y][tmp_x] = cnt;
                    }
                    else
                    {
                        break_count[dir][tmp_y][tmp_x] = 1;
                    }

                    tmp_y -= dy[dir];
                    tmp_x -= dx[dir];
                }


                if (!isInSelectedTile)
                {
                    break_count[dir][y][x] = cnt;
                }

                ret = true;
            }

            prev_type = tile.block_type;
            y += dy[dir];
            x += dx[dir];
        }

        return ret;
    }

    bool CheckBreakBlocks()
    {
        bool ret = false;
        InitBreakCount();

        for(int i = 0; i < n; i++)
        {
            if (CheckLineForBreak(i, 0, 1)) ret = true; // 오른쪽 아래 대각선
            if (CheckLineForBreak(i, 0, 2)) ret = true; // 왼쪽 아래 대각선
        }

        for(int j = 1; j < m; j++)
        {
            if (CheckLineForBreak(0, j, 2)) ret = true; // 왼쪽 아래 대각선
        }

        foreach(Tile top_tile in top_tiles)
        {
            if (CheckLineForBreak(top_tile.y, top_tile.x, 0)) ret = true; // 세로
        }

        return ret;
    }

    void InitBreakCount()
    {
        for(int i = 0; i < n; i++)
        {
            for(int j = 0; j < m; j++)
            {
                for(int k = 0; k < dir_count; k++)
                {
                    break_count[k][i][j] = 0;
                }
            }
        }
    }

    void ClearSwitchBlock()
    {
        switch_count = 0;
        switch_delay = 0;

        foreach(Tile tile in switch_tiles)
        {
            tile.isSelected = false;
        }

        switch_tiles.Clear();
    }

    void CheckTouch()
    {
        if (Input.touchCount > 0) 
        {
            Touch touch = Input.GetTouch(0);
            if(touch.phase == TouchPhase.Moved)
            {
                Vector2 touch_pos = Camera.main.ScreenToWorldPoint(touch.position);
                foreach (List<Tile> list in tiles)
                {
                    foreach (Tile tile in list)
                    {
                        if (tile.block_type != EnumBlockType.None &&
                            tile.GetComponent<CircleCollider2D>().OverlapPoint(touch_pos))
                        {
                            if (touch.phase == TouchPhase.Moved)
                            {
                                BlockClickEnter(tile);
                                return;
                            }
                        }
                    }
                }
            }
        }
    }


    void BlockClickUp()
    {
        if (switch_tiles.Count < 2)
        {
            ClearSwitchBlock();
        }
    }

    void BlockClickDown(Tile tile)
    {
        if (switch_tiles.Count == 0)
        {
            switch_tiles.Add(tile);
        }
    }

    void BlockClickEnter(Tile tile)
    {
        if (switch_tiles.Count == 1)
        {
            Tile prev_tile = switch_tiles[0];
            if(prev_tile.y != tile.y || prev_tile.x != tile.x)
            {
                int dis_y = tile.y - switch_tiles[0].y;
                int dis_x = tile.x - switch_tiles[0].x;

                bool isOk = false;
                for (int i = 0; i < dir_count; i++)
                {
                    if (dis_y * dis_y == dy[i] * dy[i] && dis_x * dis_x == dx[i] * dx[i])
                    {
                        isOk = true;
                    }
                }

                if (isOk)
                {
                    switch_tiles.Add(tile);
                }
                else
                {
                    switch_count = 0;
                    switch_delay = 0;
                    switch_tiles.Clear();
                }
            }
        }

#if UNITY_EDITOR
#else
        if (switch_tiles.Count == 0)
        {
            BlockClickDown(tile);
        }
#endif
    }

    void SwitchTiles()
    {
        if(switch_tiles.Count != 2)
        {
            Debug.LogError("Switch count is not 2");
            return;
        }

        bool isSwitching = false;
        foreach(Tile tile in switch_tiles)
        {
            if (!tile.IsBlockLocated()) isSwitching = true;
        }

        if (!isSwitching)
        {
            // 한번 switch 후 딜레이를 주기 위함.
            if(switch_count == 1)
            {
                switch_delay++;
                if (switch_delay < Settings.Instance().switch_delay) return;
            }

            Block block = switch_tiles[0].block;
            switch_tiles[0].block = switch_tiles[1].block;
            switch_tiles[1].block = block;

            EnumBlockType type = switch_tiles[0].block_type;
            switch_tiles[0].block_type = switch_tiles[1].block_type;
            switch_tiles[1].block_type = type;

            switch_count++;
            if (switch_count == 1)
            {
                int special = 0;
                foreach (Tile tile in switch_tiles)
                {
                    tile.isSelected = true;
                    if (tile.block.IsSpecialBlock())
                    {
                        special++;
                    }
                }

                // 스페셜 블록끼리 스위치 됨.
                if (special == 2)
                {
                    foreach (Tile tile in switch_tiles)
                    {
                        tile.block.StartBreak();
                    }
                }
            }
            else
            {
                ClearSwitchBlock();
            }
        }        
    }

    void CreateTitle()
    {
        GameObject obj = Resources.Load("title_obj") as GameObject;
        GameObject titleObj = Instantiate(obj, new Vector3(0, 0, 0), Quaternion.identity);
        titleObj.transform.SetParent(this.transform);
        titleObj.transform.position = new Vector3(-0.2f, 3.5f, 0);
    }

    void CreateMapManager(string name)
    {
        GameObject mapMngObj = new GameObject();
        mapMngObj.name = name;
        mapMngObj.transform.SetParent(this.transform);
        mapMng = mapMngObj.AddComponent<MapManager>();
        mapMng.Init();
    }

    void CreateBlockManager(string name)
    {
        GameObject blockMngObj = new GameObject();
        blockMngObj.name = name;
        blockMngObj.transform.SetParent(this.transform);
        blockMng = blockMngObj.AddComponent<BlockManager>();
        blockMng.Init(dy, dx);
    }

    void CreateSettings(string name)
    {
        GameObject Settings = new GameObject();
        Settings.name = name;
        Settings.transform.SetParent(this.transform);
        Settings.AddComponent<Settings>();
    }

    void CreateRingObjManager(string name)
    {
        GameObject ringObjMng = new GameObject();
        ringObjMng.name = name;
        ringObjMng.transform.SetParent(this.transform);
        ringObjMng.AddComponent<RingObjManager>();
    }

    void CreateMissileObjManager(string name)
    {
        GameObject missileObjManager = new GameObject();
        missileObjManager.name = name;
        missileObjManager.transform.SetParent(this.transform);
        missileObjManager.AddComponent<MissileObjManager>();
    }

    void CreateQuitObj()
    {
        GameObject obj = Resources.Load("quit_obj") as GameObject;
        GameObject quitObj = Instantiate(obj, new Vector3(0, 0, 0), Quaternion.identity);
        quitObj.transform.SetParent(this.transform);
        quitObj.transform.position = new Vector3(1.8f, 3.5f, 0);
    }

}