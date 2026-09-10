
//Animations Movement in Terminal
//pt in test phase
// Current task, I want to make a working gridline that moves with the arrowkeys and displays in the terminal, that means a grid animation display
// that toggles every line / character as a pixel.

// Working framework for displaying grid animations in terminal. (very goofy and clanky rn)
using System;
using System.Text;
using System.Threading;

class Program
{
    // Locked screen width (can be changed in the future with a settings menu and variables)
    const int ScreenWidth = 40;
    const int ScreenHeight = 20;

    static bool isRunning = true;
    static int score = 0;

    // Player position
    static int playerX = 10;
    static int playerY = 10;

    // Målets position
    static int itemX = 25;
    static int itemY = 5;
    static Random random = new Random();

    static void Main()
    {
        // 1. Forbered terminalvinduet
        Console.CursorVisible = false;
        Console.OutputEncoding = Encoding.UTF8; // Sikrer support for specialtegn
        
        // Tvinger vinduet og bufferen til at have NØJAGTIG samme størrelse
        // Det forhindrer scroll-bars og mindsker chancerne for stakning
        try 
        {
            Console.Clear();
            #pragma warning disable CA1416 // Kun relevant på Windows
            Console.SetWindowSize(ScreenWidth + 2, ScreenHeight + 5);
            Console.SetBufferSize(ScreenWidth + 2, ScreenHeight + 5);
            #pragma warning restore CA1416
        } 
        catch 
        { 
            // Hvis terminalen ikke tillader resizing via kode (f.eks. på visse Mac/Linux terminaler),
            // rydder vi bare skærmen fuldstændigt før start.
            Console.Clear(); 
        }

        // 2. Game Loop
        while (isRunning)
        {
            Input();
            Update();
            Render();

            Thread.Sleep(33); // Ca. 30 billeder i sekundet (FPS)
        }

        // Ryd op efter spillet er slut
        Console.Clear();
        Console.CursorVisible = true;
        Console.WriteLine($"Game Over! Din endelige score: {score}");
    }

    static void Input()
    {
        if (Console.KeyAvailable)
        {
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
            switch (keyInfo.Key)
            {
                case ConsoleKey.UpArrow:    playerY--; break;
                case ConsoleKey.DownArrow:  playerY++; break;
                case ConsoleKey.LeftArrow:  playerX--; break;
                case ConsoleKey.RightArrow: playerX++; break;
                case ConsoleKey.Escape:     isRunning = false; break;
            }
        }
    }

    static void Update()
    {
        // Wall boundaries
        if (playerX < 1) playerX = 1;
        if (playerX > ScreenWidth - 2) playerX = ScreenWidth - 2;
        if (playerY < 1) playerY = 1;
        if (playerY > ScreenHeight - 2) playerY = ScreenHeight - 2;

    }

    static void Render()
    {
        // TRICK 1: I stedet for Console.Clear() (som blinker), hopper vi altid
        // tilbage til øverste venstre hjørne (0,0) før vi tegner næste frame.
        Console.SetCursorPosition(0, 0);

        // Vi bygger hele skærmbilledet i hukommelsen først
        StringBuilder frameBuffer = new StringBuilder();

        for (int y = 0; y < ScreenHeight; y++)
        {
            for (int x = 0; x < ScreenWidth; x++)
            {
                if (y == 0 || y == ScreenHeight - 1 || x == 0 || x == ScreenWidth - 1)
                {
                    frameBuffer.Append('█'); // Væg
                }
                else if (x == playerX && y == playerY)
                {
                    frameBuffer.Append('@'); // Spiller
                }
                else if (x == itemX && y == itemY)
                {
                    frameBuffer.Append('★'); // Point-ting
                }
                else
                {
                    frameBuffer.Append(' '); // Tom plads
                }
            }
            frameBuffer.Append('\n');
        }

        // Statistik i bunden
        frameBuffer.Append($"\nScore: {score}  |  Brug piletasterne (ESC for at lukke)");
        
        // TRICK 2: Sørg for at overskrive eventuelle resterende linjer med tomme tegn,
        // hvis din tekst ændrer længde, så intet "stakker" eller skubbes ned.
        frameBuffer.Append(new string(' ', 40)); 

        // Skriv hele framen ud på én gang
        Console.Write(frameBuffer.ToString());
        
    }
}