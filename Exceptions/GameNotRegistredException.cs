using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiCatalogoJogos.Exceptions
{
    public class GameNotRegistredException:Exception
    {
        public GameNotRegistredException()
            : base("This game has not yet been registered")
        { }
    }
}
