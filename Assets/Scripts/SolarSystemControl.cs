using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SolarSystemControl : MonoBehaviour
{
    [SerializeField] GameObject sunObject;
    [SerializeField] CelestialKeplerianBody body;
    [SerializeField] List<KeplerianBodySettings> planets = new List<KeplerianBodySettings>();

    public UnityEvent onSolarSystemCreated;

    // Start is called before the first frame update
    void Start()
    {
        Rigidbody sunBody = Instantiate(sunObject, Vector3.zero, Quaternion.identity, transform).GetComponent<Rigidbody>();
        sunBody.name = "Sun";

        for (int i = 0; i < planets.Count; i++)
        {
            CelestialKeplerianBody ckb = Instantiate<CelestialKeplerianBody>(body, transform);
            ckb.SetKeplerianBodySettings(planets[i], sunBody);
        }

        onSolarSystemCreated?.Invoke();
    }
}
