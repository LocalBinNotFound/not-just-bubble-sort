using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class LevelContainer : MonoBehaviour
{
    public TextMeshProUGUI levelNumberText;
    public GameObject[] filledStars; // Assign references to filled star images in the inspector
    public Button levelButton; // Assign reference to the button in the inspector

    public void SetupLevel(int levelNumber, int starsEarned)
    {
        levelNumberText.text = levelNumber.ToString();
        for (int i = 0; i < filledStars.Length; i++)
        {
            filledStars[i].SetActive(i < starsEarned);
        }

        levelButton.onClick.AddListener(() => LoadLevel(levelNumber));
    }

    private void LoadLevel(int levelNumber)
    {
        SceneManager.LoadScene(levelNumber + 1); // +1 because the build index starts from 0
    }
}