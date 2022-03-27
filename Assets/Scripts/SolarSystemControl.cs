using System.Collections.Generic;
using UnityEngine;

public class SolarSystemControl : MonoBehaviour
{
    [SerializeField] GameObject sunObject;
    [SerializeField] CelestialKeplerianBody body;
    [SerializeField] List<KeplerianBodySettings> planets = new List<KeplerianBodySettings>();

    // Start is called before the first frame update
    void Start()
    {
        Rigidbody sunBody = Instantiate(sunObject, Vector3.zero, Quaternion.identity, transform).GetComponent<Rigidbody>();

        for (int i = 0; i < planets.Count; i++)
        {
            CelestialKeplerianBody ckb = Instantiate<CelestialKeplerianBody>(body, transform);
            ckb.SetKeplerianBodySettings(planets[i], sunBody);
        }
    }
}
