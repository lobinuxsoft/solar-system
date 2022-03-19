using UnityEngine;

public class OrbitDebugDisplay : MonoBehaviour {

    [SerializeField] OrbitDebugDisplaySettings settings;
    [SerializeField] UniverseSettings universeSettings;
    [SerializeField] CelestialBody centralBody;
 
    bool relativeToBody;

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying)
        {
            DrawOrbits();
        }
    }

    void DrawOrbits () {

        relativeToBody = centralBody ? true : false;

        CelestialBody[] bodies = FindObjectsOfType<CelestialBody> ();
        var virtualBodies = new VirtualBody[bodies.Length];
        var drawPoints = new Vector3[bodies.Length][];
        int referenceFrameIndex = 0;
        Vector3 referenceBodyInitialPosition = Vector3.zero;

        // Initialize virtual bodies (don't want to move the actual bodies)
        for (int i = 0; i < virtualBodies.Length; i++) {
            virtualBodies[i] = new VirtualBody (bodies[i]);
            drawPoints[i] = new Vector3[settings.NumSteps];

            if (bodies[i] == centralBody && relativeToBody) {
                referenceFrameIndex = i;
                referenceBodyInitialPosition = virtualBodies[i].position;
            }
        }

        // Simulate
        for (int step = 0; step < settings.NumSteps; step++) {
            Vector3 referenceBodyPosition = (relativeToBody) ? virtualBodies[referenceFrameIndex].position : Vector3.zero;
            // Update velocities
            for (int i = 0; i < virtualBodies.Length; i++) {
                virtualBodies[i].velocity += CalculateAcceleration (i, virtualBodies) * settings.TimeStep;
            }
            // Update positions
            for (int i = 0; i < virtualBodies.Length; i++) {
                Vector3 newPos = virtualBodies[i].position + virtualBodies[i].velocity * settings.TimeStep;
                virtualBodies[i].position = newPos;
                if (relativeToBody) {
                    var referenceFrameOffset = referenceBodyPosition - referenceBodyInitialPosition;
                    newPos -= referenceFrameOffset;
                }
                if (relativeToBody && i == referenceFrameIndex) {
                    newPos = referenceBodyInitialPosition;
                }

                drawPoints[i][step] = newPos;
            }
        }

        // Draw paths
        for (int bodyIndex = 0; bodyIndex < virtualBodies.Length; bodyIndex++) {
            var pathColour = settings.DebugGradient.Evaluate((float)bodyIndex / (virtualBodies.Length - 1)); //bodies[bodyIndex].gameObject.GetComponentInChildren<MeshRenderer> ().sharedMaterial.color; //

            for (int i = 0; i < drawPoints[bodyIndex].Length - 1; i++)
            {
                Gizmos.color = pathColour;
                if (i % (settings.NumSteps / 20) == 0) Gizmos.DrawSphere(drawPoints[bodyIndex][i], virtualBodies[bodyIndex].radius / 2); // Visualice spheres for celestial dimensions
                Gizmos.DrawLine(drawPoints[bodyIndex][i], drawPoints[bodyIndex][i + 1]);
            }
        }
    }

    Vector3 CalculateAcceleration (int i, VirtualBody[] virtualBodies) {
        Vector3 acceleration = Vector3.zero;
        for (int j = 0; j < virtualBodies.Length; j++) {
            if (i == j) {
                continue;
            }
            Vector3 forceDir = (virtualBodies[j].position - virtualBodies[i].position).normalized;
            float sqrDst = (virtualBodies[j].position - virtualBodies[i].position).sqrMagnitude;
            acceleration += forceDir * universeSettings.GravitationalConstant * virtualBodies[j].mass / sqrDst;
        }
        return acceleration;
    }

    void OnValidate () {
        if (settings.UsePhysicsTimeStep) {
            settings.TimeStep = universeSettings.PhysicsTimeStep;
        }
    }

    class VirtualBody {
        public Vector3 position;
        public Vector3 velocity;
        public float radius;
        public float mass;

        public VirtualBody (CelestialBody body) {
            position = body.transform.position;
            velocity = body.InitialVelocity;
            radius = body.Radius;
            mass = body.Mass;
        }
    }
}