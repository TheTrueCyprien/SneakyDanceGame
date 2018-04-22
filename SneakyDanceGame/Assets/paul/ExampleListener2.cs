using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleListener2 : MonoBehaviour, IRythmMessageTarget {
	
	public string route;
	char[] route_array = new char[] { };
	private Transform player_t;
	private BoxCollider2D player_c;
	
	int interval = 1;
	float nextTime = 1;
	int pattern_count = 0;
	
	private float tileSize = 32.0f;
	
	
	// Use this for initialization
	void Start() {
		route_array = route.ToCharArray();
		
		player_t = GetComponent<Transform>();
		player_c = GetComponent<BoxCollider2D>();
	}

    public void SongStarted(float secPerBeat) { }

    public void OnBeat(int index) {
        GetComponent<Renderer>().material.color = new Color(255 * (1 - index % 2), 0, 255 * (index % 2));

		Vector3 velocity = new Vector3(0, 0, 0);
		Vector3 rotation = new Vector3(0, 0, 0);
		
		RaycastHit2D[] results = new RaycastHit2D[1];
		
		if (route[pattern_count] == 'd')
		{
			velocity = new Vector3(0, -1.0f * tileSize, 0);
			rotation = new Vector3(0, 0, 0);
			rotation = rotation - player_t.rotation.eulerAngles;
			
			results = new RaycastHit2D[1];
			player_c.Raycast(new Vector2(0, -1.0f * tileSize), results, tileSize);
		}
		else if (route[pattern_count] == 'u')
		{
			velocity = new Vector3(0, 1.0f * tileSize, 0);
			rotation = new Vector3(0, 0, 180);
			rotation = rotation - player_t.rotation.eulerAngles;
			
			results = new RaycastHit2D[1];
			player_c.Raycast(new Vector2(0, 1.0f * tileSize), results, tileSize);
		}
		else if (route[pattern_count] == 'l') 
		{
			velocity = new Vector3(-1.0f * tileSize, 0, 0);
			rotation = new Vector3(0, 0, 270);
			rotation = rotation - player_t.rotation.eulerAngles;
			
			results = new RaycastHit2D[1];
			player_c.Raycast(new Vector2(-1.0f * tileSize,0), results, tileSize);
		}
		else if (route[pattern_count] == 'r')
		{
			velocity = new Vector3(1.0f * tileSize, 0, 0);
			rotation = new Vector3(0, 0, 90);
			rotation = rotation - player_t.rotation.eulerAngles;
			
			results = new RaycastHit2D[1];
			player_c.Raycast(new Vector2(1.0f * tileSize, 0), results, tileSize);
		}
		
		if (results[0].collider == null)
		{
			player_t.position += velocity;
			player_t.Rotate(rotation,Space.World);
			if (pattern_count < route.Length-1)
			{
				pattern_count++;
			}
			else
			{
				pattern_count = 0;
			}
		}
    }


				

}
