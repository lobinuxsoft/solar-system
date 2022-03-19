using UnityEngine;

public class UniverseSimulation : MonoBehaviour {

    [SerializeField] UniverseSettings universeSettings;
    CelestialBody[] bodies;
    static UniverseSimulation instance;

    void Awake () {

        bodies = FindObjectsOfType<CelestialBody> ();
        Time.timeScale = universeSettings.TimeScaleSimulation;
        Time.fixedDeltaTime = universeSettings.PhysicsTimeStep;
        Debug.Log ("Setting fixedDeltaTime to: " + universeSettings.PhysicsTimeStep);
    }

    private void Update()
    {
        if(universeSettings.TimeScaleSimulation != Time.timeScale) Time.timeScale = universeSettings.TimeScaleSimulation;
    }

    void FixedUpdate () {
        for (int i = 0; i < bodies.Length; i++) {
            Vector3 acceleration = CalculateAcceleration (bodies[i].Position, bodies[i]);
            bodies[i].UpdateVelocity (acceleration, universeSettings.PhysicsTimeStep);
        }

        for (int i = 0; i < bodies.Length; i++) {
            bodies[i].UpdatePosition (universeSettings.PhysicsTimeStep);
        }

    }

    public Vector3 CalculateAcceleration (Vector3 point, CelestialBody ignoreBody = null) {
        Vector3 acceleration = Vector3.zero;
        foreach (var body in Instance.bodies) {
            if (body != ignoreBody) {
                float sqrDst = (body.Position - point).sqrMagnitude;
                Vector3 forceDir = (body.Position - point).normalized;
                acceleration += forceDir * universeSettings.GravitationalConstant * body.Mass / sqrDst;
            }
        }

        return acceleration;
    }

    public static CelestialBody[] Bodies {
        get {
            return Instance.bodies;
        }
    }

    static UniverseSimulation Instance {
        get {
            if (instance == null) {
                instance = FindObjectOfType<UniverseSimulation> ();
            }
            return instance;
        }
    }
}