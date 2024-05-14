using System.Collections.Generic;

[System.Serializable]
public class UserData
{
    public string username;
    public UserItems items;
    public Dictionary<string, LevelInfo> levelMenu;
}

[System.Serializable]
public class UserItems
{
    public int hints;
    public int autoCompletes;
    public int coins;
}

[System.Serializable]
public class LevelInfo
{
    public int starsEarned;

}

