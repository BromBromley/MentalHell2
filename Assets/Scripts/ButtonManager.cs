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

        playBtn.RegisterCallback<ClickEvent>(PlayBtnFunction);
        settingBtn.RegisterCallback<ClickEvent>(SettingBtnFunction);
        exitBtn.RegisterCallback<ClickEvent>(ExitBtnFunctoion);

        //Einblenden der MainPanel Buttons
        var mainPanel = root.Q<VisualElement>("MainPanel");
        mainPanel.style.display = DisplayStyle.Flex;

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
        var mainPanel = root.Q<VisualElement>("MainPanel");
        mainPanel.style.display = DisplayStyle.None;
    }

    private void ExitBtnFunctoion(ClickEvent evt)
    {
        Debug.Log("Exit Button Clicked");
        var exitPanel = root.Q<VisualElement>("ExitPanel");
        exitPanel.style.display = DisplayStyle.Flex;
        var mainPanel = root.Q<VisualElement>("MainPanel");
        mainPanel.style.display = DisplayStyle.None;
    }
}
