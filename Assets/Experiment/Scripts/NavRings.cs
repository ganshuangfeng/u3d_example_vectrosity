using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vectrosity;

public class NavRings : MonoBehaviour {
    public int ringVertices = 120;
    public float ringWidth = 1f;
    public float markWidth = 2f;
    public float ringRadius = 10f;
    public float ringMarkCount = 36f;
    public float ringMarkLengthFactor = 0.1f;
    public Material lineMaterial;

    private float markAngle;
    private VectorLine marks;

    void Start() {
        markAngle = 360f / ringMarkCount;
        setupRing("axisX", Vector3.right);
        setupRing("axisY", Vector3.up);
        setupRing("axisZ", Vector3.forward);

        marks = setupMarks("axisMarkers");
    }

    private void setupRing(string id, Vector3 upVector) {
        var ring = new VectorLine(id, new List<Vector3>(ringVertices), ringWidth, LineType.Continuous, Joins.Weld);
        ring.material = lineMaterial;
        ring.MakeCircle(Vector3.zero, upVector, ringRadius);
        // ring.Draw3DAuto();

        GameObject container = new GameObject(id);
        container.transform.parent = transform;
        container.transform.localPosition = Vector3.zero;
        VectorManager.useDraw3D = true;
        VectorManager.ObjectSetup(container, ring, Visibility.Always, Brightness.None);
    }

    private VectorLine setupMarks(string id) {
        var markLength = ringRadius * ringMarkLengthFactor;
        var innerRad = ringRadius - markLength;
        var points = new List<Vector3>();
        for (int i = 0; i < ringMarkCount; i++) {
            var angle = Mathf.Deg2Rad * markAngle * i + (Mathf.PI / 2f);
            var xAngle = Mathf.Cos(angle);
            var yAngle = Mathf.Sin(angle);
            var x1 = new Vector3(0, ringRadius * xAngle, ringRadius * yAngle);
            var x2 = new Vector3(0, innerRad * xAngle, innerRad * yAngle);
            var y1 = new Vector3(ringRadius * xAngle, 0, ringRadius * yAngle);
            var y2 = new Vector3(innerRad * xAngle, 0, innerRad * yAngle);
            var z1 = new Vector3(ringRadius * xAngle, ringRadius * yAngle, 0);
            var z2 = new Vector3(innerRad * xAngle, innerRad * yAngle, 0);
            points.Add(x1);
            points.Add(x2);
            points.Add(y1);
            points.Add(y2);
            points.Add(z1);
            points.Add(z2);
        }

        var marks = new VectorLine(id, points, markWidth);
        GameObject container = new GameObject(id);
        container.transform.parent = transform;
        container.transform.localPosition = Vector3.zero;
        VectorManager.useDraw3D = true;
        VectorManager.ObjectSetup(container, marks, Visibility.Always, Brightness.None);

        return marks;
    }

    void Update() {
        int index;
        if (Input.GetMouseButtonDown(0) && marks.Selected(Input.mousePosition, 5, out index)) {
            // rotation axis: 0 = x, 1 = y, 2 = z
            var axisIndex = index % 3;

            var stop = index / 3;
            // angle: from forward for x and y and up for z
            var angle = stop * markAngle;

            Debug.Log($"Selected line index: {index} axis: {axisIndex} axisAngle: {angle}");
        }
    }
}
