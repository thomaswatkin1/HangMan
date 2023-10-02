using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace HangMan

{
    abstract class Program
    {
        private const string WordsFilePath = "words.txt"; // Path for words file
        private static string[] _words; // Array to store words from file
        private static string _secretWord; // The word to be guessed
        private static char[] _status; // Current state of guessed word
        private static int _lives= 9; // Number of remaining attempts (starts at 9)
        private static bool _gameWon; // Tracks if game has been won
        private static int _wrongs; // Number of wrong guesses
        private static int difficulty; // Difficulty of word
        private static string _guesses = ""; // Guessed letters
        private static string _wrongGuesses = ""; // Guessed letters that are wrong

        public static void Main(string[] args)
        {
            Console.Clear();
            
            LoadWords();

            Console.WriteLine("Welcome to Hangman.\nHeads up - you can only guess one letter at a time, not the whole word.\nLet's get started!");

            Console.Write("How long do you want your word to be? (1-3): "); // Chooses difficulty
            int difficulty = Convert.ToInt32(Console.ReadLine());
            SelectWord(difficulty); // Selects word based on difficulty
            
            InitialiseWordStatus(); // Creates empty dashes
            Console.WriteLine("Let's begin.");
            
            while (_lives > 0 && !_gameWon) // MAIN GAME - calls game functions
            {
                Console.WriteLine(DisplayGallows(_wrongs) + "\n"); // Displays gallows based on number of wrong guesses
                DisplayStatus(); // Displays current word 
                Console.WriteLine("Letters guessed: " + _guesses); // Outputs all guesses
                char guess = GetUserGuess(); // Takes guess
                CheckGuess(guess); // Checks guess

            }

            // Displays Final result

            Console.WriteLine(DisplayGallows(_wrongs));
            
            if (_gameWon)
            {
                Console.WriteLine("You won! The word was '" + _secretWord + "'. Congratulations.");
            }
            else
            {
                Console.WriteLine("You ran out of attempts. The word was '" + _secretWord + "'.");
            }
            
            Console.WriteLine("Thanks for playing!\nMade by Aaron, Jimmy and Thomas\nSeptember 2023");

        }

        private static void LoadWords() // Loads words from file
        {
            _words = File.ReadAllLines(WordsFilePath);
        }

        private static void SelectWord(int difficulty) // Selects word based on difficulty)
        {
            List<string> easy = new List<string>();
            List<string> medium = new List<string>();
            List<string> hard = new List<string>();

            foreach (string str in _words)
            {
                if (str.Length >= 1 && str.Length <= 4)
                {
                    easy.Add(str);
                }
                else if (str.Length >= 5 && str.Length <= 7)
                {
                    medium.Add(str);
                }
                else
                {
                    hard.Add(str);
                }
            }

            if (difficulty == 1)
            {
                Random random = new Random();
                _secretWord = easy[random.Next(0, easy.Count)];
            }
            else if (difficulty == 2)
            {
                Random random = new Random();
                _secretWord = medium[random.Next(0, medium.Count)];
            }
            else
            {
                Random random = new Random();
                _secretWord = hard[random.Next(0, hard.Count)];
            }

        }

        private static void InitialiseWordStatus() // Makes the status of the word, i.e. writes out '___' 
        {
            _status = new char[_secretWord.Length];
            for (int i = 0; i < _secretWord.Length; i++)
            {
                _status[i] = '_';
            }
        }

        private static string DisplayGallows(int wrongs) // Displays gallows
        {
            switch (wrongs)
            {
                case 0:
                    return "";
                case 1:
                    return "\n\n\n\n\n\n\n_____";
                case 2:
                    return "\n|\n|\n|\n|\n|\n|\n_____";
                case 3:
                    return "_______\n|\n|\n|\n|\n|\n|\n|_____";
                case 4:
                    return "_______\n|/\n|\n|\n|\n|\n|\n|_____";
                case 5:
                    return "_______\n|/   |\n|\n|\n|\n|\n|\n|_____";
                case 6:
                    return "_______\n|/   |\n|    O\n|\n|\n|\n|\n|_____";
                case 7:
                    return "_______\n|/   |\n|    O\n|    |\r\n|    |\n|\n|\n|_____";
                case 8:
                    return "_______\n|/   |\n|    O\n|  \\ | /\n|    |\n|\n|\n|_____";
                case 9:
                    return "_______\n|/   |\n|    O\n|  \\ | /\n|    |\n|   / \\\n|\n|_____";
                default:
                    return "";

            }
        }

        private static void UpdateStatus(char guess) // Updates word status
        {
            for (int i = 0; i < _secretWord.Length; i++)
            {
                if (_secretWord[i] == guess)
                {
                    _status[i] = guess;
                }
            }
        }

        private static void DisplayStatus() // Displays word status
        {
            Console.WriteLine("Word to guess: " + new string(_status));
            Console.WriteLine("Lives left: " + _lives);
        }

        private static char GetUserGuess() // Gets guess from user
        {
            char guess = '\0';
            bool validInput = false;

            do
            {
                Console.Write("Guess a letter: ");
                string input = Console.ReadLine();

                if (input != null && input.Length == 1 && char.TryParse(input.ToLower(), out guess))
                {
                    validInput = true;
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a single letter.");
                }
            } while (!validInput);

            return guess;
        }

        private static void CheckGuess(char guess) // Checks user's guess
        {
            if (_secretWord.Contains(guess)) // Checks guess against word
            {
                UpdateStatus(guess); // Adds guess to word status if correct
                _guesses += guess; // Adds guess to list of total guesses
                    
                if (_status.SequenceEqual(_secretWord)) // Game won if whole word is right
                {
                    _gameWon = true;
                }
            }
            else
            {
                if (!_wrongGuesses.Contains(guess)) // If the guess is both new and wrong...
                {
                    Console.WriteLine("Not in the word.");
                    _lives--; // Decreases lives by 1
                    _guesses += guess;
                    _wrongGuesses += guess;
                    _wrongs++;
                }

                else
                {
                    Console.WriteLine("You've guessed that already. Try again.");
                }
            }
        }
    }
}