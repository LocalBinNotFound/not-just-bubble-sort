using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    private FirebaseClient firebaseClient;
    private bool isUserSignedIn = false;
    private int totalLevels;

    void Start()
    {
        totalLevels = SceneManager.sceneCountInBuildSettings - 3;
        firebaseClient = new FirebaseClient(new FirebaseListener(this));
        firebaseClient.OnTotalStarsRetrieved += UpdateTotalStars;
        firebaseClient.OnUserDataRetrieved += UpdateUserData;
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
        User user = new(username);
        firebaseClient.RegisterOrLogin(user);
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
            int hints = PlayerPrefs.GetInt("Hints", 0);
            int autoCompletes = PlayerPrefs.GetInt("AutoComplete", 0);
            int coins = PlayerPrefs.GetInt("WalletAmount", 0);
            firebaseClient.SaveUserData(username, hints, autoCompletes, coins);
            firebaseClient.SaveAllLevelsData(username);
        }

        for (int i = 1; i <= totalLevels; i++)
        {
            PlayerPrefs.SetInt($"Level_{i}", 0);
        }
        PlayerPrefs.SetInt("Hints", 3);
        PlayerPrefs.SetInt("AutoComplete", 1);
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
        }
    }

    public void PopulateLeaderboard()
    {
        firebaseClient.RetrieveLeaderboard(leaderboardRanks => {
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
        });
    }

    private void UpdateUserData(int[] userData)
    {
        walletAmountText.text = $"{userData[2]}";
    }

    public void UpdateTotalStars(int totalStars)
    {
        starEarnedText.text = $"{totalStars}";
    }
}

public class FirebaseListener : IFirebaseListener
{
    private UIManager uiManager;

    public FirebaseListener(UIManager uiManager)
    {
        this.uiManager = uiManager;
    }

    public void OnLeaderboardRetrieveCompleted(List<User> users)
    {
    }
}
