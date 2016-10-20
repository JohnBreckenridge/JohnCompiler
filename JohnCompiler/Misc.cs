using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JohnCompiler
{
    public enum TokenTypes
    {
        ADDOP,//includes "or"
        MULOP,//includes "div, mod, and"
        ID,
        SEMICOLON,
        COLON,
        COMMA,
        ASSIGNOP,
        LBRACKET,
        RBRACKET,
        PERIOD,
        NUM,
        NONE,
        LPAR,
        RPAR,
        RELOP
        
            
    }
    public enum VarTypes
    {
        INT,
        REAL,
        NONE
    }
}
