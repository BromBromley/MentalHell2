using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class MainMenuButtonManager : MonoBehaviour
{
    [SerializeField] UIDocument uiDocument;
    private VisualElement root;
    
    void Start()
    {
        root = uiDocument.rootVisualElement;

        var playBtn  = root.Q<VisualElement>("PlayBtn");
        var settingBtn = root.Q<VisualElement>("SettingBtn");
        var creditsBtn = root.Q<VisualElement>("CreditsBtn");
        var exitBtn = root.Q<VisualElement>("ExitBtn");
        var yesBtn = root.Q<VisualElement>("YesBtn");
        var cancelBtn = root.Q<VisualElement>("CancelBtn");
        var backBtnSettings = root.Q<VisualElement>("BackBtnSettings");
        var backBtnCredits = root.Q<VisualElement>("BackBtnCredits");

        playBtn.RegisterCallback<ClickEvent>(PlayBtnFunction);
        settingBtn.RegisterCallback<ClickEvent>(SettingBtnFunction);
        creditsBtn.RegisterCallback<ClickEvent>(CreditsBtnFunction);
        exitBtn.RegisterCallback<ClickEvent>(ExitBtnFunctoion);
        yesBtn.RegisterCallback<ClickEvent>(YesBtnFunction);
        cancelBtn.RegisterCallback<ClickEvent>(CancelBtnFunction);
        backBtnSettings.RegisterCallback<ClickEvent>(BackBtnSettingsFunction);
        backBtnCredits.RegisterCallback<ClickEvent>(BackBtnCreditsFunction);
    }

    private void PlayBtnFunction(ClickEvent evt)
    {   
        //Scene Switch zu Level1
        Debug.Log("Play Button Clicked");
        SceneManager.LoadScene (sceneBuildIndex:1);
    }

    private void SettingBtnFunction(ClickEvent evt)
    {
        //Ausblenden der MainPanel Buttons
        Debug.Log("Settings Button Clicked");

        var mainBtns = root.Q<VisualElement>("MainBtns");
        mainBtns.style.display = DisplayStyle.None;
        
        var settingsPanel = root.Q<VisualElement>("SettingsPanel");
        settingsPanel.style.display = DisplayStyle.Flex;
    }

    private void BackBtnSettingsFunction(ClickEvent evt)
    {
        Debug.Log("Went back to Menu");

        var settingsPanel = root.Q<VisualElement>("SettingsPanel");
        settingsPanel.style.display = DisplayStyle.None;

        var mainBtns = root.Q<VisualElement>("MainBtns");
        mainBtns.style.display = DisplayStyle.Flex;
    }

    private void CreditsBtnFunction(ClickEvent evt)
    {
        Debug.Log("Credits Button Clicked");

        var mainBtns = root.Q<VisualElement>("MainBtns");
        mainBtns.style.display = DisplayStyle.None;
 
        var creditsPanel = root.Q<VisualElement>("CreditsPanel");
        creditsPanel.style.display = DisplayStyle.Flex;
    }

    private void BackBtnCreditsFunction(ClickEvent evt)
    {
        Debug.Log("Went back to Menu");

        var creditsPanel = root.Q<VisualElement>("CreditsPanel");
        creditsPanel.style.display = DisplayStyle.None;

        var mainBtns = root.Q<VisualElement>("MainBtns");
        mainBtns.style.display = DisplayStyle.Flex;
    }

    private void ExitBtnFunctoion(ClickEvent evt)
    {
        Debug.Log("Exit Button Clicked");

        var exitPanel = root.Q<VisualElement>("ExitPanel");
        exitPanel.style.display = DisplayStyle.Flex;

        var mainBtns = root.Q<VisualElement>("MainBtns");
        mainBtns.style.display = DisplayStyle.None;
    }

    private void YesBtnFunction(ClickEvent evt)
    {
        Debug.Log("Exited Game");
        Application.Quit();
    }

    private void CancelBtnFunction(ClickEvent evt)
    {
        var exitPanel = root.Q<VisualElement>("ExitPanel");
        exitPanel.style.display = DisplayStyle.None;

        var mainBtns = root.Q<VisualElement>("MainBtns");
        mainBtns.style.display = DisplayStyle.Flex;
    }
}
