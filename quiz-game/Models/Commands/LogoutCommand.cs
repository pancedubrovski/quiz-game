namespace quiz_game.Models.Commands
{
    public class LogoutCommand
    {
        public string Username { get; set; }
        public string RoomName { get; set; }
        public int Score { get; set; }
    }
}
