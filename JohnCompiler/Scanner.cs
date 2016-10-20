using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JohnCompiler
{
    public class Scanner
    {
        private string file;
        private int position = 0;

        public Scanner(string file)
        {
            this.file = file;
        }
        public Token Next()
        {
            int state = 0;//will determine what type of token is passed
            int check = 0;//will check for lookuptable returning a 1 or 99 to indicate being done
            int length = 0;//will record length of word
            char nextChar;
            do
            {
                state = check;
                nextChar = file[position + length];
                check = LookupTable[state][nextChar];
                length++;
            } while (check != 1 && check != 99);
            string word = file.Substring(position, length-1);
            position += length;

            //convert state into a tokentype
            TokenTypes typeState;
            
           
            if (state == 81 || word == "or")
                typeState = TokenTypes.ADDOP;
            else if (state == 82 || word == "div" || word == "mod" || word == "and")
                typeState = TokenTypes.MULOP;
            else if (state == 20)
                typeState = TokenTypes.ID;
            else if (state == 10 || state == 13 || state == 14)
                typeState = TokenTypes.NUM;
            else if (state == 31)
                typeState = TokenTypes.LBRACKET;
            else if (state == 32)
                typeState = TokenTypes.RBRACKET;
            else if (state == 33)
                typeState = TokenTypes.LPAR;
            else if (state == 34)
                typeState = TokenTypes.RPAR;
            else if (state >= 70 && state <= 72)
                typeState = TokenTypes.RELOP;
            else if (state == 84)
                typeState = TokenTypes.COLON;
            else if (state == 85)
                typeState = TokenTypes.ASSIGNOP;
            else if (state == 86)
                typeState = TokenTypes.SEMICOLON;
            else if (state == 87)
                typeState = TokenTypes.COMMA;
            else if (state == 88)
                typeState = TokenTypes.PERIOD;

            else
                typeState = TokenTypes.NONE;
            Token token = new Token(word, typeState);
            return token;
        }
        //INPUT REQUIREMENTS
        //single spaces between every token including between end of line and semicolon
        //don't need spaces inside of token (4.5 or 4E-12)
        //watch out for end of line characters? don't want them anywhere except after semicolon? don't want multiple endline characters. make sure there isn't both space and end line character
        //some kind of end character? period at end with space before and after??????
        //lowercase??
        // strip out comments {}
        //  PARSE REQUIREMENTS
        // look for specific keywords in idTokens




        private static int[] INITIAL_STATE = new int[128] //0
        {
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//0-9
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//10-19
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//20-29
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//30-39
            33,34,82,81,87,81,88,82,10,10,//40-49
            10,10,10,10,10,10,10,10,84,86,//50-59
            71,70,72,1 ,1 ,20,20,20,20,20,//60-69
            20,20,20,20,20,20,20,20,20,20,//70-79
            20,20,20,20,20,20,20,20,20,20,//80-89
            20,31,1 ,32,1 ,1 ,1 ,20,20,20,//90-99
            20,20,20,20,20,20,20,20,20,20,//100-109
            20,20,20,20,20,20,20,20,20,20,//110-119
            20,20,20,1 ,1 ,1 ,1 ,1        //120-127
        };
        private static int[] NUM_STATE = new int[128]//10
        {
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//0-9
            99,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//10-19
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//20-29
            1 ,1 ,99,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//30-39
            1 ,1 ,1 ,1 ,1 ,1 ,14,1 ,10,10,//40-49
            10,10,10,10,10,10,10,10,1 ,1 ,//50-59
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,11,//60-69
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//70-79
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//80-89
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//90-99
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//100-109
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//110-119
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1        //120-127
        };
        private static int[] EXPONENT_STATE = new int[128]//11
        {//numbers followed by capital E. followed by single + or - (optional) then more digits
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//0-9
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//10-19
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//20-29
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//30-39
            1 ,1 ,1 ,12,1 ,12,1 ,1 ,13,13,//40-49
            13,13,13,13,13,13,13,13,1 ,1 ,//50-59
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//60-69
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//70-79
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//80-89
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//90-99
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//100-109
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//110-119
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1        //120-127
        };
        private static int[] EXPONENTSIGN_STATE = new int[128]//12
        {//numbers followed by capital E followed by single + or -. will be followed by more digits
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//0-9
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//10-19
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//20-29
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//30-39
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,13,13,//40-49
            13,13,13,13,13,13,13,13,1 ,1 ,//50-59
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//60-69
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//70-79
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//80-89
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//90-99
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//100-109
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//110-119
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1        //120-127
        };
        private static int[] EXPONENTNUM_STATE = new int[128]//13
        {//numbers followed by capital E followed by single + or - and at least one digit. can have more or be doen
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//0-9
            99,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//10-19
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//20-29
            1 ,1 ,99,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//30-39
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,13,13,//40-49
            13,13,13,13,13,13,13,13,1 ,1 ,//50-59
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//60-69
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//70-79
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//80-89
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//90-99
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//100-109
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//110-119
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1        //120-127
        };
        private static int[] NUMDOT_STATE = new int[128]//14
        {//same thing as num state but a period has appeared so any more are error
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//0-9
            99,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//10-19
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//20-29
            1 ,1 ,99,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//30-39
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,10,10,//40-49
            10,10,10,10,10,10,10,10,1 ,1 ,//50-59
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,11,//60-69
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//70-79
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//80-89
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//90-99
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//100-109
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//110-119
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1        //120-127
        };
        private static int[] LETTER_STATE = new int[128]//20
        {
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//0-9
            99,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//10-19
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//20-29
            1 ,1 ,99,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//30-39
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,20,//40-49
            20,20,20,20,20,20,20,20,20,1 ,//50-59
            1 ,1 ,1 ,1 ,1 ,20,20,20,20,20,//60-69
            20,20,20,20,20,20,20,20,20,20,//70-79
            20,20,20,20,20,20,20,20,20,20,//80-89
            20,1 ,1 ,1 ,1 ,1 ,1 ,20,20,20,//90-99
            20,20,20,20,20,20,20,20,20,20,//100-109
            20,20,20,20,20,20,20,20,20,20,//110-119
            20,20,20,1 ,1 ,1 ,1 ,1        //120-127
        };
        private static int[] LEFTBRACE_STATE = new int[128]//31
        {//a left brace "[". must be followed by space
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//0-9
            99,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//10-19
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//20-29
            1 ,1 ,99,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//30-39
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//40-49
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//50-59
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//60-69
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//70-79
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//80-89
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//90-99
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//100-109
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//110-119
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1        //120-127
        };
        private static int[] RIGHTBRACE_STATE = new int[128]//32
        {//a right brace "]". must be followed by space
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//0-9
            99,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//10-19
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//20-29
            1 ,1 ,99,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//30-39
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//40-49
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//50-59
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//60-69
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//70-79
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//80-89
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//90-99
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//100-109
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//110-119
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1        //120-127
        };
        private static int[] LEFTPAR_STATE = new int[128]//33
        {//a left parenthesis "(". must be followed by space
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//0-9
            99,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//10-19
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//20-29
            1 ,1 ,99,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//30-39
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//40-49
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//50-59
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//60-69
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//70-79
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//80-89
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//90-99
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//100-109
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//110-119
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1        //120-127
        };
        private static int[] RIGHTPAR_STATE = new int[128]//34
        {//a right parenthesis ")". must be followed by space
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//0-9
            99,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//10-19
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//20-29
            1 ,1 ,99,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//30-39
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//40-49
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//50-59
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//60-69
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//70-79
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//80-89
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//90-99
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//100-109
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//110-119
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1        //120-127
        };
        private static int[] DONERELOP_STATE = new int[128]//70
        {//a relop that is compelte (=, <>, <=, >=, <, >)
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//0-9
            99,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//10-19
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//20-29
            1 ,1 ,99,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//30-39
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//40-49
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//50-59
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//60-69
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//70-79
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//80-89
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//90-99
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//100-109
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//110-119
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1        //120-127
        };
        private static int[] LEFTCHEV_STATE = new int[128]//71
        {// is either done, or followed by right chevron or equal sign
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//0-9
            99,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//10-19
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//20-29
            1 ,1 ,99,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//30-39
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//40-49
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//50-59
            1 ,70,70,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//60-69
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//70-79
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//80-89
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//90-99
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//100-109
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//110-119
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1        //120-127
        };
        private static int[] RIGHTCHEV_STATE = new int[128]//72
        {//is either done or will be followed by an equal sign
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//0-9
            99,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//10-19
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//20-29
            1 ,1 ,99,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//30-39
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//40-49
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//50-59
            1 ,70,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//60-69
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//70-79
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//80-89
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//90-99
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//100-109
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//110-119
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1        //120-127
        };
        private static int[] ADDOP_STATE = new int[128]//81
        {//consists of single +,-.must be followed by space. "or" handled in word check above
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//0-9
            99,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//10-19
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//20-29
            1 ,1 ,99,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//30-39
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//40-49
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//50-59
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//60-69
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//70-79
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//80-89
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//90-99
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//100-109
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//110-119
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1        //120-127
        };
        private static int[] MULOP_STATE = new int[128]//82
        {//consists of single *,/. must be followed by space. "div, mod, and" handled above
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//0-9
            99,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//10-19
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//20-29
            1 ,1 ,99,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//30-39
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//40-49
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//50-59
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//60-69
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//70-79
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//80-89
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//90-99
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//100-109
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//110-119
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1        //120-127
        };
        private static int[] COLON_STATE = new int[128]//84
        {//colon can be followed by equalsign for assignop //can also be followed by other things. 
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//0-9
            99,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//10-19
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//20-29
            1 ,1 ,99,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//30-39
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//40-49
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//50-59
            1 ,85,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//60-69
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//70-79
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//80-89
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//90-99
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//100-109
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//110-119
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1        //120-127
        };
        private static int[] ASSIGNOP_STATE = new int[128]//85
        {//a colon followed by an equal sign will always be assignop regardless of what follows
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//0-9
            99,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//10-19
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//20-29
            1 ,1 ,99,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//30-39
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//40-49
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//50-59
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//60-69
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//70-79
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//80-89
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//90-99
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//100-109
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//110-119
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1        //120-127
        };
        private static int[] SEMICOLON_STATE = new int[128]//86
        {//a semicolon must be followed by a space
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//0-9
            99,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//10-19
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//20-29
            1 ,1 ,99,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//30-39
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//40-49
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//50-59
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//60-69
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//70-79
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//80-89
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//90-99
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//100-109
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//110-119
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1        //120-127
        };
        private static int[] COMMA_STATE = new int[128]//87
        {//a COMMA must be followed by a space
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//0-9
            99,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//10-19
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//20-29
            1 ,1 ,99,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//30-39
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//40-49
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//50-59
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//60-69
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//70-79
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//80-89
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//90-99
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//100-109
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//110-119
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1        //120-127
        };
        private static int[] PERIOD_STATE = new int[128]//88
        {//a PERIOD must be followed by a space
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//0-9
            99,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//10-19
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//20-29
            1 ,1 ,99,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//30-39
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//40-49
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//50-59
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//60-69
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//70-79
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//80-89
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//90-99
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//100-109
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,//110-119
            1 ,1 ,1 ,1 ,1 ,1 ,1 ,1        //120-127
        };

        private static int[][] LookupTable = new int[100][]
        {
            INITIAL_STATE, null, null, null, null, null, null, null, null, null,
            NUM_STATE, EXPONENT_STATE, EXPONENTSIGN_STATE, EXPONENTNUM_STATE, NUMDOT_STATE, null, null, null, null, null,
            LETTER_STATE, null, null, null, null, null, null, null, null, null,
            null, LEFTBRACE_STATE, RIGHTBRACE_STATE, LEFTPAR_STATE, RIGHTPAR_STATE, null, null, null, null, null,
            null, null, null, null, null, null, null, null, null, null,
            null, null, null, null, null, null, null, null, null, null,
            null, null, null, null, null, null, null, null, null, null,
            DONERELOP_STATE, LEFTCHEV_STATE,  RIGHTCHEV_STATE, null, null, null, null, null, null, null,
            null, ADDOP_STATE, MULOP_STATE, null, COLON_STATE , ASSIGNOP_STATE, SEMICOLON_STATE, COMMA_STATE, PERIOD_STATE, null,
            null, null, null, null, null, null, null, null, null, null,
        };
    }
}
