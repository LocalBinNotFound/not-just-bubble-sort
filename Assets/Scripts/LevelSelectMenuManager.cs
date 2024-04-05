using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectManager : MonoBehaviour
{
    public GameObject unlockedLevelContainerPrefab;
    public GameObject lockedLevelContainerPrefab;
    public Transform levelGridParent;
    public int levelsPerPage = 9;
    private int currentPage = 0;

    void Start()
    {
        UpdateLevelSelectMenu();
    }

    public void UpdateLevelSelectMenu()
    {
        ClearLevelGrid();
        int totalLevels = SceneManager.sceneCountInBuildSettings - 2; // Excluding main menu and level select scenes
        int startLevel = currentPage * levelsPerPage;
        int endLevel = Mathf.Min(startLevel + levelsPerPage, totalLevels);
        
        for (int levelIndex = startLevel+1; levelIndex <= endLevel; levelIndex++)
        {
            int starsEarned = LoadStarsEarned(levelIndex);
            bool isUnlocked = (levelIndex == 1) || LoadStarsEarned(levelIndex - 1) >= 2;

            GameObject levelContainerPrefab = isUnlocked ? unlockedLevelContainerPrefab : lockedLevelContainerPrefab;
            GameObject levelContainer = Instantiate(levelContainerPrefab, levelGridParent);
            LevelContainer levelContainerScript = levelContainer.GetComponent<LevelContainer>();
        
            levelContainerScript.SetupLevel(levelIndex, starsEarned);
        }
    }

    private int LoadStarsEarned(int levelIndex)
    {
        string key = "Level_" + (levelIndex + 1); // +1 because levels start from 1
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
}
