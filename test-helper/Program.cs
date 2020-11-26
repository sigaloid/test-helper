using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using static test_helper.TestClasses;

namespace test_helper
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Enter test title.");
            var testtitle = Console.ReadLine();
            Console.WriteLine("How many questions?");
            var questionList = new List<Question>();
            foreach (var i in Enumerable.Range(0, int.Parse(Console.ReadLine()))) questionList.Add(getQuestion(i));
            File.WriteAllText(testtitle + ".txt", JsonConvert.SerializeObject(CreateTest(questionList, true, false, false, testtitle)));
            Console.WriteLine("Enter number of tests you would like to generate. (default is 30)");
            string testnumstring = Console.ReadLine();
            int testnum;
            testnum = testnumstring != string.Empty ? Int32.Parse(testnumstring) : 30;
            foreach (var i in Enumerable.Range(1, testnum))
            {
                var randomizedTest = CreateTest(questionList, true, true, true, testtitle);
                Directory.CreateDirectory(testtitle);
                File.WriteAllText( $"{testtitle}/{i}.test.txt", printTest(randomizedTest, false, true));
                File.WriteAllText($"{testtitle}/{i}.key.txt", printTest(randomizedTest, true, false));
            }
        }

        public static Question getQuestion(int i)
        {
            var question = new Question();
            Console.WriteLine($"Enter question #{i + 1}'s question text");
            question.question = Console.ReadLine();
            Console.WriteLine($"Question {i + 1}: Enter number of answers (press enter to choose default of 4)");
            var answercountstring = Console.ReadLine();
            var answercount = answercountstring == string.Empty ? 4 : int.Parse(answercountstring);
            Console.WriteLine("Enter correct answer");
            var correctAnswer = new Answer {isCorrectAnswer = true, text = Console.ReadLine()};
            Console.WriteLine("Begin entering other answers:");
            var answerList = new List<Answer> {correctAnswer};
            foreach (var i2 in Enumerable.Range(1, answercount - 1))
            {
                Console.WriteLine($"Enter incorrect answer #{i2} of {answercount - 1}");
                answerList.Add(new Answer {isCorrectAnswer = false, text = Console.ReadLine()});
            }

            question.answers = answerList;
            return question;
        }
    }
}