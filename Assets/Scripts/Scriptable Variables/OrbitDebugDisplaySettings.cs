using UnityEngine;

[CreateAssetMenu(fileName = "New Orbit Debug Display Settings", menuName = "Lobby Tools/ Scriptable Variables/ Orbit Debug Display Settings")]
public class OrbitDebugDisplaySettings : BaseScriptableVariable
{
    [SerializeField] Gradient debugGradient;
    [SerializeField, Range(1, 20000)] int numSteps = 1000;
    [SerializeField] float timeStep = 0.1f;
    [SerializeField] bool usePhysicsTimeStep;

    public Gradient DebugGradient => debugGradient;
    public int NumSteps => numSteps;
    public float TimeStep { get { return timeStep; } set { timeStep = value; } }
    public bool UsePhysicsTimeStep => usePhysicsTimeStep;

    public override void SaveData()
    {
        OrbitDebugDisplaySettingsStruct temp = new OrbitDebugDisplaySettingsStruct 
        {
            debugGradient = debugGradient,
            numSteps = numSteps,
            timeStep = timeStep,
            usePhysicsTimeStep = usePhysicsTimeStep
        };

        SaveData<OrbitDebugDisplaySettingsStruct>(temp);
    }

    public override void LoadData()
    {
        OrbitDebugDisplaySettingsStruct temp = LoadData<OrbitDebugDisplaySettingsStruct>();
        debugGradient = temp.debugGradient;
        numSteps = temp.numSteps;
        timeStep = temp.timeStep;
        usePhysicsTimeStep = temp.usePhysicsTimeStep;
    }

    public override void EraseSaveFile()
    {
        base.EraseSaveFile();

        debugGradient = null;
        numSteps = 1000;
        timeStep = 0.1f;
        usePhysicsTimeStep = false;
    }
}

public struct OrbitDebugDisplaySettingsStruct
{
    public Gradient debugGradient;
    public int numSteps;
    public float timeStep;
    public bool usePhysicsTimeStep;
}