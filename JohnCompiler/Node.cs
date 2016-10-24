using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JohnCompiler
{
    public abstract class Node
    {
        public abstract string ToString(int level);
    }
    public class ProgramNode : Node
    {
        //declarations node, list of statementnodes
        public string id;
        public DeclarationsNode Declarations;
        public List<StatementNode> StatementList = new List<StatementNode>();
        public List<SubProgramNode> MethodList = new List<SubProgramNode>();

        public ProgramNode(string id)
        {
            this.id = id;
        }
        public override string ToString(int level = 0)
        {
            string output = "";
            output += id+"\n";
            output += Declarations.ToString(level + 1);
            output += "Statements: \n";
            foreach(var statement in StatementList)
                output += statement.ToString(level + 1);
            output += "END";
            return output;
        }
    }
    public  class SubProgramNode : Node
    {
        public string id;
        public string type;
        public DeclarationsNode declarations;
        public List<VariableNode> argumentList = new List<VariableNode>();
        public List<StatementNode> statementList = new List<StatementNode>();
        public List<SubProgramNode> methodList = new List<SubProgramNode>();

        public SubProgramNode()
        {
            type = null;
        }
        public override string ToString(int level)//make tostring
        {
            return null;
        }
    }
    public class DeclarationsNode : Node
    {
        public List<VariableNode> variableList = new List<VariableNode>();
        public DeclarationsNode()
        {
            
        }

        public override string ToString(int level)
        {
            string output = "";
            for (int i = 0; i < level; i++) output += "--";
            output += "VARIABLE LIST:\n";
            foreach (var variable in variableList)
                output += variable.ToString(level + 1);
            return output;
        }
    }
    public class StatementNode : Node
    {
        
        public StatementNode()
        {

        }
        public override string ToString(int level)
        {
            string output = "";
            for (int i = 0; i < level; i++) output += "--";
            return output;
        }
    }
    public class AssignOpStatementNode : StatementNode
    {
        //have them stored as Nodes?
        //need variable (either ID or ID (expressionList))
        //need expression
        public VariableNode variable;
        public ExpressionNode arrayExpression;
        public ExpressionNode expression;

        public AssignOpStatementNode()
        {
            arrayExpression = null;
        }
        public override string ToString(int level)
        {
            string output = "";
            for (int i = 0; i < level; i++) output += "--";
            output += variable.variableName;
            if (arrayExpression != null)
                output += "[" + arrayExpression.ToString(level + 1) + "]";
            output += ":=" + expression.ToString(level + 1) + "\n";
            return output;
        }
    }
    public class IfThenStatementNode : StatementNode
    {
        //if expression
        public ExpressionNode ifExpression;
        //then statement
        public StatementNode thenStatement { get; set; }//do i need these?
        //else statement
        public StatementNode elseStatement { get; set; }

        public IfThenStatementNode()
        {

        }
        public override string ToString(int level)
        {
            string output = "";
            for (int i = 0; i < level; i++) output += "--";
            output += "if " + ifExpression.ToString(level + 1) + "\n";
            output += " then" + thenStatement.ToString(level + 1);
            output += " else" + elseStatement.ToString(level + 1);
            return output;
        }
    }
    public class WhileStatementNode : StatementNode
    {
        //while expression
        public ExpressionNode whileExpression;
        //do statement
        public StatementNode doStatement;

        public WhileStatementNode()
        {

        }
        public override string ToString(int level)
        {
            string output = "";
            for (int i = 0; i < level; i++) output += "--";
            output += "while " + whileExpression.ToString(level + 1) + "\n";
            output += " do" + doStatement.ToString(level + 1);
            return output;
        }
    }
    public class ProcedureStatementNode : StatementNode
    {
        //name of function
        public string id;
        //optional list of expressions for function
        public List<ExpressionNode> expressionList;

        public ProcedureStatementNode()
        {
            
        }
        public override string ToString(int level)
        {
            string output = "";
            for (int i = 0; i < level; i++) output += "--";
            output += id;
            if(expressionList.Count>0)
            {
                output += "(";
                foreach (ExpressionNode node in expressionList){
                        output += node.ToString(0) + ", ";
                }
                output += ")";
            }
            return output;
        }
    }
    public class ReadStatementNode : StatementNode
    {
        //id to read
        public string id;
        public ReadStatementNode(string id)
        {
            this.id = id;
        }
        public override string ToString(int level)
        {
            string output = "";
            for (int i = 0; i < level; i++) output += "--";
            output += "Read(" + id + ")";
            return output;
        }
    }
    public class WriteStatementNode : StatementNode
    {
        //expression to be written
        public ExpressionNode expression;
        public WriteStatementNode()
        {
            
        }
        public override string ToString(int level)
        {
            string output = "";
            for (int i = 0; i < level; i++) output += "--";
            output += "Write(" + expression.ToString(0) + ")";
            return output;
        }
    }
    public class CompoundStatementNode : StatementNode
    {
        //expression to be written
        public List<StatementNode> statementList;
        public CompoundStatementNode()
        {

        }
        public override string ToString(int level)
        {
            string output = "";
            for (int i = 0; i < level; i++) output += "--";
            output += "CompoundStatement \n";
            foreach (StatementNode node in statementList)
                output += node.ToString(level + 1)+"\n";
            return output;
        }
    }
    public class ExpressionNode : Node
    {
        public SimpleExpressionNode simpleExpression;
        public string relop;
        public SimpleExpressionNode simpleExpression2;
        public ExpressionNode()
        {
            relop = null;
        }

        public override string ToString(int level)// TO DO
        {
            string output = "";
            //for (int i = 0; i < level; i++) output += "--";
            output += simpleExpression.ToString(level + 1);
            if(relop != null)
            {
                output += relop + simpleExpression2.ToString(level + 1);
            }
            return output;
        }
    }
    public class SimpleExpressionNode : Node
    {
        public string sign;
        public TermNode term;
        public SimpleExpressionPartNode part;
        public SimpleExpressionNode()
        {
            part = null;
            sign = "";
        }

        public override string ToString(int level)// TO DO
        {
            string output = "";
            //for (int i = 0; i < level; i++) output += "--";
            if (sign != "")
                output += sign;
            output += term.ToString(level + 1);
            if (part != null)
                output += part.ToString(level+1);
            return output;
        }
    }
    public class SimpleExpressionPartNode : Node//this could instead just  be a list of addops and terms???
    {
        public string addop;
        public TermNode term;
        public SimpleExpressionPartNode part;

        public SimpleExpressionPartNode()
        {
            part = null;
        }

        public override string ToString(int level) // TO DO
        {
            string output = "";
            //for (int i = 0; i < level; i++) output += "--";
            output += addop;
            output += term.ToString(level+1);
            if (part != null)
                output += part.ToString(level+1);
            return output;
        }
    }
    public class TermNode : Node
    {
        public FactorNode factor;
        public TermPartNode part;

        public TermNode()
        {
            part = null;
        }

        public override string ToString(int level) // TO DO
        {
            string output = "";
            //for (int i = 0; i < level; i++) output += "--";
            output += factor.ToString(level + 1);
            if (part != null)
                output += part.ToString(level + 1);
            return output;
        }
    }
    public class TermPartNode : Node
    {
        public string mulop;
        public FactorNode factor;
        public TermPartNode part;

        public TermPartNode()
        {
            part = null;
        }

        public override string ToString(int level) // TO DO
        {
            string output = "";
            //for (int i = 0; i < level; i++) output += "--";
            output += mulop;
            output += factor.ToString(level + 1);
            if (part != null)
                output += part.ToString(level + 1);
            return output;
        }
    }
    public class FactorNode : Node
    {
        public string value;//for ID or NUM

        public ExpressionNode arrayExpression;
        public List<ExpressionNode> expressionList;
        public ExpressionNode parExpression;
        public bool not = false;
        public TokenTypes type;

        public FactorNode()
        {
            arrayExpression = null;
            expressionList = null;
            parExpression = null;
        }

        public override string ToString(int level) // TO DO
        {
            string output = "";
            //for (int i = 0; i < level; i++) output += "--";
            if (not)
                output += "Not ";
            if (parExpression != null)
                output += "(" + parExpression.ToString(level + 1) + ")";
            else
            {
                output += value;
                if (arrayExpression != null)
                    output += "[" + arrayExpression.ToString(level + 1) + "]";
                else if (expressionList != null)
                {
                    output += "(";
                    foreach (ExpressionNode node in expressionList)
                        output += node.ToString(level + 1) + ", ";
                    output += ")";
                }
                    
            }
            return output;
        }
    }
    public class VariableNode : Node
    {
        public string variableName;
        public VarTypes type;
        public VariableNode(string name, VarTypes type)
        {
            variableName = name;
            this.type = type;
        }
        public override string ToString(int level)
        {
            string output = "";
            for (int i = 0; i < level; i++) output += "--";
            output += variableName;
            if (type == VarTypes.INT)
                output += ": INT" + "\n";
            else if (type == VarTypes.REAL)
                output += ": REAL" + "\n";
            return output;
        }
    }
    public class ArrayVariableNode : VariableNode
    {
        public int startIndex;
        public int endIndex;
        public ArrayVariableNode(string name, VarTypes type, int start, int end) : base(name, type)
        {
            variableName = name;
            this.type = type;
            startIndex = start;
            endIndex = end;
        }

        public override string ToString(int level)
        {
            string output = "";
            for (int i = 0; i < level; i++) output += "--";
            output += variableName;
            output += "[" + startIndex + ":" + endIndex + "]";
            if (type == VarTypes.INT)
                output += ": INT" + "\n";
            else if (type == VarTypes.REAL)
                output += ": REAL" + "\n";
            return output;
        }
    }
}
