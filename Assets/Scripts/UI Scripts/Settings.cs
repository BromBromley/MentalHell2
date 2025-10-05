using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Linq;

public class Settings : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    [SerializeField] private TMP_Dropdown refreshRateDropdown;
    [SerializeField] private TMP_Dropdown displayModeDropdown;
    [SerializeField] private TMP_Dropdown qualityDropdown;

    private Resolution[] availableResolutions;
    private List<Resolution> filteredResolutions;

    void Awake()
    {
        // Temporär auf ExclusiveFullScreen setzen, damit alle Modi erkannt werden
        Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
    }

    void Start()
    {
        SetupResolutionDropdown();
        SetupDisplayModeDropdown();
        SetupQualityDropdown();

        // Gewählten Display Mode wiederherstellen
        int savedMode = PlayerPrefs.GetInt("DisplayMode", 0);
        ApplyDisplayMode(savedMode);
    }

    // ==================== RESOLUTION ====================
    private void SetupResolutionDropdown()
    {
        availableResolutions = Screen.resolutions
            .OrderByDescending(r => r.width * r.height)
            .ThenByDescending(r => r.refreshRateRatio.value)
            .ToArray();

        List<string> resolutionOptions = availableResolutions
            .Select(r => $"{r.width} x {r.height}")
            .Distinct()
            .ToList();

        resolutionDropdown.ClearOptions();
        resolutionDropdown.AddOptions(resolutionOptions);

        string savedRes = PlayerPrefs.GetString("Resolution", $"{Screen.currentResolution.width} x {Screen.currentResolution.height}");
        int savedIndex = resolutionOptions.IndexOf(savedRes);
        resolutionDropdown.value = savedIndex >= 0 ? savedIndex : 0;
        resolutionDropdown.RefreshShownValue();

        OnResolutionChanged(resolutionDropdown.value);
        resolutionDropdown.onValueChanged.AddListener(OnResolutionChanged);
    }

    private void OnResolutionChanged(int index)
    {
        string[] resParts = resolutionDropdown.options[index].text.Split('x');
        int width = int.Parse(resParts[0]);
        int height = int.Parse(resParts[1]);

        filteredResolutions = availableResolutions
            .Where(r => r.width == width && r.height == height)
            .OrderByDescending(r => r.refreshRateRatio.value)
            .ToList();

        List<string> hzOptions = filteredResolutions
            .Select(r => $"{r.refreshRateRatio.value:F0} Hz")
            .Distinct()
            .ToList();

        refreshRateDropdown.ClearOptions();
        refreshRateDropdown.AddOptions(hzOptions);

        string savedHz = PlayerPrefs.GetString("RefreshRate", $"{filteredResolutions[0].refreshRateRatio.value:F0} Hz");
        int hzIndex = hzOptions.IndexOf(savedHz);
        refreshRateDropdown.value = hzIndex >= 0 ? hzIndex : 0;
        refreshRateDropdown.RefreshShownValue();

        refreshRateDropdown.onValueChanged.RemoveAllListeners();
        refreshRateDropdown.onValueChanged.AddListener(OnRefreshRateChanged);

        PlayerPrefs.SetString("Resolution", $"{width} x {height}");
        PlayerPrefs.Save();
    }

    private void OnRefreshRateChanged(int index)
    {
        if (filteredResolutions == null || filteredResolutions.Count == 0) return;

        Resolution selected = filteredResolutions[index];
        PlayerPrefs.SetString("RefreshRate", $"{selected.refreshRateRatio.value:F0} Hz");

        var currentMode = Screen.fullScreenMode;
        Screen.SetResolution(selected.width, selected.height, currentMode, selected.refreshRateRatio);

        Debug.Log($"Resolution set to {selected.width}x{selected.height} @ {selected.refreshRateRatio.value:F0}Hz");
    }

    // ==================== DISPLAY MODE ====================
    private void SetupDisplayModeDropdown()
    {
        List<string> displayModes = new List<string> { "Fullscreen", "Borderless", "Windowed" };
        displayModeDropdown.ClearOptions();
        displayModeDropdown.AddOptions(displayModes);

        int savedMode = PlayerPrefs.GetInt("DisplayMode", 0);
        displayModeDropdown.value = savedMode;
        displayModeDropdown.RefreshShownValue();

        displayModeDropdown.onValueChanged.AddListener(OnDisplayModeChanged);
    }

    private void OnDisplayModeChanged(int index)
    {
        ApplyDisplayMode(index);
        PlayerPrefs.SetInt("DisplayMode", index);
        PlayerPrefs.Save();
    }

    private void ApplyDisplayMode(int index)
    {
        switch (index)
        {
            case 0: Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen; break;
            case 1: Screen.fullScreenMode = FullScreenMode.FullScreenWindow; break;
            case 2: Screen.fullScreenMode = FullScreenMode.Windowed; break;
        }

        Debug.Log("Display mode set to: " + displayModeDropdown.options[index].text);
    }

    // ==================== QUALITY ====================
    private void SetupQualityDropdown()
    {
        string[] qualityLevels = QualitySettings.names;
        qualityDropdown.ClearOptions();
        qualityDropdown.AddOptions(new List<string>(qualityLevels));

        int savedQuality = PlayerPrefs.GetInt("Quality", QualitySettings.GetQualityLevel());
        qualityDropdown.value = savedQuality;
        qualityDropdown.RefreshShownValue();

        qualityDropdown.onValueChanged.AddListener(OnQualityChanged);
    }

    private void OnQualityChanged(int index)
    {
        QualitySettings.SetQualityLevel(index, true);
        PlayerPrefs.SetInt("Quality", index);
        PlayerPrefs.Save();

        Debug.Log("Quality set to: " + qualityDropdown.options[index].text);
    }
}
