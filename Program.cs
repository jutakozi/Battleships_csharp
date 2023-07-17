using System;

namespace BattleshipsGame
{
    class Program
    {
        static char[,] board = new char[10, 10]; // Tworzenie dwuwymiarowej tablicy do przechowywania stanu planszy
        static int shipsRemaining = 5; // Liczba pozostałych statków do zatopienia
        static bool debugMode = false; // Flaga określająca tryb debugowania (widoczność statków)

        static void Main(string[] args)
        {
            InitializeBoard(); // Inicjalizacja planszy
            PlaceShips(); // Umieszczanie statków na planszy

            while (shipsRemaining > 0) // Pętla gry, działająca dopóki są jeszcze statki do zatopienia
            {
                DisplayBoard(); // Wyświetlanie planszy
                Console.WriteLine("Enter row and column (e.g., A5): ");
                string input = Console.ReadLine(); // Odczytywanie wiersza i kolumny wprowadzonych przez gracza

                if (input.ToLower() == "debug")
                {
                    debugMode = !debugMode; // Przełączanie trybu debugowania (widoczności statków)
                    Console.WriteLine("Debug mode: " + (debugMode ? "Enabled" : "Disabled"));
                    continue; // Kontynuowanie pętli
                }

                if (input.Length != 2)
                {
                    Console.WriteLine("Invalid input. Try again."); // Walidacja wprowadzonych danych
                    continue; // Kontynuowanie pętli
                }

                int row = input[0] - 'A'; // Konwersja wiersza na liczbę  Litera 'A' ma wartość liczbową 65 w kodzie ASCII, więc odejmując 65 od wartości znaku wiersza, otrzymujemy wartość liczbową odpowiadającą indeksowi wiersza na planszy. Dzięki temu A odpowiada indeksowi 0, B odpowiada indeksowi 1, C odpowiada indeksowi 2 itd.
                int col = input[1] - '0'; // Konwersja kolumny na liczbę  Cyfry w kodzie ASCII mają kolejne wartości liczbowe od 48 (0) do 57 (9), więc odejmując 48 od wartości znaku kolumny, otrzymujemy wartość liczbową odpowiadającą indeksowi kolumny na planszy.
                //Dzięki tym konwersjom możemy uzyskać wartości liczbowe
                //reprezentujące wiersz i kolumnę wprowadzone przez użytkownika,
                //które są wykorzystywane do indeksowania tablicy planszy.



                if (row < 0 || row >= 10 || col < 0 || col >= 10)
                {
                    Console.WriteLine("Invalid input. Try again."); // Walidacja wprowadzonych danych
                    continue; // Kontynuowanie pętli
                }

                if (board[row, col] == 'X')
                {
                    Console.WriteLine("You hit a ship!"); // Gracz trafił statek
                    board[row, col] = '!'; // Oznaczenie trafionego pola na planszy
                    shipsRemaining--; // Zmniejszenie liczby pozostałych statków
                }
                else if (board[row, col] == '!')
                {
                    Console.WriteLine("You already hit this spot. Try again."); // Gracz już trafił to pole wcześniej
                }
                else
                {
                    Console.WriteLine("You missed."); // Gracz chybił
                    board[row, col] = '-'; // Oznaczenie chybionego pola na planszy
                }
            }

            Console.WriteLine("Congratulations! You sank all the ships!"); // Komunikat o zakończeniu gry po zatopieniu wszystkich statków
        }

        static void InitializeBoard()
        {
            for (int row = 0; row < 10; row++)
            {
                for (int col = 0; col < 10; col++)
                {
                    board[row, col] = '-'; // Inicjalizacja planszy ustawiająca wszystkie pola na "-"
                }
            }
        }

        static void PlaceShips()
        {
            Random random = new Random();

            for (int i = 0; i < 5; i++)
            {
                int row = random.Next(10); // Losowy wybór wiersza
                int col = random.Next(10); // Losowy wybór kolumny

                if (board[row, col] == '-')
                {
                    board[row, col] = 'X'; // Umieszczanie statku na planszy
                }
                else
                {
                    i--; // Ponowne umieszczenie statku w przypadku zajętego pola
                }
            }
        }

        static void DisplayBoard()
        {
            Console.WriteLine("   0 1 2 3 4 5 6 7 8 9"); // Wyświetlanie nagłówka kolumn
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
                Console.WriteLine();
            }
        }
    }
}