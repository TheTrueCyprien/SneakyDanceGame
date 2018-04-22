using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy_control : MonoBehaviour, OnConeCollision, IRythmMessageTarget {

    public string route;
    char[] route_array = new char[] { };
    private Transform npc_t;
    private BoxCollider2D npc_c;
    private BoxCollider2D npc_cone;

    int interval = 1;
    float nextTime = 1;
    int pattern_count = 0;

    private float tileSize = 32.0f;


    // Use this for initialization
    void Start() {
        route_array = route.ToCharArray();

        npc_t = GetComponent<Transform>();
        npc_c = GetComponent<BoxCollider2D>();
    }
	
	// Update is called once per frame
	void Update () {
	}

    public void SongStarted(float sec) { }

    public void OnBeat(int index) {
        MoveRoute();
    }

    void MoveRoute()
    {
        Vector3 velocity = new Vector3(0, 0, 0);
        Vector3 rotation = new Vector3(0, 0, 0);

        RaycastHit2D[] results = new RaycastHit2D[4];

        if (route[pattern_count] == 'd')
        {
            velocity = new Vector3(0, -1.0f * tileSize, 0);
            rotation = new Vector3(0, 0, 0);
            rotation = rotation - npc_t.rotation.eulerAngles;

            npc_c.Raycast(new Vector2(0, -1.0f), results, tileSize);
        }
        else if (route[pattern_count] == 'u')
        {
            velocity = new Vector3(0, 1.0f * tileSize, 0);
            rotation = new Vector3(0, 0, 180);
            rotation = rotation - npc_t.rotation.eulerAngles;

            npc_c.Raycast(new Vector2(0, 1.0f), results, tileSize);
        }
        else if (route[pattern_count] == 'l')
        {
            velocity = new Vector3(-1.0f * tileSize, 0, 0);
            rotation = new Vector3(0, 0, 270);
            rotation = rotation - npc_t.rotation.eulerAngles;

            npc_c.Raycast(new Vector2(-1.0f, 0), results, tileSize);
        }
        else if (route[pattern_count] == 'r')
        {
            velocity = new Vector3(1.0f * tileSize, 0, 0);
            rotation = new Vector3(0, 0, 90);
            rotation = rotation - npc_t.rotation.eulerAngles;

            npc_c.Raycast(new Vector2(1.0f, 0), results, tileSize);
        }

        if (rotation == Vector3.zero)
        {
            bool collision = false;
            foreach (RaycastHit2D hit in results)
            {
                if (hit.collider != null)
                {
                    if (!hit.collider.gameObject.CompareTag("SightCone"))
                    {
                        collision = true;
                    }
                }
            }

            if (!collision)
            {
                npc_t.position += velocity;                
                if (pattern_count < route.Length - 1)
                {
                    pattern_count++;
                }
                else
                {
                    pattern_count = 0;
                }
            }
        }
        else
        {
            npc_t.Rotate(rotation, Space.World);
        }
    }

    public void OnConeCollision(Collision2D collision)
    {
        GameObject player = GameObject.Find("player");
        
        if (collision.gameObject == player)
        {
            print("Zugriff!");
        }
    }
}
