using UnityEngine;

public class CelestialBody : MonoBehaviour
{
    [SerializeField] float mass;
    [SerializeField] float radius;
    [SerializeField] Vector3 initialVelocity;

    Vector3 currentVelocity;

    private void Awake()
    {
        currentVelocity = initialVelocity;
    }

    public void UpdateVelocity(CelestialBody[] allBodies, float timeStep)
    {
        foreach (var otherBody in allBodies)
        {
            if (otherBody != this)
            {

            }
        }
    }
}
