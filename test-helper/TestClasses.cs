using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace test_helper
{
    public class TestClasses
    {
        private static readonly Random rng = new Random();

        public static Question CreateBasic4PartQuestion(string questionString, string correctanswer, string answer2,
            string answer3, string answer4)
        {
            var list = new List<Answer>();
            list.AddRange(new List<Answer>
            {
                new Answer {isCorrectAnswer = true, text = correctanswer},
                new Answer {isCorrectAnswer = false, text = answer2},
                new Answer {isCorrectAnswer = false, text = answer3},
                new Answer {isCorrectAnswer = false, text = answer4}
            });
            var question = new Question {answers = list, question = questionString};
            return question;
        }

        public static Test CreateTest(List<Question> questionList, bool createKey, bool randomizeAnswers,
            bool randomizeQuestions, string testtitle)
        {
            var testObject = new Test {Key = null, Questions = questionList, Title = testtitle};
            if (randomizeAnswers)
                foreach (var question in questionList)
                    try
                    {
                        question.answers = question.answers.Select(x => new {value = x, order = rng.Next()})
                                .OrderBy(x => x.order).Select(x => x.value).ToList();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        throw;
                    }

            if (randomizeQuestions)
                testObject.Questions = questionList.Select(x => new {value = x, order = rng.Next()})
                    .OrderBy(x => x.order).Select(x => x.value).ToList();
            else
                testObject.Questions = questionList;

            testObject.Key = MakeKey(testObject.Questions);
            if (createKey) testObject.Key = MakeKey(testObject.Questions);
            return testObject;
        }

        public static string MakeKey(List<Question> questionList)
        {
            var key = "";
            foreach (var question in questionList)
            foreach (var answer in question.answers.Where(a => a.isCorrectAnswer))
                key += $"{questionList.IndexOf(question) + 1}:{ColumnNameByIndex(question.answers.IndexOf(answer))}|";
            return key;
        }

        public static string MakeKey(Test Test)
        {
            var key = "";
            foreach (var question in Test.Questions)
            foreach (var answer in question.answers.Where(a => a.isCorrectAnswer))
                key += $"{Test.Questions.IndexOf(question) + 1}:{ColumnNameByIndex(question.answers.IndexOf(answer))}|";
            return key;
        }

        public static string printTest(Test test, bool printKey, bool printQuestions)
        {
            var print = "";
            if (printQuestions)
            {
                print += test.Title + Environment.NewLine;
                var i = 1;
                foreach (var q in test.Questions)
                {
                    print += $"{i}. {q.question}" + Environment.NewLine;
                    foreach (var i2 in Enumerable.Range(0, q.answers.Count))
                        print += $"    {ColumnNameByIndex(i2)}. {q.answers[i2].text}" + Environment.NewLine;
                    i++;
                }
            }

            if (printKey) print += test.Key;
            return print;
        }

        public static string ColumnNameByIndex(int ColumnIndex)
        {
            var mColumnLetters = "zabcdefghijklmnopqrstuvwxyz";
            int ModOf26, Subtract;
            var NumberInLetters = new StringBuilder();
            ColumnIndex += 1;
            while (ColumnIndex > 0)
                if (ColumnIndex <= 26)
                {
                    ModOf26 = ColumnIndex;
                    NumberInLetters.Insert(0, mColumnLetters.Substring(ModOf26, 1));
                    ColumnIndex = 0;
                }
                else
                {
                    ModOf26 = ColumnIndex % 26;
                    Subtract = ModOf26 == 0 ? 26 : ModOf26;
                    ColumnIndex = (ColumnIndex - Subtract) / 26;
                    NumberInLetters.Insert(0, mColumnLetters.Substring(ModOf26, 1));
                }

            return NumberInLetters.ToString().ToUpper();
        }

        public class Test
        {
            public string Title { get; set; }
            public List<Question> Questions { get; set; }
            public string? Key { get; set; }
        }

        public class Question
        {
            public string question { get; set; }
            public List<Answer> answers { get; set; }
        }

        public class Answer
        {
            public string text { get; set; }
            public bool isCorrectAnswer { get; set; }
        }
    }
}
