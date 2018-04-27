using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class enemy_control : MonoBehaviour, OnConeCollision, IRythmMessageTarget {

    public string route;
    char[] route_array = new char[] { };
    private Transform npc_t;
    private BoxCollider2D npc_c;
    private Transform npc_cone;
    private Animator npc_anim;
    private string last_anim_trigger;

    int pattern_count = 0;

    private float tileSize = 32.0f;
    private float secPerBeat;

    // Use this for initialization
    void Start() {
        route_array = route.ToCharArray();

        npc_t = GetComponent<Transform>();
        npc_c = GetComponent<BoxCollider2D>();
        npc_anim = GetComponent<Animator>();

        foreach (Transform child in GetComponentsInChildren<Transform>()) if (child.CompareTag("SightCone")) {
                npc_cone = child;
        }
    }
	
	// Update is called once per frame
	void Update () {
        AnimationClip current_anim = npc_anim.GetCurrentAnimatorClipInfo(0)[0].clip;
        npc_anim.speed = current_anim.length / secPerBeat;
    }

    public void SongStarted(float beatDelta) {
        secPerBeat = beatDelta;
    }

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
            rotation = rotation - npc_cone.rotation.eulerAngles;
            if(last_anim_trigger != "MoveDown")
            {
                npc_anim.SetTrigger("MoveDown");
                last_anim_trigger = "MoveDown";
            }

            npc_c.Raycast(new Vector2(0, -1.0f), results, tileSize);
        }
        else if (route[pattern_count] == 'u')
        {
            velocity = new Vector3(0, 1.0f * tileSize, 0);
            rotation = new Vector3(0, 0, 180);
            rotation = rotation - npc_cone.rotation.eulerAngles;
            if (last_anim_trigger != "MoveUp")
            {
                npc_anim.SetTrigger("MoveUp");
                last_anim_trigger = "MoveUp";
            }

            npc_c.Raycast(new Vector2(0, 1.0f), results, tileSize);
        }
        else if (route[pattern_count] == 'l')
        {
            velocity = new Vector3(-1.0f * tileSize, 0, 0);
            rotation = new Vector3(0, 0, 270);
            rotation = rotation - npc_cone.rotation.eulerAngles;
            if (last_anim_trigger != "MoveLeft")
            {
                npc_anim.SetTrigger("MoveLeft");
                last_anim_trigger = "MoveLeft";
            }

            npc_c.Raycast(new Vector2(-1.0f, 0), results, tileSize);
        }
        else if (route[pattern_count] == 'r')
        {
            velocity = new Vector3(1.0f * tileSize, 0, 0);
            rotation = new Vector3(0, 0, 90);
            rotation = rotation - npc_cone.rotation.eulerAngles;

            npc_c.Raycast(new Vector2(1.0f, 0), results, tileSize);
            if (last_anim_trigger != "MoveRight")
            {
                npc_anim.SetTrigger("MoveRight");
                last_anim_trigger = "MoveRight";
            }
        } else if (route[pattern_count] == '2')
        {
            rotation = new Vector3(0, 0, 0);
            rotation = rotation - npc_cone.rotation.eulerAngles;
            if (last_anim_trigger != "IdleDown")
            {
                npc_anim.SetTrigger("IdleDown");
                last_anim_trigger = "IdleDown";
            }
        }
        else if (route[pattern_count] == '8')
        {
            rotation = new Vector3(0, 0, 180);
            rotation = rotation - npc_cone.rotation.eulerAngles;
            if (last_anim_trigger != "IdleUp")
            {
                npc_anim.SetTrigger("IdleUp");
                last_anim_trigger = "IdleUp";
            }
        }
        else if (route[pattern_count] == '4')
        {
            rotation = new Vector3(0, 0, 270);
            rotation = rotation - npc_cone.rotation.eulerAngles;
            if (last_anim_trigger != "IdleLeft")
            {
                npc_anim.SetTrigger("IdleLeft");
                last_anim_trigger = "IdleLeft";
            }
        }
        else if (route[pattern_count] == '6')
        {
            rotation = new Vector3(0, 0, 90);
            rotation = rotation - npc_cone.rotation.eulerAngles;
            if (last_anim_trigger != "IdleRight")
            {
                npc_anim.SetTrigger("IdleRight");
                last_anim_trigger = "IdleRight";
            }
        }

        npc_cone.Rotate(rotation, Space.World);
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

    public void OnConeCollision(Collision2D collision)
    {
        GameObject player = GameObject.Find("player");
        
        if (collision.gameObject == player)
        {
            //int layerMask = 1 << 9;
            //RaycastHit2D hit = Physics2D.Raycast(gameObject.GetComponent<BoxCollider2D>().bounds.center, player.GetComponent<BoxCollider2D>().bounds.center, Mathf.Infinity, layerMask);
            //if (hit.collider != null && hit.collider.gameObject == player) {
                UIManager.instance.playerSpotted();
            //}
        }
    }
}
