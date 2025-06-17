using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;
using System.Linq;

public class SettingsController : MonoBehaviour
{
    private DropdownField resolutionDropdown;

    private Resolution[] availableResolutions;
    private int currentResolutionIndex = 0;

    void OnEnable()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        //Referenzen holen
        resolutionDropdown = root.Q<DropdownField>("ResolutionDrop");

        //Aufloesung auslesen
        availableResolutions = Screen.resolutions
            .Select(res => new Resolution { width = res.width, height = res.height })
            .Distinct() //Doppelte Eintraege entfernen
            .OrderByDescending(res => res.width * res.height)
            .ToArray();

        List<string> resolutionOptions = new List<string>();
        for (int i = 0; i < availableResolutions.Length; i++)
        {
            string resString = $"{availableResolutions[i].width} x {availableResolutions[i].height}";
            resolutionOptions.Add(resString);

            //Aktuelle Aufloesung ermitteln
            if (availableResolutions[i].width == Screen.currentResolution.width &&
                availableResolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.choices = resolutionOptions;

        //Gespeicherte Auswahl setzen
        string savedRes = PlayerPrefs.GetString("Resolution", resolutionOptions[currentResolutionIndex]);
        resolutionDropdown.value = savedRes;

        resolutionDropdown.RegisterValueChangedCallback(OnResolutionChanged);
    }

    private void OnResolutionChanged(ChangeEvent<string> evt)
    {
        Resolution selectedResolution = availableResolutions
            .FirstOrDefault(r => $"{r.width} x {r.height}" == evt.newValue);

        if (selectedResolution.width > 0 && selectedResolution.height > 0)
        {
            PlayerPrefs.SetString("Resolution", evt.newValue);
            Screen.SetResolution(selectedResolution.width, selectedResolution.height, FullScreenMode.FullScreenWindow);
        }
        else
        {
            Debug.LogWarning("Gewaehlte Aufloesung wurde nicht gefunden.");
        }
    }
}
