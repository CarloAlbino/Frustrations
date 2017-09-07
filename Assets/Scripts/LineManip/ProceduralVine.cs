using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralVine : MonoBehaviour {

    [SerializeField]
    private Leaf m_base = null;

    public int m_minSegements = 5;
    public int m_maxSegements = 12;

    public float m_minHeight = 1.0f;
    public float m_maxHeight = 2.0f;

    public float m_minDistance = 0.5f;
    public float m_maxDistance = 1.0f;

    public float m_curveWidth = 0.5f;
    public Material m_curveMaterial;

	void Update () {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (m_base == null)
            {
                int segements = Random.Range(m_minSegements, m_maxSegements);
                float height = Random.Range(m_minHeight, m_maxHeight);
                float distance = Random.Range(m_minDistance, m_maxDistance);

                GameObject newNode = new GameObject();
                newNode.transform.parent = this.transform;

                newNode.AddComponent<Leaf>();
                m_base = newNode.GetComponent<Leaf>();
                m_base.nodeType = NodeType.Leaf;
                m_base.gameObject.name = "Plant Base";

                m_base.SetCurveProperties(m_curveWidth, m_curveMaterial);
                m_base.GenerateCurve(transform.position, transform.position, segements, height, distance, (Random.Range(0, 2) == 0) ? -1 : 1);
            }
            else
            {
                List<PlantNode> leafs = m_base.GetLeafs(m_base);

                foreach(PlantNode pn in leafs)
                {
                    int randomBranching = Random.Range(0, 4);

                    if (randomBranching == 0)
                    {
                        pn.nodeType = NodeType.Flower;
                    }

                    for (int i = 0; i < randomBranching; i++)
                    {
                        int segements = Random.Range(m_minSegements, m_maxSegements);
                        float height = Random.Range(m_minHeight, m_maxHeight);
                        float distance = Random.Range(m_minDistance, m_maxDistance);

                        GameObject newNode = new GameObject();
                        newNode.name = "Stem";
                        newNode.transform.parent = pn.transform;
                        newNode.AddComponent<Leaf>();

                        Leaf newLeaf = newNode.GetComponent<Leaf>();
                        newLeaf.nodeType = NodeType.Leaf;
                        newLeaf.parent = pn;

                        newLeaf.transform.position = pn.transform.position + ((Leaf)pn).connectionPoint;
                        newLeaf.SetCurveProperties(m_curveWidth, m_curveMaterial);
                        newLeaf.GenerateCurve(Vector3.zero, Vector3.zero/*newLeaf.transform.position, newLeaf.transform.position/*((Leaf)pn).getPrevPoint*/, segements, height, distance, (Random.Range(0, 2) == 0) ? -1 : 1);

                        pn.children.Add(newLeaf);
                    }
                }
            }
        }

	}
}
