using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class UIManager2 : MonoBehaviour
{
    [SerializeField] private GameObject mainUI;
    [SerializeField] private GameObject settingsUI;
    [SerializeField] private GameObject creditsUI;

    void Start()
    {
        mainUI.SetActive(true);
        settingsUI.SetActive(false);
    }
    
    // Start Game
    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        Debug.Log("Switched to Graveyard Scene");
    }

    // Switch to Settings UI
    public void SwitchToSettings()
    {
        mainUI.SetActive(false);
        settingsUI.SetActive(true);
        Debug.Log("Switched to Settings UI");
    }

    // Switch to Credits UI
    public void SwitchToCredits()
    {
        mainUI.SetActive(false);
        creditsUI.SetActive(true);
        Debug.Log("Switched to Credits UI");
    }

    // Switch to Main UI
    public void SwitchToMain()
    {
        mainUI.SetActive(true);
        settingsUI.SetActive(false);
        creditsUI.SetActive(false);
        Debug.Log("Switched to Main UI");
    }

    // Quit Game
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quitted Game");
    }

}
