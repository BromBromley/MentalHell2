using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DiscordManager : MonoBehaviour
{
    public static DiscordManager Instance;

    private Discord.Discord discord;

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
        discord = new Discord.Discord(1385606125797376030, (ulong)Discord.CreateFlags.Default);
        ChangeActivity();
    }

    void Update()
    {
        discord.RunCallbacks();
    }

    void OnDisable()
    {
        discord.Dispose();
    }

    public void ChangeActivity()
    {
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
