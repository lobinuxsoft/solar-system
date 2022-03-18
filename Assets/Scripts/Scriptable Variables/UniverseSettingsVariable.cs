using UnityEngine;

[CreateAssetMenu(fileName = "New Universe Settings", menuName = "Lobby Tools/ Scriptable Variables/ Universe Settings")]
public class UniverseSettingsVariable : BaseScriptableVariable
{
    [SerializeField] float gravitationalConstant = 0.0001f;
    [SerializeField] float physicsTimeStep = 0.01f;

    public float GravitationalConstant => gravitationalConstant;
    public float PhysicsTimeStep => physicsTimeStep;

    public override void SaveData()
    {
        UniverseSettingsVariableStruct temp = new UniverseSettingsVariableStruct { gravitationalConstant = gravitationalConstant, physicsTimeStep = physicsTimeStep };
        SaveData<UniverseSettingsVariableStruct>(temp);
    }

    public override void LoadData()
    {
        UniverseSettingsVariableStruct temp = LoadData<UniverseSettingsVariableStruct>();
        gravitationalConstant = temp.gravitationalConstant;
        physicsTimeStep = temp.physicsTimeStep;
    }

    public override void EraseSaveFile()
    {
        base.EraseSaveFile();

        gravitationalConstant = 0.0001f;
        physicsTimeStep = 0.01f;
    }
}

public struct UniverseSettingsVariableStruct
{
    public float gravitationalConstant;
    public float physicsTimeStep;
}
