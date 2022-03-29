using Cinemachine;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CelestialSelector : MonoBehaviour
{
    [SerializeField] UniverseSettings settings;
    [SerializeField] Transform celestialsContainer;
    [SerializeField] Button buttonPref;
    [SerializeField] Transform buttonContainer;
    [SerializeField] CinemachineVirtualCamera defaultVCam;
    [SerializeField] AnimationCurve orbitSpeed;

    public UnityEvent<int> onPlanetSelected;

    CinemachineVirtualCamera vcam;
    CinemachineOrbitalTransposer orbitalTransposer;

    private void Awake()
    {
        vcam = defaultVCam;
        orbitalTransposer = vcam.GetCinemachineComponent<CinemachineOrbitalTransposer>();
        orbitalTransposer.m_XAxis.m_MaxSpeed = 0;
    }

    private void Update()
    {
        if (orbitalTransposer)
        {
            orbitalTransposer.m_XAxis.m_MaxSpeed = Input.GetMouseButton(1) ? orbitSpeed.Evaluate(settings.TimeScaleSimulation / settings.MaxTimeScale) : 0;
        }
    }

    public void CreateSelector()
    {
        for (int i = 0; i < celestialsContainer.childCount; i++)
        {
            var tempButton = Instantiate<Button>(buttonPref, buttonContainer);
            tempButton.onClick.AddListener(() => CelestialSelected(tempButton.transform.GetSiblingIndex()));

            tempButton.GetComponentInChildren<TextMeshProUGUI>().text = celestialsContainer.GetChild(i).name;
        }

        var defaultViewButton = Instantiate<Button>(buttonPref, buttonContainer);
        defaultViewButton.onClick.AddListener(() => CelestialSelected(-1));
        defaultViewButton.GetComponentInChildren<TextMeshProUGUI>().text = "Default View";
    }

    void CelestialSelected(int index)
    {
        for (int i = 0; i < buttonContainer.childCount; i++)
        {
            buttonContainer.GetChild(i).gameObject.SetActive(!(index == i));
        }

        if(index < 0)
        {
            defaultVCam.gameObject.SetActive(true);

            if (vcam) vcam.gameObject.SetActive(false);

            vcam = defaultVCam;
        }
        else
        {
            var tempCam = celestialsContainer.GetChild(index).GetComponent<CelestialKeplerianBody>().VirtualCamera;

            tempCam.gameObject.SetActive(true);

            if (vcam) vcam.gameObject.SetActive(false);

            vcam = tempCam;
        }

        if (vcam) orbitalTransposer = vcam.GetCinemachineComponent<CinemachineOrbitalTransposer>();

        onPlanetSelected?.Invoke(index);
    }
}
