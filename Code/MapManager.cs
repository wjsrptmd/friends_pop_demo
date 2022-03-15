using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public GameObject tile_unit;
    public float tile_offset_x = -0.2f;
    public float tile_offset_y = 0.02f;


    public List<List<Tile>> CreateMap(string file_path)
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

                float start_x = (x_size - 1) * (unit_x / 2) * (-1);
                float start_y = (y_size - 1) * (unit_y / 2);

                float cur_x = 0;
                float cur_y = 0;

                for (int i = 0; i < y_size; i++)
                {
                    cur_x = start_x;
                    cur_y = start_y - (unit_y * i);

                    int cnt = 0;
                    List<Tile> list = new List<Tile>();
                    foreach(char ch in map_data[i])
                    {
                        int type = Convert.ToInt32(ch - '0');
                        Tile tile = new Tile()
                        {
                            pos = new Vector3(cur_x, cur_y, 0),
                            block_type = EnumClass.IntToEnumBlock(type)
                        };
                        

                        list.Add(tile);
                        cur_x += unit_x;
                        if(cnt % 2 == 0)
                        {
                            cur_y += unit_y / 2;
                        } else
                        {
                            cur_y -= unit_y / 2;
                        }
                        cnt++;
                    }
                    tiles.Add(list);
                }

                foreach(List<Tile> list in tiles)
                {
                    foreach(Tile tile in list) {
                        if (tile.block_type != EnumBlockType.None)
                        {
                            GameObject obj = Instantiate(tile_unit, tile.pos, Quaternion.identity);
                            obj.AddComponent<CircleCollider2D>();
                            obj.GetComponent<CircleCollider2D>().isTrigger = true;
                            obj.transform.SetParent(this.transform);
                        }
                    }
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
