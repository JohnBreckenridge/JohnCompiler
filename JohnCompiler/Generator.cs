using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JohnCompiler
{
    public class Generator
    {
        public ProgramNode program;
        public StringBuilder code = new StringBuilder();
        //dictionary of variables recording their location in ram??

        public Generator(ProgramNode input)
        {
            program = input;

        }

        public void AddHeader()
        {
            code.Append("blah");
        }

    }
}
