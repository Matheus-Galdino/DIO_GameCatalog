using System;

namespace APIGamesCatalog.Exceptions
{
    public class GameAlreadyAddedException : ApplicationException
    {
        public GameAlreadyAddedException() : base("Este jogo já foi cadastrado") { }
    }
}
