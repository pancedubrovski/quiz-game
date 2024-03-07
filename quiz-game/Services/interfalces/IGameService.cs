using quiz_game.Models;
using quiz_game.Models.Commands;
using quiz_game.Models.Enums;
using quiz_game.Models.events;
using quiz_game.Models.Events;

namespace quiz_game.Services.interfalces
{
    public interface IGameService
    {
        public Room CheckFreeGroup();
        public Room CreateRoom();
        public Task<StartGame> StartGame(UserCommands command, string acction);
        public QuestionEvent GetNotAnswarQuestion(string roomName,QuestionStatus status);
        public OnSubmitAnswerModel CheckAnswerQuestion(AnswerEvent answerEvent);
        public bool CheckIsAllPlayerAnswerd(string roomName, int questionId);
        public Task OnDisconecntClient(LogoutCommand command);
        public int GetOnlineUsers(string roomName);

        
    }
}
