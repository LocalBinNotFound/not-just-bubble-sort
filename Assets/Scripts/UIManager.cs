using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;

public class UIManager : MonoBehaviour
{
    public GameObject loginMenu;
    public GameObject userMenu;
    public TMP_InputField usernameInput;
    public TextMeshProUGUI usernameText;
    public TextMeshProUGUI starEarnedText;
    public TextMeshProUGUI walletAmountText;
    public Transform leaderboardContent;
    public GameObject levelRankPrefab; 
    private bool isUserSignedIn = false;
    private int totalLevels;

    void Start()
    {
        totalLevels = SceneManager.sceneCountInBuildSettings - 3;
        UserDataObject.Instance.OnUserDataUpdated += OnUserDataUpdated;
        CheckUserSignInState();
    }

    public void OnSignInButtonClicked()
    {
        string username = usernameInput.text.Trim();
        if (string.IsNullOrEmpty(username))
        {
            Debug.LogError("Username cannot be empty.");
            return;
        }
        FirebaseDataManager.Instance.RegisterOrLogin(username);
        
        isUserSignedIn = true;
        loginMenu.SetActive(false);
        userMenu.SetActive(true);
        usernameText.text = $"{username}";
        PlayerPrefs.SetInt("IsUserSignedIn", 1);
        PlayerPrefs.SetString("Username", username);
    }

    public void OnSignOutButtonClicked()
    {
        string username = PlayerPrefs.GetString("Username", "");
        if (!string.IsNullOrEmpty(username))
        {
            FirebaseDataManager.Instance.SignOutAndSaveData(username);
        }

        for (int i = 1; i <= totalLevels; i++)
        {
            PlayerPrefs.SetInt($"Level_{i}", 0);
        }
        PlayerPrefs.SetInt("Hints", 3);
        PlayerPrefs.SetInt("AutoCompletes", 1);
        PlayerPrefs.SetInt("WalletAmount", 100);
        PlayerPrefs.Save();

        isUserSignedIn = false;
        loginMenu.SetActive(true);
        userMenu.SetActive(false);
        PlayerPrefs.SetInt("IsUserSignedIn", 0);
        PlayerPrefs.DeleteKey("Username");
    }

    private void CheckUserSignInState()
    {
        isUserSignedIn = PlayerPrefs.GetInt("IsUserSignedIn", 0) == 1;
        
        loginMenu.SetActive(!isUserSignedIn);
        userMenu.SetActive(isUserSignedIn);

        if (isUserSignedIn)
        {
            usernameText.text = PlayerPrefs.GetString("Username", "");
            walletAmountText.text = $"{PlayerPrefs.GetInt("WalletAmount")}";
            starEarnedText.text = $"{PlayerPrefs.GetInt("TotalStarsEarned")}";
        }
    }

    private void OnUserDataUpdated()
    {
        walletAmountText.text = $"{PlayerPrefs.GetInt("WalletAmount")}";
        starEarnedText.text = $"{PlayerPrefs.GetInt("TotalStarsEarned")}";
    }

    public void PopulateLeaderboard()
    {
        FirebaseDataManager.Instance.RetrieveLeaderboard();
    }

    public void UpdateLeaderboardUI(string jsonData)
    {
        List<LevelRank> leaderboardRanks = JsonConvert.DeserializeObject<List<LevelRank>>(jsonData);
        foreach (Transform child in leaderboardContent.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (LevelRank rank in leaderboardRanks)
        {
            GameObject rankInstance = Instantiate(levelRankPrefab, leaderboardContent);
            LevelRankItem rankItem = rankInstance.GetComponent<LevelRankItem>();
            rankItem.Setup(rank);
        }
    }

    void OnApplicationQuit()
    {
        PlayerPrefs.SetInt("IsUserSignedIn", 0);
        PlayerPrefs.Save();
    }
}
