using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Linq;

public class UIManager : MonoBehaviour
{
    [SerializeField] private UIDocument uiDocument;

    private VisualElement root;
    private VisualElement mainBtns;
    private VisualElement settingsPanel;
    private VisualElement creditsPanel;
    private VisualElement exitPanel;

    private DropdownField resolutionDropdown;

    private Resolution[] availableResolutions;
    private int currentResolutionIndex = 0;
    
    void Start()
    {
        root = uiDocument.rootVisualElement;

        mainBtns = root.Q<VisualElement>("MainBtns");
        settingsPanel = root.Q<VisualElement>("SettingsPanel");
        creditsPanel = root.Q<VisualElement>("CreditsPanel");
        exitPanel = root.Q<VisualElement>("ExitPanel");

        var playBtn  = root.Q<Button>("PlayBtn");
        var settingBtn = root.Q<Button>("SettingBtn");
        var creditsBtn = root.Q<Button>("CreditsBtn");
        var exitBtn = root.Q<Button>("ExitBtn");
        var yesBtn = root.Q<Button>("YesBtn");
        var cancelBtn = root.Q<Button>("CancelBtn");
        var backBtnSettings = root.Q<Button>("BackBtnSettings");
        var backBtnCredits = root.Q<Button>("BackBtnCredits");

        playBtn.RegisterCallback<ClickEvent>(PlayBtnFunction);
        settingBtn.RegisterCallback<ClickEvent>(SettingBtnFunction);
        creditsBtn.RegisterCallback<ClickEvent>(CreditsBtnFunction);
        exitBtn.RegisterCallback<ClickEvent>(ExitBtnFunction);
        yesBtn.RegisterCallback<ClickEvent>(YesBtnFunction);
        cancelBtn.RegisterCallback<ClickEvent>(CancelBtnFunction);
        backBtnSettings.RegisterCallback<ClickEvent>(BackBtnSettingsFunction);
        backBtnCredits.RegisterCallback<ClickEvent>(BackBtnCreditsFunction);
    }

    private void PlayBtnFunction(ClickEvent evt)
    {   
        //Scene Switch zu Graveyard Scene
        Debug.Log("Play Button Clicked");
        SceneManager.LoadScene (sceneBuildIndex:1);
    }

    private void SettingBtnFunction(ClickEvent evt)
    {
        Debug.Log("Settings Button Clicked");
        ShowPanel(settingsPanel);
    }

    private void BackBtnSettingsFunction(ClickEvent evt)
    {
        Debug.Log("Went back to Menu from Settings");
        BackToMainFrom(settingsPanel);
    }

    private void CreditsBtnFunction(ClickEvent evt)
    {
        Debug.Log("Credits Button Clicked");
        ShowPanel(creditsPanel);
    }

    private void BackBtnCreditsFunction(ClickEvent evt)
    {
        Debug.Log("Went back to Menu");
        BackToMainFrom(creditsPanel);
    }

    private void ExitBtnFunction(ClickEvent evt)
    {
        Debug.Log("Exit Button Clicked");
        ShowPanel(exitPanel);
    }

    private void YesBtnFunction(ClickEvent evt)
    {
        Debug.Log("Exited Game");
        Application.Quit();
    }

    private void CancelBtnFunction(ClickEvent evt)
    {
        Debug.Log("Cancelled Exit");
        BackToMainFrom(exitPanel);
    }

    private void ShowPanel(VisualElement panelToShow)
    {
        mainBtns.style.display = DisplayStyle.None;
        panelToShow.style.display = DisplayStyle.Flex;
    }

    private void BackToMainFrom(VisualElement panelToHide)
    {
        panelToHide.style.display = DisplayStyle.None;
        mainBtns.style.display = DisplayStyle.Flex;
    }


    // Resolution Optionen fuer Settings Menu

    void OnEnable()
    {
        var root =GetComponent<UIDocument>().rootVisualElement;

        // Referenz holen
        resolutionDropdown = root.Q<DropdownField>("ResolutionDrop");

        // Aufloesung auslesen
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


        // Quality Settings

        //var root = uiDocument.rootVisualElement;

        var dropdown = root.Q<DropdownField>("QualityDrop");

        // Qualitaetslevel aus Unity holen
        string[] qualityLevels = QualitySettings.names;
        int currentQualityLevel = QualitySettings.GetQualityLevel();

        dropdown.choices = new List<string>(qualityLevels);
        dropdown.value = qualityLevels[currentQualityLevel];

        // Event-Handler fuer Auswahl
        dropdown.RegisterValueChangedCallback(evt =>
        {
            int selectedIndex = dropdown.choices.IndexOf(evt.newValue);
            QualitySettings.SetQualityLevel(selectedIndex, true);
            Debug.Log("Quality changed to: " + evt.newValue);
        });
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
