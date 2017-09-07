using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LineCurve : MonoBehaviour {

    private LineRenderer m_line;

    public int m_numOfPoints = 5;

	void Start () {
        m_line = GetComponent<LineRenderer>();
	}
	
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("CLicking");
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 10;
            AddNewPoint(Camera.main.ScreenToWorldPoint(mousePos));
        }
	}

    private void AddNewPoint(Vector3 newPoint)
    {
        if(m_line != null)
            Debug.Log("Adding new Point");

        int startIndex = m_line.positionCount;

        if(startIndex == 0)
        {
            Vector3[] newPoints = new Vector3[1];
            newPoints[0] = newPoint;
            m_line.positionCount = newPoints.Length;
            m_line.SetPositions(newPoints);
            return;
        }

        Vector3[] existingPoints = new Vector3[startIndex + m_numOfPoints - 1];
        m_line.GetPositions(existingPoints);

        Vector3[] prevPoint = new Vector3[m_numOfPoints];
        if (existingPoints.Length > m_numOfPoints)
        {
            for (int i = 0; i < m_numOfPoints; i++)
            {
                prevPoint[i] = existingPoints[startIndex - (m_numOfPoints - 1 - i)];
            }
        }

        Vector3[] points = new Vector3[4];
        points[0] = existingPoints[startIndex - 1];
        points[1] = startIndex - m_numOfPoints >= 0 ? /*new Vector3(points[0].x + ((-1) * existingPoints[startIndex - 2].x), points[0].y, points[0].z)*/
            GetTangent(prevPoint, (1.0f / m_numOfPoints))
            : new Vector3(points[0].x, points[0].y, points[0].z);
        points[2] = new Vector3((points[1].x - newPoint.x) / 2, (points[0].y - newPoint.y) / 2, points[0].z);
        points[3] = newPoint;

        for (int i = startIndex; i < existingPoints.Length; i++)
        {
            existingPoints[i] = GetBezierPoint(points, (1.0f / (m_numOfPoints - 2)) * (i - startIndex + 1));
        }

        m_line.positionCount = existingPoints.Length;
        m_line.SetPositions(existingPoints);
    }

    private Vector3 GetBezierPoint(Vector3[] p, float t)
    {
        //Vector3 p1 = new Vector3(p[0].x, (p[0].y + p[3].y) / 2.0f, p[0].z);//(p0.z + p3.z) / 2.0f);
        //Vector3 p2 = new Vector3((p[0].x + p[3].x) / 2.0f, p[3].y, p[0].z);// (p0.z + p3.z) / 2.0f);

        float omt = 1f - t;
        float omt2 = omt * omt;
        float t2 = t * t;
        return p[0] * (omt2 * omt) +
                p[1] * (3f * omt2 * t) +
                p[2] * (3f * omt * t2) +
                p[3] * (t2 * t);
    }

    Vector3 GetTangent(Vector3[] pts, float t)
    {
        float omt = 1f - t;
        float omt2 = omt * omt;
        float t2 = t * t;
        Vector3 tangent =
                    pts[0] * (-omt2) +
                    pts[1] * (3 * omt2 - 2 * omt) +
                    pts[2] * (-3 * t2 + 2 * t) +
                    pts[3] * (t2);
        return tangent.normalized;
    }
}
