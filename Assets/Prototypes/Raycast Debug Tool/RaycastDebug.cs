using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RaycastDebugMode
{
    Raycast,
    OverlapSphere
}

public class RaycastDebug : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer;

    [SerializeField] private Transform hitVisual;

    [SerializeField] private Transform origin;

    [Header("CUSTOMIZE")]
    [SerializeField] private RaycastDebugMode raycastDebugMode;
    [SerializeField] private float maxDistance;
    [SerializeField] private LayerMask checkLayerMask;
    [SerializeField] private float updateInterval;

    [Header("CUSTOMIZE - COLOR")]
    [SerializeField] private Color normalDebugRaycastColor;
    [SerializeField] private Color blockedByObstacleDebugRaycastColor;

    [Header("CUSTOMIZE - OVERLAP SPHERE")]
    [SerializeField] private float radiusOverlapSphere;

    #region PRIVATE FIELD
    private List<Vector3> lineRendererPoints;
    private List<GradientColorKey> lineRendererGradientColorKeys;
    #endregion

    private void Awake()
    {
        lineRendererPoints = new List<Vector3>();
        lineRendererGradientColorKeys = new List<GradientColorKey>();

        if (raycastDebugMode == RaycastDebugMode.Raycast)
        {
            StartCoroutine(Debugging());
        }
        else
        {

        }
    }

    private IEnumerator Debugging()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(updateInterval);

        lineRenderer.gameObject.SetActive(true);

        while (true)
        {
            Ray ray = new Ray(origin.position, direction: origin.forward);

            lineRendererPoints.Clear();
            lineRendererGradientColorKeys.Clear();

            lineRendererPoints.Add(origin.position);
            lineRendererGradientColorKeys.Add(new GradientColorKey(normalDebugRaycastColor, 0));

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                lineRendererPoints.Add(hit.point);
                lineRendererGradientColorKeys.Add(new GradientColorKey(normalDebugRaycastColor, Vector3.Distance(hit.point, origin.position) / maxDistance));

                lineRendererPoints.Add(origin.position + maxDistance * origin.forward);
                lineRendererGradientColorKeys.Add(new GradientColorKey(blockedByObstacleDebugRaycastColor, 1));

                hitVisual.position = hit.point;
                hitVisual.gameObject.SetActive(true);
            }
            else
            {
                lineRendererPoints.Add(origin.position + maxDistance * origin.forward);
                lineRendererGradientColorKeys.Add(new GradientColorKey(normalDebugRaycastColor, 1));

                hitVisual.gameObject.SetActive(false);
            }

            lineRenderer.positionCount = lineRendererPoints.Count;
            lineRenderer.SetPositions(lineRendererPoints.ToArray());

            Gradient gradient = new Gradient();

            gradient.colorKeys = lineRendererGradientColorKeys.ToArray();

            lineRenderer.colorGradient = gradient;

            yield return waitForSeconds;
        }
    }

    private IEnumerator DebuggingSphereOverlapCast()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(updateInterval);

        while (true)
        {
            Collider[] colliders = Physics.OverlapSphere(origin.position, 5);
        }
    }
}
