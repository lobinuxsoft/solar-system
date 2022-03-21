using UnityEngine;

[CreateAssetMenu(fileName = "New Universe Behaviour", menuName = "Lobby Tools/ Behaviour/ Universe Behaviour")]
public class UniverseBehaviour : ScriptableObject
{
    [SerializeField] UniverseSettings universeSettings;

    CelestialBody[] celestials;

    public void InitializeBehaviour(CelestialBody[] celestials)
    {
        this.celestials = celestials;

        Time.timeScale = universeSettings.TimeScaleSimulation;

        Time.timeScale = universeSettings.TimeScaleSimulation;
        Time.fixedDeltaTime = universeSettings.PhysicsTimeStep;
    }

    public void UpdateBehaviour()
    {
        if (universeSettings.TimeScaleSimulation != Time.timeScale) 
            Time.timeScale = universeSettings.TimeScaleSimulation;

        Gravity();
    }

    void Gravity()
    {
        for (int i = 0; i < celestials.Length; i++)
        {
            Vector3 acceleration = CalculateAcceleration(celestials[i].Position, celestials[i]);
            celestials[i].UpdateVelocity(acceleration, universeSettings.PhysicsTimeStep);
        }

        for (int i = 0; i < celestials.Length; i++)
        {
            celestials[i].UpdatePosition(universeSettings.PhysicsTimeStep);
        }
    }

    Vector3 CalculateAcceleration(Vector3 point, CelestialBody ignoreBody = null)
    {
        Vector3 acceleration = Vector3.zero;
        foreach (var body in celestials)
        {
            if (body != ignoreBody)
            {
                float sqrDst = (body.Position - point).sqrMagnitude;
                Vector3 forceDir = (body.Position - point).normalized;
                acceleration += forceDir * universeSettings.GravitationalConstant * body.Mass / sqrDst;
            }
        }

        return acceleration;
    }
}
