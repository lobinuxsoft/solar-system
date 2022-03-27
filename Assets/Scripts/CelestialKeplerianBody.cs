using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(Rigidbody))]
public partial class CelestialKeplerianBody : MonoBehaviour
{
    [SerializeField] UniverseSettings settings;
    [Space]

    [Header("Keplerian data for orbit calculation")]
    [SerializeField] KeplerianBodySettings keplerianSettings = default;
    [Space]

    [SerializeField, Tooltip("Reference body for calculate the orbit")] Rigidbody referenceBody;
    [SerializeField] float meanAnomaly;
    [Space]

    [Header("Settings")]
    [SerializeField] float accuracyTolerance = 1e-6f;
    [SerializeField, Tooltip("usually converges after 3-5 iterations")] int maxIterations = 5;

    //Numbers which only change if orbit or mass changes
    [HideInInspector] [SerializeField] float mu;
    [HideInInspector] [SerializeField] float n, cosLOAN, sinLOAN, sinI, cosI, trueAnomalyConstant;

    Rigidbody body = default;
    MeshRenderer meshRenderer = default;
    TrailRenderer trailRenderer = default;

    public void SetKeplerianBodySettings(KeplerianBodySettings kbs, Rigidbody bodyRef = null)
    {
        keplerianSettings = kbs;

        body = GetComponent<Rigidbody>();
        body.useGravity = false;

        meshRenderer = GetComponent<MeshRenderer>();
        trailRenderer = GetComponent<TrailRenderer>();

        name = keplerianSettings.name;
        transform.localScale = Vector3.one * keplerianSettings.PlanetRadius;
        meshRenderer.material = keplerianSettings.PlanetMaterial;
        trailRenderer.startColor = keplerianSettings.OrbitColor;
        trailRenderer.endColor = keplerianSettings.OrbitColor;
        trailRenderer.startWidth = keplerianSettings.PlanetRadius * .6f;
        trailRenderer.endWidth = 0;
        body.mass = keplerianSettings.PlanetMass;

        referenceBody = bodyRef;

        if (referenceBody) CalculateSemiConstants();
    }

    /// <summary>
    /// Function f(x) = 0
    /// </summary>
    /// <param name="E"></param>
    /// <param name="e"></param>
    /// <param name="M"></param>
    /// <returns></returns>
    public float F(float E, float e, float M)
    {
        return (M - E + e * Mathf.Sin(E));
    }

    /// <summary>
    /// Derivative of the function
    /// </summary>
    /// <param name="E"></param>
    /// <param name="e"></param>
    /// <returns></returns>
    public float DF(float E, float e)
    {
        return (-1f) + e * Mathf.Cos(E);
    }

    /// <summary>
    /// Numbers that only need to be calculated once if the orbit doesn't change.
    /// </summary>
    public void CalculateSemiConstants()
    {
        mu = settings.GravitationalConstant * referenceBody.mass;
        n = Mathf.Sqrt(mu / Mathf.Pow(keplerianSettings.SemiMajorAxis, 3));
        trueAnomalyConstant = Mathf.Sqrt((1 + keplerianSettings.Eccentricity) / (1 - keplerianSettings.Eccentricity));
        cosLOAN = Mathf.Cos(keplerianSettings.LongitudeOfAcendingNode);
        sinLOAN = Mathf.Sin(keplerianSettings.LongitudeOfAcendingNode);
        cosI = Mathf.Cos(keplerianSettings.Inclination);
        sinI = Mathf.Sin(keplerianSettings.Inclination);
    }

    void Update()
    {
        Time.timeScale = settings.TimeScaleSimulation;

        if (referenceBody)
        {
            CalculateSemiConstants();

            meanAnomaly = (float)(n * (Time.time - keplerianSettings.MeanLongitude));

            float E1 = meanAnomaly;   //initial guess
            float difference = 1f;
            for (int i = 0; difference > accuracyTolerance && i < maxIterations; i++)
            {
                float E0 = E1;
                E1 = E0 - F(E0, keplerianSettings.Eccentricity, meanAnomaly) / DF(E0, keplerianSettings.Eccentricity);
                difference = Mathf.Abs(E1 - E0);
            }
            float EccentricAnomaly = E1;

            float trueAnomaly = 2 * Mathf.Atan(trueAnomalyConstant * Mathf.Tan(EccentricAnomaly / 2));
            float distance = keplerianSettings.SemiMajorAxis * (1 - keplerianSettings.Eccentricity * Mathf.Cos(EccentricAnomaly));

            float cosAOPPlusTA = Mathf.Cos(keplerianSettings.ArgumenOfPeriapsis + trueAnomaly);
            float sinAOPPlusTA = Mathf.Sin(keplerianSettings.ArgumenOfPeriapsis + trueAnomaly);

            float x = distance * ((cosLOAN * cosAOPPlusTA) - (sinLOAN * sinAOPPlusTA * cosI));
            float z = distance * ((sinLOAN * cosAOPPlusTA) + (cosLOAN * sinAOPPlusTA * cosI));      //Switching z and y to be aligned with xz not xy
            float y = distance * (sinI * sinAOPPlusTA);

            body.MovePosition(new Vector3(x, y, z) + referenceBody.position);
        }
    }

#if UNITY_EDITOR

    [Space, Header("Editor Settings")]
    [SerializeField] Color orbitColor = Color.white;
    [SerializeField] int orbitResolution = 50;
    List<Vector3> orbitalPoints = new List<Vector3>();

    private void OnDrawGizmos()
    {
        if (!keplerianSettings) return;

        orbitalPoints.Clear();

        if (orbitalPoints.Count == 0)
        {
            if (referenceBody == null)
            {
                Debug.LogWarning($"Add a reference body to {gameObject.name}");
                return;
            }

            CalculateSemiConstants();

            Vector3 pos = referenceBody.position;
            float orbitFraction = 1f / orbitResolution;

            for (int i = 0; i < orbitResolution + 1; i++)
            {
                float EccentricAnomaly = i * orbitFraction * Math.TAU;

                float trueAnomaly = 2 * Mathf.Atan(trueAnomalyConstant * Mathf.Tan(EccentricAnomaly / 2));
                float distance = keplerianSettings.SemiMajorAxis * (1 - keplerianSettings.Eccentricity * Mathf.Cos(EccentricAnomaly));

                float cosAOPPlusTA = Mathf.Cos(keplerianSettings.ArgumenOfPeriapsis + trueAnomaly);
                float sinAOPPlusTA = Mathf.Sin(keplerianSettings.ArgumenOfPeriapsis + trueAnomaly);

                float x = distance * ((cosLOAN * cosAOPPlusTA) - (sinLOAN * sinAOPPlusTA * cosI));
                float z = distance * ((sinLOAN * cosAOPPlusTA) + (cosLOAN * sinAOPPlusTA * cosI));
                float y = distance * (sinI * sinAOPPlusTA);

                float meanAnomaly = EccentricAnomaly - keplerianSettings.Eccentricity * Mathf.Sin(EccentricAnomaly);

                orbitalPoints.Add(pos + new Vector3(x, y, z));
            }
        }

        Handles.color = keplerianSettings.OrbitColor;
        Handles.Label(transform.position + Vector3.up * keplerianSettings.PlanetRadius, name);
        Handles.DrawAAPolyLine(orbitalPoints.ToArray());

        if (!Application.isPlaying)
        {
            if (!body) body = GetComponent<Rigidbody>();
            if (!meshRenderer) meshRenderer = GetComponent<MeshRenderer>();
            if (!trailRenderer) trailRenderer = GetComponent<TrailRenderer>();

            name = keplerianSettings.name;
            transform.position = orbitalPoints[0];
            transform.localScale = Vector3.one * keplerianSettings.PlanetRadius;
            meshRenderer.material = keplerianSettings.PlanetMaterial;
            trailRenderer.startColor = keplerianSettings.OrbitColor;
            trailRenderer.endColor = keplerianSettings.OrbitColor;
            trailRenderer.startWidth = keplerianSettings.PlanetRadius * .6f;
            trailRenderer.endWidth = 0;
            body.mass = keplerianSettings.PlanetMass;
        }
    }
#endif

}
