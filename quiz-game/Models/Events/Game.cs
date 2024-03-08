namespace quiz_game.Models.events
{
    public class StartGame
    {
        public string Username { get; set; }
        public string RoomName { get; set; }
        public string GameStatus { get; set; }
        public int BestScore { get; set; }
    }
}
