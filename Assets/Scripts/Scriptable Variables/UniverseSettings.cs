using UnityEngine;

[CreateAssetMenu(fileName = "New Universe Settings", menuName = "Lobby Tools/ Scriptable Variables/ Universe Settings")]
public class UniverseSettings : BaseScriptableVariable
{
    public const float TAU = 6.28318530718f;
    const float minTimescale = .25f;
    const float maxTimescale = 40f;

    [SerializeField, Range(minTimescale, maxTimescale)] float timeScaleSimulation = 1f;
    [SerializeField] float gravitationalConstant = 0.0001f;
    [SerializeField] float physicsTimeStep = 0.01f;

    public float TimeScaleSimulation 
    {
        get { return timeScaleSimulation; } 
        set 
        { 
            timeScaleSimulation = Mathf.Clamp(value, minTimescale, maxTimescale);
            Time.timeScale = timeScaleSimulation;
        }
    }

    public float MaxTimeScale => maxTimescale;

    public float GravitationalConstant => gravitationalConstant;
    public float PhysicsTimeStep => physicsTimeStep;

    public override void SaveData()
    {
        UniverseSettingsStruct temp = new UniverseSettingsStruct 
        {
            timeScaleSimulation = timeScaleSimulation,
            gravitationalConstant = gravitationalConstant,
            physicsTimeStep = physicsTimeStep
        };
        SaveData<UniverseSettingsStruct>(temp);
    }

    public override void LoadData()
    {
        UniverseSettingsStruct temp = LoadData<UniverseSettingsStruct>();
        timeScaleSimulation = temp.timeScaleSimulation;
        gravitationalConstant = temp.gravitationalConstant;
        physicsTimeStep = temp.physicsTimeStep;
    }

    public override void EraseSaveFile()
    {
        base.EraseSaveFile();

        timeScaleSimulation = 1f;
        gravitationalConstant = 0.0001f;
        physicsTimeStep = 0.01f;
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        TimeScaleSimulation = timeScaleSimulation;
    }
#endif
}

public struct UniverseSettingsStruct
{
    public float timeScaleSimulation;
    public float gravitationalConstant;
    public float physicsTimeStep;
}
