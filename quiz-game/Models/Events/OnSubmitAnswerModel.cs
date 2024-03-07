namespace quiz_game.Models.events
{
    public class OnSubmitAnswerModel
    {
        public int QuestionId { get; set; }
        public string Username { get; set; }
        public bool Result { get; set; }
    }
}
