using UnityEngine;
using System.Collections;

public class ScreenMesh : MonoBehaviour {

    public Transform[] points;
    private Vector3[] newVertices;

    public Vector2[] newUV;
    public int[] newTriangles;
    private MeshFilter mf;

    void Start()
    {
        newVertices = new Vector3[points.Length];
        for (int i = 0; i < points.Length; i++)
        {
            newVertices[i] = points[i].position;
        }

        Mesh mesh = new Mesh();
        mf = GetComponent<MeshFilter>();
        mf.mesh = mesh;
        mesh.vertices = newVertices;
        mesh.uv = newUV;
        mesh.triangles = newTriangles;
    }
	
	// Update is called once per frame
	void Update () {
        for (int i = 0; i < points.Length; i++)
        {
            newVertices[i] = points[i].position;
        }
        mf.mesh.vertices = newVertices;
	}
}
