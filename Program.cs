using static System.Console;

namespace Hangman
{
    class Game
    {
        private int remainingGuesses;
        private string? wordToGuess;
        private List<char>? guessedLetters;
        private bool won;
        private bool lost;

        public void Start()
        {
            // Lisez la liste de mots à partir du fichier
            List<string> words = ReadWordsFromFile("words.txt");

            // Choisissez un mot au hasard à partir de la liste
            wordToGuess = ChooseRandomWord(words);

            // Initialisez le nombre de tentatives restantes (par exemple, 8 pour un jeu du pendu standard)
            remainingGuesses = 8;

            // Initialisez la liste des lettres devinées
            guessedLetters = new List<char>();

            // Affichez les instructions au joueur
            WriteLine("Bienvenue dans le jeu du pendu! Vous devez deviner le mot caché en devinant une lettre à la fois.");
            WriteLine("Le mot à deviner contient {0} lettres.", wordToGuess.Length);
            WriteLine("Vous avez {0} tentatives pour deviner le mot.", remainingGuesses);
        }

        private List<string> ReadWordsFromFile(string fileName)
        {
            // Ouvrez le fichier et lisez chaque ligne en une entrée dans la liste
            return File.ReadAllLines(fileName).ToList();
        }

        private string ChooseRandomWord(List<string> words)
        {
            // Générez un nombre aléatoire pour choisir un index dans la liste de mots
            Random random = new Random();
            int index = random.Next(words.Count);

            // Renvoyez le mot à l'index choisi
            return words[index];
        }

        public void TakeGuess(char letter)
        {
             // Ajoutez la lettre à la liste des lettres devinées
            if (guessedLetters == null)
            {
                guessedLetters = new List<char>();
            }
            guessedLetters.Add(letter);

            // Si la lettre est dans le mot à deviner, révélez-la dans le mot masqué affiché au joueur
            if (wordToGuess != null && wordToGuess.Contains(letter))
            {
                WriteLine("La lettre {0} se trouve dans le mot!", letter);
            }
            else
            {
                // Sinon, décrémentez le nombre de tentatives restantes
                remainingGuesses--;
                WriteLine("La lettre {0} ne se trouve pas dans le mot.", letter);
                WriteLine("Il vous reste {0} tentatives.", remainingGuesses);

            }

            // Vérifiez si le joueur a gagné ou perdu
            if (HasWon())
            {
                won = true;
                WriteLine("Félicitations, vous avez deviné le mot {0}!", wordToGuess);
            }
            else if (HasLost())
            {
                lost = true;
                WriteLine("Désolé, vous avez épuisé toutes vos tentatives. Le mot à deviner était {0}.", wordToGuess);
            }
        }
        public void ShowGuessProgress()
            {
                // Si wordToGuess est null, affichez un message d'erreur et sortez de la méthode
                if (wordToGuess == null)
                {
                    WriteLine("Erreur: wordToGuess est null.");
                    return;
                }
                // Affichez chaque lettre du mot à deviner
                foreach (char c in wordToGuess)
                {
                    // Si la lettre a été devinée, affichez-la
                    if (guessedLetters == null || guessedLetters.Contains(c))
                    {
                        Write(c + " ");
                    }
                    // Sinon, affichez un tiret pour masquer la lettre
                    else
                    {
                        Write("_ ");
                    }
                }
                WriteLine();
            }

        public bool IsRunning()
        {
            return !won && !lost;
        }

        public bool HasWon()
        {
            // Si wordToGuess ou guessedLetters est null, renvoyez false
            if (wordToGuess == null || guessedLetters == null)
            {
                return false;
            }
            // Vérifiez si toutes les lettres du mot à deviner ont été devinées
            return !wordToGuess.Any(c => !guessedLetters.Contains(c));
        }

        public bool HasLost()
        {
            // Vérifiez si le nombre de tentatives restantes est égal à 0
            return remainingGuesses == 0;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Game game = new Game();
            game.Start();

            while (game.IsRunning())
            {
                WriteLine("Entrez une lettre:");
                string? input = ReadLine();

                // Si l'entrée est null ou vide, ignorez-la et demandez à nouveau une lettre
                if (string.IsNullOrEmpty(input))
                {
                    continue;
                }

                char letter = input[0];
                game.TakeGuess(letter);
                game.ShowGuessProgress();
            }

            if (game.HasWon())
            {
                WriteLine("Vous avez gagné!");
            }
            else
            {
                WriteLine("Vous avez perdu.");
            }
        }
    }
}