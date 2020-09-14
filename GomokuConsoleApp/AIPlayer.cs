using System;
using System.Threading;

namespace GomokuConsoleApp
{
    class AIPlayer : Player
    {
        //Данные из игровой доски
        private char[,] _board;
        private char[ ] _initialColumnValues;
        private int [ ] _initialLineValues;
        private char _emptyCellSign;
        private int _numberOfLines = 0;
        private int _numberOfColumns = 0;

        /// <summary>
        /// Продолжительность символов в линию.
        /// </summary>
        private LineOfSigns _lineOfSigns = new LineOfSigns();

        /// <summary>
        /// Массив для вычисления более приоритетной ячейки.
        /// </summary>
        private Cell[,] _cell;


        public AIPlayer(int number, char sign, ConsoleColor playerColor, char[,] board, char emptyCellSign, char[] InitialColumnValues, int[] InitialLineValues) : base(number, sign, playerColor)
        {
            _board = board;
            _initialColumnValues = InitialColumnValues;
            _initialLineValues = InitialLineValues;
            _emptyCellSign = emptyCellSign;
            _numberOfLines = board.GetLength(0);
            _numberOfColumns = board.GetLength(1);
            _cell = new Cell[_numberOfLines, _numberOfColumns];
        }

        /// <summary>
        /// Ход компьютера
        /// </summary>
        /// <returns></returns>
        public override string Move()
        {
            Console.ForegroundColor = PlayerColor;
            Console.WriteLine($"Игрок №{Number}({Sign}) думает!");
            Console.ResetColor();
            CoordinatesOfMove coordinatesOfMove = new CoordinatesOfMove();
            HorizontalAnalysis();
            VerticalAnalysis();
            LeftDiagonalAnalysis();
            RightDiagonalAnalysis();
            PriorityAssessment(ref coordinatesOfMove);
            Thread.Sleep(1000);
            return ConvertToCoordinate(coordinatesOfMove.Y, coordinatesOfMove.X);
        }

        /// <summary>
        /// Метод вычисляет приоритетные координаты
        /// </summary>
        /// <param name="coordinatesOfMove"></param>
        private void PriorityAssessment(ref CoordinatesOfMove coordinatesOfMove)
        {
            for (int i = 0; i < _numberOfLines; i++)
            {
                for (int j = 0; j < _numberOfColumns; j++)
                {
                    if (_cell[i, j].MyLine == 5)
                        SetNewCoordinates(ref coordinatesOfMove, i, j, priority: 8);

                    else if (coordinatesOfMove.Priority < 7 && _cell[i, j].OpponentLine == 5)
                        SetNewCoordinates(ref coordinatesOfMove, i, j, priority: 7);

                    else if (coordinatesOfMove.Priority < 6 && _cell[i, j].MyLine == 4)
                        SetNewCoordinates(ref coordinatesOfMove, i, j, priority: 6);
                    else if (coordinatesOfMove.Priority == 6 && _cell[i, j].MyLine == _cell[coordinatesOfMove.Y, coordinatesOfMove.X].MyLine)
                        CheckMyPoints(ref coordinatesOfMove, i, j, 6);

                    else if (coordinatesOfMove.Priority < 5 && _cell[i, j].OpponentLine == 4)
                        SetNewCoordinates(ref coordinatesOfMove, i, j, priority: 5);
                    else if (coordinatesOfMove.Priority == 5 && _cell[i, j].OpponentLine == _cell[coordinatesOfMove.Y, coordinatesOfMove.X].OpponentLine)
                        CheckOpponentPoints(ref coordinatesOfMove, i, j, 5);

                    else if (coordinatesOfMove.Priority < 4 && _cell[i, j].MyLine == 3)
                        SetNewCoordinates(ref coordinatesOfMove, i, j, priority: 4);
                    else if (coordinatesOfMove.Priority == 4 && _cell[i, j].MyLine == _cell[coordinatesOfMove.Y, coordinatesOfMove.X].MyLine)
                        CheckMyPoints(ref coordinatesOfMove, i, j, 4);

                    else if (coordinatesOfMove.Priority < 3 && _cell[i, j].OpponentLine == 3)
                        SetNewCoordinates(ref coordinatesOfMove, i, j, priority: 3);
                    else if (coordinatesOfMove.Priority == 3 && _cell[i, j].OpponentLine == _cell[coordinatesOfMove.Y, coordinatesOfMove.X].OpponentLine)
                        CheckOpponentPoints(ref coordinatesOfMove, i, j, 3);

                    else if (coordinatesOfMove.Priority < 2 && _cell[i, j].MyLine == 2)
                        SetNewCoordinates(ref coordinatesOfMove, i, j, priority: 2);
                    else if (coordinatesOfMove.Priority == 2 && _cell[i, j].MyLine == _cell[coordinatesOfMove.Y, coordinatesOfMove.X].MyLine)
                        CheckMyPoints(ref coordinatesOfMove, i, j, 2);

                    else if (coordinatesOfMove.Priority < 1 && _cell[i, j].OpponentLine == 2)
                        SetNewCoordinates(ref coordinatesOfMove, i, j, priority: 1);
                    else if (coordinatesOfMove.Priority == 1 && _cell[i, j].OpponentLine == _cell[coordinatesOfMove.Y, coordinatesOfMove.X].OpponentLine)
                        CheckOpponentPoints(ref coordinatesOfMove, i, j, 1);
                    else if (coordinatesOfMove.Priority == 0)
                    {
                        coordinatesOfMove.Y = 7;
                        coordinatesOfMove.X = 7;
                    }
                }
            }
            ClearPointBoard();
        }

        /// <summary>
        /// Метод проверяет координаты компьютера в спорной ситуации
        /// </summary>
        /// <param name="coordinatesOfMove"></param>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <param name="priority"></param>
        private void CheckMyPoints(ref CoordinatesOfMove coordinatesOfMove, int i, int j, int priority)
        {
            Random randForEqualPriority = new Random();
            if (_cell[i, j].myPoints > _cell[coordinatesOfMove.Y, coordinatesOfMove.X].myPoints)
                SetNewCoordinates(ref coordinatesOfMove, i, j, priority);
            else if (_cell[i, j].myPoints == _cell[coordinatesOfMove.Y, coordinatesOfMove.X].myPoints && randForEqualPriority.Next(0, 100) > 50)
                SetNewCoordinates(ref coordinatesOfMove, i, j, priority);
        }

        /// <summary>
        /// Метод проверяет координаты оппонента в спорной ситуации.
        /// </summary>
        /// <param name="coordinatesOfMove"></param>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <param name="priority"></param>
        private void CheckOpponentPoints(ref CoordinatesOfMove coordinatesOfMove, int i, int j, int priority)
        {
            Random randForEqualPriority = new Random();
            if (_cell[i, j].OpponentPoints > _cell[coordinatesOfMove.Y, coordinatesOfMove.X].OpponentPoints)
                SetNewCoordinates(ref coordinatesOfMove, i, j, priority);
            else if (_cell[i, j].OpponentPoints == _cell[coordinatesOfMove.Y, coordinatesOfMove.X].OpponentPoints && randForEqualPriority.Next(0, 100) > 50)
                SetNewCoordinates(ref coordinatesOfMove, i, j, priority);
        }

        /// <summary>
        /// Метод устанавливает новые координаты.
        /// </summary>
        /// <param name="coordinatesOfMove"></param>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <param name="priority"></param>
        private static void SetNewCoordinates(ref CoordinatesOfMove coordinatesOfMove, int i, int j, int priority)
        {
            coordinatesOfMove.Y = i;
            coordinatesOfMove.X = j;
            coordinatesOfMove.Priority = priority;
        }

        /// <summary>
        /// Подсчёт длины знаков и очков ячеек по горизонтали.
        /// </summary>
        private void HorizontalAnalysis()
        {
            for (int i = 0; i < _numberOfLines; i++)
            {
                ResetLineOfSigns();
                for (int j = 0; j < _numberOfColumns; j++)
                {
                    Analysis(y:i, x:j);
                }
                FillCell();
            }
        }

        /// <summary>
        /// Подсчёт длины знаков и очков ячеек по вертикали.
        /// </summary>
        private void VerticalAnalysis()
        {
            for (int i = 0; i < _numberOfColumns; i++)
            {
                ResetLineOfSigns();
                for (int j = 0; j < _numberOfLines; j++)
                {
                    Analysis(y:j, x:i);
                }
                FillCell();
            }
        }

        /// <summary>
        /// Подсчёт длины знаков и очков ячеек по левой диагонали.
        /// </summary>
        private void LeftDiagonalAnalysis()
        {
            for (int i = 0; i < _numberOfLines; i++)
            {
                ResetLineOfSigns();
                int y = i;
                int x = 0;
                for (; y >= 0; y--, x++)
                {
                    Analysis(y, x);
                }
                FillCell();
            }
            for (int i = 1; i < _numberOfColumns; i++)
            {
                ResetLineOfSigns();
                int y = _numberOfColumns-1;
                int x = i;
                for (; x < _numberOfColumns; y--, x++)
                {
                    Analysis(y, x);
                }
                FillCell();
            }
        }

        /// <summary>
        /// Подсчёт длины знаков и очков ячеек по правой диагонали.
        /// </summary>
        private void RightDiagonalAnalysis()
        {
            for (int i = 0; i < _numberOfLines; i++)
            {
                ResetLineOfSigns();
                int y = i;
                int x = _numberOfColumns - 1;
                for (; y >= 0; y--, x--)
                {
                    Analysis(y, x);
                }
                FillCell();
            }

            for (int i = _numberOfColumns - 2; i >= 0; i--)
            {
                ResetLineOfSigns();
                int y = _numberOfColumns - 1;
                int x = i;
                for (; x >= 0; y--, x--)
                {
                    Analysis(y, x);
                }
                FillCell();
            }
        }

        /// <summary>
        /// Метод проверяет продолжительность выставленных символов в ряд.
        /// </summary>
        /// <param name="y"></param>
        /// <param name="x"></param>
        private void Analysis(int y, int x)
        {
            if (_board[y, x] == _emptyCellSign && _lineOfSigns.SignOfLine ==_emptyCellSign)
            {
                _lineOfSigns.StartOfLineY = y;
                _lineOfSigns.StartOfLineX = x;
                _lineOfSigns.IsNotClosed = true;
            }
            else if (_board[y, x] != _emptyCellSign && _lineOfSigns.SignOfLine == _emptyCellSign && _lineOfSigns.IsNotClosed)
            {
                SetNewLineOfSigns(sign: _board[y, x], isNoClosed: true);
            }
            else if (_board[y, x] != _emptyCellSign && _lineOfSigns.SignOfLine == _emptyCellSign && _lineOfSigns.IsNotClosed == false)
            {
                SetNewLineOfSigns(sign: _board[y, x], isNoClosed: false);
            }
            else if (_board[y, x] != _emptyCellSign && _board[y, x] == _lineOfSigns.SignOfLine)
            {
                _lineOfSigns.CountOfSigns++;
            }
            else if (_board[y, x] != _emptyCellSign && _lineOfSigns.SignOfLine != _emptyCellSign && _board[y, x] != _lineOfSigns.SignOfLine)
            {
                FillCell();
                SetNewLineOfSigns(sign: _board[y, x], isNoClosed: false);
            }
            else if (_board[y, x] == _emptyCellSign)
            {
                FillCell();
                FillCell(y, x);
                _lineOfSigns.SignOfLine = _emptyCellSign;
                _lineOfSigns.StartOfLineY = y;
                _lineOfSigns.StartOfLineX = x;
                _lineOfSigns.CountOfSigns = 0;
                _lineOfSigns.IsNotClosed = true;
            }
        }

        /// <summary>
        /// Сбрасывает линию символов.
        /// </summary>
        private void ResetLineOfSigns()
        {
            _lineOfSigns.CountOfSigns = 0;
            _lineOfSigns.IsNotClosed = false;
            _lineOfSigns.SignOfLine = _emptyCellSign;
        }

        /// <summary>
        /// Устанавливает новую линию символов.
        /// </summary>
        /// <param name="sign"></param>
        /// <param name="isNoClosed"></param>
        private void SetNewLineOfSigns(char sign, bool isNoClosed = true)
        {
            _lineOfSigns.SignOfLine = sign;
            _lineOfSigns.IsNotClosed = isNoClosed;
            _lineOfSigns.CountOfSigns = 2;
        }

        /// <summary>
        /// Заполняет последнюю пустую ячейку.
        /// </summary>
        private void FillCell()
        {
            if (_lineOfSigns.IsNotClosed && _lineOfSigns.SignOfLine == Sign)
            {
                _cell[_lineOfSigns.StartOfLineY, _lineOfSigns.StartOfLineX].MyLine = (_cell[_lineOfSigns.StartOfLineY, _lineOfSigns.StartOfLineX].MyLine < _lineOfSigns.CountOfSigns) ? _lineOfSigns.CountOfSigns : _cell[_lineOfSigns.StartOfLineY, _lineOfSigns.StartOfLineX].MyLine;
                _cell[_lineOfSigns.StartOfLineY, _lineOfSigns.StartOfLineX].myPoints = _cell[_lineOfSigns.StartOfLineY, _lineOfSigns.StartOfLineX].myPoints + _lineOfSigns.CountOfSigns;
            }
            else if (_lineOfSigns.IsNotClosed && _lineOfSigns.SignOfLine != Sign)
            {
                _cell[_lineOfSigns.StartOfLineY, _lineOfSigns.StartOfLineX].OpponentLine = (_cell[_lineOfSigns.StartOfLineY, _lineOfSigns.StartOfLineX].OpponentLine < _lineOfSigns.CountOfSigns) ? _lineOfSigns.CountOfSigns : _cell[_lineOfSigns.StartOfLineY, _lineOfSigns.StartOfLineX].OpponentLine;
                _cell[_lineOfSigns.StartOfLineY, _lineOfSigns.StartOfLineX].OpponentPoints = _cell[_lineOfSigns.StartOfLineY, _lineOfSigns.StartOfLineX].OpponentPoints + _lineOfSigns.CountOfSigns;
            }
        }

        /// <summary>
        /// Заполняет указанную пустую ячейку.
        /// </summary>
        /// <param name="y"></param>
        /// <param name="x"></param>
        private void FillCell(int y, int x)
        {
            if (_lineOfSigns.CountOfSigns > 0 && _lineOfSigns.SignOfLine == Sign)
            {
                _cell[y, x].MyLine = (_cell[y, x].MyLine < _lineOfSigns.CountOfSigns) ? _lineOfSigns.CountOfSigns : _cell[y, x].MyLine;
                _cell[y, x].myPoints = _cell[y, x].myPoints + _lineOfSigns.CountOfSigns;
            }
            else if (_lineOfSigns.CountOfSigns > 0 && _lineOfSigns.SignOfLine != Sign)
            {
                _cell[y, x].OpponentLine = (_cell[y, x].OpponentLine < _lineOfSigns.CountOfSigns) ? _lineOfSigns.CountOfSigns : _cell[y, x].OpponentLine;
                _cell[y, x].OpponentPoints = _cell[y, x].OpponentPoints + _lineOfSigns.CountOfSigns;
            }
        }

        /// <summary>
        /// Обнуляет вес ячеек.
        /// </summary>
        private void ClearPointBoard()
        {
            for (int i = 0; i < _numberOfLines; i++)
            {
                for (int j = 0; j < _numberOfColumns; j ++)
                {
                    _cell[i, j].MyLine = 0;
                    _cell[i, j].myPoints = 0;
                    _cell[i, j].OpponentLine = 0;
                    _cell[i, j].OpponentPoints = 0;
                }
            }
        }

        /// <summary>
        /// Метод конвертирует полученные координаты для вывода в консоль.
        /// </summary>
        /// <param name="LineIndex"></param>
        /// <param name="columnIndex"></param>
        /// <returns></returns>
        private string ConvertToCoordinate(int LineIndex, int columnIndex)
        {
            return Convert.ToString(_initialColumnValues[columnIndex] + "-" + _initialLineValues[LineIndex]);
        }
    }
}
