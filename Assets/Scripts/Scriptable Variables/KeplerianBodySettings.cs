using UnityEngine;

[CreateAssetMenu(fileName = "New Keplerian Body Settings", menuName = "Lobby Tools/ Keplerian Orbit/ Keplerian Body Settings")]
public class KeplerianBodySettings : BaseScriptableVariable
{
    [Space(10)]
    [Header("Standar Settings")]
    [SerializeField] Material planetMaterial;
    [SerializeField] float planetRadius = 1;
    [SerializeField] float planetMass = 1;

    [Space(10)]
    [Header("Orbital Keplerian Parameters")]

    [Tooltip("The size of the orbit")]
    [SerializeField] float semiMajorAxis = 20f;

    [Tooltip("The shape of the orbit")]
    [SerializeField, Range(0f, 0.99f)] float eccentricity = 0;

    [Tooltip("The inclination of the orbit")]
    [SerializeField, Range(0f, Math.TAU)] float inclination = 0f;

    [Tooltip("Defines the angle between the reference direction and the upward crossing of the orbit on the reference plane")]
    [SerializeField, Range(0f, Math.TAU)] float longitudeOfAcendingNode = 0;

    [Tooltip("Defines the angle between the ascending node and the periapsis")]
    [SerializeField, Range(0f, Math.TAU)] float argumentOfPeriapsis = 0;

    [Tooltip("Is the ecliptic longitude at which an orbiting body could be found if its orbit were circular and free of perturbations")]
    [SerializeField] float meanLongitude = 0;

    public Material PlanetMaterial => planetMaterial;
    public float PlanetRadius => planetRadius;
    public float PlanetMass => planetMass;
    public float SemiMajorAxis => semiMajorAxis;
    public float Eccentricity => eccentricity;
    public float Inclination => inclination;
    public float LongitudeOfAcendingNode => longitudeOfAcendingNode;
    public float ArgumenOfPeriapsis => argumentOfPeriapsis;
    public float MeanLongitude => meanLongitude;

    public override void SaveData()
    {
        KeplerianBodySettingsStruct temp = new KeplerianBodySettingsStruct
        {
            planetRadius = this.planetRadius,
            planetMass = this.planetMass,
            semiMajorAxis = this.semiMajorAxis,
            eccentricity = this.eccentricity,
            inclination = this.inclination,
            longitudeOfAcendingNode = this.longitudeOfAcendingNode,
            argumentOfPeriapsis = this.argumentOfPeriapsis,
            meanLongitude = this.meanLongitude
        };

        SaveData<KeplerianBodySettingsStruct>(temp);
    }

    public override void LoadData()
    {
        KeplerianBodySettingsStruct temp = LoadData<KeplerianBodySettingsStruct>();

        planetRadius = temp.planetRadius;
        planetMass = temp.planetMass;
        semiMajorAxis = temp.semiMajorAxis;
        eccentricity = temp.eccentricity;
        inclination = temp.inclination;
        longitudeOfAcendingNode = temp.longitudeOfAcendingNode;
        argumentOfPeriapsis = temp.argumentOfPeriapsis;
        meanLongitude = temp.meanLongitude;
    }

    public override void EraseSaveFile()
    {
        base.EraseSaveFile();

        planetRadius = 1;
        planetMass = 1;
        semiMajorAxis = 20f;
        eccentricity = 0;
        inclination = 0f;
        longitudeOfAcendingNode = 0;
        argumentOfPeriapsis = 0;
        meanLongitude = 0;
    }
}

struct KeplerianBodySettingsStruct
{
    public float planetRadius;
    public float planetMass;
    public float semiMajorAxis;
    public float eccentricity;
    public float inclination;
    public float longitudeOfAcendingNode;
    public float argumentOfPeriapsis;
    public float meanLongitude;
}
