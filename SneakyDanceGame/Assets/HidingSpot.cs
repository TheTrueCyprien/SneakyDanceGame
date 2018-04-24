using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HidingSpot : MonoBehaviour, IHideMessage, IRythmMessageTarget {

	private Animator hide_anim;
	private float beatDeltaTime;

	private GameObject player;
	private Vector3 pos;
	private bool isHiding;
	private int hidingCounter;

	public void Start() {
        hide_anim = gameObject.GetComponent<Animator>();
        player = GameObject.Find("player");
        isHiding = false;
	}

	public void Update() {
		AnimationClip current_anim = hide_anim.GetCurrentAnimatorClipInfo(0)[0].clip;
		hide_anim.speed = current_anim.length / beatDeltaTime;
	}

	public void hide(GameObject p) {
		if (!isHiding) {
			pos = player.transform.position;
			//deactivate player components
			player.GetComponent<SpriteRenderer> ().enabled = false;
			player.GetComponent<BoxCollider2D> ().enabled = false;
			hidingCounter = 3;
			isHiding = true;
            hide_anim.SetBool("hiding", true);
		}
	}

	public void OnBeat(int index)
	{
		//hide_anim.SetTrigger("onBeat");
        if (isHiding)
        {
		    hidingCounter--;
		    if (hidingCounter == 0) {
			    player.GetComponent<SpriteRenderer>().enabled = true;
			    player.GetComponent<BoxCollider2D>().enabled = true;
                hide_anim.SetBool("hiding", false);

                //evict player
                player.transform.position = pos;
		    }

        }
	}

	public void SongStarted(float secPerBeat) {
		beatDeltaTime = secPerBeat;
	}
}
