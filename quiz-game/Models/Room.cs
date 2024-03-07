using Microsoft.AspNetCore.Http.HttpResults;
using quiz_game.Models.Enums;
using System.Text;

namespace quiz_game.Models
{
    public class Room
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedOn { get; set; }
        public RoomStatus Status { get; set; }
        public List<User> Users { get; set; }
        public List<Question> Questions { get; set; }

        public Room(string roomName)
        {
            Name = roomName.Trim();
            CreatedOn = DateTime.Now;
            Status = RoomStatus.draft;

            Users = new List<User>();
            Questions = new List<Question>();
        }


       
       
    }
}
