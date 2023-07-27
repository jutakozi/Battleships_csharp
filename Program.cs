using System;

namespace BattleshipsGame
{
    class Program
    {
        static char[,] boardPlayer1 = new char[10, 10]; // Plansza gracza 1
        static char[,] boardPlayer2 = new char[10, 10]; // Plansza gracza 2
        static int shipsRemainingPlayer1 = 5; // Liczba pozostałych statków gracza 1
        static int shipsRemainingPlayer2 = 5; // Liczba pozostałych statków gracza 2
        static bool debugMode = false; // Tryb debugowania (widoczność statków)
        static bool player1Turn = true; // Zmienna wskazująca, czy jest tura gracza 1

        static void Main(string[] args)
        {
            InitializeBoard(boardPlayer1); // Inicjalizacja planszy dla gracza 1
            InitializeBoard(boardPlayer2); // Inicjalizacja planszy dla gracza 2
            PlaceShips(boardPlayer1); // Umieszczenie statków na planszy gracza 1
            PlaceShips(boardPlayer2); // Umieszczenie statków na planszy gracza 2

            // Pętla główna gry, działająca dopóki jednemu z graczy nie zostaną zatopione wszystkie statki
            while (shipsRemainingPlayer1 > 0 && shipsRemainingPlayer2 > 0)
            {
                if (player1Turn)
                {
                    Console.WriteLine("Player 1's Turn");
                    PlayerTurn(boardPlayer2); // Tura gracza 1
                }
                else
                {
                    Console.WriteLine("Player 2's Turn");
                    PlayerTurn(boardPlayer1); // Tura gracza 2
                }

                player1Turn = !player1Turn; // Zmiana tury na przeciwnika po każdej rundzie
            }

            // Wyświetlenie wyniku gry
            if (shipsRemainingPlayer1 == 0)
            {
                Console.WriteLine("Player 2 wins! All ships of Player 1 have been sunk.");
            }
            else
            {
                Console.WriteLine("Player 1 wins! All ships of Player 2 have been sunk.");
            }
        }

        // Inicjalizacja planszy ustawiająca wszystkie pola na "-"
        static void InitializeBoard(char[,] board)
        {
            for (int row = 0; row < 10; row++)
            {
                for (int col = 0; col < 10; col++)
                {
                    board[row, col] = '-';
                }
            }
        }

        // Umieszczenie 5 statków na planszy
        static void PlaceShips(char[,] board)
        {
            Random random = new Random();

            for (int i = 0; i < 5; i++)
            {
                int row = random.Next(10); // Losowy wybór wiersza
                int col = random.Next(10); // Losowy wybór kolumny

                if (board[row, col] == '-')
                {
                    board[row, col] = 'X'; // Umieszczenie statku na planszy
                }
                else
                {
                    i--; // Ponowne umieszczenie statku w przypadku zajętego pola
                }
            }
        }

        // Funkcja obsługująca turę gracza
        static void PlayerTurn(char[,] targetBoard)
        {
            DisplayBoard(targetBoard); // Wyświetlenie planszy

            Console.WriteLine("Enter row and column (e.g., A5): ");
            string input = Console.ReadLine(); // Odczytanie wiersza i kolumny wprowadzonej przez gracza

            // Sprawdzenie czy gracz chce włączyć tryb debugowania
            if (input.ToLower() == "debug")
            {
                debugMode = !debugMode; // Przełączanie trybu debugowania (widoczności statków)
                Console.WriteLine("Debug mode: " + (debugMode ? "Enabled" : "Disabled"));
                PlayerTurn(targetBoard); // Rekurencyjne wywołanie tury dla tego samego gracza
                return; // Zakończenie funkcji w przypadku trybu debugowania
            }

            // Walidacja wprowadzonych danych
            if (input.Length != 2)
            {
                Console.WriteLine("Invalid input. Try again.");
                PlayerTurn(targetBoard); // Rekurencyjne wywołanie tury dla tego samego gracza
                return;
            }

            int row = input[0] - 'A'; // Konwersja wiersza na liczbę
            int col = input[1] - '0'; // Konwersja kolumny na liczbę

            // Sprawdzenie czy wprowadzone dane są w zakresie planszy
            if (row < 0 || row >= 10 || col < 0 || col >= 10)
            {
                Console.WriteLine("Invalid input. Try again.");
                PlayerTurn(targetBoard); // Rekurencyjne wywołanie tury dla tego samego gracza
                return;
            }

            // Obsługa trafienia lub spudłowania
            if (targetBoard[row, col] == 'X')
            {
                Console.WriteLine("You hit a ship!"); // Gracz trafił statek przeciwnika
                targetBoard[row, col] = '!'; // Oznaczenie trafienia na planszy przeciwnika
                if (targetBoard == boardPlayer1)
                    shipsRemainingPlayer1--; // Zmniejszenie liczby pozostałych statków gracza 1
                else
                    shipsRemainingPlayer2--; // Zmniejszenie liczby pozostałych statków gracza 2
            }
            else if (targetBoard[row, col] == '!' || targetBoard[row, col] == '-')
            {
                Console.WriteLine("You missed."); // Gracz spudłował
                targetBoard[row, col] = '#'; // Oznaczenie chybionego strzału na planszy przeciwnika
            }
        }

        // Wyświetlenie planszy
        static void DisplayBoard(char[,] board)
        {
            Console.WriteLine("   0 1 2 3 4 5 6 7 8 9");
            for (int row = 0; row < 10; row++)
            {
                Console.Write((char)('A' + row) + "  "); // Wyświetlanie indeksów wierszy
                for (int col = 0; col < 10; col++)
                {
                    if (debugMode || board[row, col] != 'X')
                    {
                        Console.Write(board[row, col] + " "); // Wyświetlanie stanu planszy (pól)
                    }
                    else
                    {
                        Console.Write("- "); // Wyświetlanie nieodkrytych pól statków w trybie debugowania
                    }
                }
                Console.WriteLine(); // Nowa linia po każdym wierszu
            }
        }
    }
}
