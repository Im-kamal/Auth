using System.Text.Json.Serialization;

namespace Auth.Data
{
    public class Question
    {
        public int Id { get; set; }
        public string Text { get; set; }

      // تجاهل الخصائص التي تسبب الدورات المرجعية
        public ICollection<QuestionOption>? Options { get; set; }

        [JsonIgnore] // تجاهل الخصائص التي تسبب الدورات المرجعية
        public ICollection<UserAnswer>? UserAnswers { get; set; }
    }

    public class QuestionOption
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public string Text { get; set; }

        [JsonIgnore]
        public Question? Question { get; set; }
    }
    public class UserAnswer
    {
        public int Id { get; set; }
        public string UserId { get; set; }  
        public int QuestionId { get; set; }
        public int SelectedOptionId { get; set; }
        public  Question? Question { get; set; }
        public  QuestionOption? SelectedOption { get; set; }
    }

}
