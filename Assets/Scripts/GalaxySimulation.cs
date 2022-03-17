using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GalaxySimulation : MonoBehaviour
{
    CelestialBody[] bodies;
    static GalaxySimulation instance;

    private void Awake()
    {
        bodies = FindObjectsOfType<CelestialBody>();
    }
}
