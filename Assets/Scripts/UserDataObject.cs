using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Threading.Tasks;


public class UserDataObject : MonoBehaviour
{
    public static UserDataObject Instance;
    public event Action OnUserDataUpdated;

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

    public void UpdateUserData(string jsondata)
    {
        int totalStarsEarned = 0;
        UserData userData = JsonConvert.DeserializeObject<UserData>(jsondata);
        PlayerPrefs.SetInt("Hints", userData.items.hints);
        PlayerPrefs.SetInt("AutoCompletes", userData.items.autoCompletes);
        PlayerPrefs.SetInt("WalletAmount", userData.items.coins);

        foreach (var level in userData.levelMenu)
        {
            string key = $"{level.Key}";
            int stars = level.Value.starsEarned;
            totalStarsEarned += stars;
            PlayerPrefs.SetInt(key, stars);
        }
        PlayerPrefs.SetInt("TotalStarsEarned", totalStarsEarned);
        PlayerPrefs.Save();

        OnUserDataUpdated?.Invoke();
    }
}