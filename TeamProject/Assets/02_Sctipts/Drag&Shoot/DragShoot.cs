using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class DragShoot : MonoBehaviour
{
    public Vector3 startPos = Vector3.zero;
    public Vector3 endPos = Vector3.zero;
    public Vector3 camOffset = new Vector3(0f, 0f, 10f);

    private LineRenderer lineRenderer;

    private void Start()
    {
        //lineRenderer = GetComponentInChildren<LineRenderer>();
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            lineRenderer.enabled = true;
            startPos = MouseScreenValue();
            lineRenderer.SetPosition(0, startPos);
        }

        if (Input.GetMouseButton(0))
        {
            endPos = MouseScreenValue();
            lineRenderer.SetPosition(1, endPos);
        }

        if (Input.GetMouseButtonUp(0))
        {
            lineRenderer.enabled = false;
        }
    }

    private Vector3 MouseScreenValue()
    {
        Vector3 _pos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));
        return _pos;
    }
}
