using System.Collections.Generic;

public interface IFirebaseListener
{
    void OnLeaderboardRetrieveCompleted(List<User> users);
}
