using System;
using System.IO;

namespace HangMan

{
    class Program
    {
        public static string WordsFilePath = "words.txt"; // Path for words file
        public static string[] words; // Array to store words from file
        public static string word; // The word to be guessed
        public static char[] status; // Current state of guessed word
        public static int remainingAttempts = 6; // Number of remaining attempts
        public static bool gameWon = false; // Tracks if game has been won
        
        public static void Main(string[] args)
        {
            LoadWords();
            SelectWord();
            InitialiseWordStatus();
            
            Console.WriteLine("Welcome to Hangman.");

            while (remainingAttempts > 0 && !gameWon) // Main game - calls game functions
            {
                DisplayGallows();
                DisplayStatus();
                CheckGuess();
  
            }
            
            // Displays Final result
            
            Console.Clear();
            DisplayGallows();
            if (gameWon)
            {
                Console.WriteLine("You won! Congratulations.");
            }
            else
            {
                Console.WriteLine("You ran out of attempts. The word was '"+word+"'.");
            }
            
        }

        public static void LoadWords() // Loads words from file
        {
            words = File.ReadAllLines(WordsFilePath);
        }

        
        public static void SelectWord() // Selects random word
        {
            // (Word selector function goes here)
        }

        static void InitialiseWordStatus() // Makes the status of the word, i.e. '___' 
        {
            // (Initialise word status function goes here)
        }

        static void DisplayGallows() // Displays gallows
        {
            // (Gallows function goes here)
        }

        static void UpdateGuessedWord(char guess) // Updates word status
        {
            // (Word status update function goes here)
        }

        static void DisplayStatus() // Displays word status
        {
            // (Word status display function goes here)
        }

        static void GetUserGuess() // Gets guess from user
        {
            // (Get user guess function goes here)
        }
        static void CheckGuess() // Checks guess
        {
            // (Guess check function goes here)
        }
    }
}