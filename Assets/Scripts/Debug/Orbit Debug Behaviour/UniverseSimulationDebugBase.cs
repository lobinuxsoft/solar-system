using UnityEngine;

public class UniverseSimulationDebugBase : ScriptableObject
{
    [SerializeField] protected UniverseSettings universeSettings;
    [SerializeField] protected Gradient debugGradient;
    [SerializeField, Range(1, 20000)] protected int numSteps = 1000;
    [SerializeField] protected float timeStep = 0.1f;

    protected bool relativeToBody;

    public virtual void DebugDraw(CelestialBody centralBody) { }
}
