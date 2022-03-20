using UnityEngine;

public class UniverseSimulation : MonoBehaviour 
{
    [SerializeField] UniverseBehaviour behaviour;

    CelestialBody[] bodies;

    void Awake () {

        bodies = FindObjectsOfType<CelestialBody> ();
        behaviour.InitializeBehaviour(bodies);
    }

    void FixedUpdate()
    {
        behaviour.UpdateBehaviour();
    }
}