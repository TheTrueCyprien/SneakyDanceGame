using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;

public class DynamicSightCone : MonoBehaviour {

    public float fov = 60f;
    public float radius = 144f;
    public int sections = 64;
    public int order_in_layer = -900;
    MeshRenderer cone_renderer;
    MeshFilter cone_filter;
    Tilemap tilemap;
    GameObject player;

    // Use this for initialization
    void Start () {
        tilemap = GameObject.Find("Tilemap").GetComponent<Tilemap>();
        cone_renderer = GetComponent<MeshRenderer>();
        cone_filter = GetComponent<MeshFilter>();
        player = GameObject.FindGameObjectWithTag("Player");
    }


    // Update is called once per frame
    private void Update()
    {
        float distance = Vector2.Distance(transform.position, player.transform.position);
        if (distance <= radius && Vector2.Angle(-transform.up, player.transform.position - transform.position) <= fov/2f && player.GetComponent<BoxCollider2D>().enabled)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, (player.transform.position - transform.position), distance, 1 << 8);
            if (hit.collider == null) {
                UIManager.instance.playerSpotted();
            }
        }     
    }

    void LateUpdate () {
        HashSet<Vector2> points = new HashSet<Vector2>();
        points.Add(Vector2.zero);
        Vector3 from = Quaternion.Euler(0, 0, -fov/2)*(-transform.up*radius) + transform.position;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, (from - transform.position), radius, 1 << 8);
        if (hit.collider != null) {
            points.Add(transform.InverseTransformPoint(hit.point));
            //getTilemapColliderPoints(hit, points);
        }
        else
        {
            points.Add(transform.InverseTransformPoint(from));
        }

        for (int step = 1; step <= sections; step++) {
            Vector2 targetDirection = Quaternion.Euler(0, 0, fov / sections * step) * (from - transform.position);
            hit = Physics2D.Raycast(transform.position, targetDirection, radius, 1<<8);
            if (hit.collider != null)
            {
                points.Add(transform.InverseTransformPoint(hit.point));
                //getTilemapColliderPoints(hit, points);
            }
            else {
                points.Add(transform.InverseTransformPoint(targetDirection + (Vector2)transform.position));
            }
        }
        Vector2[] mesh = new Vector2[points.Count];
        points.CopyTo(mesh);
        generateMesh(mesh);
    }

    private void getTilemapColliderPoints(RaycastHit2D hit, HashSet<Vector2> output) {
        Vector3Int tile = tilemap.WorldToCell(hit.point);
        Vector3 center = tilemap.GetCellCenterWorld(tile);
        Vector3 extents = tilemap.GetBoundsLocal(tile).extents;
        Debug.Log(extents);
        Debug.Log("Global Center: " + tilemap.GetCellCenterWorld(tile));
        Debug.DrawRay(transform.position, tilemap.GetCellCenterWorld(tile) - transform.position, Color.cyan, 0.6f);

        foreach (float x in new float[] { center.x - extents.x, center.x + extents.x } ) {
            foreach (float y in new float[] { center.y - extents.y, center.y + extents.y })
            {
                Vector3 corner = new Vector3(x, y, 0);
                if (Vector2.Angle(-transform.up, corner - transform.position) <= fov/2) {
                    RaycastHit2D hit2 = Physics2D.Raycast(transform.position, corner - transform.position, radius, 1 << 8);
                    if (hit2.collider != null)
                    {
                        output.Add(transform.InverseTransformPoint(hit2.point));
                        Debug.DrawRay(transform.position, (Vector3)hit2.point - transform.position, Color.blue, 0.6f);
                    }
                    else {
                        Debug.DrawRay(transform.position, (corner - transform.position), Color.green, 0.6f);
                    }
                    
                }
            }
        }
        
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject parent = transform.parent.gameObject;
        ExecuteEvents.Execute<OnConeCollision>(parent, null, (x, y) => x.OnConeCollision(collision));
    }

    void generateMesh(Vector2[] vertices2D)
    {

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
        if (cone_renderer == null)
            cone_renderer = gameObject.AddComponent(typeof(MeshRenderer)) as MeshRenderer;
        
        if (cone_filter == null)
            cone_filter = gameObject.AddComponent(typeof(MeshFilter)) as MeshFilter;

        cone_renderer.sortingOrder = order_in_layer;
        cone_filter.mesh = msh;
    }
}
