using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectManager : MonoBehaviour
{
    public GameObject unlockedLevelContainerPrefab;
    public GameObject lockedLevelContainerPrefab;
    public GameObject orientationReminderDialog;
    public Transform levelGridParent;
    public LevelLoader levelLoader;
    public int levelsPerPage = 9;
    private int currentPage = 0;

    void Start()
    {
        UpdateLevelSelectMenu();
    }

    public void UpdateLevelSelectMenu()
    {
        ClearLevelGrid();
        int totalLevels = SceneManager.sceneCountInBuildSettings - 3; 
        int startSceneIndex = currentPage * levelsPerPage + 2;
        int endSceneIndex = Mathf.Min(startSceneIndex + levelsPerPage-1, totalLevels+1);
        Debug.Log((startSceneIndex, endSceneIndex));

        for (int sceneIndex = startSceneIndex; sceneIndex <= endSceneIndex; sceneIndex++)
        {
            int levelIndex = sceneIndex-1;
            int starsEarned = LoadStarsEarned(levelIndex);
            bool isUnlocked = (levelIndex == 1) || LoadStarsEarned(levelIndex - 1) >= 2;
            Debug.Log($"Level {levelIndex} has {starsEarned} stars");


            GameObject levelContainerPrefab = isUnlocked ? unlockedLevelContainerPrefab : lockedLevelContainerPrefab;
            GameObject levelContainer = Instantiate(levelContainerPrefab, levelGridParent);
            LevelContainer levelContainerScript = levelContainer.GetComponent<LevelContainer>();

            levelContainerScript.SetupLevel(levelIndex, starsEarned, isUnlocked, levelLoader);
        }
    }
    private int LoadStarsEarned(int levelIndex)
    {
        string key = "Level_" + (levelIndex); // +1 because levels start from 1
        return PlayerPrefs.GetInt(key, 0);
    }

    public void ChangePage(int change)
    {
        currentPage += change;
        currentPage = Mathf.Clamp(currentPage, 0, (SceneManager.sceneCountInBuildSettings - 3) / levelsPerPage);
        UpdateLevelSelectMenu();
    }

    private void ClearLevelGrid()
    {
        foreach (Transform child in levelGridParent)
        {
            Destroy(child.gameObject);
        }
    }
    public void OnLevelSelected(int levelIndex)
    {
        orientationReminderDialog.SetActive(true); // Show the reminder dialog
        levelLoader.LoadLevel(levelIndex); // Set to load the level when "Continue" is clicked
    }

    public void OnContinueClicked()
    {
        orientationReminderDialog.SetActive(false); // Hide the dialog
        // No need to call LoadLevel here because it's already set to begin loading when OnLevelSelected is called
    }
}
