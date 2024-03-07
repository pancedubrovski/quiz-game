namespace quiz_game.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public int Points { get; set; }

        public User(string username)
        {
            Username = username;
            Points = 0;
        }

    }
}
