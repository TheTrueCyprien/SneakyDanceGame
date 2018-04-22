using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerControl : MonoBehaviour {

    private Transform player_t;
    private BoxCollider2D player_c;

    private float tileSize = 32.0f;
    

	// Use this for initialization
	void Start () {
        player_t = GetComponent<Transform>();
        player_c = GetComponent<BoxCollider2D>();
	}

    // Update is called once per frame
    void Update () {

        if (Input.GetButtonDown("Horizontal"))
        {
            float moveHorizontal = Mathf.Round(Input.GetAxisRaw("Horizontal"));

            if (moveHorizontal > 0)
            {
                moveHorizontal = 1.0f;
            }
            else if (moveHorizontal < 0)
            {
                moveHorizontal = -1.0f;
            }

            Vector3 velocity = new Vector3(tileSize * moveHorizontal, 0, 0);

            RaycastHit2D[] results = new RaycastHit2D[4];
            player_c.Raycast(new Vector2(moveHorizontal, 0),results,tileSize);

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
                player_t.position += velocity;
            }
        }

        else if (Input.GetButtonDown("Vertical"))
        {
            float moveVertical = Mathf.Round(Input.GetAxisRaw("Vertical"));

            if (moveVertical > 0)
            {
                moveVertical = 1.0f;
            }
            else if (moveVertical < 0)
            {
                moveVertical = -1.0f;
            }
            Vector3 velocity = new Vector3(0, tileSize * moveVertical, 0);

            RaycastHit2D[] results = new RaycastHit2D[4];
            player_c.Raycast(new Vector2(0,moveVertical), results, tileSize);

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
                player_t.position += velocity;
            }
        }
    }
}
