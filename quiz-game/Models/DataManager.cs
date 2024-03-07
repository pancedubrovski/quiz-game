using quiz_game.Models.Enums;

namespace quiz_game.Models
{
    public static class DataManager 
    {

        private static List<Room> Rooms { get; set; } = new List<Room> { };
        public static List<Room> GetRooms()
        {
            return Rooms;
        }

        public static Question AddQuestion(string roomName)
        {

            Room room =  Rooms.Find(r => r.Name == roomName);
            if (room == null)
            {
                throw new Exception($"Room {roomName} dosen't exist");
            }
            Random random = new Random();
            var isBonusRoom = random.Next(0, 100) < 30 ? true : false;

            Question question = null;
            if (isBonusRoom)
                question = new BonusQuestion(room.Questions.Count() +1);
            else
                question = new Question(room.Questions.Count() + 1,QuestionKind.Question);
            room.Questions.Add(question);
            return question;
        }
    }
}
