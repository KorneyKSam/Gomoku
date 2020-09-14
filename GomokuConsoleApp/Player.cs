

using System;

namespace GomokuConsoleApp
{
    abstract class Player
    {

        protected int Number { get; set; }
        public char Sign { get; set; }
        public ConsoleColor PlayerColor { get; set; }

        /// <summary>
        /// Создание игрока с его номером и знаком.
        /// </summary>
        /// <param name="number"></param>
        /// <param name="sign"></param>
        public Player(int number, char sign, ConsoleColor playerColor)
        {
            Number = number;
            Sign = sign;
            PlayerColor = playerColor;
        }

        /// <summary>
        /// Метод позволяет совершить ход пользователю или боту.
        /// </summary>
        /// <returns></returns>
        public abstract string Move();

    }
}
