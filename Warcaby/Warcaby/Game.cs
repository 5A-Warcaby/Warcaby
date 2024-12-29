using System;

namespace Warcaby
{
     public void PlayGame()
     { 
          public const int Size = 8;
          public char[,] board;
          public const char Empty = '.';
          public const char White = 'w';
          public const char Black = 'b';
          public const char WhiteKing = 'W';
          public const char BlackKing = 'B';
          public char this[int row, int col]
          {
              get { return board[row, col]; }
              set { board[row, col] = value; }
          }
          
          public Board()
          {
              board = new char[Size, Size];
              InitializeBoard();
          }
          
          private void InitializeBoard()
          {
              for (int i = 0; i < Size; i++)
                  for (int j = 0; j < Size; j++)
                      board[i, j] = (i + j) % 2 == 0 ? ' ' : Empty;
          
              for (int i = 0; i < 3; i++)
                  for (int j = 0; j < Size; j++)
                      if ((i + j) % 2 != 0)
                          board[i, j] = Black;
          
              for (int i = 5; i < Size; i++)
                  for (int j = 0; j < Size; j++)
                      if ((i + j) % 2 != 0)
                          board[i, j] = White;
          
          }
          
          public void DisplayBoard()
          {
              Console.Clear();
              Console.WriteLine("  A B C D E F G H");
              for (int i = 0; i < Size; i++)
              {
                  Console.Write(8 - i + " ");
                  for (int j = 0; j < Size; j++)
                      Console.Write(board[i, j] + " ");
                  Console.WriteLine();
              }
              Console.WriteLine("  A B C D E F G H");
          }
          private bool CanMoveDiagonally(int startRow, int startCol, int endRow, int endCol)
          {
              int rowStep = endRow > startRow ? 1 : -1;
              int colStep = endCol > startCol ? 1 : -1;
          
              int currentRow = startRow + rowStep;
              int currentCol = startCol + colStep;
          
              while (currentRow != endRow && currentCol != endCol)
              {
                  if (board[currentRow, currentCol] != Empty)
                      return false;
          
                  currentRow += rowStep;
                  currentCol += colStep;
              }
              return true;
          }
     }
}
