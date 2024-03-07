using quiz_game.Models.Enums;
using System.Data;
using System.Text;

namespace quiz_game.Models
{
    public class BonusQuestion : Question
    {
        public double CorrectAnswer { get; set; }
        public BonusQuestion(int numberOfQuestion) : base(numberOfQuestion,QuestionKind.BonusQuestion)
        {
        }

        public override string GenerateExpression()
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

            double res = Convert.ToDouble(new DataTable().Compute(expresion.ToString(), null));
            CorrectAnswer = res;
            expresion.Append("=?");

            return expresion.ToString();
        }
        public bool CheckAnswer(double answer)
        {
            return CorrectAnswer == answer;
        }
    }
}
