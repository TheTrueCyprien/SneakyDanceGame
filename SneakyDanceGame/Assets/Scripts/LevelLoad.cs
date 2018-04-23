using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class LevelLoad : MonoBehaviour {

    public bool[,] tileMap;

    private void Awake()
    {
        
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += gatherTiles;
    }


    private void gatherTiles(Scene scene, LoadSceneMode mode)
    {

    }

    // Use this for initialization
    void Start () {
		
	}
}
