namespace WebsiteForum.Models
{
    public class Answers
    {
        public int Id { get; set; }
        public int Author { get; set; }
        public int ForQuestion { get; set; }
        public string AnswerText { get; set; }
        //public int Rating { get; set; }
    }
}
