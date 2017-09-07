using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralVine : MonoBehaviour {

    [SerializeField]
    private List<Leaf> m_leafs = new List<Leaf>();

    public int m_minSegements = 5;
    public int m_maxSegements = 12;

    public float m_minHeight = 1.0f;
    public float m_maxHeight = 2.0f;

    public float m_minDistance = 0.5f;
    public float m_maxDistance = 1.0f;

    public float m_curveWidth = 0.5f;
    public Material m_curveMaterial;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject newNode = new GameObject();
            newNode.transform.parent = this.transform;

            newNode.AddComponent<Leaf>();
            m_leafs.Add(newNode.GetComponent<Leaf>());

            int segements = Random.Range(m_minSegements, m_maxSegements);
            float height = Random.Range(m_minHeight, m_maxHeight);
            float distance = Random.Range(m_minDistance, m_maxDistance);

            if (m_leafs.Count <= 1)
            {
                m_leafs[m_leafs.Count - 1].SetCurveProperties(m_curveWidth, m_curveMaterial);
                m_leafs[m_leafs.Count - 1].GenerateCurve(transform.position, transform.position, segements, height, distance, (Random.Range(0, 2) == 0) ? -1 : 1);
            }
            else
            {
                m_leafs[m_leafs.Count - 1].transform.position = m_leafs[m_leafs.Count - 2].transform.position + m_leafs[m_leafs.Count - 2].connectionPoint;
                m_leafs[m_leafs.Count - 1].SetCurveProperties(m_curveWidth, m_curveMaterial);
                m_leafs[m_leafs.Count - 1].GenerateCurve(transform.position, transform.position + m_leafs[m_leafs.Count - 2].connectionPoint, segements, height, distance, (Random.Range(0, 2) == 0) ? -1 : 1);
            }
        }

	}
}
