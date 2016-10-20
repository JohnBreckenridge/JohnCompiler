using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JohnCompiler
{
    public class Token
    {
        public string value;
        public TokenTypes type;
        public Token()
        {
            value = " ";
            type = TokenTypes.ID;
        }
        public Token(string value, TokenTypes type)
        {
            this.value = value;
            this.type = type;
        }
    }
}
