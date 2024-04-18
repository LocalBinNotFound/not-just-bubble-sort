using System;
using System.Collections.Generic;
using System.Collections;
using System.Threading.Tasks;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;
using UnityEngine.SceneManagement;


public class FirebaseClient
{
    private IFirebaseListener listener;
    private int totalLevels = SceneManager.sceneCountInBuildSettings - 3;
    public Action<int> OnTotalStarsRetrieved;
    public Action<int[]> OnUserDataRetrieved;

    public FirebaseClient(IFirebaseListener listener)
    {
        this.listener = listener;
    }

    public FirebaseClient()
    {
        //blank on purpose
    }

    public void RegisterOrLogin(User user)
    {
        DatabaseReference userRef = FirebaseDatabase.DefaultInstance.GetReference("users").Child(user.username);
        userRef.Child("username").SetValueAsync(user.username).ContinueWithOnMainThread(regTask => {
            if (regTask.IsFaulted)
            {
                Debug.LogError("Error registering/logging in user: " + regTask.Exception);
            }
            else if (regTask.IsCompleted)
            {
                userRef.Child("levelMenu").GetValueAsync().ContinueWithOnMainThread(levelTask => {
                    if (!levelTask.Result.Exists)
                    {
                        DatabaseReference levelsRef = FirebaseDatabase.DefaultInstance.GetReference($"users/{user.username}/levelMenu");
                        for (int i = 1; i <= totalLevels; i++)
                        {
                            levelsRef.Child($"Level_{i}").Child("starsEarned").SetValueAsync(0);
                        }
                    }
                });
                userRef.Child("items").GetValueAsync().ContinueWithOnMainThread(itemsTask => {
                    if (!itemsTask.Result.Exists)
                    {
                        userRef.Child("items").Child("hints").SetValueAsync(3);
                        userRef.Child("items").Child("autoCompletes").SetValueAsync(1);
                        userRef.Child("items").Child("coins").SetValueAsync(100);

                        DatabaseReference levelsRef = FirebaseDatabase.DefaultInstance.GetReference($"users/{user.username}/levelMenu");
                        for (int i = 1; i <= totalLevels; i++)
                        {
                            levelsRef.Child($"Level_{i}").Child("starsEarned").SetValueAsync(0);
                        }
                    }
                });
            }
        });
        RetrieveLevelsData(user.username);
        RetrieveUserData(user.username);
    }

    public void UpdateCompleteDuration(User user)
    {
        FirebaseDatabase.DefaultInstance.GetReference("users").Child(user.username).Child("completeDuration").SetValueAsync(user.completeDuration);
    }

    public void RetrieveLeaderboard(Action<List<LevelRank>> onCompleted)
    {
        DatabaseReference levelRef = FirebaseDatabase.DefaultInstance.GetReference("levels");

        levelRef.GetValueAsync().ContinueWithOnMainThread(task => {
            if (task.IsFaulted)
            {
                Debug.LogError("Error retrieving leaderboard data: " + task.Exception);
            }
            else if (task.IsCompleted)
            {
                List<LevelRank> leaderboardRanks = new List<LevelRank>();
                DataSnapshot snapshot = task.Result;
                if (snapshot.Exists)
                {
                    foreach (DataSnapshot levelSnapshot in snapshot.Children)
                    {
                        string levelName = levelSnapshot.Key;
                        string username = levelSnapshot.Child("player").Value.ToString();
                        float time = float.Parse(levelSnapshot.Child("time").Value.ToString());
                        leaderboardRanks.Add(new LevelRank(levelName, username, time));
                        Debug.Log($"{levelName}, {username}, {time}");
                    }
                }
                onCompleted(leaderboardRanks);
            }
        });
    }
    
    // get all levels progress from firebase
    public void RetrieveLevelsData(string username)
    {
        string path = $"users/{username}/levelMenu";
        DatabaseReference levelsRef = FirebaseDatabase.DefaultInstance.GetReference(path);
        int totalStars = 0;

        levelsRef.GetValueAsync().ContinueWithOnMainThread(task => {
            if (task.IsFaulted)
            {
                Debug.LogError("Error retrieving levels data: " + task.Exception);
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                if (snapshot.Exists)
                {
                    foreach (DataSnapshot levelSnapshot in snapshot.Children)
                    {
                        int levelIndex = int.Parse(levelSnapshot.Key.Replace("Level_", ""));
                        int starsEarned = int.Parse(levelSnapshot.Child("starsEarned").Value.ToString());
                        PlayerPrefs.SetInt($"Level_{levelIndex}", starsEarned);
                        totalStars += starsEarned;
                    }
                    OnTotalStarsRetrieved?.Invoke(totalStars);

                }
            }
        });
    }
    // upload level progress to firebase
    public void SaveAllLevelsData(string username)
    {
        Dictionary<string, object> levelsDataToUpdate = new Dictionary<string, object>();

        for (int i = 1; i <= totalLevels; i++)
        {
            string key = $"Level_{i}";
            int starsEarned = PlayerPrefs.GetInt(key, 0);
            string levelPath = $"levelMenu/{key}/starsEarned";
            levelsDataToUpdate[levelPath] = starsEarned;
        }

        string userPath = $"users/{username}";
        DatabaseReference userRef = FirebaseDatabase.DefaultInstance.GetReference(userPath);

        userRef.UpdateChildrenAsync(levelsDataToUpdate)
            .ContinueWithOnMainThread(task =>
            {
                if (task.IsFaulted)
                {
                    Debug.LogError("Failed to save all level data: " + task.Exception);
                }
            });
    }

    // save user items to firebase
    public void SaveUserData(string username, int hints, int autoCompletes, int coins)
    {
        string path = $"users/{username}/items";
        Dictionary<string, object> itemsData = new Dictionary<string, object>
        {
            { "hints", hints },
            { "autoCompletes", autoCompletes },
            { "coins", coins }
        };

        DatabaseReference itemsRef = FirebaseDatabase.DefaultInstance.GetReference(path);
        itemsRef.UpdateChildrenAsync(itemsData)
            .ContinueWithOnMainThread(task =>
            {
                if (task.IsFaulted)
                {
                    Debug.LogError("Failed to save user item data: " + task.Exception);
                }
            });
    }

    // get user items from database
    public void RetrieveUserData(string username)
    {
        string path = $"users/{username}/items";
        DatabaseReference itemsRef = FirebaseDatabase.DefaultInstance.GetReference(path);

        itemsRef.GetValueAsync().ContinueWithOnMainThread(task => {
            if (task.IsFaulted)
            {
                Debug.LogError("Error retrieving user item data: " + task.Exception);
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                if (snapshot.Exists)
                {
                    int hints = int.Parse(snapshot.Child("hints").Value.ToString());
                    int autoCompletes = int.Parse(snapshot.Child("autoCompletes").Value.ToString());
                    int coins = int.Parse(snapshot.Child("coins").Value.ToString());
                    PlayerPrefs.SetInt("Hints", hints);
                    PlayerPrefs.SetInt("AutoComplete", autoCompletes);
                    PlayerPrefs.SetInt("WalletAmount", coins);
                    OnUserDataRetrieved?.Invoke(new int[] {hints, autoCompletes, coins});
                }
            }
        });
    }

    //save the data of user with best score to database
    public IEnumerator UploadUserScore(string levelName, string playerName, int timeSpent)
    {
        DatabaseReference rootRef = FirebaseDatabase.DefaultInstance.RootReference;
        Task<DataSnapshot> DBTask = rootRef.Child("levels").Child(levelName).GetValueAsync();

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else if (DBTask.Result.Value == null)
        {
            //No data exists yet
            rootRef.Child("levels").Child(levelName).Child("player").SetValueAsync(playerName);
            rootRef.Child("levels").Child(levelName).Child("time").SetValueAsync(timeSpent);
        }
        else
        {
            DataSnapshot snapshot = DBTask.Result;

            if (int.Parse(snapshot.Child("time").Value.ToString().Trim()) > timeSpent)
            {
                rootRef.Child("levels").Child(levelName).Child("time").SetValueAsync(timeSpent);
                rootRef.Child("levels").Child(levelName).Child("player").SetValueAsync(playerName);
            }

        }
    }
}
