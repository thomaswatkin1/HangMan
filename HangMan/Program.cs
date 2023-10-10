using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace HangMan

{
    internal abstract class Program
    {
        // Variable creation
        private const string WordsFilePath = "words.txt"; // Path for words file
        private static string[] _words; // Array to hold words from file
        private static string _secretWord; // The word to be guessed
        private static char[] _status = {'\0'}; // Current state of guessed word
        private static int _lives= 9; // Number of remaining attempts (starts at 9)
        private static bool _gameWon; // Tracks if game has been won
        private static int _wrongs; // Number of wrong guesses
        private static int _difficulty; // Difficulty of word
        private static string _guesses = ""; // Guessed letters
        private static string _wrongGuesses = "";
        private static bool _playAgain = true;

        public static void Main(string[] args)
        {
            Console.Clear();
            LoadWords();
            
            Console.WriteLine("Welcome to Hangman.\nHeads up - you can only guess one letter at a time, not the whole word.\nLet's get started!");

            while (_playAgain)
            {
                // INITIAL SETUP OF GAME
                ClearGame();
                ChooseDifficulty();
                SelectWord();
                InitialiseWordStatus();
                
                Console.WriteLine("Let's begin.");

                // MAIN GAME - CALLS GAME FUNCTIONS
                while (_lives > 0 && !_gameWon)
                {
                    Console.WriteLine(DisplayGallows(_wrongs) + "\n"); // Displays gallows based on number of wrong guesses
                    DisplayStatus();
                    Console.WriteLine("Letters guessed: " + _guesses); // Outputs all guesses
                    char guess = GetUserGuess();
                    CheckGuess(guess);

                }
                
                // DISPLAYS RESULT
                Console.Clear();
                Console.WriteLine(DisplayGallows(_wrongs));
                if (_gameWon)
                {
                    Console.WriteLine("You won! The word was '" + _secretWord + "'. Congratulations.");
                }
                else
                {
                    Console.WriteLine("Unlucky, you ran out of attempts. The word was '" + _secretWord + "'.");
                }
                PlayAgain();
            }
            
            Console.WriteLine("\nOkay, thanks for playing!\nMade by Aaron, Jimmy and Thomas\nSeptember / October 2023");
        }

        private static void LoadWords() // Loads words from file
        {
            _words = File.ReadAllLines(WordsFilePath);
        }

        private static void ClearGame() // Clears anything stored from previous turn
        {
            Array.Clear(_status, 0, _status.Length);
            _wrongGuesses = "";
            _secretWord = "";
            _gameWon = false;
            _guesses = "";
            _wrongs = 0;
            _lives = 9;
        }

        private static void ChooseDifficulty() // Chooses word difficulty
        {
            while (true)
            {
                Console.Write("Please select your difficulty (type a number from 1-3): ");
                string input = Console.ReadLine();

                if (input != null && input.Length == 1 && int.TryParse(input, out _difficulty)) // Validates it's a number
                {
                    if (_difficulty > 0 && _difficulty < 4) // Validates it's between 1 and 3
                    {
                        break;
                    }
                }
                Console.WriteLine("** Invalid input. Please enter a number between 1 and 3. **");
            }
        } 
        
        private static void SelectWord() // Selects word based on difficulty)
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

            if (_difficulty == 1)
            {
                Random random = new Random();
                _secretWord = easy[random.Next(0, easy.Count)];
            }
            else if (_difficulty == 2)
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
                string input = Console.ReadLine()?.ToLower();

                if (input != null && input.Length == 1 && !int.TryParse(input, out _) && char.TryParse(input.ToLower(), out guess)) // Validates guess
                {
                    validInput = true;
                }
                else
                {
                    Console.WriteLine("** Invalid input. Please enter a single letter. **");
                }
            } while (!validInput);
            
            return guess;
        }

        private static void CheckGuess(char guess) // Checks user's guess
        {
            if (_guesses.Contains(guess))
            {
                Console.WriteLine("You've guessed that already. Try again.");
            }
            else if (!_guesses.Contains(guess) && _secretWord.Contains(guess)) // If the guess is in the word...
            {
                UpdateStatus(guess);
                _guesses += guess;
                    
                if (_status.SequenceEqual(_secretWord))
                {
                    _gameWon = true;
                }
            }
            else if (!_wrongGuesses.Contains(guess)) // If the guess is both new and wrong...
            {
                Console.WriteLine("Not in the word.");
                _lives--;
                _guesses += guess;
                _wrongGuesses += guess;
                _wrongs++;
            }
        }

        private static void PlayAgain() // Asks if user wants to play again
        {
            while (true) // Keeps asking until the user inputs a correctly formatted answer
            {
                Console.Write("Would you like to play again? (Type 'y' for yes, 'n' for no): ");
                string again = Convert.ToString(Console.ReadLine());
                
                if (again.ToUpper() == "Y")
                {
                    Console.WriteLine("Okay, playing again!");
                    _playAgain = true;
                    break; // Exits loop and plays again
                }
                if (again.ToUpper() == "N")
                {
                    _playAgain = false;
                    break; // Exits loop and quits program
                }
                Console.WriteLine("Invalid input. Please type 'Y' or 'N'.");
            }

            
            
        }
    }
}