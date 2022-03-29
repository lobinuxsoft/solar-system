using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimeScalerControl : MonoBehaviour
{
    [SerializeField] UniverseSettings settings;
    [SerializeField] TextMeshProUGUI label;
    [SerializeField] Slider slider;

    // Start is called before the first frame update
    void Start()
    {
        slider.SetValueWithoutNotify(settings.TimeScaleSimulation);
        label.text = $"Simulation Time Scale: {settings.TimeScaleSimulation:0.0}";

        slider.onValueChanged.AddListener(OnValueChange);
    }

    private void OnValueChange(float value)
    {
        settings.TimeScaleSimulation = value;
        label.text = $"Simulation Time Scale: {settings.TimeScaleSimulation:0.0}";
    }
}
