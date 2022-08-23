
using System;
using System.Linq;
using WebsiteForum.Models;
using WebsiteForum.DataBase;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace WebsiteForum.Controllers
{
    public class ForumController : Controller
    {
        private DataBaseContext dataBase;
        public static int CurrentQuestionId;
        public ForumController(DataBaseContext context)
        {
            dataBase = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await dataBase.Questions.ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> Index(string Empsearch)
        {
            var question = from x in dataBase.Questions select x;
            if (!String.IsNullOrEmpty(Empsearch))
            {
                question = question.Where(x => x.QuestionText.Contains(Empsearch));
            }
            return View(await question.AsNoTracking().ToListAsync());
        }

        [HttpGet]
        public IActionResult CreateQuestion()
        {
            return View(new Questions());
        }
        public async Task<IActionResult> CreateQuestion(Questions questions)
        {
            User user = AccountController.usercurrent;
            if (user.Id == 0)
            {
                return RedirectToAction("Login", "Account");
            }
            if (user.Id != 0)
            {
                dataBase.Questions.Add(new Questions { Author = user.Id, QuestionText = questions.QuestionText , PostDate = DateTime.Now });
                await dataBase.SaveChangesAsync();
            }
            return RedirectToAction("Index", "Account");
        }
        
        public async Task<IActionResult> ShowQuestion(int id)
        {
            CurrentQuestionId = id;
            Questions questions = await dataBase.Questions.FindAsync(id);
            if (questions == null)
            {
                return NotFound();
            }

            User user = await dataBase.Users.FindAsync(questions.Author);
            List<Answers> answers = new List<Answers>();
            foreach (var item in dataBase.Answers)
            {
                if (item.ForQuestion == id)
                {
                    answers.Add(item);
                }
            }
            var parent = new Tuple<User , Questions , List<Answers>>(user , questions , answers);
            return View(parent);
        }

        public IActionResult ShowAnswersToTheQuestion()
        {
            return View(dataBase.Answers.ToList());
        }

        [HttpPost]
        public async Task<IActionResult> CreateAnswer(string answerText)
        {
            int questionID = CurrentQuestionId;
            User user = AccountController.usercurrent;
            Questions questions = null;
            if (user.Id == 0)
            {
                return RedirectToAction("Login", "Account");
            }

            if (user.Id != 0)
            {
                questions = await dataBase.Questions.FindAsync(questionID);
                if (questions == null)
                {
                    return NotFound();
                }

                dataBase.Answers.Add(new Answers { Author = user.Id, ForQuestion = questions.Id , AnswerText = answerText });
                await dataBase.SaveChangesAsync();
            }
            return RedirectToAction("Index" , "Forum");
        }

        [HttpGet]
        public IActionResult CurrentUserQuestionsList()
        {
            User user = AccountController.usercurrent;
            if (user.Id == 0)
            {
                return RedirectToAction("Login", "Account");
            }

            List<Questions> questions = new List<Questions>();
            foreach (var item in dataBase.Questions)
            {
                if (item.Author == user.Id)
                {
                    questions.Add(item);
                }
            }
            return View(questions);
        }

        [HttpGet]
        public IActionResult Edit()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Edit(int id , string TextQuestion)
        {
            Questions questions = dataBase.Questions.Find(id);
            questions.QuestionText = TextQuestion;
            dataBase.SaveChanges();

            return RedirectToAction("Index" , "Account");
        }
    }
}
