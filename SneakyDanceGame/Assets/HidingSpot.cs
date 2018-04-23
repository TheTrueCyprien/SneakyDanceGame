using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HidingSpot : MonoBehaviour, IHideMessage, IRythmMessageTarget {

	private Animator hide_anim;
	private float beatDeltaTime;

	public void Update() {
		AnimationClip current_anim = hide_anim.GetCurrentAnimatorClipInfo(0)[0].clip;
		hide_anim.speed = current_anim.length / beatDeltaTime;
		hide_anim.ResetTrigger("onBeat");
	}

	public void hide(GameObject player) {
		//deactivate player
	}

	public void OnBeat(int index)
	{
		hide_anim.SetTrigger("onBeat");
	}

	public void SongStarted(float secPerBeat) {
		beatDeltaTime = secPerBeat;
	}
}
