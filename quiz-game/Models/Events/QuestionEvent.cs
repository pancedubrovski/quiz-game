using quiz_game.Models.Enums;

namespace quiz_game.Models.events
{
    public class QuestionEvent
    {
        public int QuestionId { get; set; }
        public string Expression { get; set; }
        public string Status { get; set; }
        public string Kind { get; set; }
    }
}
