using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject loginMenu;
    public GameObject userMenu;
    public TMP_InputField usernameInput;
    public TextMeshProUGUI usernameText;
    public GameObject leaderboardPanel;
    public GameObject levelRankPrefab; 
    private FirebaseClient firebaseClient;
    private bool isUserSignedIn = false;
    private List<LevelRank> ranks;

    void Start()
    {
        firebaseClient = new FirebaseClient(new FirebaseListener(this));
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
        User user = new User(username);
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
        isUserSignedIn = false;
        loginMenu.SetActive(true);
        userMenu.SetActive(false);
        PlayerPrefs.SetInt("IsUserSignedIn", 0);
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

    public void OnLeaderboardButtonClicked()
    {
        firebaseClient.RetrieveLeaderboard();
    }

    public void PopulateLeaderboard()
    {
        foreach (LevelRank rank in ranks)
        {
            GameObject rankInstance = Instantiate(levelRankPrefab, leaderboardPanel.transform);
            LevelRankItem rankItem = rankInstance.GetComponent<LevelRankItem>();
            rankItem.Setup(rank);
        }
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
         uiManager.PopulateLeaderboard();
    }
}
