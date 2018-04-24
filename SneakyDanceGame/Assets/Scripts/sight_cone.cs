using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class sight_cone : MonoBehaviour {


	// Use this for initialization
	void Start () {

    }
	
	// Update is called once per frame
	void Update () {

	}

    void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject parent = transform.parent.gameObject;
        ExecuteEvents.Execute<OnConeCollision>(parent, null, (x, y) => x.OnConeCollision(collision));
    }

}
