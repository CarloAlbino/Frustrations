using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flower : MonoBehaviour {
    private GameObject[] m_petals;

    public Vector3[] m_startVerts = new Vector3[5];
    public int m_numOfPetals = 8;
    
    void Start()
    {
        m_petals = new GameObject[m_numOfPetals];

        for(int i = 0; i < m_numOfPetals; i++)
        {
            m_petals[i] = new GameObject();
            m_petals[i].transform.parent = this.transform;

            GameObject petalMesh = new GameObject();
            petalMesh.transform.parent = m_petals[i].transform;

            petalMesh.AddComponent<FlowerPetal>();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //int turnDeg = 360 / m_numOfPetals;

            //for (int i = 0; i < m_numOfPetals; i++)
            //{
            //    m_petals[i].GetComponentInChildren<FlowerPetal>().SetPetalVerts(m_startVerts);
            //    m_petals[i].transform.eulerAngles = new Vector3(0, 65.0f, turnDeg * i);
            //}

            StartCoroutine(Cycle(1));
        }
    }

    IEnumerator Cycle(float time)
    {
        int turnDeg = 360 / m_numOfPetals;

        for (int i = 0; i < m_numOfPetals; i++)
        {
            yield return new WaitForSeconds(time);
            m_petals[i].GetComponentInChildren<FlowerPetal>().SetPetalVerts(m_startVerts);
            m_petals[i].transform.eulerAngles = new Vector3(0, 0, turnDeg * i);
        }
    }
}
