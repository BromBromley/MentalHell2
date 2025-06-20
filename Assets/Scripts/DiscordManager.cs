using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DiscordManager : MonoBehaviour
{
    Discord.Discord discord;

    void Start()
    {
        discord = new Discord.Discord(1385606125797376030, (ulong)Discord.CreateFlags.Default);
        ChangeActivity();
    }

    void OnDisable()
    {
        discord.Dispose();
    }

    public void ChangeActivity()
    {
        var activityManager = discord.GetActivityManager();
        var activity = new Discord.Activity {
            State = "Playing",
            Details = "Currently Developing"
        };
        activityManager.UpdateActivity(activity, (res) => {
            if (res == Discord.Result.Ok) {
                Debug.Log("Activity updated!");
            } else {
                Debug.LogError("Failed to update activity: " + res);
            }
        });
    }

    void Update()
    {
        discord.RunCallbacks();
    }
}