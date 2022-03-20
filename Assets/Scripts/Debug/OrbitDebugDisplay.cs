using UnityEngine;

public partial class OrbitDebugDisplay : MonoBehaviour
{
    [SerializeField] OrbitDebugBehaviour orbitDebugBehaviour;
    [SerializeField] CelestialBody centralBody;

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying)
        {
            orbitDebugBehaviour.DrawOrbits(centralBody);
        }
    }
}