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
          public bool MovePiece(bool isWhiteTurn, int startRow, int startCol, int endRow, int endCol, out bool additionalCapture)
          {
              additionalCapture = false;
              char piece = board[startRow, startCol];
              char target = board[endRow, endCol];
          
              if ((isWhiteTurn && (piece != White && piece != WhiteKing)) || (!isWhiteTurn && (piece != Black && piece != BlackKing)))
                  return false;
          
              if (target != Empty)
                  return false;
          
              int rowDiff = endRow - startRow;
              int colDiff = endCol - startCol;
              if (piece == WhiteKing || piece == BlackKing)
              {
                  if (Math.Abs(rowDiff) == Math.Abs(colDiff)) // Ruch na przekątnej
                  {
                      if (CanMoveDiagonally(startRow, startCol, endRow, endCol))
                      {
                          board[endRow, endCol] = piece;
                          board[startRow, startCol] = Empty;
                          additionalCapture = CanCapture(endRow, endCol, isWhiteTurn, piece);
                          return true;
                      }
                  }
              }
          
              if (piece == WhiteKing || piece == BlackKing)
              {
                  int rowStep = rowDiff > 0 ? 1 : -1;
                  int colStep = colDiff > 0 ? 1 : -1;
          
                  int currentRow = startRow + rowStep;
                  int currentCol = startCol + colStep;
          
                  bool captured = false;
          
                  while (currentRow != endRow && currentCol != endCol)
                  {
                      char currentPiece = board[currentRow, currentCol];
          
                      if (currentPiece != Empty)
                      {
                          if ((isWhiteTurn && (currentPiece == Black || currentPiece == BlackKing)) ||
                              (!isWhiteTurn && (currentPiece == White || currentPiece == WhiteKing)))
                          {
                              if (captured)
                                  return false; // Możemy zbić tylko raz w jednym ruchu
          
                              captured = true;
                          }
                          else
                          {
                              return false; // Napotkano przeszkodę
                          }
                      }
          
                      currentRow += rowStep;
                      currentCol += colStep;
                  }
          
                  if (captured)
                  {
                      // Usuń zbitego przeciwnika
                      int capturedRow = (startRow + endRow) / 2;
                      int capturedCol = (startCol + endCol) / 2;
                      board[capturedRow, capturedCol] = Empty;
                  }
          
                  board[endRow, endCol] = piece;
                  board[startRow, startCol] = Empty;
          
                  additionalCapture = CanCapture(endRow, endCol, isWhiteTurn, piece);
                  return true;
              }
          
          
          
              if (Math.Abs(rowDiff) == 1 && Math.Abs(colDiff) == 1)
              {
                  board[endRow, endCol] = piece;
                  board[startRow, startCol] = Empty;
                  PromoteIfNeeded(endRow, endCol, isWhiteTurn);
                  return true;
              }
              else if (Math.Abs(rowDiff) >= 2 && Math.Abs(rowDiff) == Math.Abs(colDiff))
              {
                  int stepRow = rowDiff / Math.Abs(rowDiff);
                  int stepCol = colDiff / Math.Abs(colDiff);
                  int midRow = startRow + stepRow;
                  int midCol = startCol + stepCol;
                  char midPiece = board[midRow, midCol];
          
                  if ((isWhiteTurn && (midPiece == Black || midPiece == BlackKing)) ||
                      (!isWhiteTurn && (midPiece == White || midPiece == WhiteKing)))
                  {
                      board[endRow, endCol] = piece;
                      board[startRow, startCol] = Empty;
                      board[midRow, midCol] = Empty;
                      PromoteIfNeeded(endRow, endCol, isWhiteTurn);
          
                      additionalCapture = CanCapture(endRow, endCol, isWhiteTurn, piece);
                      return true;
                  }
              }
          
              return false;
          }
          private bool CanCapture(int row, int col, bool isWhiteTurn, char piece)
          {
              int[] directions = { -1, 1 };
          
              foreach (int rowDir in directions)
              {
                  foreach (int colDir in directions)
                  {
                      int currentRow = row + rowDir;
                      int currentCol = col + colDir;
          
                      while (currentRow >= 0 && currentRow < Size && currentCol >= 0 && currentCol < Size)
                      {
                          char currentPiece = board[currentRow, currentCol];
          
                          // Znalezienie przeciwnika na ścieżce
                          if ((isWhiteTurn && (currentPiece == Black || currentPiece == BlackKing)) ||
                              (!isWhiteTurn && (currentPiece == White || currentPiece == WhiteKing)))
                          {
                              int nextRow = currentRow + rowDir;
                              int nextCol = currentCol + colDir;
          
                              if (nextRow >= 0 && nextRow < Size && nextCol >= 0 && nextCol < Size &&
                                  board[nextRow, nextCol] == Empty)
                              {
                                  return true;
                              }
                              break; // Wyjście, bo nie można kontynuować po znalezieniu przeciwnika
                          }
          
                          if (currentPiece != Empty)
                              break; // Napotkano własny pionek lub inną przeszkodę
          
                          currentRow += rowDir;
                          currentCol += colDir;
                      }
                  }
              }
          
              return false;
          }
          private void PromoteIfNeeded(int row, int col, bool isWhiteTurn)
          {
              if (isWhiteTurn && row == 0)
                  board[row, col] = WhiteKing;
              else if (!isWhiteTurn && row == 7)
                  board[row, col] = BlackKing;
          }
     }
}
