using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiCatalogoJogos.Exceptions
{
    public class PreviouslyRegistredGame:Exception
    {
        public PreviouslyRegistredGame()
            : base("This game has already been registered")
        { }
    }
}
