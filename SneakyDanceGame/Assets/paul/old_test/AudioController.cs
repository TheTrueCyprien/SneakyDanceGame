using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour {

	public AudioSource MasterBeat;
	public AudioSource Layer1;
	public AudioSource Layer2;
	public AudioSource Layer3;

	private AudioSource[] aLayers;
	private int LayerCounter;

	private List<AudioSource> fadeInQueue;
	private List<AudioSource> fadeOutQueue;

	// Use this for initialization
	void Start () {
		aLayers = new AudioSource[] { Layer1, Layer2, Layer3 };
		foreach(AudioSource layer in aLayers) {
			layer.volume = 0f;
		}
		LayerCounter = 0;

		fadeInQueue = new List<AudioSource>();
		fadeOutQueue = new List<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown("Fire1")) {
			if (LayerCounter < aLayers.Length) {
				aLayers[LayerCounter].Play();
				aLayers [LayerCounter].timeSamples = MasterBeat.timeSamples;
				fadeInQueue.Add(aLayers[LayerCounter]);
				LayerCounter++;
			}
		}
		else if (Input.GetButtonDown("Fire2")) {
			if (LayerCounter > 0) {
				fadeOutQueue.Add(aLayers [--LayerCounter]);
			}
		}

		handleQueues();
	}

	void handleQueues(){
		for (int i = 0; i < fadeInQueue.Count; i++) {
			AudioSource item = fadeInQueue[i];
			if (item.volume < 1f) {
				item.volume = Mathf.Min (item.volume + 0.5f * Time.deltaTime, 1f);
			} else if (item.volume == 1f) {
				fadeInQueue.Remove(item);
			}
		}

		for (int i = 0; i < fadeOutQueue.Count; i++) {
			AudioSource item = fadeOutQueue[i];
			if (item.volume > 0f) {
				item.volume = Mathf.Max( item.volume - 0.5f * Time.deltaTime, 0f );
			} else if (item.volume == 0f) {
				fadeOutQueue.Remove(item);
				item.Stop();
			}
		}
	}
}
