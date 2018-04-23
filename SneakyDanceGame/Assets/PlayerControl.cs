using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerControl : MonoBehaviour, IRythmMessageTarget
{

    private Transform player_t;
    private BoxCollider2D player_c;
    private Animator player_anim;
    private float lastBeatTime;
    private float beatDeltaTime;
    private bool beatSkipped = true;
    private bool comboVerified = false;

    public float tileSize = 32.0f;
    public float offbeatTolerance = 0.15f;

	// Use this for initialization
	void Start () {
        player_t = GetComponent<Transform>();
        player_c = GetComponent<BoxCollider2D>();
        player_anim = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update () {

        if (Input.GetButtonDown("Horizontal"))
        {
            beatSkipped = false;
            if (isOffBeat())
            {
                UIManager.instance.DropCombo();
            }
            else
            {
                UIManager.instance.IncrementCombo();
            }

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
                        UIManager.instance.DropCombo();
                        break;
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
            beatSkipped = false;
            if (isOffBeat())
            {
                UIManager.instance.DropCombo();
            }
            else
            {
                UIManager.instance.IncrementCombo();
            }

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
                        UIManager.instance.DropCombo();
                        break;
                    }
                }
            }

            if (!collision)
            {
                player_t.position += velocity;
            }
        }


        if (UIManager.instance.combo > 0 && !comboVerified && (Time.time - lastBeatTime > beatDeltaTime * offbeatTolerance)) {
            if (beatSkipped)
            {
                UIManager.instance.DropCombo();
            }
            comboVerified = true;
            beatSkipped = true;
        }
    }

    private bool isOffBeat() {
        float timing = Time.time - lastBeatTime;
        float offbeat = Mathf.Min(timing, Mathf.Abs(timing - beatDeltaTime));
        return (offbeat / beatDeltaTime) > offbeatTolerance;
    }

    public void OnBeat(int index)
    {
        AnimationClip current_anim = player_anim.GetCurrentAnimatorClipInfo(0)[0].clip;
        float stepsize = 1f/(current_anim.frameRate * current_anim.length);
        player_anim.SetFloat("normalTime", (player_anim.GetFloat("normalTime") + stepsize) % 1f);
        lastBeatTime = Time.time;
        comboVerified = false;
    }

    public void SongStarted(float secPerBeat)
    {
        beatDeltaTime = secPerBeat;
    }
}
