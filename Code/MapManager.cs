using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public GameObject tile_unit;
    public float tile_offset_x = -0.2f;
    public float tile_offset_y = 0.02f;


    public List<List<Tile>> CreateTileMap(string file_path)
    {
        List<List<Tile>> tiles = new List<List<Tile>>();

        try
        {
            string[] map_data = System.IO.File.ReadAllLines(file_path);
            if(map_data.Length == 0)
            {
                Debug.LogError(String.Format("{0} is Empty", file_path));
            }
            else
            {
                int x_size = map_data[0].Length;
                int y_size = map_data.Length;

                tiles.Clear();

                Bounds bounds = tile_unit.GetComponent<SpriteRenderer>().bounds;
                float unit_x = bounds.size.x + tile_offset_x;
                float unit_y = bounds.size.y + tile_offset_y;

                float start_x = (x_size - 1) * (unit_x / 2) + Settings.Instance().offset_start_x;
                float start_y = (y_size - 1) * (unit_y / 2) + Settings.Instance().offset_start_y;

                float cur_x = 0;
                float cur_y = 0;

                for (int i = 0; i < y_size; i++)
                {
                    cur_y = start_y;
                    cur_x = start_x - (unit_x * 2 * i);

                    List<Tile> list = new List<Tile>();
                    for (int j = 0; j < x_size; j++)
                    {
                        int type = Convert.ToInt32(map_data[i][j] - '0');
                        EnumBlockType block_type = EnumClass.IntToEnumBlock(type);
                        Vector3 pos = new Vector3(cur_x, cur_y, 0);
                        Tile tile;
                        if (block_type != EnumBlockType.None)
                        {
                            GameObject obj = Instantiate(tile_unit, pos, Quaternion.identity);
                            obj.AddComponent<Tile>();
                            tile = obj.GetComponent<Tile>();
                            obj.transform.SetParent(this.transform);
                        } else
                        {
                            tile = gameObject.AddComponent<Tile>();
                        }

                        tile.y = i;
                        tile.x = j;
                        tile.pos = new Vector3(cur_x, cur_y, 0);
                        tile.block_type = EnumClass.IntToEnumBlock(type);                        

                        list.Add(tile);

                        cur_y -= (unit_y / 2);
                        cur_x += unit_x;
                    }
                    tiles.Add(list);
                }
            }
        }
        catch(Exception ex)
        {
            Debug.LogError(ex.Message);
        }

        return tiles;
    }
}
