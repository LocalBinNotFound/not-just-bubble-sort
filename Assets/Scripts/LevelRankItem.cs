using UnityEngine;
using TMPro;

public class LevelRankItem : MonoBehaviour
{
    public TextMeshProUGUI levelNameText;
    public TextMeshProUGUI usernameText;
    public TextMeshProUGUI timeText;

    public void Setup(LevelRank rank)
    {
        levelNameText.text = rank.levelName;
        usernameText.text = rank.username;
        timeText.text = rank.time.ToString();
    }
}

public class LevelRank
{
    public string levelName;
    public string username;
    public float time;
    public LevelRank(string levelName, string username, float time)
    {
        this.levelName = levelName;
        this.username = username;
        this.time = time;
    }
}
