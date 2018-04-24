using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class dynamic_sight_cone : MonoBehaviour {

    Transform[] visionTargets;
    MeshFilter filter;

	// Use this for initialization
	void Start () {
        visionTargets = GetComponentsInChildren<Transform>();
    }
	
	// Update is called once per frame
	void Update () {
        List<Vector2> vision = new List<Vector2>();

        foreach (Transform target in visionTargets) {
            RaycastHit2D hit = Physics2D.Raycast(gameObject.transform.position, target.position, Vector2.Distance(gameObject.transform.position, target.position));
            if (hit.collider != null)
            {
                vision.Add(hit.point);
            }
            else
            {
                vision.Add(target.transform.position);
            }
        }
        generateMesh(vision.ToArray());


	}

    void generateMesh(Vector2[] vertices2D) {
        
        // Use the triangulator to get indices for creating triangles
        Triangulator tr = new Triangulator(vertices2D);
        int[] indices = tr.Triangulate();

        // Create the Vector3 vertices
        Vector3[] vertices = new Vector3[vertices2D.Length];
        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i] = new Vector3(vertices2D[i].x, vertices2D[i].y, 0);
        }

        // Create the mesh
        Mesh msh = new Mesh();
        msh.vertices = vertices;
        msh.triangles = indices;
        msh.RecalculateNormals();
        msh.RecalculateBounds();

        // Set up game object with mesh;
        if (filter == null)
        {
            filter = gameObject.AddComponent(typeof(MeshFilter)) as MeshFilter;
        }
        filter.mesh = msh;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject parent = transform.parent.gameObject;
        ExecuteEvents.Execute<OnConeCollision>(parent, null, (x, y) => x.OnConeCollision(collision));
    }

}
