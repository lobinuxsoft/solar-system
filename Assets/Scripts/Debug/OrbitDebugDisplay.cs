using UnityEngine;

public partial class OrbitDebugDisplay : MonoBehaviour
{
    [SerializeField] UniverseSimulationDebugBase orbitDebugBehaviour;
    [SerializeField] CelestialBody centralBody;

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying)
        {
            orbitDebugBehaviour.DebugDraw(centralBody);
        }
    }
}