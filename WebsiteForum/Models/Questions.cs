using System;
using System.ComponentModel.DataAnnotations;

namespace WebsiteForum.Models
{
    public class Questions
    {
        public int Id { get; set; }
        public int Author { get; set; }
        public string QuestionText { get; set; }
        public DateTime PostDate { get; set; }
    }
}
