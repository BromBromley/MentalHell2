using System.IO;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    // this script manages the saving system

    private string savePath;
    public static GameData saveData = new GameData();


    void Awake()
    {
        savePath = Application.persistentDataPath + "/gamesave.json";
    }


    public void SaveGame(GameData data)
    {
        string json = JsonUtility.ToJson(data, true);
        if (File.Exists(savePath))
        {
            JsonUtility.FromJsonOverwrite(json, savePath);
        }
        else
        {
            File.WriteAllText(savePath, json);
        }
        Debug.Log("game has been saved");
    }


    public GameData LoadGame()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            return JsonUtility.FromJson<GameData>(json);
        }
        return null;
    }

}
