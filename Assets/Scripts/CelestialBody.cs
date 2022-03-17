using UnityEngine;

public class CelestialBody : MonoBehaviour
{
    [SerializeField] float radius;
    [SerializeField] float surfaceGravity;
    [SerializeField] Vector3 initialVelocity;

    Rigidbody body = default;

    float gravitationalConstant = 0.0001f;

    Transform meshHolder;

    public Vector3 velocity { get; private set; }
    public float mass { get; private set; }

    private void Awake()
    {
        velocity = initialVelocity;

        body = GetComponent<Rigidbody>();
        body.mass = mass;
    }

    private void OnValidate()
    {
        mass = surfaceGravity * radius / gravitationalConstant;
        meshHolder = transform.GetChild(0);
        meshHolder.localScale = Vector3.one * radius;
    }

    public void UpdateVelocity(CelestialBody[] allBodies, float timeStep)
    {
        foreach (var otherBody in allBodies)
        {
            if (otherBody != this)
            {
                float sqrDst = (otherBody.body.position - body.position).sqrMagnitude;
                Vector3 forceDir = (otherBody.body.position - body.position).normalized;

                Vector3 acceleration = forceDir * gravitationalConstant * otherBody.mass / sqrDst;
                velocity += acceleration * timeStep;
            }
        }
    }

    public void UpdateVelocity(Vector3 acceleration, float timeStep)
    {
        velocity += acceleration * timeStep;
    }

    public void UpdatePosition(float timeStep)
    {
        body.MovePosition(body.position + velocity * timeStep);
    }

    public Rigidbody Body => body;

    public Vector3 Position => body.position;
}
