using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LevelContainer : MonoBehaviour
{
    public TextMeshProUGUI levelNumberText;
    public GameObject[] filledStars; // Assign references to filled star images in the inspector
    public Button levelButton; // Assign reference to the button in the inspector
    private LevelLoader levelLoader; // Reference to the LevelLoader

    public void SetupLevel(int levelNumber, int starsEarned, bool isUnlocked, LevelLoader loader)
    {
        levelLoader = loader;

        levelNumberText.text = levelNumber.ToString();
        for (int i = 0; i < filledStars.Length; i++)
        {
            filledStars[i].SetActive(i < starsEarned);
        }

        if (isUnlocked)
        {
            levelButton.onClick.AddListener(() => levelLoader.LoadLevel(levelNumber+1)); // No +1 here
        }
    }

}
