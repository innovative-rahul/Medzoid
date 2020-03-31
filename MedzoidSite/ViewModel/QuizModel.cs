using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MedzoidSite.ViewModel
{
    public class QuizModel
    {
        public int id { get; set; }
        public string answer { get; set; }
    }


    public class CorrectAnswers
    {
        public string Question { get; set; }
        public string Answer { get; set; }
        public string AnswerMarked { get; set; }
        public string corrAns { get; set; }
    }
}