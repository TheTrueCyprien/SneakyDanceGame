using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SoundManager : MonoBehaviour {

    public static SoundManager instance;

    public List<GameObject> Listeners;

    private AudioSource MasterBeat;
    private List<AudioSource> aLayers;
    private List<AudioSource> fadeInQueue;
    private List<AudioSource> fadeOutQueue;

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

    //beatsteps at which an update is triggered
    private int nextIndex = 0;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Use this for initialization
    void Start () {
        aLayers = new List<AudioSource>();
        fadeInQueue = new List<AudioSource>();
        fadeOutQueue = new List<AudioSource>();

        foreach (AudioSource layer in GetComponentsInChildren<AudioSource>()) if (layer.CompareTag("AudioChannel"))
            {
                layer.volume = 0f;
                aLayers.Add(layer);
            }

        //calculate how many seconds is one beat
        secPerBeat = 60f / bpm;

        //record the time when the song starts
        dsptimesong = (float)AudioSettings.dspTime;

        //start the song
        foreach (AudioSource child in GetComponentsInChildren<AudioSource>()) if (child.CompareTag("Master-Audio"))
            {
                MasterBeat = child;
                child.Play();
            }

        foreach (GameObject Listener in Listeners)
        {
            ExecuteEvents.Execute<IRythmMessageTarget>(Listener, null, (x, y) => x.SongStarted(secPerBeat));
        }
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
        handleQueues();
    }

    public void failSound() {
        GetComponent<AudioSource>().Play();
    }

    public void fadeInChannel(int n) {
        aLayers[n].Play();
        aLayers[n].timeSamples = MasterBeat.timeSamples;
        fadeInQueue.Add(aLayers[n]);
    }

    public void fadeOutChannel(int n) {
        fadeOutQueue.Add(aLayers[n]);
    }

    void handleQueues()
    {
        for (int i = 0; i < fadeInQueue.Count; i++)
        {
            AudioSource item = fadeInQueue[i];
            if (item.volume < 1f)
            {
                item.volume = Mathf.Min(item.volume + 0.5f * Time.deltaTime, 1f);
            }
            else if (item.volume == 1f)
            {
                fadeInQueue.Remove(item);
            }
        }

        for (int i = 0; i < fadeOutQueue.Count; i++)
        {
            AudioSource item = fadeOutQueue[i];
            if (item.volume > 0f)
            {
                item.volume = Mathf.Max(item.volume - 0.5f * Time.deltaTime, 0f);
            }
            else if (item.volume == 0f)
            {
                fadeOutQueue.Remove(item);
                item.Stop();
            }
        }
    }
}
