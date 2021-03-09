using System;

namespace TicTacToe
{
    class Program
    {
        static TicTacToeGame game = new TicTacToeGame();
        static void Main(string[] args)
        {
            while (game.HasLegalMove() && game.GetWinner() == TicTacToeGame.TicTacToeSquare.Empty)
            {
                Console.Clear();
                Console.WriteLine(game.GetBoardString());

                MakeMove(TicTacToeGame.TicTacToeSquare.X);

                if (game.HasLegalMove() && game.GetWinner() != TicTacToeGame.TicTacToeSquare.Empty)
                    break;

                Console.Clear();
                Console.WriteLine(game.GetBoardString());

                MakeMove(TicTacToeGame.TicTacToeSquare.O);
            }

            switch(game.GetWinner())
            {
                case TicTacToeGame.TicTacToeSquare.X:
                    Console.WriteLine("Player X wins!");
                    return;
                case TicTacToeGame.TicTacToeSquare.O:
                    Console.WriteLine("Player O wins!");
                    return;
                default:
                    Console.WriteLine("Draw");
                    return;
            }
        }

        static void MakeMove(TicTacToeGame.TicTacToeSquare square)
        {
            string player = "";
            if (square == TicTacToeGame.TicTacToeSquare.X) player = "X";
            if (square == TicTacToeGame.TicTacToeSquare.O) player = "O";

            Console.Write($"Player {player}: ");
            string input = Console.ReadLine();

            if (input == null || !input.Contains(','))
            {
                Console.WriteLine("Input coordinates (x,y)");
                MakeMove(square);
                return;
            }

            try
            {
                int x = Int16.Parse(input.Split(',', 2)[0]) - 1;
                int y = Int16.Parse(input.Split(',', 2)[1]) - 1;

                var res = game.TryMakeMove(x, y, square);

                if (!res.Item1)
                {
                    Console.WriteLine(res.Item2);
                    MakeMove(square);
                    return;
                }
            }
            catch
            {
                Console.WriteLine("Input coordinates (x,y)");
                MakeMove(square);
                return;
            }
        }
    }

    public class TicTacToeGame
    {
        public enum TicTacToeSquare
        {
            Empty, X, O
        }

        protected TicTacToeSquare[,] Board;

        public TicTacToeGame()
        {
            Board = new TicTacToeSquare[3,3];
            
            for (int x = 0; x < 3; x++)
                for (int y = 0; y < 3; y++)
                Board[x,y] = TicTacToeSquare.Empty;
        }

        public (bool, string) TryMakeMove(int x, int y, TicTacToeSquare move)
        {
            if (x < 0 || x > 2 || y < 0 || y > 2)
                return (false, "Square out of bounds.");

            if (Board[x,y] != TicTacToeGame.TicTacToeSquare.Empty)
                return (false, "Cannot place on occupied square.");

            Board[x,y] = move;

            return (true, "");
        }

        public TicTacToeSquare GetWinner()
        {
            for (int x = 0; x < 3; x++)
            {
                // Scan columns
                if ((Board[x, 0] == Board[x, 1]) && Board[x, 0] == Board[x, 2])
                    return Board[x, 0];

                // Scan diagonal starting at 0,0
                if (x == 0 && (Board[0, 0] == Board[1, 1]) && Board[0, 0] == Board[2, 2])
                    return Board[0, 0];

                // Scan diagonal starting at 0, 2
                if (x == 2 && (Board[2, 0] == Board[1, 1]) && Board[2, 0] == Board[0, 2])
                    return Board[2, 0];
            }

            for (int y = 0; y < 3; y++)
            {
                // Scan rows
                if ((Board[0, y] == Board[1, y]) && Board[0, y] == Board[2, y])
                    return Board[0, y];
            }

            return TicTacToeSquare.Empty;
        }

        public bool HasLegalMove()
        {
            foreach (var square in Board)
                if (square == TicTacToeSquare.Empty)
                    return true;

            return false;
        }

        public string GetBoardString()
        {
            string output = "";

            output += GetChar(Board[0,0]) + '|' + GetChar(Board[1,0]) + '|' + GetChar(Board[2,0]) + '\n';
            output += GetChar(Board[0,1]) + '|' + GetChar(Board[1,1]) + '|' + GetChar(Board[2,1]) + '\n';
            output += GetChar(Board[0,2], true) + '|' + GetChar(Board[1,2], true) + '|' + GetChar(Board[2,2], true) + '\n';

            return output;
        }

        public string GetChar(TicTacToeSquare square, bool lastRow = false)
        {
            switch (square)
            {
                case TicTacToeSquare.X:
                    return "x";
                case TicTacToeSquare.O:
                    return "O";
            }

            if (lastRow)
                return " ";

            return "_";
        }
    }
}
