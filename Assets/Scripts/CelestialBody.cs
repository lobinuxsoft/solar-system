using UnityEngine;

public class CelestialBody : MonoBehaviour
{
    [Tooltip("Radius in Kilometers")]
    [SerializeField] float radius;
    [SerializeField] Vector3 initialVelocity;
    [SerializeField] float mass;

    [SerializeField] UniverseSettings universeSettings;

    Rigidbody body = default;
    TrailRenderer trailRenderer = default;

    public Vector3 Velocity { get; private set; }
    public float Mass => mass;
    public Vector3 Position => body.position;
    public Vector3 InitialVelocity => initialVelocity;

    public float Radius => radius;
    public Rigidbody Body => body;


    private void Awake()
    {
        Velocity = initialVelocity;
        body = GetComponent<Rigidbody>();
        body.mass = Mass;
        body.useGravity = false;
        body.isKinematic = true;

        trailRenderer = GetComponent<TrailRenderer>();
        trailRenderer.startWidth = radius * .5f;
        trailRenderer.enabled = true;
    }

    private void OnValidate()
    {
        transform.localScale = Vector3.one * radius;

        body = GetComponent<Rigidbody>();
        body.mass = Mass;
        body.useGravity = false;
        body.isKinematic = true;

        trailRenderer = GetComponent<TrailRenderer>();
        trailRenderer.startWidth = radius * .5f;
        trailRenderer.enabled = false;
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
