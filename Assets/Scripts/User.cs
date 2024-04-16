public class User
{
    public string username;
    public int completeDuration;

    public User(string username)
    {
        this.username = username;
        this.completeDuration = -1;
    }

    public User(string username, int completeDuration)
    {
        this.username = username;
        this.completeDuration = completeDuration;
    }

    public User()
    {}
}