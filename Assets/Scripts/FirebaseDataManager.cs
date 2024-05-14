using System;
using System.Collections.Generic;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;

using FirebaseWebGL.Scripts.FirebaseBridge;


public class FirebaseDataManager : MonoBehaviour
{
    public static FirebaseDataManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void RegisterOrLogin(string username)
    {
        int totalLevels = SceneManager.sceneCountInBuildSettings - 3;
        FirebaseDatabase.RegisterOrLogin(username, totalLevels);
    }

    public void SignOutAndSaveData(string username)
    {
        UserData userData = new UserData
        {
            username = username,
            items = new UserItems
            {
                hints = PlayerPrefs.GetInt("Hints", 0),
                autoCompletes = PlayerPrefs.GetInt("AutoCompletes", 0),
                coins = PlayerPrefs.GetInt("WalletAmount", 0)
            },
            levelMenu = new Dictionary<string, LevelInfo>()
        };

        for (int i = 1; i <= SceneManager.sceneCountInBuildSettings - 3; i++)
        {
            string key = $"Level_{i}";
            int stars = PlayerPrefs.GetInt(key, 0);
            userData.levelMenu[key] = new LevelInfo { starsEarned = stars };
        }
        string jsondata = JsonConvert.SerializeObject(userData);
        FirebaseDatabase.UpdateJSON($"users/{username}", jsondata, "OnDataSaved", "OnDataSaveFailed");
    }

    public void UploadLevelScore(string levelName, string playerName, float timeSpent)
    {
        FirebaseDatabase.UploadLevelScore(levelName, playerName, timeSpent.ToString("F2"));
    }

    public void RetrieveLeaderboard()
    {
        FirebaseDatabase.RetrieveLeaderboard();
    }
}