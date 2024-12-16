using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTrailEffect : MonoBehaviour
{
    [field: SerializeField] private float decayRate { get; set; }
    private LineRenderer lineRenderer;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (lineRenderer.endColor.a <= 0)
        {
            Destroy(this.gameObject);
        }
        lineRenderer.endColor = new Color(lineRenderer.endColor.r, lineRenderer.endColor.g, lineRenderer.endColor.b, lineRenderer.endColor.a - decayRate * Time.deltaTime);
    }
}
