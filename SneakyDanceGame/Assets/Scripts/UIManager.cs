using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    public static UIManager instance;

    public Text comboText;
    public int combo = 0;

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
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void IncrementCombo() {
        comboText.gameObject.SetActive(true);
        combo++;
        comboText.text = string.Format("x{0}", combo);   
    }

    public void DropCombo() {
        comboText.gameObject.SetActive(false);
        combo = 0;
    }
}
