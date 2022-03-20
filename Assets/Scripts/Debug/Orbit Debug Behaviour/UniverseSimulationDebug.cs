using UnityEngine;

[CreateAssetMenu(fileName = "New Universal Simulation Debug", menuName = "Lobby Tools/ Orbit Debug Display/ Universal Simulation Debug")]
public class UniverseSimulationDebug : UniverseSimulationDebugBase
{
    
    public override void DebugDraw(CelestialBody centralBody)
    {
        relativeToBody = centralBody ? true : false;

        CelestialBody[] bodies = FindObjectsOfType<CelestialBody>();

        var virtualBodies = new VirtualBody[bodies.Length];
        var drawPoints = new Vector3[bodies.Length][];
        int referenceFrameIndex = 0;
        Vector3 referenceBodyInitialPosition = Vector3.zero;

        // Initialize virtual bodies (don't want to move the actual bodies)
        for (int i = 0; i < virtualBodies.Length; i++)
        {
            virtualBodies[i] = new VirtualBody(bodies[i]);
            drawPoints[i] = new Vector3[numSteps];

            if (bodies[i] == centralBody && relativeToBody)
            {
                referenceFrameIndex = i;
                referenceBodyInitialPosition = virtualBodies[i].position;
            }
        }

        // Simulate
        for (int step = 0; step < numSteps; step++)
        {
            Vector3 referenceBodyPosition = (relativeToBody) ? virtualBodies[referenceFrameIndex].position : Vector3.zero;
            // Update velocities
            for (int i = 0; i < virtualBodies.Length; i++)
            {
                virtualBodies[i].velocity += CalculateAcceleration(i, virtualBodies) * timeStep;
            }
            // Update positions
            for (int i = 0; i < virtualBodies.Length; i++)
            {
                Vector3 newPos = virtualBodies[i].position + virtualBodies[i].velocity * timeStep;
                virtualBodies[i].position = newPos;
                if (relativeToBody)
                {
                    var referenceFrameOffset = referenceBodyPosition - referenceBodyInitialPosition;
                    newPos -= referenceFrameOffset;
                }
                if (relativeToBody && i == referenceFrameIndex)
                {
                    newPos = referenceBodyInitialPosition;
                }

                drawPoints[i][step] = newPos;
            }
        }

        // Draw paths
        for (int bodyIndex = 0; bodyIndex < virtualBodies.Length; bodyIndex++)
        {
            var pathColour = debugGradient.Evaluate((float)bodyIndex / (virtualBodies.Length - 1));

            for (int i = 0; i < drawPoints[bodyIndex].Length - 1; i++)
            {
                Gizmos.color = pathColour;
                if (i % (numSteps / 20) == 0) Gizmos.DrawSphere(drawPoints[bodyIndex][i], virtualBodies[bodyIndex].radius / 2); // Visualice spheres for celestial dimensions
                Gizmos.DrawLine(drawPoints[bodyIndex][i], drawPoints[bodyIndex][i + 1]);
            }
        }
    }

    Vector3 CalculateAcceleration(int i, VirtualBody[] virtualBodies)
    {
        Vector3 acceleration = Vector3.zero;

        for (int j = 0; j < virtualBodies.Length; j++)
        {
            if (i == j)
            {
                continue;
            }

            Vector3 forceDir = (virtualBodies[j].position - virtualBodies[i].position).normalized;
            float sqrDst = (virtualBodies[j].position - virtualBodies[i].position).sqrMagnitude;
            acceleration += forceDir * universeSettings.GravitationalConstant * virtualBodies[j].mass / sqrDst;
        }

        return acceleration;
    }
}
