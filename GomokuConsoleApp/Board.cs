using System;
using System.Globalization;

namespace GomokuConsoleApp
{
    class Board
    {
        /// <summary>
        /// Игровая доска.
        /// </summary>
        public char[,] BoardArray { get; private set; }

        /// <summary>
        /// Символ пустой ячейки.
        /// </summary>
        public char EmptyCellSign { get; private set; }

        /// <summary>
        /// Количество пустых ячеек.
        /// </summary>
        public int NumberOfEmptyCells { get; private set; }

        /// <summary>
        /// Координаты столбцов.
        /// </summary>
        public char[] InitialColumnValues { get; private set; }

        /// <summary>
        /// Координаты строк.
        /// </summary>
        public int[] InitialLineValues { get; private set; }

        /// <summary>
        /// Введённый индекс строки.
        /// </summary>
        private int _enteredLineIndex;

        /// <summary>
        /// Введённый индекс столбца.
        /// </summary>
        private int _enteredColumnIndex;

        /// <summary>
        /// Количество символов для доски 15x15.
        /// </summary>
        private const int CountOfSymbolsForBoard15x15 = 61;

        private int _numberOfLines = 0;
        private int _numberOfColumns = 0;

        public Board(char[] initialColumnValues)
        {
            InitialColumnValues = initialColumnValues;
        }

        /// <summary>
        /// Метод создаёт игровую доску и заполняет её указанными символами.
        /// </summary>
        /// <param name="numberOfLines"></param>
        /// <param name="numberOfColumns"></param>
        /// <param name="fillSign"></param>
        public void SetBoardArray(int numberOfLines, int numberOfColumns, char emptyCellSign)
        {
            EmptyCellSign = emptyCellSign;
            _numberOfColumns = numberOfColumns;
            _numberOfLines = numberOfLines;
            NumberOfEmptyCells = _numberOfLines * _numberOfColumns;
            BoardArray = new char[_numberOfLines, _numberOfColumns];
            InitialLineValues = new int[_numberOfLines];
            for (int i = 0, j = _numberOfLines; i < InitialLineValues.Length; i++, j--) InitialLineValues[i] = j;
            for (int i = 0; i < _numberOfLines; i++)
            {
                for (int j = 0; j < _numberOfColumns; j++)
                {
                    BoardArray[i, j] = emptyCellSign;
                }
            }
        }

        /// <summary>
        /// Метод принимает координаты, разделяет их и вносит в ячейку принимаемый символ.
        /// </summary>
        /// <param name="coordinates"></param>
        /// <param name="PlayerSign"></param>
        public void FillInCoordinates(string coordinates, char playerSign)
        {
            //Разделение строк и столбцов
            string[] splittedCoordinates = coordinates.Split('-');

            //Сопоставление с реальными индексами массива
            _enteredLineIndex = Array.FindIndex(InitialLineValues, i => i == Convert.ToInt32(splittedCoordinates[1]));
            _enteredColumnIndex = Array.FindIndex(InitialColumnValues, i => i == Convert.ToChar(splittedCoordinates[0]));
            
            //Проверка занятости ячейки
            if (BoardArray[_enteredLineIndex, _enteredColumnIndex] != EmptyCellSign)
            {
                throw new Exception("Ячейка уже занята!");
            }
            BoardArray[_enteredLineIndex, _enteredColumnIndex] = playerSign;
            NumberOfEmptyCells--;
        }

        /// <summary>
        /// Метод ищет победителя.
        /// </summary>
        /// <param name="firstPlayerSign"></param>
        /// <param name="secondPlayerSign"></param>
        public bool CheckLastCoordinates()
        {
            return HorizontalCheck() || VerticalCheck() || LeftDiagonalCkeck() || RightDiagonalCheck();
        }

        /// <summary>
        /// Проверка правой диагонали ячейки.
        /// </summary>
        /// <returns></returns>
        private bool RightDiagonalCheck()
        {
            uint countOfSigns = 1;
            char lastSign = EmptyCellSign;
            int rowDiagonalIndex = 0;
            int columnDiagonalIndex = 0;

            //Поиск крайней позиции по диагонали
            for (int i = _enteredLineIndex, j = _enteredColumnIndex; i < _numberOfLines && j < _numberOfColumns;)
            {
                if (i == _numberOfLines - 1 || j == _numberOfColumns - 1)
                {
                    rowDiagonalIndex = i;
                    columnDiagonalIndex = j;
                    break;
                }
                else
                {
                    i++;
                    j++;
                }
            }

            //Проверка диагонали
            for (int i = rowDiagonalIndex, j = columnDiagonalIndex; i >= 0 && j >= 0; i--, j--)
            {
                countOfSigns = (BoardArray[i, j] != EmptyCellSign && lastSign == BoardArray[i, j]) ? countOfSigns + 1 : 1;
                lastSign = BoardArray[i, j];
                if (countOfSigns == 5) return true;
            }
            return false;
        }

        /// <summary>
        /// Проверка левой диагонали ячейки.
        /// </summary>
        /// <returns></returns>
        private bool LeftDiagonalCkeck()
        {
            uint countOfSigns = 1;
            char lastSign = EmptyCellSign;
            int rowDiagonalIndex = 0;
            int columnDiagonalIndex = 0;

            //Поиск крайней позиции по диагонали
            for (int i = _enteredLineIndex, j = _enteredColumnIndex; i < _numberOfLines && j >= 0;)
            {
                if (i == _numberOfLines - 1 || j == 0)
                {
                    rowDiagonalIndex = i;
                    columnDiagonalIndex = j;
                    break;
                }
                else
                {
                    i++;
                    j--;
                }
            }

            //Проверка диагонали
            for (int i = rowDiagonalIndex, j = columnDiagonalIndex; i >= 0 && j < _numberOfColumns; i--, j++)
            {
                countOfSigns = (BoardArray[i, j] != EmptyCellSign && lastSign == BoardArray[i, j]) ? countOfSigns + 1 : 1;
                lastSign = BoardArray[i, j];
                if (countOfSigns == 5) return true;
            }
            return false;
        }

        /// <summary>
        /// Проверка вертикали ячейки.
        /// </summary>
        /// <returns></returns>
        private bool VerticalCheck()
        {
            uint countOfSigns = 1;
            char lastSign = EmptyCellSign;

            for (int i = 0; i < _numberOfLines; i++)
            {
                countOfSigns = (BoardArray[i, _enteredColumnIndex] != EmptyCellSign && lastSign == BoardArray[i, _enteredColumnIndex]) ? countOfSigns + 1 : 1;
                lastSign = BoardArray[i, _enteredColumnIndex];
                if (countOfSigns == 5) return true;
            }
            return false;
        }

        /// <summary>
        /// Проверка горизонтали ячейки.
        /// </summary>
        /// <returns></returns>
        private bool HorizontalCheck()
        {
            uint countOfSigns = 1;
            char lastSign = EmptyCellSign;

            for (int i = 0; i < _numberOfColumns; i++)
            {
                countOfSigns = (BoardArray[_enteredLineIndex, i] != EmptyCellSign && lastSign == BoardArray[_enteredLineIndex, i]) ? countOfSigns + 1 : 1;
                lastSign = BoardArray[_enteredLineIndex, i];
                if (countOfSigns == 5) return true;
            }
            return false;
        }

        /// <summary>
        /// Метод выводит на консоль символы столбцов, строк и игровую доску, изменяет цвет у принимаемых символов.
        /// </summary>
        /// <param name="firstPlayerSign"></param>
        /// <param name="secondPlayerSign"></param>
        public void PrintBoard(char firstPlayerSign, char secondPlayerSign)
        {
            Console.Clear();
            Console.Write("\t");
            for (int i = 0; i < _numberOfLines; i++)
            {
                Console.Write($"  {InitialColumnValues[i]} ");
            }
            Console.WriteLine();
            for (int i = 0; i < _numberOfLines; i++)
            {
                Console.Write($"{InitialLineValues[i]}\t| ");
                for (int j = 0; j < _numberOfColumns; j++)
                {
                    if (BoardArray[i, j] == firstPlayerSign)
                        Console.ForegroundColor = ConsoleColor.Red;
                    else if (BoardArray[i, j] == secondPlayerSign)
                        Console.ForegroundColor = ConsoleColor.Blue;
                    Console.Write(BoardArray[i, j]);
                    Console.ResetColor();
                    Console.Write(" | ");
                }
                Console.WriteLine($"\n\t{new string('-', CountOfSymbolsForBoard15x15)}");
            }
            Console.WriteLine(new string('_', 100));
        }
    }
}
