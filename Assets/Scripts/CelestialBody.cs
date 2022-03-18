using UnityEngine;

public class CelestialBody : MonoBehaviour
{
    [SerializeField] float radius;
    [SerializeField] float surfaceGravity;
    [SerializeField] Vector3 initialVelocity;

    [SerializeField] UniverseSettingsVariable universeSettings;

    Rigidbody body = default;

    Transform meshHolder;

    public Vector3 Velocity { get; private set; }
    public float Mass { get; private set; }
    public Vector3 Position => body.position;
    public Vector3 InitialVelocity => initialVelocity;
    public Rigidbody Body => body;


    private void Awake()
    {
        Velocity = initialVelocity;

        body = GetComponent<Rigidbody>();
        body.mass = Mass;
        body.useGravity = false;
        body.isKinematic = true;
    }

    private void OnValidate()
    {
        Mass = surfaceGravity * radius  * radius / universeSettings.GravitationalConstant;
        meshHolder = transform.GetChild(0);
        meshHolder.localScale = Vector3.one * radius;

        body = GetComponent<Rigidbody>();
        body.mass = Mass;
        body.useGravity = false;
        body.isKinematic = true;
    }

    public void UpdateVelocity(CelestialBody[] allBodies, float timeStep)
    {
        foreach (var otherBody in allBodies)
        {
            if (otherBody != this)
            {
                float sqrDst = (otherBody.body.position - body.position).sqrMagnitude;
                Vector3 forceDir = (otherBody.body.position - body.position).normalized;

                Vector3 acceleration = forceDir * universeSettings.GravitationalConstant * otherBody.Mass / sqrDst;
                Velocity += acceleration * timeStep;
            }
        }
    }

    public void UpdateVelocity(Vector3 acceleration, float timeStep)
    {
        Velocity += acceleration * timeStep;
    }

    public void UpdatePosition(float timeStep)
    {
        body.MovePosition(body.position + Velocity * timeStep);
    }
}
