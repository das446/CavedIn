using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridOverlay : MonoBehaviour {

	public bool show = true;

    public float startX, startY, stopX, stopY;
    public float Step;
    public Material m;
	public float lineWidth;
    public void DrawGrid()
    {
        for (float x = startX; x <= stopX; x += Step)
        {
            GameObject go = new GameObject();
            go.transform.parent = transform;
            LineRenderer lr = go.AddComponent<LineRenderer>();
            lr.material = m;
            lr.startColor = Color.green;
            lr.endColor = Color.green;
			lr.startWidth = lineWidth;
            lr.endWidth = lineWidth;
            // the Z value is a bad hack to fix the order stuff is drawn
            lr.SetPosition(0, new Vector3(x, startY, 100));
            lr.SetPosition(1, new Vector3(x, stopY, 100));
        }
        for (float y = startY; y <= stopY; y += Step)
        {
            GameObject go = new GameObject();
            go.transform.parent = transform;
            LineRenderer lr = go.AddComponent<LineRenderer>();
			lr.startWidth=lineWidth;
			lr.endWidth=lineWidth;
            lr.material = m;
            lr.startColor = Color.green;
            lr.endColor = Color.green;
            lr.SetPosition(0, new Vector3(startX, y, 100));
            lr.SetPosition(1, new Vector3(stopX, y, 100));
            
        }
    }
}
