namespace TikTakToeGame;

static class Program
{
    private static readonly char[,] _board = new char[3, 3];
    private const char _player1 = 'X';
    private const char _player2 = 'O';
    private static readonly char[] players = { _player1, _player2 };
    private static readonly Dictionary<string, char> _namePlayers = new();
    private static readonly Dictionary<string, (int, int)> _positions = new()
{
    {"top-left", (0, 0) },
    {"top-middle", (0, 1) },
    {"top-right", (0, 2) },
    {"middle-left", (1, 0)},
    {"middle", (1, 1)},
    {"middle-right", (1, 2)},
    {"bottom-left", (2, 0)},
    {"bottom-middle", (2, 1)},
    {"bottom-right", (2, 2)}
};
    private static readonly List<List<(int, int)>> _winningPositions = new List<List<(int, int)>>
{
    new List<(int, int)> { (0, 0), (0, 1), (0, 2) },
    new List<(int, int)> { (1, 0), (1, 1), (1, 2) },
    new List<(int, int)> { (2, 0), (2, 1), (2, 2) },
    new List<(int, int)> { (0, 0), (1, 0), (2, 0) },
    new List<(int, int)> { (0, 1), (1, 1), (2, 1) },
    new List<(int, int)> { (0, 2), (1, 2), (2, 2) },
    new List<(int, int)> { (0, 0), (1, 1), (2, 2) },
    new List<(int, int)> { (0, 2), (1, 1), (2, 0) }
};


    static void Main()
    {
        FillBoard();

        PrintBoard();

        Console.WriteLine("Welcome to TikTakToe Game!");
        Console.WriteLine("Write player X's name:");
        string input1 = Console.ReadLine();
        while (string.IsNullOrEmpty(input1))
        {
            Console.WriteLine("Name cannot be empty, please write X's name.");
            input1 = Console.ReadLine();
        }

        _namePlayers.Add(input1, _player1);
        Console.WriteLine("Write player O's name:");
        string input2 = Console.ReadLine();
        while (string.IsNullOrEmpty(input2))
        {
            Console.WriteLine("Name cannot be empty, please write O's name.");
            input2 = Console.ReadLine();
        }
        _namePlayers.Add(input2, _player2);

        Console.WriteLine("Pick the position you want to place your X or O");
        int i = 1;
        var positionMap = new Dictionary<int, string>();
        foreach (var pos in _positions)
        {
            Console.WriteLine($"{i} - {pos.Key}");
            positionMap.Add(i, pos.Key);
            i++;
        }
        var playerTurn = players[0];

        while (true)
        {
            Console.WriteLine($"{_namePlayers.FirstOrDefault(x => x.Value == playerTurn).Key}'s turn");
            Console.Write($"{playerTurn} Picked - ");
            var pickedPos = Console.ReadKey().KeyChar;

            PositionPicker(positionMap, pickedPos, playerTurn);
            if (IsWinningPosition(out string message))
            {
                if (message.Contains('X') || message.Contains('O'))
                {
                    Console.WriteLine($"{message}");
                    break;
                }
            }
            else if (IsBoardFull())
            {
                Console.WriteLine("Game is a draw!");
                break;
            }
            playerTurn = IsPositionValid();
        }
    }

    private static bool IsWinningPosition(out string message)
    {
        foreach (var winningPosition in _winningPositions)
        {
            var (row1, col1) = winningPosition[0];
            var (row2, col2) = winningPosition[1];
            var (row3, col3) = winningPosition[2];

            if (_board[row1, col1] == _board[row2, col2] && _board[row2, col2] == _board[row3, col3])
            {
                message = $"{_board[row1, col1]} has won the game!";
                return true;
            }
        }
        message = "No one has won the game!";
        return false;
    }

    private static bool IsBoardFull()
    {
        return !_board.Cast<char>().Any(x => x == '-');
    }

    private static char IsPositionValid()
    {
        int xCount = 0;
        int oCount = 0;
        for (int i = 0; i < _board.GetLength(0); i++)
        {
            for (int j = 0; j < _board.GetLength(1); j++)
            {
                if (_board[i, j] == _player1)
                {
                    xCount++;
                }
                else if (_board[i, j] == _player2)
                {
                    oCount++;
                }
            }
        }
        if (xCount > oCount)
        {
            return _player2;
        }
        else
        {
            return _player1;
        }
    }
    private static void PositionPicker(Dictionary<int, string> positionMap, char pickedPos, char playerTurn)
    {
        if (int.TryParse(pickedPos.ToString(), out int index) && positionMap.TryGetValue(index, out string tomove))
        {
            var (row, col) = _positions[tomove];
            if (_board[row, col] != '-')
            {
                Console.WriteLine("Invalid input. Please choose a valid position.");
                return;
            }
            _board[row, col] = playerTurn;
            Console.WriteLine();
            PrintBoard();
        }
        else
        {
            Console.WriteLine("Invalid input. Please choose a valid position.");
        }
    }

    private static void FillBoard()
    {
        for (int row = 0; row < _board.GetLength(0); row++)
        {
            for (int col = 0; col < _board.GetLength(1); col++)
            {
                _board[row, col] = '-';
            }
        }
    }

    private static void PrintBoard()
    {
        for (int row = 0; row < _board.GetLength(0); row++)
        {
            for (int col = 0; col < _board.GetLength(1); col++)
            {
                Console.Write(_board[row, col] + " ");
            }
            Console.WriteLine();
        }
    }
}