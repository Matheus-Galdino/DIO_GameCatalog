using System;

namespace APIGamesCatalog.Exceptions
{
    public class GameNotAddedException : ApplicationException
    {
        public GameNotAddedException() : base("Este jogo não está cadastrado") { }
    }
}
