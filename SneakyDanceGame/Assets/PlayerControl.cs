using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
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

        AnimationClip current_anim = player_anim.GetCurrentAnimatorClipInfo(0)[0].clip;
        player_anim.speed = current_anim.length / beatDeltaTime;
        player_anim.ResetTrigger("onBeat");

        if (Input.GetButtonDown("Horizontal"))
        {
            beatSkipped = false;
            if (isOffBeat())
            {
                stumble();
            }
            else
            {
                UIManager.instance.IncrementCombo();
                player_anim.SetInteger("combo", player_anim.GetInteger("combo") + 1);
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
						if (hit.collider.gameObject.CompareTag ("HidingSpot")) {
							ExecuteEvents.Execute<IHideMessage>(hit.collider.gameObject, null, (x, y) => x.hide(gameObject));
							break;
						}

                        if (!hit.collider.gameObject.CompareTag("SightCone"))
                        {
                            collision = true;
                            break;
                        }
                    }
                }

                if (!collision)
                {
                    player_t.position += velocity;
                }
                else {
                    stumble();
                }
            }

        }

        else if (Input.GetButtonDown("Vertical"))
        {
            beatSkipped = false;
            if (isOffBeat())
            {
                stumble();
            }
            else
            {
                UIManager.instance.IncrementCombo();
                player_anim.SetInteger("combo", player_anim.GetInteger("combo")+1);
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
						if (hit.collider.gameObject.CompareTag ("HidingSpot")) {
							ExecuteEvents.Execute<IHideMessage>(hit.collider.gameObject, null, (x, y) => x.hide(gameObject));
							break;
						}

                        if (!hit.collider.gameObject.CompareTag("SightCone"))
                        {
                            collision = true;
                            break;
                        }
                    }
                }

                if (!collision)
                {
                    player_t.position += velocity;
                }
                else
                {
                    stumble();
                }
            }

        }


        if (UIManager.instance.combo > 0 && !comboVerified && (Time.time - lastBeatTime > beatDeltaTime * offbeatTolerance)) {
            if (beatSkipped)
            {
                UIManager.instance.DropCombo();
                player_anim.SetInteger("combo", 0);
            }
            comboVerified = true;
            beatSkipped = true;
        }
    }

    private void stumble() {
        UIManager.instance.DropCombo();
        SoundManager.instance.failSound();
        player_anim.SetTrigger("stumble");
        player_anim.SetInteger("combo", 0);
    }

    private bool isOffBeat() {
        float timing = Time.time - lastBeatTime;
        float offbeat = Mathf.Min(timing, Mathf.Abs(timing - beatDeltaTime));
        return (offbeat / beatDeltaTime) > offbeatTolerance;
    }

    public void OnBeat(int index)
    {
        player_anim.SetTrigger("onBeat");
        lastBeatTime = Time.time;
        comboVerified = false;
    }

    public void SongStarted(float secPerBeat)
    {
        beatDeltaTime = secPerBeat;
    }
}
