
namespace quiz_game.Models.Events
{

    public class AnswerEvent {
        public int QuestionId { get; set; }
        public string Result { get; set; }
        public string Username { get; set; }
        public string RoomName { get; set; }
    }

}
