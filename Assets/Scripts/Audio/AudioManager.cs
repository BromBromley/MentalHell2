using UnityEngine;
using System;
using UnityEngine.Audio;
using System.Collections;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
using System.Linq;
using System.Collections.Generic;

/*
    This Script will be attached to an Empty GameObject in the Scene
    Containing all the various Sounds in the scene in the arrays

    Example Code
    // declare AudioManager (Add the object to the script)
    private AudioManager audioManager;

    // play the sound
    audioManager.Play("sound_name");

    // functions the audio manager offers
    * Play
    * PlayRandom
    * Pause
    * UnPause
    * Stop
    * Queue
    * PauseAllSounds
    * StopAllSounds
    * PauseAllSoundtracks
    * PauseAllAmbienceSounds
    * PauseAllSFXSounds
    * UnPauseAllSounds
    
*/

public class AudioManager : MonoBehaviour {
    /*
        
    ██╗░░░██╗░█████╗░██████╗░██╗░█████╗░██████╗░██╗░░░░░███████╗░██████╗
    ██║░░░██║██╔══██╗██╔══██╗██║██╔══██╗██╔══██╗██║░░░░░██╔════╝██╔════╝
    ╚██╗░██╔╝███████║██████╔╝██║███████║██████╦╝██║░░░░░█████╗░░╚█████╗░
    ░╚████╔╝░██╔══██║██╔══██╗██║██╔══██║██╔══██╗██║░░░░░██╔══╝░░░╚═══██╗
    ░░╚██╔╝░░██║░░██║██║░░██║██║██║░░██║██████╦╝███████╗███████╗██████╔╝
    ░░░╚═╝░░░╚═╝░░╚═╝╚═╝░░╚═╝╚═╝╚═╝░░╚═╝╚═════╝░╚══════╝╚══════╝╚═════╝░
    */
    [Header("Audio Tracks")]

    // make Audiomanager Accessable from everywhere
    public static AudioManager instance;
    // Arrays for the Sound types

    public Sound[] sounds;   // only ONE Array with all sounds
    private List<AudioSource> audioSourcesSoundtrack = new List<AudioSource>();
    private List<AudioSource> audioSourcesAmbient = new List<AudioSource>();
    private List<AudioSource> audioSourcesSFX = new List<AudioSource>();
    private AudioSource source;

    // get the gameobjects determining for the AudioSources
    private GameObject SoundtrackEmpty;
    private GameObject AmbientEmpty;
    private GameObject SFXEmpty;

    private bool isSceneChanging = false;

    /*
    ███████╗██╗░░░██╗███████╗███╗░░██╗████████╗░██████╗
    ██╔════╝██║░░░██║██╔════╝████╗░██║╚══██╔══╝██╔════╝
    █████╗░░╚██╗░██╔╝█████╗░░██╔██╗██║░░░██║░░░╚█████╗░
    ██╔══╝░░░╚████╔╝░██╔══╝░░██║╚████║░░░██║░░░░╚═══██╗
    ███████╗░░╚██╔╝░░███████╗██║░╚███║░░░██║░░░██████╔╝
    ╚══════╝░░░╚═╝░░░╚══════╝╚═╝░░╚══╝░░░╚═╝░░░╚═════╝░
    */
    public void Awake(){
        if (instance == null)
            instance = this; // set the Audiomanager Instance to this object
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject); // maintain AudioManager Empty through new scene loading
    }

    // initialize audio on enabled gameObject (on start does not work)
    void OnEnable() {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    void OnDisable() {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        // Reset scene-changing flag so fade coroutines run after load
        isSceneChanging = false;
        Initialize();
    }

    private void OnSceneUnloaded(Scene scene) {
        isSceneChanging = true;
    }

    /*
    ███████╗██╗░░░██╗███╗░░██╗░█████╗░████████╗██╗░█████╗░███╗░░██╗░██████╗
    ██╔════╝██║░░░██║████╗░██║██╔══██╗╚══██╔══╝██║██╔══██╗████╗░██║██╔════╝
    █████╗░░██║░░░██║██╔██╗██║██║░░╚═╝░░░██║░░░██║██║░░██║██╔██╗██║╚█████╗░
    ██╔══╝░░██║░░░██║██║╚████║██║░░██╗░░░██║░░░██║██║░░██║██║╚████║░╚═══██╗
    ██║░░░░░╚██████╔╝██║░╚███║╚█████╔╝░░░██║░░░██║╚█████╔╝██║░╚███║██████╔╝
    ╚═╝░░░░░░╚═════╝░╚═╝░░╚══╝░╚════╝░░░░╚═╝░░░╚═╝░╚════╝░╚═╝░░╚══╝╚═════╝░
    */

    /*
        Find/Create the empty GameObjects as base for the AudioSources
    */
    private void Initialize() {
        DestroyAllAudioSources();   // reset all AudioSources to avoid duplicates

        SoundtrackEmpty = GameObject.FindWithTag("SoundtrackEmpty");
        if (SoundtrackEmpty == null) {
            Debug.Log("Creating SoundtrackEmpty GameObject");
            SoundtrackEmpty = new GameObject("Soundtrack");
            SoundtrackEmpty.transform.parent = this.transform;
            SoundtrackEmpty.tag = "SoundtrackEmpty";
        }
        audioSourcesSoundtrack.Add(SoundtrackEmpty?.AddComponent<AudioSource>());
        SoundtrackEmpty?.GetComponent<AudioSource>()?.gameObject.SetActive(true);

        AmbientEmpty = GameObject.FindWithTag("AmbientEmpty");
        if (AmbientEmpty == null) {
            Debug.Log("Creating AmbientEmpty GameObject");
            AmbientEmpty = new GameObject("Ambient");
            AmbientEmpty.transform.parent = this.transform;
            AmbientEmpty.tag = "AmbientEmpty";
        }
        audioSourcesAmbient.Add(AmbientEmpty?.AddComponent<AudioSource>());
        AmbientEmpty?.GetComponent<AudioSource>()?.gameObject.SetActive(true);

        SFXEmpty = GameObject.FindWithTag("SFXEmpty");
        if (SFXEmpty == null) {
            Debug.Log("Creating SFXEmpty GameObject");
            SFXEmpty = new GameObject("SFX");
            SFXEmpty.transform.parent = this.transform;
            SFXEmpty.tag = "SFXEmpty";
        }
        audioSourcesSFX.Add(SFXEmpty?.AddComponent<AudioSource>());
        SFXEmpty?.GetComponent<AudioSource>()?.gameObject.SetActive(true);
    }

    /*
        Copy Sound Data over from sound to GameObject and get the GameObject the Sound will play from
    */
    private AudioSource CopyAudioDataToEmpty(Sound sound, GameObject emptySoundGameObject){
        source = GetAvailableAudioSource(emptySoundGameObject, sound.type);
        source.clip = sound.clip;
        source.volume = sound.volume;
        source.pitch = sound.pitch;
        source.loop = sound.loop;
        source.outputAudioMixerGroup = sound.output;
        source.spatialBlend = sound.spatialBlend;
        source.maxDistance = sound.maxDistance;
        source.minDistance = sound.minDistance;
        source.rolloffMode = sound.rolloffMode;

        return source;
    }

    private AudioSource GetAvailableAudioSource(GameObject parent, Sound.SoundType type) {
        List<AudioSource> pool = GetAudioSourcesOfType(type);
        foreach (var source in pool) {
            if (!source.isPlaying) {
                return source;
            }
        }

        // Kein freier AudioSource → neue erstellen
        AudioSource newSource = parent.AddComponent<AudioSource>();
        pool.Add(newSource);
        return newSource;
    }

    private List<AudioSource> GetAudioSourcesOfType(Sound.SoundType type) {
        switch (type) {
            case Sound.SoundType.Music:
                return audioSourcesSoundtrack;

            case Sound.SoundType.Ambient:
                return audioSourcesAmbient;

            case Sound.SoundType.SFX:
                return audioSourcesSFX;
    
            default:
                Debug.LogError("Unknown sound type: " + type);
                return new List<AudioSource>(); 
        }
    }

    private AudioSource GetAudioSourceByType(Sound.SoundType type) {
        switch (type) {
            case Sound.SoundType.Music:
                return SoundtrackEmpty.GetComponent<AudioSource>();

            case Sound.SoundType.Ambient:
                return AmbientEmpty.GetComponent<AudioSource>();

            case Sound.SoundType.SFX:
                return SFXEmpty.GetComponent<AudioSource>();
    
            default:
                Debug.LogError("Unknown sound type: " + type);
                return null; 
        }
    }

    private GameObject GetParentByType(Sound.SoundType type) {
        switch (type) {
            case Sound.SoundType.Music:
                return SoundtrackEmpty;

            case Sound.SoundType.Ambient:
                return AmbientEmpty;

            case Sound.SoundType.SFX:
                return SFXEmpty;
    
            default:
                Debug.LogError("Unknown sound type: " + type);
                return null; 
        }
    }

    public Sound GetSoundByName(string name) {
        Sound sound = Array.Find(sounds, x => x.name == name);
        if (sound == null) {
            Debug.LogError("Sound not found: " + name);
        }
        return sound;
    }

    /** KEY FUNCTIONS **
    * Play(string)
    * PlayRandom(string[])
    * Pause(string, float)
    * UnPause
    * Stop
    * Queue
    * PauseAllSounds
    * StopAllSounds
    * PauseAllSoundtracks
    * PauseAllAmbienceSounds
    * PauseAllSFXSounds
    * UnPauseAllSounds
    */

    public void Play(string name, float fadeInSeconds = 0.0f, float fadeOutSeconds = 0.0f) {
        Sound sound = Array.Find(sounds, x => x.name == name);
        if (sound == null) {
            Debug.LogError("Sound not found: " + name);
            return;
        }

        GameObject parent = GetParentByType(sound.type);
        AudioSource source = CopyAudioDataToEmpty(sound, parent);
        
        if (!source.enabled)
            source.enabled = true;
        if (!source.gameObject.activeInHierarchy)
            source.gameObject.SetActive(true);

        bool fadeIn = fadeInSeconds > 0.0f;
        bool fadeOut = fadeOutSeconds > 0.0f;

        float originalVolume = sound.volume;
        source.volume = fadeIn ? 0f : originalVolume;

        source.Play();

        if (fadeIn) {
            float fadeDuration = fadeInSeconds;
            StartCoroutine(FadeVolume(source, originalVolume, fadeDuration));
        }

        if (fadeOut) {
            float fadeDuration = fadeOutSeconds;
            StartCoroutine(FadeOut(source, fadeDuration));
        }
    }

    public void PlayRandom(string[] names) {
        if (names == null || names.Length == 0) 
            return;

        string soundName = names[UnityEngine.Random.Range(0, names.Length)];
        Play(soundName);
    }

    public void Pause(string name, float fadeOutSeconds = 0.0f) {
        Sound sound = Array.Find(sounds, x => x.name == name);
        if (sound == null) {
            Debug.LogError("Sound not found: " + name);
            return;
        }

        AudioSource source = GetAudioSourceByType(sound.type);

        bool fadeOut = fadeOutSeconds > 0.0f;
        
        if (fadeOut) {
            float fadeDuration = fadeOutSeconds;
            StartCoroutine(FadeOut(source, fadeDuration));
        }
        source.Pause();
    }

    public void UnPause(string name) {
        Sound sound = Array.Find(sounds, x => x.name == name);
        if (sound == null) {
            Debug.LogError("Sound not found: " + name);
            return;
        }

        AudioSource source = GetAudioSourceByType(sound.type);
        
        source.UnPause();
    }

    public void Stop(string name, float fadeOutSeconds = 0.0f) {
        Sound sound = Array.Find(sounds, x => x.name == name);
        if (sound == null) {
            Debug.LogError("Sound not found: " + name);
            return;
        }

        AudioSource source = GetAudioSourceByType(sound.type);
       
        bool fadeOut = fadeOutSeconds > 0.0f;
        if (fadeOut) {
            float fadeDuration = fadeOutSeconds;
            StartCoroutine(FadeOut(source, fadeDuration));
        }
        source.Stop();
    }

    public void Queue(string name) {
        Sound nextSound = Array.Find(sounds, x => x.name == name);
        Debug.Log("Queueing sound: " + nextSound.name);
        if (nextSound == null) 
            return;

        AudioSource currentSource = GetAudioSourceByType(nextSound.type);
        if (currentSource == null || currentSource.loop) 
            return;

        StartCoroutine(PlayAfterCurrentEnds(currentSource, nextSound));
    }

    public void PauseAllSounds() {
        for (int i = 0; i < audioSourcesSoundtrack.Count; i++) {
            audioSourcesSoundtrack[i].Pause();
        }

        for (int i = 0; i < audioSourcesAmbient.Count; i++) {
            audioSourcesAmbient[i].Pause();
        }

        for (int i = 0; i < audioSourcesSFX.Count; i++) {
            audioSourcesSFX[i].Pause();
        }
    }

    public void StopAllSounds() {
        for (int i = 0; i < audioSourcesSoundtrack.Count; i++) {
            audioSourcesSoundtrack[i].Stop();
        }

        for (int i = 0; i < audioSourcesAmbient.Count; i++) {
            audioSourcesAmbient[i].Stop();
        }

        for (int i = 0; i < audioSourcesSFX.Count; i++) {
            audioSourcesSFX[i].Stop();
        }
    }

    public void PauseAllSoundtracks() {
        foreach (AudioSource audioSource in audioSourcesSoundtrack) {
            audioSource.Pause();
        }
    }

    public void PauseAllAmbienceSounds() {
        foreach (AudioSource audioSource in audioSourcesAmbient) {
            audioSource.Pause();
        }
    }

    public void StopCurrentAmbientAndPlay(string name, float fadeInSeconds = 0.0f, float fadeOutSeconds = 0.0f) {
        Sound newSound = Array.Find(sounds, x => x.name == name);
        if (newSound == null) {
            Debug.LogError("Sound not found: " + name);
            return;
        }

        // find the currently playing ambient sound
        AudioSource currentAmbient = audioSourcesAmbient.FirstOrDefault(src => src.isPlaying);
        Debug.Log("Current ambient sound: " + (currentAmbient != null ? currentAmbient.clip.name : "None"));
        if (currentAmbient != null && currentAmbient.clip != null) {
            float fadeDuration = fadeOutSeconds;
            StartCoroutine(FadeOutAndPlayNewAmbient(currentAmbient, newSound, fadeInSeconds, fadeDuration));
        } else {
            Play(name, fadeInSeconds);
        }
    }

    private IEnumerator FadeOutAndPlayNewAmbient(AudioSource currentAmbient, Sound newSound, float fadeInSeconds, float fadeDuration) {
        Coroutine fadeOutCoroutine = StartCoroutine(FadeVolume(currentAmbient, 0f, fadeDuration));
        Play(newSound.name, fadeInSeconds);
        yield return fadeOutCoroutine;
        currentAmbient.Stop();
    }

    public void PauseAllSFXSounds() {
        foreach (AudioSource audioSource in audioSourcesSFX) {
            audioSource.Pause();
        }
    }

    public void UnPauseAllSounds() {
        for (int i = 0; i < audioSourcesSoundtrack.Count; i++) {
            audioSourcesSoundtrack[i].UnPause();
        }

        for (int i = 0; i < audioSourcesAmbient.Count; i++) {
            audioSourcesAmbient[i].UnPause();
        }

        for (int i = 0; i < audioSourcesSFX.Count; i++) {
            audioSourcesSFX[i].UnPause();
        }
    }

    private IEnumerator FadeVolume(AudioSource source, float targetVolume, float duration) {
        Debug.Log("Fading volume from " + source.volume + " to " + targetVolume + " over " + duration + " seconds.");
        float startVolume = source.volume;
        float time = 0f;

        while (time < duration) {
            if (isSceneChanging || source == null) 
                yield break;    // stop fading if scene is changing or source is null
            
            time += Time.deltaTime;
            source.volume = Mathf.Lerp(startVolume, targetVolume, time / duration);
            yield return null;
        }
        source.volume = targetVolume;

        Debug.Log("Volume fade complete");
    }

    private IEnumerator FadeOut(AudioSource source, float fadeDuration) {
        float clipLength = source.clip.length;
        float waitTime = Mathf.Max(clipLength - fadeDuration, 0f);
        yield return new WaitForSeconds(waitTime);

        StartCoroutine(FadeVolume(source, 0f, fadeDuration));
    }

    private IEnumerator PlayAfterCurrentEnds(AudioSource currentSource, Sound nextSound) {
        if (currentSource != null && currentSource.isPlaying) {
            yield return new WaitUntil(() => !currentSource.isPlaying);
            Debug.Log("Current sound ended, playing next sound: " + nextSound.name);
            currentSource.Pause();
            Destroy(currentSource);
        }

        // Neues AudioSource setzen
        GameObject parent = GetParentByType(nextSound.type);
        AudioSource newSource = CopyAudioDataToEmpty(nextSound, parent);
        newSource.Play();
    }

    private void DestroyAllAudioSources(){
        for (int i = 0; i < audioSourcesSoundtrack.Count; i++) {
            Destroy(audioSourcesSoundtrack[i]);
        }

        for (int i = 0; i < audioSourcesAmbient.Count; i++) {
            Destroy(audioSourcesAmbient[i]);
        }

        for (int i = 0; i < audioSourcesSFX.Count; i++) {
            Destroy(audioSourcesSFX[i]);
        }

        audioSourcesSoundtrack.Clear();
        audioSourcesAmbient.Clear();
        audioSourcesSFX.Clear();
    } 
}