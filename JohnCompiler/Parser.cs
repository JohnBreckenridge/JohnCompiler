using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JohnCompiler
{
    public class Parser
    {
        private Scanner scanner;
        private Token returnToken;
        private Token token;
        private Token nextToken;
        public ProgramNode program { get; private set; }
        public Parser(Scanner scanner)
        {
            this.scanner = scanner;
            token = scanner.Next();
            nextToken = scanner.Next();
            this.program = Parse();
        }
        public ProgramNode Parse()
        {
            Match(TokenTypes.ID, "program");
            Match(TokenTypes.ID);
            program = new ProgramNode(returnToken.value);
            Match(TokenTypes.SEMICOLON);
            
            program.Declarations=Declarations();

            SubProgramDeclarations(program.MethodList);
             
            CompoundStatement(program.StatementList);
            Match(TokenTypes.PERIOD);
            return program;
        }
        public DeclarationsNode Declarations()
        {
            var declarations = new DeclarationsNode();
            while (token.value == "var")
            {
                Match(TokenTypes.ID, "var");
                VarTypes type = VarTypes.NONE;
                var tempList = new List<string>();//holds the names of declared variables
                Match(TokenTypes.ID);
                tempList.Add(returnToken.value);
                while (token.value==",")
                {
                    Match(TokenTypes.COMMA);
                    Match(TokenTypes.ID);
                    tempList.Add(returnToken.value);
                }
                Match(TokenTypes.COLON);

                if(token.value=="array")//if its an array
                {
                    Match(TokenTypes.ID, "array");
                    Match(TokenTypes.LBRACKET);
                    Match(TokenTypes.NUM);
                    int startIndex = int.Parse(returnToken.value);
                    Match(TokenTypes.COLON);
                    Match(TokenTypes.NUM);
                    int endIndex = int.Parse(returnToken.value);
                    Match(TokenTypes.RBRACKET);
                    Match(TokenTypes.ID, "of");
                    
                    if (token.value == "integer")
                        type = VarTypes.INT;
                    else if (token.value == "real")
                        type = VarTypes.REAL;
                    Match(TokenTypes.ID);
                    foreach (var name in tempList)
                    {
                        var var = new ArrayVariableNode(name, type, startIndex, endIndex);
                        declarations.variableList.Add(var);
                    }
                }
                else//if its not an array
                {
                    if (token.value == "integer")
                        type = VarTypes.INT;
                    else if (token.value == "real")
                        type = VarTypes.REAL;
                    Match(TokenTypes.ID);
                    foreach (var name in tempList)
                    {
                        var vari = new VariableNode(name, type);
                        declarations.variableList.Add(vari);
                    }
                }
                Match(TokenTypes.SEMICOLON);
            }
            return declarations;
        }
        public void SubProgramDeclarations(List<SubProgramNode> methodList)
        {
            while (token.value == "function" || token.value == "procedure")
            {
                Match(TokenTypes.ID);
                methodList.Add(SubProgram());
            }
        }
        public SubProgramNode SubProgram()
        {
            SubProgramNode subProgramNode = new SubProgramNode();
            Match(TokenTypes.ID);
            subProgramNode.id = returnToken.value;
            if(token.value == "(")
            {
                bool end;
                Match(TokenTypes.LPAR);
                do
                {
                   
                    VarTypes type = VarTypes.NONE;
                    var tempList = new List<string>();//holds the names of declared variables
                    Match(TokenTypes.ID);
                    tempList.Add(returnToken.value);
                    while (token.value == ",")
                    {
                        Match(TokenTypes.COMMA);
                        Match(TokenTypes.ID);
                        tempList.Add(returnToken.value);
                    }
                    Match(TokenTypes.COLON);
                    if (token.value == "array")//if its an array
                    {
                        Match(TokenTypes.ID, "array");
                        Match(TokenTypes.LBRACKET);
                        Match(TokenTypes.NUM);
                        int startIndex = int.Parse(returnToken.value);
                        Match(TokenTypes.COLON);
                        Match(TokenTypes.NUM);
                        int endIndex = int.Parse(returnToken.value);
                        Match(TokenTypes.RBRACKET);
                        Match(TokenTypes.ID, "of");

                        if (token.value == "integer")
                            type = VarTypes.INT;
                        else if (token.value == "real")
                            type = VarTypes.REAL;
                        Match(TokenTypes.ID);
                        foreach (var name in tempList)
                        {
                            var var = new ArrayVariableNode(name, type, startIndex, endIndex);
                            subProgramNode.argumentList.Add(var);
                        }
                    }
                    else//if its not an array
                    {
                        if (token.value == "integer")
                            type = VarTypes.INT;
                        else if (token.value == "real")
                            type = VarTypes.REAL;
                        Match(TokenTypes.ID);
                        foreach (var name in tempList)
                        {
                            var vari = new VariableNode(name, type);
                            subProgramNode.argumentList.Add(vari);
                        }
                    }
                    end = token.value == ")";
                    if (!end)
                        Match(TokenTypes.SEMICOLON);
                } while (!end);
                Match(TokenTypes.RPAR);
                if (token.value == ":")
                {
                    Match(TokenTypes.COLON);
                    Match(TokenTypes.ID);
                    subProgramNode.type = returnToken.value;
                }
                Match(TokenTypes.SEMICOLON);
            }
            subProgramNode.declarations = Declarations();
            SubProgramDeclarations(subProgramNode.methodList);
            CompoundStatement(subProgramNode.statementList);
            return subProgramNode;
        }
        public void CompoundStatement(List<StatementNode> statementList)
        {
            Match(TokenTypes.ID, "begin");
            while (token.value != "end")
            {
                statementList.Add(Statement());
                if (token.value != "end")
                    Match(TokenTypes.SEMICOLON);
            }
            Match(TokenTypes.ID, "end");
            
        }
        public StatementNode Statement()
        {
            StatementNode statementNode = null;
            if (token.value=="if")
            {
                var ifStatementNode = new IfThenStatementNode();
                Match(TokenTypes.ID, "if");
                ifStatementNode.ifExpression = Expression();
                Match(TokenTypes.ID, "then");
                ifStatementNode.thenStatement = Statement();
                Match(TokenTypes.ID, "else");
                ifStatementNode.elseStatement = Statement();
                statementNode = ifStatementNode;
            }
            else if(token.value=="while")
            {
                var whileStatementNode = new WhileStatementNode();
                Match(TokenTypes.ID, "while");
                whileStatementNode.whileExpression = Expression();
                Match(TokenTypes.ID, "do");
                whileStatementNode.doStatement = Statement();
                statementNode = whileStatementNode;
            }
            else if(token.value == "begin")
            {
                var statementList = new List<StatementNode>();
                CompoundStatement(statementList);
            }
            else if (token.value == "read")
            {
                Match(TokenTypes.ID, "read");
                Match(TokenTypes.LPAR);
                Match(TokenTypes.ID);
                var readStatementNode = new ReadStatementNode(returnToken.value);
                Match(TokenTypes.RPAR);
                statementNode = readStatementNode;
            }
            else if (token.value == "write")
            {
                Match(TokenTypes.ID, "write");
                Match(TokenTypes.LPAR);
                var writeStatementNode = new WriteStatementNode();
                writeStatementNode.expression = Expression();
                Match(TokenTypes.RPAR);
                statementNode = writeStatementNode;
            }
            //else if(nextToken.type==TokenTypes.ASSIGNOP)
            else
            {
                Match(TokenTypes.ID);
                string id = returnToken.value;
                bool array = false;
                var index = new ExpressionNode();//for use in array assignops
                var expressionList = new List<ExpressionNode>();//for use in procedure statement
                if (token.value == "[")
                {
                    Match(TokenTypes.LBRACKET);
                    index = Expression();
                    Match(TokenTypes.RBRACKET);
                    array = true;
                }
                else if (token.value == "(")
                {
                    Match(TokenTypes.LPAR);
                    do
                    {
                        expressionList.Add(Expression());
                    } while (token.value == ",");
                    Match(TokenTypes.RPAR);
                }
                if( token.type == TokenTypes.ASSIGNOP)//assignop statement
                {
                    var assignStatementNode = new AssignOpStatementNode();
                    int i = 0;
                    while (program.Declarations.variableList[i].variableName != id)
                    {
                        i++;
                        if (i > program.Declarations.variableList.Count)
                            throw new Exception("");
                    }

                    if(array)
                    {
                        ArrayVariableNode tempArrayVar = program.Declarations.variableList[i] as ArrayVariableNode;
                        if (tempArrayVar == null)
                            throw new Exception("");
                        assignStatementNode.variable = new ArrayVariableNode(id, program.Declarations.variableList[i].type, tempArrayVar.startIndex, tempArrayVar.endIndex);
                        assignStatementNode.arrayExpression = index;
                    }
                    else
                    {
                        assignStatementNode.variable = new VariableNode(id, program.Declarations.variableList[i].type);
                    }
                    Match(TokenTypes.ASSIGNOP);
                    assignStatementNode.expression = Expression();
                    statementNode = assignStatementNode;
                }
                else//procedure statement
                {
                    var procedureStatementNode = new ProcedureStatementNode();
                    procedureStatementNode.id = id;
                    procedureStatementNode.expressionList = expressionList;
                    statementNode = procedureStatementNode;
                }

               
            }

            return statementNode;
        
        }
        public ExpressionNode Expression()
        {
            var expression = new ExpressionNode();
            expression.simpleExpression = SimpleExpression();
            if (token.type == TokenTypes.RELOP)
            {
                expression.relop = token.value;
                Match(TokenTypes.RELOP);
                expression.simpleExpression2 = SimpleExpression();
            }
            return expression;
        }
        public SimpleExpressionNode SimpleExpression()
        {
            var simpleExpression = new SimpleExpressionNode();
            if (token.type == TokenTypes.ADDOP && token.value != "or")
            {
                simpleExpression.sign = token.value;
                Match(TokenTypes.ADDOP);
            }
            simpleExpression.term = Term();
            if (token.type == TokenTypes.ADDOP)
            {
                simpleExpression.part = SimpleExpressionPart();
            }

            return simpleExpression;
        }
        public TermNode Term()
        {
            var term = new TermNode();
            term.factor = Factor();
            if (token.type == TokenTypes.MULOP)
            {
                term.part = TermPart();
            }
            return term;
        }
        public SimpleExpressionPartNode SimpleExpressionPart()
        {
            var simpleExpressionPart = new SimpleExpressionPartNode();
            Match(TokenTypes.ADDOP);
            simpleExpressionPart.addop = returnToken.value;
            simpleExpressionPart.term = Term();
            if(token.type == TokenTypes.ADDOP)
            {
                simpleExpressionPart.part = SimpleExpressionPart();
            }

            return simpleExpressionPart;
        }
        public TermPartNode TermPart()
        {
            var termPart = new TermPartNode();
            Match(TokenTypes.MULOP);
            termPart.mulop = returnToken.value;
            termPart.factor = Factor();
            if (token.type == TokenTypes.MULOP)
            {
                termPart.part = TermPart();
            }

            return termPart;
        }
        public FactorNode Factor()
        {
            var factor = new FactorNode();
            if (token.value == "not")
            {
                factor.not = true;
                Match(TokenTypes.ID, "not");
            }
            if(token.type == TokenTypes.ID)
            {
                factor.value = token.value;
                factor.type = TokenTypes.ID;
                Match(TokenTypes.ID);
                if(token.type == TokenTypes.LPAR)
                {
                    Match(TokenTypes.LPAR);
                    bool end;
                    do
                    {
                        factor.expressionList.Add(Expression());
                        end = token.value == ")";
                        if (!end)
                            Match(TokenTypes.COMMA);
                    } while (!end);
                }
                else if (token.type==TokenTypes.LBRACKET)
                {
                    Match(TokenTypes.LBRACKET);
                    factor.arrayExpression = Expression();
                    Match(TokenTypes.RBRACKET);
                }
            }
            else if (token.type == TokenTypes.NUM)
            {
                factor.value = token.value;
                Match(TokenTypes.NUM);
                factor.type = TokenTypes.NUM;
            }
            else if (token.type == TokenTypes.LPAR)
            {
                Match(TokenTypes.LPAR);
                factor.parExpression = Expression();
                Match(TokenTypes.RPAR);
            }
            
            return factor;
        }

        private void Match(TokenTypes type, string value = null)
        {
            if (type == token.type && (value == null || value == token.value))
            {
                returnToken = token;
                token = nextToken;
                nextToken = scanner.Next();
            }
            else
            {
                throw new Exception("match failed");
            }
        }
    }
}
