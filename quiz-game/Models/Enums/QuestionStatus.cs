using System.Runtime.Serialization;

namespace quiz_game.Models.Enums
{
    public enum QuestionStatus
    {
        [EnumMember(Value = "NotAnswered")]
        NotAnswered,
        [EnumMember(Value = "Current")]
        Current,
        [EnumMember(Value = "Success")]
        Success,
        [EnumMember(Value = "Failed")]
        Failed
    }
}
