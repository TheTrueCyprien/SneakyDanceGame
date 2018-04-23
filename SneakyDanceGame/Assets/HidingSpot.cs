using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HidingSpot : MonoBehaviour, IHideMessage, IRythmMessageTarget {

	private Animator hide_anim;
	private float beatDeltaTime;

	public GameObject player;
	private Vector3 pos;
	private bool isHiding;
	private int hidingCounter;

	public void Start() {
		isHiding = false;
	}

	public void Update() {
		//AnimationClip current_anim = hide_anim.GetCurrentAnimatorClipInfo(0)[0].clip;
		//hide_anim.speed = current_anim.length / beatDeltaTime;
		//hide_anim.ResetTrigger("onBeat");
	}

	public void hide(GameObject p) {
		if (!isHiding) {
			pos = player.transform.position;
			//deactivate player components
			player.GetComponent<SpriteRenderer> ().enabled = false;
			player.GetComponent<BoxCollider2D> ().enabled = false;
			hidingCounter = 3;
			isHiding = true;
		}
	}

	public void OnBeat(int index)
	{
		//hide_anim.SetTrigger("onBeat");
		hidingCounter--;
		if (hidingCounter == 0) {
			print("show yourself!");
			player.GetComponent<SpriteRenderer>().enabled = true;
			player.GetComponent<BoxCollider2D>().enabled = true;
			isHiding = false;

			//evict player
			player.transform.position = pos;
		}
	}

	public void SongStarted(float secPerBeat) {
		beatDeltaTime = secPerBeat;
	}
}
