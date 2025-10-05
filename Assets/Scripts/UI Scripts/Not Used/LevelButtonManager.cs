using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class LevelButtonManager : MonoBehaviour
{
    [SerializeField] UIDocument uiDocument;
    private VisualElement root;
    
    void Start()
    {
        root = uiDocument.rootVisualElement;

        var mainMenuBtn = root.Q<VisualElement>("MainMenuBtn");
        var reloadBtn = root.Q<VisualElement>("ReloadBtn");

        mainMenuBtn.RegisterCallback<ClickEvent>(MainMenuBtnFunction);
        reloadBtn.RegisterCallback<ClickEvent>(ReloadBtnFunction);
    }

    private void MainMenuBtnFunction(ClickEvent evt)
    {
        Debug.Log("Switched Back to Main Menu");
        SceneManager.LoadScene (sceneBuildIndex:0);
    }

    private void ReloadBtnFunction(ClickEvent evt)
    {
        Debug.Log("Reloaded Level");
        SceneManager.LoadScene (sceneBuildIndex:1);
    }
}