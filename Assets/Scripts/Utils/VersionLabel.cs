using TMPro;
using UnityEngine;

public class VersionLabel : MonoBehaviour
{
    TextMeshProUGUI textMeshPro;

    // Start is called before the first frame update
    void Start()
    {
        textMeshPro = GetComponent<TextMeshProUGUI>();
        textMeshPro.text = $"version {Application.version}";
    }
}