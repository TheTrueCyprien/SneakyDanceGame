using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy_control : MonoBehaviour {

    char[] route = new char[] { };
    private Transform player_t;
    private BoxCollider2D player_c;

    int interval = 1;
    float nextTime = 1;
    int pattern_count = 0;

    private float tileSize = 32.0f;


    // Use this for initialization
    void Start() {
        route = new char[] {'d', 'r', 'u', 'l'};

        player_t = GetComponent<Transform>();
        player_c = GetComponent<BoxCollider2D>();
    }
	
	// Update is called once per frame
	void Update () {
		
        if (Time.time >= nextTime)
        {
            Vector3 velocity = new Vector3(0, 0, 0);

            RaycastHit2D[] results = new RaycastHit2D[1];

            if (route[pattern_count] == 'd')
            {
                velocity = new Vector3(0, -1.0f * tileSize, 0);

                results = new RaycastHit2D[1];
                player_c.Raycast(new Vector2(0, -1.0f * tileSize), results, tileSize);
            }
            else if (route[pattern_count] == 'u')
            {
                velocity = new Vector3(0, 1.0f * tileSize, 0);

                results = new RaycastHit2D[1];
                player_c.Raycast(new Vector2(0, 1.0f * tileSize), results, tileSize);
            }
            else if (route[pattern_count] == 'l') 
            {
                velocity = new Vector3(-1.0f * tileSize, 0, 0);

                results = new RaycastHit2D[1];
                player_c.Raycast(new Vector2(-1.0f * tileSize,0), results, tileSize);
            }
            else if (route[pattern_count] == 'r')
            {
                velocity = new Vector3(1.0f * tileSize, 0, 0);

                results = new RaycastHit2D[1];
                player_c.Raycast(new Vector2(1.0f * tileSize, 0), results, tileSize);
            }

            if (results[0].collider == null)
            {
                player_t.position += velocity;
                if (pattern_count < route.Length-1)
                {
                    pattern_count++;
                }
                else
                {
                    pattern_count = 0;
                }
            }

            nextTime += interval;
        }
	}
}
