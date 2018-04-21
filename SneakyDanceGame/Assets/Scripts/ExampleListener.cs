using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleListener : MonoBehaviour, IRythmMessageTarget {

    public void OnBeat(int index) {
        GetComponent<Renderer>().material.color = new Color(255 * (1 - index % 2), 0, 255 * (index % 2));

    }

}
