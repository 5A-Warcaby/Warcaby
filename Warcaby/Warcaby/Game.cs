using System;

namespace Warcaby
{
    public class Game
    {
        private Board board;
        private bool isWhiteTurn;

        public Game()
        {
            board = new Board();
            isWhiteTurn = true;
        }


        public void PlayGame()
        {
            bool additionalCapture;

            while (true)
            {
                board.DisplayBoard();
                Console.WriteLine(isWhiteTurn ? "White's turn" : "Black's turn.");

                int startRow, startCol, endRow, endCol;

                if (!RequestMove(isWhiteTurn, out startRow, out startCol, out endRow, out endCol, out additionalCapture))
                {
                    Console.WriteLine("Invalid move! Try again.");
                    continue;
                }

                while (additionalCapture)
                {
                    board.DisplayBoard();
                    Console.WriteLine("You can make another capture.");
                    if (!RequestMove(isWhiteTurn, out startRow, out startCol, out endRow, out endCol, out additionalCapture))
                    {
                        Console.WriteLine("Invalid move! Try again.");
                    }
                }

                if (board.IsGameOver(isWhiteTurn))
                {
                    board.DisplayBoard();
                    Console.WriteLine(isWhiteTurn ? "Black wins!" : "White wins!");
                    break;
                }


                isWhiteTurn = !isWhiteTurn;
            }

        }

        private bool RequestMove(bool isWhiteTurn, out int startRow, out int startCol, out int endRow, out int endCol, out bool additionalCapture)
        {
            // Inicjalizujemy zmienne wyjściowe
            startRow = -1;
            startCol = -1;
            endRow = -1;
            endCol = -1;
            additionalCapture = false;

            Console.Write("Choose a piece (e.g., B6): ");
            if (!ParseInput(out startRow, out startCol))
                return false; // Nieudane parsowanie pozycji początkowej.

            Console.Write("Choose a target position (e.g., C5): ");
            if (!ParseInput(out endRow, out endCol))
                return false; // Nieudane parsowanie pozycji docelowej.

            if (board.MovePiece(isWhiteTurn, startRow, startCol, endRow, endCol, out additionalCapture))
            {
                char piece = board[endRow, endCol];
                if (piece == Board.WhiteKing || piece == Board.BlackKing)
                {
                    Console.WriteLine("Promotion! Your piece has been crowned as a king.");
                }
            }


            // Próba wykonania ruchu
            if (board.MovePiece(isWhiteTurn, startRow, startCol, endRow, endCol, out additionalCapture))
            {
                // Sprawdzanie promocji po ruchu
                if (board[endRow, endCol] == Board.WhiteKing || board[endRow, endCol] == Board.BlackKing)
                {
                    Console.WriteLine("Promotion! Your piece has been crowned as a king.");
                }
                return true; // Ruch się powiódł.
            }

            return false; // Ruch był nieprawidłowy.
        }




        private bool ParseInput(out int row, out int col)
        {
            row = col = -1;
            string input = Console.ReadLine();
            if (input.Length != 2)
                return false;

            col = input[0] - 'A';
            row = 8 - (input[1] - '0');

            return col >= 0 && col < 8 && row >= 0 && row < 8;
        }
    }
}
