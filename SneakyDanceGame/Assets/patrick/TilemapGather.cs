using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class TilemapGather : MonoBehaviour
{

    public bool[,] nodes;

    private void Awake()
    {

    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += gatherTiles;
    }


    private void gatherTiles(Scene scene, LoadSceneMode mode)
    {
        Tilemap tilemap = GetComponent<Tilemap>();

        BoundsInt bounds = tilemap.cellBounds;
        TileBase[] allTiles = tilemap.GetTilesBlock(bounds);
        Vector3 mapSize = tilemap.size;

        TileBase[,] tiles2D = new TileBase[(int) mapSize[0],(int) mapSize[1]];

        for (int i = 0; i < allTiles.Length; i++)
        {
            tiles2D[i / (int) mapSize[0], i % (int) mapSize[0]] = allTiles[i];
            //if (allTiles[i].)
        }
        
    }

    // Use this for initialization
    void Start()
    {

    }
}
