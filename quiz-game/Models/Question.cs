using quiz_game.Models.Enums;
using System.Data;
using System.Linq.Expressions;
using System.Text;

namespace quiz_game.Models
{
    public class Question
    {
        public int QuestionId { get; set; }
        public string Expression { get; set; }
        public QuestionStatus Status { get; set; }
        public bool IsCorrected { get; set; }
        public QuestionKind Kind { get; set; }
        public Dictionary<User,QuestionStatus> UserStatus { get; set; }


        public Question(int numberOfQuestion, QuestionKind questionKind)
        {
            QuestionId = numberOfQuestion;
            Expression = GenerateExpression();
            Status = QuestionStatus.Current;
            UserStatus = new Dictionary<User, QuestionStatus>();
            Kind = questionKind;

        }
       



        public virtual string GenerateExpression()
        {

            string[] opereands = ["+", "-", "*", "/"];

            StringBuilder expresion = new StringBuilder();
            var random = new Random();
            int firstOperand = random.Next(-10, 10);
            expresion.Append(firstOperand);

            string operand = opereands[random.Next(0, 3)];
            expresion.Append(operand);

            int secondOperand = random.Next(-10, 10);
            expresion.Append(secondOperand);

            IsCorrected = random.Next(0, 1) == 1 ? true: false;
            double res = Convert.ToDouble(new DataTable().Compute(expresion.ToString(), null));
            expresion.Append("=" + (IsCorrected ? res : res+random.Next(1,5)));

            return expresion.ToString();
        }

        public virtual bool CheckAnswer(bool answer)
        {
            return IsCorrected == answer;
        }

    }
}
