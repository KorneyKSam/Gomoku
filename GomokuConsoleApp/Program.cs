using System;
using System.Linq;


namespace GomokuConsoleApp
{
    class Program
    {
        public enum GameStatus : byte { Settings, GameParty, FirstPlayerWins, SecondPlayerWins, NoOneWins };
        private const int WindowHeight = 40;
        private const string InitialColumnValues = "abcdefghijklmnopqrstuvwxyz"; 

        static void Main(string[] args)
        {
            int turnForPlayer = 1;
            int numberOfPlayers = 2;
            Console.WindowHeight = WindowHeight;
            GameStatus gameStatus = GameStatus.Settings;
            Board board = new Board(InitialColumnValues.ToArray());
            Player player1 = null; 
            Player player2 = null; 
            while (true)
            {
                try
                {
                    switch (gameStatus)
                    {
                        case GameStatus.Settings:
                            Console.Clear();
                            Console.Write("1.Два игрока\n2.Против ИИ\n3.Два ИИ\n\nВыберите тип игры:");
                            board.SetBoardArray(numberOfLines: 15, numberOfColumns: 15, emptyCellSign: ' ');
                            int gameType = Convert.ToInt32(Console.ReadLine());
                            if (gameType == 1)
                            {
                                player1 = new HumanPlayer(number: 1, sign: 'X', playerColor: ConsoleColor.Red);
                                player2 = new HumanPlayer(number: 1, sign: '0', playerColor: ConsoleColor.Blue);
                            }
                            else if (gameType == 2)
                            {
                                player1 = new HumanPlayer(number: 1, sign: 'X', playerColor: ConsoleColor.Red);
                                player2 = new AIPlayer(number: 2, sign: '0', playerColor: ConsoleColor.Blue, board.BoardArray, board.EmptyCellSign, board.InitialColumnValues, board.InitialLineValues);
                            }
                            else if (gameType == 3)
                            {
                                player1 = new AIPlayer(number: 1, sign: 'X', playerColor: ConsoleColor.Red, board.BoardArray, board.EmptyCellSign, board.InitialColumnValues, board.InitialLineValues);
                                player2 = new AIPlayer(number: 2, sign: '0', playerColor: ConsoleColor.Blue, board.BoardArray, board.EmptyCellSign, board.InitialColumnValues, board.InitialLineValues);
                            }
                            else
                            {
                                throw new Exception("Введён неверный тип!");
                            }
                            gameStatus = GameStatus.GameParty;
                            break;

                        case GameStatus.GameParty:
                            board.PrintBoard(player1.Sign, player2.Sign);
                            if (turnForPlayer == 1) board.FillInCoordinates(player1.Move(), player1.Sign);
                            else board.FillInCoordinates(player2.Move(), player2.Sign);
                            CheckGameStatus(board, ref turnForPlayer, ref numberOfPlayers, ref gameStatus);
                            break;

                        case GameStatus.FirstPlayerWins:
                        case GameStatus.SecondPlayerWins:
                            board.PrintBoard(player1.Sign, player2.Sign);
                            Console.Write((gameStatus == GameStatus.FirstPlayerWins)? "Первый игрок победил!\nНажмите любую клавишу чтобы продолжить..." : "Второй игорок победил!\nНажмите любую клавишу чтобы продолжить...");
                            Console.ReadLine();
                            gameStatus = GameStatus.Settings;
                            break;

                        case GameStatus.NoOneWins:
                            board.PrintBoard(player1.Sign, player2.Sign);
                            Console.Write("Ничья!\nНажмите любую клавишу чтобы продолжить...");
                            Console.ReadLine();
                            gameStatus = GameStatus.Settings;
                            break;
                    }
                }
                catch (Exception exception)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Ошибка: {exception.Message} Нажмите любую клавишу чтобы продолжить...\n");
                    Console.ResetColor();
                    Console.ReadLine();
                    continue;
                }
            }
        }

        /// <summary>
        /// Проверка статуса игры.
        /// </summary>
        /// <param name="turnForPlayer"></param>
        /// <param name="gameStatus"></param>
        /// <param name="board"></param>
        private static void CheckGameStatus(Board board, ref int turnForPlayer, ref int numberOfPlayers, ref GameStatus gameStatus)
        {
            if (board.CheckLastCoordinates())
                gameStatus = (turnForPlayer == 1) ? GameStatus.FirstPlayerWins : GameStatus.SecondPlayerWins;
            else if (board.NumberOfEmptyCells <= 0) gameStatus = GameStatus.NoOneWins;
            else    turnForPlayer = (turnForPlayer >= numberOfPlayers) ? 1 : turnForPlayer + 1;
        }

    }
}
