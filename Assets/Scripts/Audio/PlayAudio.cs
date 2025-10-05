using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayAudio : MonoBehaviour {
    public bool playOnAwake = true; // Autoplay soundtrack on startup
    private AudioManager audioManager;

    public void Start() {
        audioManager = AudioManager.instance;   // Get the AudioManager instance
    }

    void OnEnable() {
        // Subscribe to audio relevant events
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable() {
        // Unsubscribe from audio relevant events
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        PlaySceneAudio(scene.name);
    }

    private void PlaySceneAudio(string sceneName) {
        if (playOnAwake) {
            switch (sceneName) {
                case "Graveyard":
                    Invoke(nameof(PlayNeverGonnaGiveYouUp), 0.25f);
                    break;
                case "IntroSequence":
                    //Invoke(nameof(PlayIntroSequenceSoundtrack), 0);
                    break;
                case "MainMenu":
                    Invoke(nameof(PlayNeverGonnaGiveYouUp), 0.25f);
                    break;
                default:
                    Debug.Log("No soundtrack for scene: " + sceneName);
                    break;
            }
        } else {
            Debug.Log("Autoplay is disabled");
        }
    }

    
    public void PlayNeverGonnaGiveYouUp() {
        Debug.Log("Playing Da Meme");
        audioManager.Play("NeverGonnaGiveYouUp");
    }
    /*
    private void PlayIntroSequenceSoundtrack() {
        Debug.Log("Playing Intro Sequence Soundtrack");
        audioManager.Play("IntroSequence");
    }

    private void PlayLevel01Soundtrack() {
        Debug.Log("Playing Level01 Soundtrack");
        audioManager.Play("SoundtrackLayer1", 3.0f);
    }
    */

}
