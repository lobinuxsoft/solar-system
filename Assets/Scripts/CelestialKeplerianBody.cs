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

    [Header("Orbital Keplerian Parameters")]
    
    [SerializeField,Tooltip("a - size")] float semiMajorAxis = 20f;
    [SerializeField, Tooltip("e - shape")][Range(0f, 0.99f)] float eccentricity;
    [SerializeField, Tooltip("i - tilt")][Range(0f, Math.TAU)] float inclination = 0f;
    [SerializeField, Tooltip("n - swivel")][Range(0f, Math.TAU)] float longitudeOfAcendingNode;
    [SerializeField, Tooltip("w - position")][Range(0f, Math.TAU)] float argumentOfPeriapsis;
    [SerializeField, Tooltip("L - offset")] float meanLongitude;
    [SerializeField, Tooltip("Reference boy for calculate the orbit")] Rigidbody referenceBody;
    [SerializeField] float meanAnomaly;
    [Space]

    [Header("Settings")]
    [SerializeField] float accuracyTolerance = 1e-6f;
    [SerializeField, Tooltip("usually converges after 3-5 iterations")] int maxIterations = 5;

    //Numbers which only change if orbit or mass changes
    [HideInInspector] [SerializeField] float mu;
    [HideInInspector] [SerializeField] float n, cosLOAN, sinLOAN, sinI, cosI, trueAnomalyConstant;

    Rigidbody body = default;

    private void OnValidate() => orbitalPoints.Clear();

    void Awake()
    {
        body = GetComponent<Rigidbody>();
        body.useGravity = false;

        if(referenceBody) CalculateSemiConstants();
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
        n = Mathf.Sqrt(mu / Mathf.Pow(semiMajorAxis, 3));
        trueAnomalyConstant = Mathf.Sqrt((1 + eccentricity) / (1 - eccentricity));
        cosLOAN = Mathf.Cos(longitudeOfAcendingNode);
        sinLOAN = Mathf.Sin(longitudeOfAcendingNode);
        cosI = Mathf.Cos(inclination);
        sinI = Mathf.Sin(inclination);
    }

    void Update()
    {
        Time.timeScale = settings.TimeScaleSimulation;

        if (referenceBody)
        {
            CalculateSemiConstants();

            meanAnomaly = (float)(n * (Time.time - meanLongitude));

            float E1 = meanAnomaly;   //initial guess
            float difference = 1f;
            for (int i = 0; difference > accuracyTolerance && i < maxIterations; i++)
            {
                float E0 = E1;
                E1 = E0 - F(E0, eccentricity, meanAnomaly) / DF(E0, eccentricity);
                difference = Mathf.Abs(E1 - E0);
            }
            float EccentricAnomaly = E1;

            float trueAnomaly = 2 * Mathf.Atan(trueAnomalyConstant * Mathf.Tan(EccentricAnomaly / 2));
            float distance = semiMajorAxis * (1 - eccentricity * Mathf.Cos(EccentricAnomaly));

            float cosAOPPlusTA = Mathf.Cos(argumentOfPeriapsis + trueAnomaly);
            float sinAOPPlusTA = Mathf.Sin(argumentOfPeriapsis + trueAnomaly);

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
                float distance = semiMajorAxis * (1 - eccentricity * Mathf.Cos(EccentricAnomaly));

                float cosAOPPlusTA = Mathf.Cos(argumentOfPeriapsis + trueAnomaly);
                float sinAOPPlusTA = Mathf.Sin(argumentOfPeriapsis + trueAnomaly);

                float x = distance * ((cosLOAN * cosAOPPlusTA) - (sinLOAN * sinAOPPlusTA * cosI));
                float z = distance * ((sinLOAN * cosAOPPlusTA) + (cosLOAN * sinAOPPlusTA * cosI));
                float y = distance * (sinI * sinAOPPlusTA);

                float meanAnomaly = EccentricAnomaly - eccentricity * Mathf.Sin(EccentricAnomaly);

                orbitalPoints.Add(pos + new Vector3(x, y, z));
            }
        }
        Handles.color = orbitColor;
        Handles.DrawAAPolyLine(orbitalPoints.ToArray());
        
        if(!Application.isPlaying) transform.position = orbitalPoints[0];
    }
#endif

}
