using System;

namespace GomokuConsoleApp
{
    class HumanPlayer : Player
    {
        public HumanPlayer(int number, char sign, ConsoleColor playerColor)
            :base(number, sign, playerColor) { }

        /// <summary>
        /// Метод даёт возможность ввести координаты с клавиатуры.
        /// </summary>
        /// <returns></returns>
        public override string Move()
        {
            Console.ForegroundColor = PlayerColor;
            Console.WriteLine($"Ход игрока №{Number}({Sign})");
            Console.ResetColor();
            Console.Write("Введите координаты выбранной ячейки (пример: a-1): ");
            string coordinate = Console.ReadLine();
            return coordinate;
        }
    }
}
