using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Leaf : PlantNode {

    private LineRenderer m_line;
    [SerializeField]
    Vector3[] bezierPoints = new Vector3[4];

    public Vector3 connectionPoint { get { return m_line.GetPosition(m_line.positionCount - 1); } }
    public Vector3 getPrevPoint { get { return bezierPoints[2]/*m_line.GetPosition(m_line.positionCount - 2)*/; } }

    void Start ()
    {
        m_line = GetComponent<LineRenderer>();
	}
	
	void Update ()
    {
		
	}

    public void GenerateCurve(Vector3 startPoint, Vector3 prevPoint, int segements, float height, float distance, int direction)
    {
        if(m_line == null)
            m_line = GetComponent<LineRenderer>();

        // Create bezier control points
        bezierPoints[0] = startPoint;
        bezierPoints[1] = startPoint != prevPoint ? GetOppositeDirection(startPoint - prevPoint)/* * distance * 0.5f*/
            : new Vector3(bezierPoints[0].x, bezierPoints[0].y + height*0.5f, bezierPoints[0].z);
        // 3 done before 2
        bezierPoints[3] = new Vector3(direction * (distance *0.5f + bezierPoints[0].x), bezierPoints[0].y + height, bezierPoints[0].z);
        // 2 based on 3
        bezierPoints[2] = new Vector3(direction * (bezierPoints[3].x - bezierPoints[1].x) / 2, (bezierPoints[3].y - bezierPoints[1].y) / 2, bezierPoints[0].z);

        // Create points on curve
        Vector3[] newPoints = new Vector3[segements];
        for(int i = 0; i < segements; i++)
        {
            newPoints[i] = GetBezierPoint(bezierPoints, (1.0f / segements) * i);
        }

        // Set curve points
        m_line.positionCount = newPoints.Length;
        m_line.SetPositions(newPoints);
    }

    public void SetCurveProperties(float width, Material mat)
    {
        if (m_line == null)
            m_line = GetComponent<LineRenderer>();

        m_line.startWidth = width;
        m_line.endWidth = width;
        m_line.material = mat;
        m_line.useWorldSpace = false;
    }

    public Vector3 GetBezierPoint(Vector3[] p, float t)
    {
        float omt = 1f - t;
        float omt2 = omt * omt;
        float t2 = t * t;
        return p[0] * (omt2 * omt) +
                p[1] * (3f * omt2 * t) +
                p[2] * (3f * omt * t2) +
                p[3] * (t2 * t);
    }

    public Vector3 GetOppositeDirection(Vector3 v)
    {
        return (-v);//.normalized;
    }
}
