using System.Collections.Generic;

namespace WebsiteForum.Models
{
    public class MainPageModel
    {
        public User User { get; set; }
        public Questions Questions { get; set; }
        public List<Answers> Answers { get; set; }
    }
}
