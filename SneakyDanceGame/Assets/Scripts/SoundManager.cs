using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SoundManager : MonoBehaviour {

    public GameObject[] Listeners;

    //the current position of the song (in seconds)
    private float songPosition;

    //the current position of the song (in beats)
    private float songPosInBeats;

    //the duration of a beat
    float secPerBeat;

    //how much time (in seconds) has passed since the song started
    float dsptimesong;

    //beats per minute of a song
    public float bpm = 100;

    //keep all the position-in-beats of notes in the song
    private float[] notes;

    //beatsteps at which an update is triggered
    private int nextIndex = 0;

    // Use this for initialization
    void Start () {
        //calculate how many seconds is one beat
        secPerBeat = 60f / bpm;

        //record the time when the song starts
        dsptimesong = (float)AudioSettings.dspTime;

        //start the song
        GetComponent<AudioSource>().Play();
    }
	
	// Update is called once per frame
	void Update () {
        //calculate the position in seconds
        songPosition = (float)(AudioSettings.dspTime - dsptimesong);

        //calculate the position in beats
        songPosInBeats = songPosition / secPerBeat;

        if (songPosInBeats > nextIndex)
        {
            foreach(GameObject Listener in Listeners) {
                ExecuteEvents.Execute<IRythmMessageTarget>(Listener, null, (x, y) => x.OnBeat(nextIndex));
            }
            nextIndex++;
        }
    }
}
