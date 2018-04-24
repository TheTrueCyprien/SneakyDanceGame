using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour, IRythmMessageTarget
{

    public static UIManager instance;

    public Text comboText;
    public Text comboDescription;
    public int combo = 0;
    public GameObject screenBorder;

    private float lastBeat = 0f;
    private float beatDeltaTime = 0f;

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

    // Update is called once per frame
    void Update () {
        if (Time.time - lastBeat > beatDeltaTime * 0.15) {
            screenBorder.SetActive(false);
        }
	}

    public void IncrementCombo() {
        comboText.gameObject.SetActive(true);
        combo++;
        comboText.text = string.Format("x{0}", combo);
        switch (combo)
        {
            case 25:
                SoundManager.instance.fadeInChannel(0);
                comboDescription.gameObject.SetActive(true);
                comboDescription.text = "Damn";
                break;
            case 50:
                SoundManager.instance.fadeInChannel(1);
                comboDescription.text = "Cool";
                break;
            case 75:
                comboDescription.text = "Beautiful";
                break;
            case 100:
                SoundManager.instance.fadeInChannel(2);
                comboDescription.text = "Amazing";
                break;
            case 150:
                comboDescription.text = "Stunning";
                break;
        }
    }

    public void DropCombo() {
        comboText.gameObject.SetActive(false);
        comboDescription.gameObject.SetActive(false);
        if (combo >= 25) {
            SoundManager.instance.fadeOutChannel(0);
            if (combo >= 50)
            {
                SoundManager.instance.fadeOutChannel(1);
                if (combo >= 100)
                {
                    SoundManager.instance.fadeOutChannel(2);
                }
            }
        }
        combo = 0;
    }

    public void OnBeat(int index)
    {
        lastBeat = Time.time;
        screenBorder.SetActive(true);
    }

    public void SongStarted(float secPerBeat) {
        beatDeltaTime = secPerBeat;
    }

    public void playerSpotted() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
