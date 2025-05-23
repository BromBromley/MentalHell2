using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    [SerializeField] UIDocument uiDocument;
    private VisualElement root;
    
    void Start()
    {
        root = uiDocument.rootVisualElement;

        var playBtn  = root.Q<VisualElement>("PlayBtn");
        var settingBtn = root.Q<VisualElement>("SettingBtn");
        var exitBtn = root.Q<VisualElement>("ExitBtn");
        var yesBtn = root.Q<VisualElement>("YesBtn");
        var cancelBtn = root.Q<VisualElement>("CancelBtn");

        playBtn.RegisterCallback<ClickEvent>(PlayBtnFunction);
        settingBtn.RegisterCallback<ClickEvent>(SettingBtnFunction);
        exitBtn.RegisterCallback<ClickEvent>(ExitBtnFunctoion);
        yesBtn.RegisterCallback<ClickEvent>(YesBtnFunction);
        cancelBtn.RegisterCallback<ClickEvent>(CancelBtnFunction);

        //Einblenden der MainPanel Buttons
        var mainBtns = root.Q<VisualElement>("MainBtns");
        mainBtns.style.display = DisplayStyle.Flex;

        //Ausblenden des Exit Popups
        var exitPanel = root.Q<VisualElement>("ExitPanel");
        exitPanel.style.display = DisplayStyle.None;
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
