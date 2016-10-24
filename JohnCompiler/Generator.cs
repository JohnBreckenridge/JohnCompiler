using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JohnCompiler
{
    public class Generator
    {
        private ProgramNode program;
        private StringBuilder code = new StringBuilder();
        //dictionary of variables recording their location in ram??

        public Generator(ProgramNode input)
        {
            program = input;




            code.Append(".assembly extern mscorlib {} \n");
            code.Append(".assembly " + program.id + "{}\n\n");
            code.Append(".method static public void main() il managed\n{\n\n");
            code.Append(".entrypoint\n.maxstack 8\n");

            code.Append(Declarations());
            foreach(StatementNode statement in program.StatementList)
            {
                code.Append(Statement(statement));
            }
            code.Append("ret\n}");
        }
        private string Declarations()
        {
            string output = "";
            output += ".locals init (";
            for(int i=0;i<program.Declarations.variableList.Count();i++)
            {
                output += "[" + i + "]";
                if (program.Declarations.variableList[i].type == VarTypes.INT)
                    output += " int32 ";
                else if(program.Declarations.variableList[i].type == VarTypes.REAL)
                    output += " float32 ";
                output += program.Declarations.variableList[i].variableName;
                output += i != program.Declarations.variableList.Count() - 1 ? ",\n" : ")\n";
            }
            return output;
        }
        private string Statement(StatementNode statement)
        {
            string output = "";
            AssignOpStatementNode node = statement as AssignOpStatementNode;
            if(node!=null)
                output += AssignOpStatement(node);

            return output;
        }
        private string AssignOpStatement(AssignOpStatementNode node)
        {
            string output = "";
            output += Expression(node.expression);
            output += "stloc." + program.Declarations.variableList.FindIndex(x => node.variable.variableName == x.variableName)+"\n";
            return output;
        }
        private string ReadStatement(ReadStatementNode node)
        {
            string output = "";

            return output;
        }
        private string WriteStatement(WriteStatementNode node)
        {
            string output = "";
            output+=Expression(node.expression);
            return output;
        }
        private string IfThenStatement(IfThenStatementNode node)
        {
            string output = "";

            return output;
        }
        private string WhileStatement(WhileStatementNode node)
        {
            string output = "";

            return output;
        }
        private string CompoundStatement(CompoundStatementNode node)
        {
            string output = "";

            return output;
        }
        private string ProcedureStatement(ProcedureStatementNode node)
        {
            string output = "";

            return output;
        }

        private string Expression (ExpressionNode node)
        {
            string output = "";
            output += SimpleExpression(node.simpleExpression);
            if(node.relop!=null)
            {
                output += SimpleExpression(node.simpleExpression2);
                switch(node.relop)
                {
                    case ">":
                        output += "cgt\n";
                        break;
                    case "<":
                        output += "clt\n";
                        break;
                    case "<=":
                        output += "clt\n";
                        output += SimpleExpression(node.simpleExpression);
                        output += SimpleExpression(node.simpleExpression2);
                        output += "ceq\n";
                        output += "or\n";
                        break;
                    case ">=":
                        output += "cgt\n";
                        output += SimpleExpression(node.simpleExpression);
                        output += SimpleExpression(node.simpleExpression2);
                        output += "ceq\n";
                        output += "or\n";
                        break;
                    case "=":
                        output += "ceq\n";
                        break;
                    case "<>":
                        output += "ceq\n";
                        output += "not\n";
                        break;
                }
               

            }
            


            return output;
        }
        private string SimpleExpression (SimpleExpressionNode node)
        {
            string output = "";
            output += Term(node.term);
            if (node.sign == "-")
            {
                output += "lcd.i4.m1\n";
                output += "mul\n";
            }
            if (node.part != null)
                output += SimpleExpressionPart(node.part);
            return output;
        }
        private string Term (TermNode node)
        {
            string output = "";
            output += Factor(node.factor);
            if (node.part != null)
                output += TermPart(node.part);
            return output;
        }
        private string SimpleExpressionPart(SimpleExpressionPartNode node)
        {
            string output = "";
            output += Term(node.term);
            switch (node.addop)
            {
                case "+":
                    output += "add\n";
                    break;
                case "-":
                    output += "sub\n";
                    break;
                case "or":
                    output += "or\n";
                    break;
            }
            if(node.part != null)
                output += SimpleExpressionPart(node.part);
            return output;
        }
        private string TermPart(TermPartNode node)
        {
            string output = "";
            output += Factor(node.factor);
            switch (node.mulop)
            {
                case "*":
                    output += "mul\n";
                    break;
                case "/":
                    output += "div\n";
                    break;
                case "div":
                    output += "div\n";
                    break;
                case "mod":
                    output += "rem\n";
                    break;
                case "and":
                    output += "and\n";
                    break;
            }
            if (node.part != null)
                output += TermPart(node.part);
            return output;
        }
        private string Factor(FactorNode node)
        {
            string output = "";

            //needs to address various froms of NUM
            if(node.type==TokenTypes.ID)
                output += "ldloc." + program.Declarations.variableList.FindIndex(x => node.value == x.variableName) + "\n";
            if (node.type == TokenTypes.NUM)
                output += "ldc.i4." + node.value+"\n";


            if (node.not)
                output += "not\n";
            return output;
        }

        public string Generate()
        {
            return code.ToString();
        }

        public void AddHeader()
        {
            code.Append("blah");
        }

    }
}
