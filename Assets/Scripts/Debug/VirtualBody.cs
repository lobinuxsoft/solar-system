using UnityEngine;

public class VirtualBody
{
    public Vector3 position;
    public Vector3 velocity;
    public float radius;
    public float mass;

    public VirtualBody(CelestialBody body)
    {
        position = body.transform.position;
        velocity = body.InitialVelocity;
        radius = body.Radius;
        mass = body.Mass;
    }
}