using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameService : MonoBehaviour
{
    public MapManager mapMng;
    public BlockManager blockMng;
    private List<List<Tile>> tiles;

    void Start()
    {
        string file_path = string.Format("{0}/Assets/MapData/map_data.txt", System.IO.Directory.GetCurrentDirectory());

        tiles = mapMng.CreateMap(file_path);
        InitPlaceBlocks(tiles);

        Block.OnDrag += BlockDrag;
        Block.OnEnter += BlockClickEnter;
    }

    void Update()
    {
        
    }

    void BlockDrag(Block block)
    {
        string message = string.Format("x : {0}, y : {1}", block.GetX(), block.GetY());
        Debug.Log(message);
    }

    void BlockClickEnter(Block block)
    {
        string message = string.Format("x : {0}, y : {1}", block.GetX(), block.GetY());
        Debug.Log(message);
    }

    void InitPlaceBlocks(List<List<Tile>> tiles)
    {
        blockMng.Init();

        int y_size = tiles.Count;
        int x_size = tiles[0].Count;

        for (int i = 0; i < y_size; i++)
        {
            for (int j = 0; j < x_size; j++)
            {
                Tile tile = tiles[i][j];
                if(tile.block_type != EnumBlockType.None)
                {                    
                    GameObject obj = blockMng.PopBlock(tile.block_type);
                    obj.transform.position = tile.pos;
                    obj.GetComponent<Block>().SetPosition(i, j);
                    tile.obj = obj;
                }
            }
        }
    }
}
