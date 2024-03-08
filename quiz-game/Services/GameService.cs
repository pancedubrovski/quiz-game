using quiz_game.Utilites;
using quiz_game.Database.Entites;
using quiz_game.Models;
using quiz_game.Models.Commands;
using quiz_game.Models.Enums;
using quiz_game.Models.events;
using quiz_game.Repositories.Interfaces;
using quiz_game.Services.interfalces;
using quiz_game.Models.Events;


namespace quiz_game.Services
{
    public class GameService : IGameService
    {
        private readonly IUserRepository _userRepository;
        public GameService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public Room CheckFreeGroup()
        {
            return DataManager.GetRooms().FindLast(r => r.Status == RoomStatus.draft || (
            r.Status == RoomStatus.active && r.Users.Count < 6));
        }

        public Room CreateRoom()
        {
           
            string roomName = "room-" + DataManager.NextId;
            Room room = new Room(roomName);
            DataManager.GetRooms().Add(room);
            DataManager.NextId += 1;
            return room;
            

        }
        public QuestionEvent GetNotAnswarQuestion(string roomName, QuestionStatus status)
        {

            Question question = null;
            if (QuestionStatus.Current == status)
            {
                question = DataManager.GetRooms().Find(e => e.Name == roomName)
                    .Questions.FindLast(q => q.Status == QuestionStatus.Current);
            }
            else if (QuestionStatus.NotAnswered == status)
            {
                question = DataManager.AddQuestion(roomName);
            }
            if (question is null)
            {
                throw new Exception("question is null");
            }

            return new QuestionEvent
            {
                Expression = question.Expression,
                QuestionId = question.QuestionId,
                Status = question.Status.ToString(),
                Kind = question.Kind.ToString()
            };
        }


        public OnSubmitAnswerModel CheckAnswerQuestion(AnswerEvent answerEvent)
        {
            
            Room room = DataManager.GetRooms().Find(r => r.Name == answerEvent.RoomName);
            Question question =room.Questions.FirstOrDefault(e => e.QuestionId == answerEvent.QuestionId);
            bool isCorrected = false;
            if (Double.TryParse(answerEvent.Result, out double res))
            {
                isCorrected = (question as BonusQuestion).CheckAnswer(res);
            }
            else if (Boolean.TryParse(answerEvent.Result, out bool t))
            { 
                isCorrected = question.CheckAnswer(t);
            }

            User user = room.Users.FirstOrDefault(e => e.Username == answerEvent.Username);
            
            if (isCorrected)
            {
                question.Status = QuestionStatus.Success;
                user.Points += 1;
                question.UserStatus.Add(user, QuestionStatus.Success);
            }
            else
            {
                question.UserStatus.Add(user, QuestionStatus.Failed);
            }

            return new OnSubmitAnswerModel
            {
                Username = user.Username,
                Result = isCorrected,
                QuestionId = answerEvent.QuestionId
            };
        }

        public bool CheckIsAllPlayerAnswerd(string roomName,int questionId)
        {
            Room room = DataManager.GetRooms().Find(r => r.Name == roomName);
            Question question = room.Questions.FirstOrDefault(e => e.QuestionId == questionId);

            if(question.UserStatus.Count >= room.Users.Count)
            {
                return true;
            }
            return false;
        }

        public async Task OnDisconecntClient(LogoutCommand command)
        {
            Room room = DataManager.GetRooms().Find(r => r.Name == command.RoomName);
            if (command.RoomName is null)
            {
                throw new Exception($"Room {command.RoomName} dosn;t exist ");
            }
            User user = room.Users.Find(u => u.Username == command.Username);
            if (user is null)
            {
                throw new Exception($"User {command.Username} dosn;t exist ");
            }
            room.Users.Remove(user);
            if(room.Status == RoomStatus.full) {
                room.Status = RoomStatus.active;
            }else if(room.Users.Count < 1)
            {
                DataManager.RemoveRoom(room);
            }

            await _userRepository.Logout(command);
         


        }
        public  async Task<StartGame> StartGame(UserCommands command,string action)
        {
                UserEntity user = null;
            
                if (action == Events.REGISTER_USER)
                    user = await _userRepository.RegisterUser(command);
                else
                    user = _userRepository.LoginUser(command);

                Room room = CheckFreeGroup() ?? CreateRoom();
                GameStatus gameSatus = AddUserToRoom(room, new User(user.Username));
                return new StartGame
                {
                    Username = user.Username,
                    RoomName = room.Name,
                    GameStatus = gameSatus.ToString(),
                    BestScore = user.BestScore
                };
            

        }

        public GameStatus AddUserToRoom(Room room, User user)
        {
            room.Users.Add(user);
            if(room.Status == RoomStatus.active){
                if (room.Users.Count >= 5)
                {
                    room.Status = RoomStatus.full;
                }
                return GameStatus.Started;
            }
            else if (room.Users.Count >= 2 && room.Status == RoomStatus.draft)
            {
               room.Status = RoomStatus.active;
                return GameStatus.StartGame;
            }
            return GameStatus.WaitingPlayers;
          
           

        }

        public int GetOnlineUsers(string roomName)
        {
          
            Room room = DataManager.GetRooms().FindLast(e => e.Name == roomName);
            if (room is null)
            {
                throw new Exception("Room " + " dosn't exist");
            }
            return room.Users.Count;
           
        }
    }
}
