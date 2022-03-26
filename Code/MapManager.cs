using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    private GameObject tile_unit;
    private float tile_offset_x = -0.15f;
    private float tile_offset_y = 0.010f;

    public void Init()
    {
        tile_unit = Util.CreateObjForPng("tile_unit", new Vector3(0.08f, 0.08f, 0));
        tile_unit.AddComponent<CircleCollider2D>();
        tile_unit.GetComponent<CircleCollider2D>().isTrigger = true;
    }

    public List<List<Tile>> CreateTileMap()
    {
        List<List<Tile>> tiles = new List<List<Tile>>();

        try
        {
            TextAsset txt_file = Resources.Load<TextAsset>("MapData/map_data");            
            string[] map_data = txt_file.text.Split(new[] { "\r\n", "\r", "\n"}, StringSplitOptions.None);

            if (map_data.Length == 0)
            {
                Debug.LogError(String.Format("map_data.txt is Empty"));
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
                    if (map_data[i].Length == 0) break;

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
                            obj.SetActive(true);
                            obj.AddComponent<Tile>();
                            tile = obj.GetComponent<Tile>();
                            obj.transform.SetParent(this.transform);
                        }
                        else
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
