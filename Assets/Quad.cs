using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class Quad : MonoBehaviour {

    private MeshFilter m_mf;
    private Mesh m_mesh;
    private MeshRenderer m_renderer;

    private Vector3[] m_finalVerts = new Vector3[4];
    private Vector3[] m_finalNormals = new Vector3[4];
    private Vector2[] m_finalUVs = new Vector2[4];

    private Vector3[] m_workingVerts = new Vector3[4];
    private Vector3[] m_workingNormals = new Vector3[4];
    private Vector2[] m_workingUVs = new Vector2[4];

    private int[] m_triangles = new int[] {
        0, 2, 3,
        3, 1, 0
    };

    public Color m_nextColor;

    public Vector3[] m_startVerts = new Vector3[4];
    public Vector3[] m_startNormals = new Vector3[4];
    public Vector2[] m_startUVs = new Vector2[4];

    public Vector2 m_vertLimits = new Vector2(-1, 1);
    public Vector2 m_normalLimits = new Vector2(-1, 1);
    public Vector2 m_uvLimits = new Vector2(-1, 1);

    public float m_speed = 1.0f;

    void Start() {
        m_mf = GetComponent<MeshFilter>();
        if (m_mf.sharedMesh == null)
        {
            m_mf.sharedMesh = new Mesh();
        }
        m_mesh = m_mf.sharedMesh;
        m_renderer = GetComponent<MeshRenderer>();

        GenerateFinalPoints();
        m_nextColor = GetRandomColor();

        m_mesh.Clear();
        m_mesh.vertices = m_startVerts;
        m_mesh.normals = m_startNormals;
        m_mesh.uv = m_startUVs;
        m_mesh.triangles = m_triangles;
    }

    void Update()
    {
        m_workingVerts = m_mesh.vertices;
        m_workingNormals = m_mesh.normals;

        m_mesh.Clear();
        m_mesh.vertices = LerpVectorArray(m_workingVerts, m_finalVerts, m_speed * Time.deltaTime);
        m_mesh.normals = LerpVectorArray(m_workingNormals, m_finalNormals, m_speed * Time.deltaTime);
        m_mesh.uv = m_startUVs;
        m_mesh.triangles = m_triangles;

        m_renderer.materials[0].color = Color.Lerp(m_renderer.material.color, m_nextColor, m_speed * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            GenerateFinalPoints();
            m_nextColor = GetRandomColor();
        }
    }

    private void GenerateFinalPoints()
    {
        for(int i = 0; i < 4; i++)
        {
            m_finalVerts[i] = new Vector3(GetRandomNum(m_vertLimits), GetRandomNum(m_vertLimits), GetRandomNum(m_vertLimits));
            m_finalNormals[i] = new Vector3(GetRandomNum(m_uvLimits), GetRandomNum(m_uvLimits), GetRandomNum(m_uvLimits));
            m_finalUVs[i] = new Vector2(GetRandomNum(m_uvLimits), GetRandomNum(m_uvLimits));
        }
    }

    private float GetRandomNum(Vector2 range)
    {
        return Random.Range(range.x, range.y);
    }

    private Vector3[] LerpVectorArray(Vector3[] a, Vector3[] b, float t)
    {
        Vector3[] result = new Vector3[(a.Length < b.Length ? a.Length : b.Length)];

        for(int i = 0; i < result.Length; i++)
        {
            result[i] = Vector3.Lerp(a[i], b[i], t);
        }

        return result;
    }

    private Vector2[] LerpVectorArray(Vector2[] a, Vector2[] b, float t)
    {
        Vector2[] result = new Vector2[(a.Length < b.Length ? a.Length : b.Length)];

        for (int i = 0; i < result.Length; i++)
        {
            result[i] = Vector2.Lerp(a[i], b[i], t);
        }

        return result;
    }

    private Color GetRandomColor()
    {
        return new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
    }
}
