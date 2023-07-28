using System;
class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Witaj w grze w statki!");

        // Pytamy użytkownika, czy chce włączyć tryb debug.
        bool isDebugMode = AskForDebugMode();

        // Tworzymy nową instancję klasy Game, która będzie zarządzać grą.
        Game game = new Game(isDebugMode);

        // Inicjalizujemy położenie statków na planszach.
        game.InitializeShips();

        // Inicjalizujemy plansze gracza i przeciwnika (początkowo są puste).
        game.InitializeBoard();

        // Główna pętla gry, która działa dopóki gra nie jest zakończona.
        while (!game.IsGameOver())
        {
            // Czyścimy konsolę przed wyświetleniem planszy.
            Console.Clear();

            // Wyświetlamy plansze gracza i przeciwnika.
            game.DisplayBoards();

            // Gracz wykonuje ruch.
            game.PlayerMove();

            // Jeśli gra nadal nie jest zakończona, przeciwnik wykonuje ruch.
            if (!game.IsGameOver())
            {
                game.EnemyMove();
            }
        }

        // Gra się zakończyła, wyświetlamy końcowy wynik.
        Console.Clear();
        game.DisplayBoards();
        game.DisplayResult();

        // Czekamy na dowolny klawisz przed zakończeniem programu.
        Console.ReadLine();
    }

    // Metoda pyta użytkownika, czy chce włączyć tryb debug i zwraca odpowiednią wartość boolowską.
    static bool AskForDebugMode()
    {
        Console.Write("Czy chcesz włączyć tryb debug? (tak/nie): ");
        string input = Console.ReadLine();
        return input.ToLower() == "tak";
    }
}

class Game
{
    // Stała rozmiaru planszy.
    private const int BoardSize = 10;

    // Dwuwymiarowe tablice reprezentujące plansze gracza i przeciwnika.
    private char[,] playerBoard = new char[BoardSize, BoardSize];
    private char[,] enemyBoard = new char[BoardSize, BoardSize];

    // Obiekty statków gracza i przeciwnika.
    private Ship playerShip;
    private Ship enemyShip;

    // Obiekt Random do generowania losowych pozycji statków.
    private Random random = new Random();

    // Zmienna przechowująca informację o trybie debug.
    private bool isDebugMode;

    public Game(bool isDebugMode)
    {
        this.isDebugMode = isDebugMode;
    }

    // Metoda inicjalizuje położenie statków na planszach.
    public void InitializeShips()
    {
        playerShip = new Ship();
        enemyShip = new Ship();

        // Losowo umiejscawiamy statek gracza na planszy (uproszczone).
        int playerX = random.Next(0, BoardSize - Ship.Size);
        int playerY = random.Next(0, BoardSize);
        playerShip.Place(playerX, playerY);

        int enemyX, enemyY;
        do
        {
            enemyX = random.Next(0, BoardSize - Ship.Size);
            enemyY = random.Next(0, BoardSize);
        } while (AreShipsTooClose(playerX, playerY, enemyX, enemyY));

        enemyShip.Place(enemyX, enemyY);
    }

    private bool AreShipsTooClose(int playerX, int playerY, int enemyX, int enemyY)
    {
        // Sprawdzamy, czy statki są zbyt blisko na osi X lub Y.
        bool tooCloseOnX = Math.Abs(playerX - enemyX) <= 2;
        bool tooCloseOnY = Math.Abs(playerY - enemyY) <= 2;

        // Jeśli choć jedna z tych wartości jest prawdziwa, to oznacza, że statki są zbyt blisko.
        return tooCloseOnX || tooCloseOnY;
    }

    // Metoda inicjalizuje plansze gracza i przeciwnika.
    public void InitializeBoard()
    {
        for (int x = 0; x < BoardSize; x++)
        {
            for (int y = 0; y < BoardSize; y++)
            {
                playerBoard[x, y] = '~'; // '~' oznacza wodę
                enemyBoard[x, y] = '~';
            }
        }

        if (isDebugMode)
        {
            // W trybie debug pokaż położenie statków na planszy.
            DisplayDebugBoard(playerShip, playerBoard);
            DisplayDebugBoard(enemyShip, enemyBoard);
        }
    }

    // Metoda dodaje na planszy reprezentację statku w trybie debug.
    private void DisplayDebugBoard(Ship ship, char[,] board)
    {
        for (int i = 0; i < Ship.Size; i++)
        {
            int x = ship.X + i;
            int y = ship.Y;
            board[x, y] = 'S'; // 'S' oznacza statek
        }

        for (int x = 0; x < BoardSize; x++)
        {
            for (int y = 0; y < BoardSize; y++)
            {
                if (board[x, y] == 'O') // Jeśli to pole to było oznaczone jako 'O'
                {
                    board[x, y] = '~'; // Przywróć oznaczenie dla wody
                }
            }
        }
    }

    // Metoda wyświetla plansze gracza i przeciwnika.
    public void DisplayBoards()
    {
        Console.WriteLine("Twoja plansza:");
        DisplayBoard(playerBoard);

        Console.WriteLine("Plansza przeciwnika:");
        DisplayBoard(enemyBoard);
    }

    // Metoda wyświetla planszę.
    private void DisplayBoard(char[,] board)
    {
        Console.Write("   ");
        for (int i = 0; i < BoardSize; i++)
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Write($"{i} ");
            Console.ResetColor();
        }
        Console.WriteLine();

        for (int x = 0; x < BoardSize; x++)
        {
            Console.Write($"{x} |");
            for (int y = 0; y < BoardSize; y++)
            {
                char cell = board[x, y];
                if (isDebugMode && cell == '~')
                {
                    Console.BackgroundColor = ConsoleColor.Blue;
                    Console.Write("- ");
                }
                else if (cell == 'S')
                {
                    Console.BackgroundColor = ConsoleColor.Yellow;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.Write($"{cell} ");
                }
                else if (cell == 'X')
                {
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.Write($"{cell} ");
                }
                else if (cell == 'O')
                {
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.Write($"{cell} ");
                }
                else
                {
                    Console.BackgroundColor = ConsoleColor.Cyan;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.Write($"{cell} ");
                }
                Console.ResetColor();
            }
            Console.WriteLine();
        }

        // Dodajemy dodatkową linię na dole planszy, aby była symetryczna.
        Console.Write("   ");
        for (int i = 0; i < BoardSize; i++)
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Write("--");
            Console.ResetColor();
        }
        Console.WriteLine();
    }

    // Metoda odpowiada za ruch gracza.
    public void PlayerMove()
    {
        Console.WriteLine("\nTwój ruch!");

        int x, y;

        // Pobieramy współrzędne X od gracza, dopóki nie poda poprawnej wartości.
        do
        {
            Console.Write("Podaj X: ");
        } while (!int.TryParse(Console.ReadLine(), out x) || x < 0 || x >= BoardSize);

        // Pobieramy współrzędne Y od gracza, dopóki nie poda poprawnej wartości.
        do
        {
            Console.Write("Podaj Y: ");
        } while (!int.TryParse(Console.ReadLine(), out y) || y < 0 || y >= BoardSize);

        // Sprawdzamy, czy strzał gracza trafił statek przeciwnika.
        if (enemyShip.IsHit(x, y))
        {
            // Jeśli strzał trafił, oznaczamy trafienie na planszy przeciwnika znakiem 'X'.
            enemyBoard[x, y] = 'X';
            Console.WriteLine("Trafiony!");
        }
        else
        {
            // Jeśli strzał nie trafił, oznaczamy nietrafienie na planszy przeciwnika znakiem 'O'.
            enemyBoard[x, y] = 'O';
            Console.WriteLine("Pudło!");
        }

        // Oczekujemy na dowolny klawisz przed kontynuacją gry.
        Console.WriteLine("Naciśnij dowolny klawisz, aby kontynuować...");
        Console.ReadKey();
    }

    // Metoda odpowiada za ruch przeciwnika.
    // Metoda odpowiada za ruch przeciwnika.
    public void EnemyMove()
    {
        Console.WriteLine("\nRuch przeciwnika...");

        int x, y;

        // Wybieramy współrzędne X i Y przeciwnika losowo (bez unikania pól).
        x = random.Next(0, BoardSize);
        y = random.Next(0, BoardSize);

        if (playerShip.IsHit(x, y))
        {
            // Jeśli strzał przeciwnika trafił statek gracza, oznaczamy trafienie na planszy gracza znakiem 'X'.
            playerBoard[x, y] = 'X';
            Console.WriteLine("Twój statek został trafiony!");
        }
        else
        {
            // Jeśli strzał przeciwnika nie trafił, oznaczamy nietrafienie na planszy gracza znakiem 'O'.
            playerBoard[x, y] = 'O';
            Console.WriteLine("Przeciwnik spudłował!");
        }

        Console.WriteLine("Naciśnij dowolny klawisz, aby kontynuować...");
        Console.ReadKey();
    }

    // Metoda sprawdza, czy gra się zakończyła.
    public bool IsGameOver()
    {
        return playerShip.IsSunk() || enemyShip.IsSunk();
    }

    // Metoda wyświetla wynik gry.
    public void DisplayResult()
    {
        if (playerShip.IsSunk())
        {
            Console.WriteLine("Przegrałeś!");
        }
        else
        {
            Console.WriteLine("Gratulacje! Wygrałeś!");
        }
    }
}

class Ship
{
    // Stała określająca rozmiar statku.
    public const int Size = 3;

    // Współrzędne statku na planszy.
    public int X { get; set; }
    public int Y { get; set; }

    // Tablica przechowująca informacje o trafieniach w statek.
    private bool[] hits;

    // Metoda ustawia położenie statku na planszy.
    public void Place(int x, int y)
    {
        X = x;
        Y = y;
        hits = new bool[Size];
    }

    // Metoda sprawdza, czy dane współrzędne (x, y) reprezentują trafienie w statek.
    // Parametry:
    //   x: Współrzędna X strzału na planszy.
    //   y: Współrzędna Y strzału na planszy.
    // Zwraca:
    //   true, jeśli strzał trafił w statek (w niezatopioną część statku).
    //   false, jeśli strzał nie trafił w statek lub jeśli ta część statku już została trafiona.
    public bool IsHit(int x, int y)
    {
        // Przechodzimy przez każdą część statku przy użyciu pętli "for".
        for (int i = 0; i < Size; i++)
        {
            // Sprawdzamy, czy dana część statku nie została jeszcze trafiona (!hits[i]).
            // Oraz czy współrzędne strzału (x, y) odpowiadają pozycji danej części statku.
            // Jeśli warunek jest spełniony, oznacza to, że strzał trafił w niezatopioną część statku.
            if (!hits[i] && X + i == x && Y == y)
            {
                // Ustawiamy wartość hits[i] na true, co oznacza, że dana część statku została trafiona.
                hits[i] = true;

                // Zwracamy true, wskazując, że strzał trafił w statek.
                return true;
            }
        }

        // Jeśli pętla nie znalazła żadnej części statku, która była trafiona,
        // zwracamy false, oznaczając, że strzał nie trafił w żadną część statku.
        return false;
    }

    // Metoda sprawdza, czy statek został zatopiony, czyli czy wszystkie jego części zostały trafione.
    // Zwraca:
    //   true, jeśli wszystkie części statku zostały trafione (statek jest zatopiony).
    //   false, jeśli przynajmniej jedna część statku nie została trafiona (statek nie jest zatopiony).
    public bool IsSunk()
    {
        // Przechodzimy przez każdą część statku przy użyciu pętli "foreach".
        foreach (var hit in hits)
        {
            // Sprawdzamy, czy dana część statku została trafiona.
            // Warunek "!hit" oznacza "jeśli hit jest false", czyli sprawdzamy, czy hit jest równy false.
            // Jeśli hit jest false, to oznacza, że dana część statku nie została trafiona.
            if (!hit)
            {
                // Jeśli znaleźliśmy część statku, która nie została trafiona, to od razu zwracamy false,
                // co oznacza, że statek nie jest zatopiony.
                return false;
            }
        }

        // Jeśli pętla nie znalazła żadnej części statku, która nie została trafiona,
        // to oznacza, że wszystkie części statku są trafione (wszystkie elementy w tablicy hits są true).
        // W takim przypadku, zwracamy true, co oznacza, że statek jest zatopiony.
        return true;
    }

}