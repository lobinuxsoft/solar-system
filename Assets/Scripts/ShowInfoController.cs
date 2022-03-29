using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShowInfoController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI planetNameText;
    [SerializeField] TextMeshProUGUI planetInfoText;
    [SerializeField] List<KeplerianBodySettings> planetsInfo = new List<KeplerianBodySettings>();

    CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0;
    }

    public void ShowInfo(int index)
    {
        if(0 <= index && index < planetsInfo.Count)
        {
            planetNameText.text = planetsInfo[index].name;
            planetInfoText.text = planetsInfo[index].PlanetInfo;
            canvasGroup.alpha = 1;
        }
        else
        {
            HideInfo();
        }
    }

    private void HideInfo()
    {
        canvasGroup.alpha = 0;
    }
}
