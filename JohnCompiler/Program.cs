using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JohnCompiler
{
    class Program
    {
        static void Main(string[] args)
        {
            string file = File.ReadAllText(args[0]);
            file = Sanitize(file);

            Scanner scanner = new Scanner(file);
            Parser parser = new Parser(scanner);
            Console.Write(parser.program.ToString());
            Generator generator = new Generator(parser.program);
            string output = generator.Generate();
            File.WriteAllText(args[1], output);
            //parser.program is final output in form of a ProgramNode
        }
        public static string Sanitize(string input)
        {
            input = input.Replace('\r', '\n');//turn all carriage return into new line
            for (int i = 0; i < input.Length; i++)//remove comments
            {
                if(input[i]=='{')
                {
                    int j = 0;
                    do
                    {
                        j++;
                    } while (input[i + j] != '}');
                    input.Remove(i, j);
                }
            }
            for (int i = 0; i < input.Length - 1; i++)//remove duplicate spaces/newlines
            {
                if ((input[i] == 32 || input[i] == '\n') && (input[i + 1] == 32 || input[i + 1] == '\n')) 
                {
                    input = input.Remove(i, 1);
                    i--;
                }
            }
            for (int i = 0; i < input.Length - 1; i++)//puts spaces inbetween semicolon and whatever comes before
            {
                if (input[i] == ';' && (input[i - 1] != ' ' && input[i - 1] != '\n'))
                    input = input.Insert(i," ");
            }
            int end = input.LastIndexOf('.');
            input = input.Insert(end + 1, " [ [ [");//put meaningless tokens at the end of the file so when Parser trys to get nextToken it doesn't fail.
            return input;
        }
        //INPUT REQUIREMENTS
        //single spaces between every token including between end of line and semicolon and commas
        //watch out for end of line characters? don't want them anywhere except after semicolon?
        //some kind of end character? period at end with space before and after??????
        //lowercase??
        // strip out comments {}
        //ENDLINE IS TREATED AS A SPACE
        //NO TABS ALLOWED
        //ONLY PUT SEMICOLONS TO SEPERATE STATEMENTS INBETWEEN "BEGIN" AND "END". DON'T PUT ONE AFTER LAST STATEMENT OR FOR OTHER TYPES OF STATEMENTS
    }
}
