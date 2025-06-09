using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class MainMenuButtonManager : MonoBehaviour
{
    [SerializeField] private UIDocument uiDocument;

    private VisualElement root;
    private VisualElement mainBtns;
    private VisualElement settingsPanel;
    private VisualElement creditsPanel;
    private VisualElement exitPanel;
    
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
}
