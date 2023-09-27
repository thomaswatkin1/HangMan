using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace HangMan

{
    class Program
    {
        private static string WordsFilePath = "words.txt"; // Path for words file
        private static string[] words; // Array to store words from file
        private static string secretWord; // The word to be guessed
        private static char[] status; // Current state of guessed word
        private static int remainingAttempts = 9; // Number of remaining attempts
        private static bool gameWon; // Tracks if game has been won
        private static int wrongs = 0; // Number of wrong guesses
        private static string difficulty; // Difficulty of word
        private static string guesses = ""; // Guessed letters
        private static string wrongGuesses = ""; // Guessed letters that are wrong

        public static void Main(string[] args)
        {
            LoadWords();

            Console.WriteLine("Welcome to Hangman.");
            
            Console.Write("Please select your difficulty (1-3): "); // Chooses difficulty
            int difficulty = Convert.ToInt32(Console.ReadLine());
            SelectWord(difficulty);
            InitialiseWordStatus(); // Creates empty dashes

            while (remainingAttempts > 0 && !gameWon) // MAIN GAME - calls game functions
            {
                Console.WriteLine(DisplayGallows(wrongs)+"\n"); // Displays gallows
                DisplayStatus(); // Displays current word 
                Console.WriteLine("Letters guessed: "+guesses);
                char guess = GetUserGuess(); // Takes guess
                
                if (secretWord.Contains(guess)) // Checks guess
                {
                    UpdateStatus(guess);
                    
                    if (status.SequenceEqual(secretWord)) // Game won if whole word is right
                    {
                        gameWon = true;
                    }
                }
                else
                {
                    if (!wrongGuesses.Contains(guess))
                    {
                        Console.WriteLine("Not in the word.");
                        remainingAttempts--;
                        guesses += guess;
                        wrongGuesses += guess;
                        wrongs++;
                    }

                    else
                    {
                        Console.WriteLine("You've guessed that already. Try again.");
                    }
                }

            }

            // Displays Final result

            Console.WriteLine(DisplayGallows(wrongs));
            if (gameWon)
            {
                Console.WriteLine("You won! The word was '"+ secretWord +"'. Congratulations.");
            }
            else
            {
                Console.WriteLine("You ran out of attempts. The word was '" + secretWord + "'.");
            }

        }

        private static void LoadWords() // Loads words from file
        {
            words = File.ReadAllLines(WordsFilePath);
        }

        private static void SelectWord(int difficulty) // Selects random word (NEEDS UPDATING)
        {
            List<string> easy = new List<string>();
            List<string> medium = new List<string>();
            List<string> hard = new List<string>();

            foreach (string str in words)
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
                secretWord = easy[random.Next(0, easy.Count)];
            }
            else if (difficulty == 2)
            {
                Random random = new Random();
                secretWord = medium[random.Next(0, medium.Count)];
            }
            else
            {
                Random random = new Random();
                secretWord = hard[random.Next(0, hard.Count)];
            }

        }

        private static void InitialiseWordStatus() // Makes the status of the word, i.e. '___' 
        {
            status = new char[secretWord.Length];
            for (int i = 0; i < secretWord.Length; i++)
            {
                status[i] = '_';
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
            for (int i = 0; i < secretWord.Length; i++)
            {
                if (secretWord[i] == guess)
                {
                    status[i] = guess;
                }
            }
        }

        private static void DisplayStatus() // Displays word status
        {
            Console.WriteLine("Word to guess: " + new string(status));
            Console.WriteLine("Remaining Attempts: " + remainingAttempts);
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
    }
}