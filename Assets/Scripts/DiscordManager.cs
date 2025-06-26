using UnityEngine;
using System;

public class DiscordManager : MonoBehaviour
{
    public static DiscordManager Instance;

    private Discord.Discord discord;
    private bool discordInitialized = false;

    void Awake()
    {
        // Singleton-Schutz
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        try
        {
            discord = new Discord.Discord(1385606125797376030, (ulong)Discord.CreateFlags.NoRequireDiscord);
            discordInitialized = true;
            ChangeActivity();
        }
        catch (Exception e)
        {
            Debug.LogWarning("Discord konnte nicht initialisiert werden: " + e.Message);
        }
    }

    void Update()
    {
        if (discordInitialized)
        {
            try
            {
                discord.RunCallbacks();
            }
            catch (Exception e)
            {
                Debug.LogWarning("Fehler beim Ausführen von Discord-Callbacks: " + e.Message);
            }
        }
    }

    void OnDisable()
    {
        if (discordInitialized)
        {
            discord.Dispose();
        }
    }

    public void ChangeActivity()
    {
        if (!discordInitialized) return;

        var activityManager = discord.GetActivityManager();
        var activity = new Discord.Activity
        {
            State = "Playing",
            Details = "Tech Test Demo"
        };

        activityManager.UpdateActivity(activity, (res) =>
        {
            if (res == Discord.Result.Ok)
            {
                Debug.Log("Activity updated!");
            }
            else
            {
                Debug.LogError("Failed to update activity: " + res);
            }
        });
    }
}
