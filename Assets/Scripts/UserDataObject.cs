using UnityEngine;
using System.Collections.Generic;

public class UserDataObject : MonoBehaviour
{
    public void UpdateUserData(string jsondata)
    {
        Debug.Log("Received user data: " + jsondata);
        UserData userData = JsonUtility.FromJson<UserData>(jsondata);
        PlayerPrefs.SetInt("Hints", userData.items.hints);
        PlayerPrefs.SetInt("AutoCompletes", userData.items.autoCompletes);
        PlayerPrefs.SetInt("Coins", userData.items.coins);
        Debug.Log("Hints: " + PlayerPrefs.GetInt("Hints"));
        Debug.Log("AutoCompletes: " + PlayerPrefs.GetInt("AutoCompletes"));
        Debug.Log("Coins: " + PlayerPrefs.GetInt("Coins"));

        foreach (var level in userData.levelMenu)
        {
            string key = level.Key.ToString();
            int val = int.Parse(level.Value.ToString());
            PlayerPrefs.SetInt(key, val);
        }
        PlayerPrefs.Save();


    }
}

[System.Serializable]
public class UserData
{
    public string username;
    public UserItems items;
    public Dictionary<string, int> levelMenu;
}

[System.Serializable]
public class UserItems
{
    public int hints;
    public int autoCompletes;
    public int coins;
}

