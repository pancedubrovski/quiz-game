using Microsoft.AspNetCore.SignalR;
using quiz_game.Models.Enums;
using quiz_game.Models.events;
using quiz_game.Models.Events;
using quiz_game.Services;
using quiz_game.Services.interfalces;
using quiz_game.Utilites;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace quiz_game.Hubs
{
    public class GameHub : Hub
    {

        private readonly IGameService _gameService;


        public GameHub(IGameService gameService)
        {
            _gameService = gameService;
        }

        public override async Task OnConnectedAsync()
        {
            await Clients.Caller.SendAsync(Events.USER_CONNECTED);
        }

        public async Task AddUserData(StartGame startGame)
        {
            try
            {
                if (startGame.Username == null)
                {
                    return;
                }
                Enum.TryParse<GameStatus>(startGame.GameStatus, out GameStatus status);
                await Groups.AddToGroupAsync(Context.ConnectionId, startGame.RoomName);
                int onlineUsers = _gameService.GetOnlineUsers(startGame.RoomName);

                if (GameStatus.StartGame == status)
                {
                    QuestionEvent question = _gameService.GetNotAnswarQuestion(startGame.RoomName, QuestionStatus.NotAnswered);
                    await Clients.Groups(startGame.RoomName).SendAsync(Events.NEXT_QUESTION, question);
                }
                else if (GameStatus.Started == status)
                {
                    QuestionEvent question = _gameService.GetNotAnswarQuestion(startGame.RoomName, QuestionStatus.Current);
                    await Clients.Caller.SendAsync(Events.NEXT_QUESTION, question);
                }

                await Clients.Groups(startGame.RoomName).SendAsync(Events.ONLINE_USERS, onlineUsers);
            }
            catch (Exception e)
            {
                throw;
            }
        
        }
        public async Task SubmitAnswer(AnswerEvent answer)
        {
          
            var model = _gameService.CheckAnswerQuestion(answer);

            if (model.Result || (!model.Result &&
                _gameService.CheckIsAllPlayerAnswerd(answer.RoomName, answer.QuestionId)))
            {
                QuestionEvent question = _gameService
                    .GetNotAnswarQuestion(answer.RoomName, QuestionStatus.NotAnswered);

                await Clients.Groups(answer.RoomName).SendAsync(Events.ON_SUBMIT_ANSWER, model);
                await Clients.Groups(answer.RoomName).SendAsync(Events.NEXT_QUESTION, question);
            }else
            {
                await Clients.Caller.SendAsync(Events.ON_SUBMIT_ANSWER, model);
            }
        }

    }
}
