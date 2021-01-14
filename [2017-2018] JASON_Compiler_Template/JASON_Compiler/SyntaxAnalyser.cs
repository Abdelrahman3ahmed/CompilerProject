using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JASON_Compiler
{

    public class Node
    {
        public List<Node> children = new List<Node>();
        public string Name;
        public Node(string Name)
        {
            this.Name = Name;
        }
    }

    class SyntaxAnalyser
    {
        int InputPointer = 0;
        List<Token> TokenStream;
        public Node root;
        public TreeNode Root;
        public SyntaxAnalyser() 
        {
            TokenStream = Tiny_Compiler.Jason_Scanner.Tokens;
            root = new Node("Program");
        }
        public void AssignTreeNode()
        {
            root = Parse();
            Root = PrintParseTree(root);
        }
        public void Match()
        {
            InputPointer++;
            if (TokenStream[InputPointer].token_type == Token_Class.Semicolon)
                InputPointer++;
        }
        public void PrintPointer()
        {
            Console.WriteLine("Input Pointer : " + InputPointer);
        }
        public Node Parse()
        {
            root.children.Add(FNST());
            root.children.Add(MAFC());
            return root;
        }
        private   Node MAFC()//30
        {
            Node node = new Node("Main Function");
            if (TokenStream[InputPointer].token_type == Token_Class.Int || TokenStream[InputPointer].token_type == Token_Class.String || TokenStream[InputPointer].token_type == Token_Class.Float) 
              //Float Token added
            {
                node.children.Add(DT());
            }
            if (TokenStream[InputPointer].token_type == Token_Class.main)
            {
                node.children.Add(new Node("main"));
                Match();
            }
            if (TokenStream[InputPointer].token_type == Token_Class.LParanthesis)
            {
                Node temp = new Node(TokenStream[InputPointer].lex);
                node.children.Add(temp);
                Match();
            }
            if (TokenStream[InputPointer].token_type == Token_Class.RParanthesis)
            {
                Node temp = new Node(TokenStream[InputPointer].lex);
                node.children.Add(temp);
                Match();
            }
            node.children.Add(FNBO());
            return node;
        }
        private   Node RTST()
        {
            Node node = new Node("Return_Statement");
            if (TokenStream[InputPointer].token_type == Token_Class.Return)
            {
                node.children.Add(new Node(TokenStream[InputPointer].lex));
                Match();
            }
            node.children.Add(EXP());
            return node;
        }
        bool equationTerminator = false;
        private Node EXP()
        {
            Node node = new Node("Expression");
            if (TokenStream[InputPointer].ToString().Equals("\""))
            {
                node.children.Add(STR());
            }
            else
            {
                int xPointer = InputPointer;
                Node temp = EQ();
                if(equationTerminator)
                {
                    node.children.Add(temp);
                }
                else
                {
                    InputPointer = xPointer;
                    node.children.Add(T());
                }
            }
            return node;
        }

        private Node EQ()
        {
            Node node = new Node("Equation");
            
            if (TokenStream[InputPointer].token_type == Token_Class.LParanthesis)
            {
                node.children.Add(new Node(TokenStream[InputPointer].lex));
                Match();
            }
            node.children.Add(T());
            Node temp = AT();
            if(temp != null)
            {
                node.children.Add(temp);
            }
            if (TokenStream[InputPointer].token_type == Token_Class.RParanthesis)
            {
                node.children.Add(new Node(TokenStream[InputPointer].lex));
                Match();
                Node temp1 = AT();
                if (temp1 != null)
                {
                    node.children.Add(temp1);
                }
            }
            return node;
        }

        private Node AT()
        {
            Node node = new Node("Arithmatic OP Term");
            if (TokenStream[InputPointer].token_type == Token_Class.PlusOp || TokenStream[InputPointer].token_type == Token_Class.MinusOp ||
                TokenStream[InputPointer].token_type == Token_Class.MultiplyOp || TokenStream[InputPointer].token_type == Token_Class.DivideOp)//AO
            {
                equationTerminator = true;
                node.children.Add(AO());
                node.children.Add(EQ());
            }
            else
            {
                return null;
            }
            if (TokenStream[InputPointer].token_type == Token_Class.PlusOp || TokenStream[InputPointer].token_type == Token_Class.MinusOp ||
                TokenStream[InputPointer].token_type == Token_Class.MultiplyOp || TokenStream[InputPointer].token_type == Token_Class.DivideOp)
            {
                equationTerminator = true;
                AT();
            }
            return node;
        }

        private Node AO()
        {
            Node node = new Node("Arithmatic Operation");
            if (TokenStream[InputPointer].token_type == Token_Class.PlusOp || TokenStream[InputPointer].token_type == Token_Class.MinusOp ||
                TokenStream[InputPointer].token_type == Token_Class.MultiplyOp || TokenStream[InputPointer].token_type == Token_Class.DivideOp)
            {
                node.children.Add(new Node(TokenStream[InputPointer].lex));
                Match();
            }
            return node;
        }

        private Node T()
        {
            Node node = new Node("Term");
            
            if (TokenStream[InputPointer].token_type == Token_Class.Call)
            {
                node.children.Add(FC());
            }
            else if (TokenStream[InputPointer].token_type == Token_Class.Idenifier)
            {
                node.children.Add(Id());
            }
            else if (TokenStream[InputPointer].token_type == Token_Class.Constant)
            {
                node.children.Add(N());
            }
            return node;
        }
        private Node FC()
        {
            Node node = new Node("Function_Call");
            node.children.Add(Id());
            if (TokenStream[InputPointer].token_type == Token_Class.LParanthesis)
            {
                node.children.Add(new Node(TokenStream[InputPointer].lex));
                Match();
            }
            Node temp = FC1();
            if(temp!=null)
            {
                node.children.Add(FC1());
            }
            
            if (TokenStream[InputPointer].token_type == Token_Class.RParanthesis)
            {
                node.children.Add(new Node(TokenStream[InputPointer].lex));
                Match();
            }
            return node;
        }
        private Node FC1()
        {
            Node node = new Node("Call");
            if (TokenStream[InputPointer].token_type == Token_Class.Idenifier)
            {
                node.children.Add(Id());
            }
            if (TokenStream[InputPointer].token_type == Token_Class.Comma)
            {
                node.children.Add(new Node(TokenStream[InputPointer].lex));
                Match();
                node.children.Add(Id());
            }
            else
            {
                return null;
            } 
            if (TokenStream[InputPointer].token_type == Token_Class.Comma)
            {
                node.children.Add(FC1());
            }
            return node;
        }

        private Node N()
        {
            Node node = new Node("Number");
            Node temp = new Node(TokenStream[InputPointer].lex);
            node.children.Add(temp);
            Match();
            return node;
        }

        private Node STR()
        {
            Node node = new Node("String");
            if (TokenStream[InputPointer].lex.Equals("\""))
            {
                Node node1 = new Node("\"");
                node.children.Add(node1);
                Match();
            }
            if (TokenStream[InputPointer].token_type == Token_Class.String)
            {
                node.children.Add(new Node(TokenStream[InputPointer].lex));
                Match();
            }
            if (TokenStream[InputPointer].ToString().Equals("\""))
            {
                Node node1 = new Node("\"");
                node.children.Add(node1);
                Match();
            }
            return node;
        }
        private Node FNBO()
        {
            Node node = new Node("Function_Body");
            if (TokenStream[InputPointer].token_type == Token_Class.LeftBraces)
            {
                node.children.Add(new Node(TokenStream[InputPointer].lex));
                Match();
            }
            else
            {
                CheckForErrorList(TokenStream[InputPointer].token_type, Token_Class.LeftBraces);
            }
            node.children.Add(FNBO1());
            if (TokenStream[InputPointer].token_type == Token_Class.Return)
            {
                node.children.Add(RTST());
            }
            else
            {
                CheckForErrorList(TokenStream[InputPointer].token_type, Token_Class.Return);
            }
            Console.WriteLine("IP: " + TokenStream[InputPointer].lex);
            if (TokenStream[InputPointer].token_type == Token_Class.RightBraces)
            {
                node.children.Add(new Node(TokenStream[InputPointer].lex));
            }
            else
            {
                CheckForErrorList(TokenStream[InputPointer].token_type, Token_Class.RightBraces);
            }
            return node;
        }     
        private  Node FNBO1()
        {
            Node node = new Node("Statements");
            if (TokenStream[InputPointer].token_type == Token_Class.Int ||
                TokenStream[InputPointer].token_type == Token_Class.String ||
              TokenStream[InputPointer].token_type == Token_Class.Float)
            {
                node.children.Add(DS());
            }
            if (TokenStream[InputPointer].token_type == Token_Class.Write)
            {
                node.children.Add(WRST());
            }
            if (TokenStream[InputPointer].token_type == Token_Class.Read)
            {
                node.children.Add(RST());
            }
            if (TokenStream[InputPointer].token_type == Token_Class.Idenifier)
            {
                int xPointer = InputPointer;
                Match();
                if (TokenStream[InputPointer].token_type == Token_Class.assigment)
                {
                    InputPointer = xPointer;
                    node.children.Add(ASS());
                }
                else
                {
                    InputPointer = xPointer;
                    node.children.Add(COST());
                }
            }
            if (TokenStream[InputPointer].token_type == Token_Class.If)
            {
                node.children.Add(IF());
            }
            Console.WriteLine("IP: " + TokenStream[InputPointer].lex);
            
            if (TokenStream[InputPointer].token_type == Token_Class.comment)
            {
                node.children.Add(COMM());
            }
            if (TokenStream[InputPointer].token_type == Token_Class.RePEat)
            {
                node.children.Add(REPST());
            }
            if (TokenStream[InputPointer].token_type == Token_Class.Call)
            {
                node.children.Add(FNST());
            }
            return node;
        }

        private Node FNST()
        {
            Node node = new Node("Function_Statement");
            node.children.Add(FND());
            node.children.Add(FNBO());
            return node;
        }
        private Node FND()
        {
            Node node = new Node("Function_Declaration");
            node.children.Add(DT());
            node.children.Add(FNN());
            if (TokenStream[InputPointer].token_type == Token_Class.LParanthesis)
            {
                node.children.Add(new Node(TokenStream[InputPointer].lex));
                Match();
            }
            else
            {
                CheckForErrorList(TokenStream[InputPointer].token_type, Token_Class.LParanthesis);
            }
            Node temp = FND1();
            if(temp != null)
            {
                node.children.Add(temp);
            }
            if (TokenStream[InputPointer].token_type == Token_Class.RParanthesis)
            {
                node.children.Add(new Node(TokenStream[InputPointer].lex));
                Match();
            }
            else
            {
                CheckForErrorList(TokenStream[InputPointer].token_type, Token_Class.RParanthesis);
            }
            return node;
        }

        private Node FND1()
        {
            Node node = new Node("Function_Declaration_Statement");
            if (TokenStream[InputPointer].token_type == Token_Class.Integer || TokenStream[InputPointer].token_type == Token_Class.String ||
                TokenStream[InputPointer].token_type == Token_Class.Float)
            {
                node.children.Add(PRA());
            }
            if (TokenStream[InputPointer].token_type == Token_Class.Comma)
            {
                node.children.Add(new Node(","));
                Match();
                node.children.Add(PRA());
            }
            else
            {
                return null;
            }
            if (TokenStream[InputPointer].token_type == Token_Class.Comma)
            {
                FND1();
            }
            return node;
        }
        private Node PRA()
        {
            Node node = new Node("Parameter");
            node.children.Add(DT());
            node.children.Add(Id());
            return node;
        }

        private Node FNN()
        {
            Node node = new Node("Function_Name");
            node.children.Add(Id());
            return node;
        }

        private Node REPST()
        {
            Node node = new Node("Repeat");
            if (TokenStream[InputPointer].token_type == Token_Class.RePEat)
            {
                node.children.Add(new Node(TokenStream[InputPointer].lex));
                Match();
            }
            node.children.Add(REPST1());
            if (TokenStream[InputPointer].token_type == Token_Class.Until)
            {
                node.children.Add(new Node(TokenStream[InputPointer].lex));
                Match();
            }
            node.children.Add(CON());
            return node;
        }

        private Node REPST1()//24
        {

            Node node = new Node("Repeat_Statement");
            if (TokenStream[InputPointer].token_type == Token_Class.Integer || TokenStream[InputPointer].token_type == Token_Class.String ||
                TokenStream[InputPointer].token_type == Token_Class.Float)
            {
                node.children.Add(DS());
            }
            if (TokenStream[InputPointer].token_type == Token_Class.Write)
            {
                node.children.Add(WRST());
            }
            if (TokenStream[InputPointer].token_type == Token_Class.Read)
            {
                node.children.Add(RST());
            }
            if (TokenStream[InputPointer].token_type == Token_Class.Idenifier)
            {
                int xPointer = InputPointer;
                Match();
                if (TokenStream[InputPointer].token_type == Token_Class.assigment)
                {
                    InputPointer = xPointer;
                    node.children.Add(ASS());
                }
                else
                {
                    InputPointer = xPointer;
                    node.children.Add(COST());
                }
            }
            if (TokenStream[InputPointer].token_type != Token_Class.Until)
            {
                node.children.Add(REPST1());
            }
            return node;

        }


        private Node COMM()
        {
            Node node = new Node("COMM");
            if (TokenStream[InputPointer].token_type == Token_Class.comment)
            {
                Node node1 = new Node(TokenStream[InputPointer].lex);
                node.children.Add(node1);
                Match();
            }
            return node;
        }

        private Node IF() //CFG need some edit
        {
            Node node = new Node("IF");
            if (TokenStream[InputPointer].token_type == Token_Class.If)
            {
                Node node1 = new Node(TokenStream[InputPointer].lex);
                node.children.Add(node1);
                Match();
            }
            node.children.Add(CON());
            if (TokenStream[InputPointer].token_type == Token_Class.Then)
            {
                Node node1 = new Node(TokenStream[InputPointer].lex);
                node.children.Add(node1);
                Match();
            }
            node.children.Add(IF1());
            if (TokenStream[InputPointer].token_type == Token_Class.elseif)
            {
                node.children.Add(ELIFST());
            }
            else if (TokenStream[InputPointer].token_type == Token_Class.Else)
            {
                node.children.Add(ELSEST());
            }
            else if (TokenStream[InputPointer].token_type == Token_Class.End)
            {
                Node node1 = new Node(TokenStream[InputPointer].lex);
                node.children.Add(node1);
                Match();
            }
            return node;
        }

        private Node IF1()
        {
            Node node = new Node("IF_Statements");
            if (TokenStream[InputPointer].token_type == Token_Class.Integer || TokenStream[InputPointer].token_type == Token_Class.String ||
                TokenStream[InputPointer].token_type == Token_Class.Float)
            {
                node.children.Add(DS());
            }
            if (TokenStream[InputPointer].token_type == Token_Class.Write)
            {
                node.children.Add(WRST());
            }
            if (TokenStream[InputPointer].token_type == Token_Class.Read)
            {
                node.children.Add(RST());
            }
            if (TokenStream[InputPointer].token_type == Token_Class.Idenifier)
            {
                int xPointer = InputPointer;
                Match();
                if (TokenStream[InputPointer].token_type == Token_Class.assigment)
                {
                    InputPointer = xPointer;
                    node.children.Add(ASS());
                }
                else
                {
                    InputPointer = xPointer;
                    node.children.Add(COST());
                }
            }
            Console.WriteLine("IP: " + TokenStream[InputPointer].lex);
            if (TokenStream[InputPointer].token_type != Token_Class.elseif && TokenStream[InputPointer].token_type != Token_Class.Else && TokenStream[InputPointer].token_type != Token_Class.End)
            {
                node.children.Add(IF1());
            }

            return node;
        }


        private Node ELIFST() //22
        {
            Node node = new Node("Else_IF_Statement");
            if (TokenStream[InputPointer].token_type == Token_Class.elseif)
            {
                Node node1 = new Node(TokenStream[InputPointer].lex);
                node.children.Add(node1);
                Match();
            }
            node.children.Add(CON());
            if (TokenStream[InputPointer].token_type == Token_Class.Then)
            {
                Node node1 = new Node(TokenStream[InputPointer].lex);
                node.children.Add(node1);
                Match();
            }
            node.children.Add(ELIFST1()); //statements of else if
            if (TokenStream[InputPointer].token_type == Token_Class.elseif)
            {
                Node node1 = new Node(TokenStream[InputPointer].lex);
                node.children.Add(node1);
                Match();
                node.children.Add(ELIFST2()); 
            }
            if (TokenStream[InputPointer].token_type == Token_Class.Else)
            {
                node.children.Add(ELSEST());
            }
            else if (TokenStream[InputPointer].token_type == Token_Class.End)
            {
                Node node1 = new Node(TokenStream[InputPointer].lex);
                node.children.Add(node1);
                Match();
            }
            return node;
        }
       
        private Node ELIFST1()//22
        {
            Node node = new Node("Else_IF_Statements");
            if (TokenStream[InputPointer].token_type == Token_Class.Integer || TokenStream[InputPointer].token_type == Token_Class.String ||
                TokenStream[InputPointer].token_type == Token_Class.Float)
            {
                node.children.Add(DS());
            }
            if (TokenStream[InputPointer].token_type == Token_Class.Write)
            {
                node.children.Add(WRST());
            }
            if (TokenStream[InputPointer].token_type == Token_Class.Read)
            {
                node.children.Add(RST());
            }
            if (TokenStream[InputPointer].token_type == Token_Class.Idenifier)
            {
                int xPointer = InputPointer;
                Match();
                if (TokenStream[InputPointer].token_type == Token_Class.assigment)
                {
                    InputPointer = xPointer;
                    node.children.Add(ASS());
                }
                else
                {
                    InputPointer = xPointer;
                    node.children.Add(COST());
                }
            }
            Console.WriteLine("IP: " + TokenStream[InputPointer].lex);
            if (TokenStream[InputPointer].token_type != Token_Class.elseif && TokenStream[InputPointer].token_type != Token_Class.Else && TokenStream[InputPointer].token_type != Token_Class.End)
            {
                node.children.Add(ELIFST1());
            }
            return node;
        }

        private Node ELIFST2()//22
        {
            Node node = new Node("Else_IF_Statement2");
            node.children.Add(ELIFST());
            return node;
        }

        private Node ELSEST() //23
        {
            Node node = new Node("Else_Statement");
            if (TokenStream[InputPointer].token_type == Token_Class.Else)
            {
                node.children.Add(new Node(TokenStream[InputPointer].lex));
                Match();
            }
            node.children.Add(ELSEST1());

            if (TokenStream[InputPointer].token_type == Token_Class.End)
            {
                Node node1 = new Node(TokenStream[InputPointer].lex);
                node.children.Add(node1);
                Match();
            }
            return node;
        }

        private Node ELSEST1()//23
        {
            Node node = new Node("Else_Statements");
            if (TokenStream[InputPointer].token_type == Token_Class.Integer || TokenStream[InputPointer].token_type == Token_Class.String ||
                TokenStream[InputPointer].token_type == Token_Class.Float)
            {
                node.children.Add(DS());
            }
            if (TokenStream[InputPointer].token_type == Token_Class.Write)
            {
                node.children.Add(WRST());
            }
            if (TokenStream[InputPointer].token_type == Token_Class.Read)
            {
                node.children.Add(RST());
            }
            if (TokenStream[InputPointer].token_type == Token_Class.Idenifier)
            {
                int xPointer = InputPointer;
                Match();
                if (TokenStream[InputPointer].token_type == Token_Class.assigment)
                {
                    InputPointer = xPointer;
                    node.children.Add(ASS());
                }
                else
                {
                    InputPointer = xPointer;
                    node.children.Add(COST());
                }
            }
            if (TokenStream[InputPointer].token_type != Token_Class.End)
            {
                node.children.Add(ELIFST1());
            }
            return node;
        }

        private Node COST()
        {
            Node node = new Node("Condition_Statement");
            node.children.Add(CON());
            Node temp = COST1();
            if (temp != null)
            {
                node.children.Add(temp);
            }
            return node;
        }

        private Node COST1()
        {
            Node node = new Node("Statement");
            if (TokenStream[InputPointer].token_type == Token_Class.and || TokenStream[InputPointer].token_type == Token_Class.or)
            {
                node.children.Add(BLOP());
            }
            else if (TokenStream[InputPointer].token_type == Token_Class.Idenifier)
            {
                node.children.Add(CON());
            }
            else
            {
                return null;
            }
            if (TokenStream[InputPointer].token_type == Token_Class.and || TokenStream[InputPointer].token_type == Token_Class.or || TokenStream[InputPointer].token_type == Token_Class.Idenifier)
            {
                Node temp = COST1();
                if(temp != null)
                {
                    node.children.Add(temp);
                }
            }
            return node;
        }

        private Node CON()
        {
            Node node = new Node("Condition");
            node.children.Add(Id());
            node.children.Add(COP());
            node.children.Add(T());
            return node;
        }

        private Node COP()
        {
            Node node = new Node("Condition_Operator");
            if (TokenStream[InputPointer].token_type == Token_Class.LessThanOp)
            {
                Node node1 = new Node(TokenStream[InputPointer].lex);
                node.children.Add(node1);
                Match();
            }
            if (TokenStream[InputPointer].token_type == Token_Class.GreaterThanOp)
            {
                Node node1 = new Node(TokenStream[InputPointer].lex);
                node.children.Add(node1);
                Match();
            }
            if (TokenStream[InputPointer].token_type == Token_Class.equal_compersion)
            {
                Node node1 = new Node(TokenStream[InputPointer].lex);
                node.children.Add(node1);
                Match();
            }
            if (TokenStream[InputPointer].token_type == Token_Class.notequal)
            {
                Node node1 = new Node(TokenStream[InputPointer].lex);
                node.children.Add(node1);
                Console.WriteLine("3malt Match");
                Match();
            }
            return node;
        }

        private Node BLOP()
        {
            Node node = new Node("Boolean_Operator");
            if (TokenStream[InputPointer].token_type == Token_Class.and)
            {
                Node node1 = new Node(TokenStream[InputPointer].lex);
                node.children.Add(node1);
                Match();
            }
            if (TokenStream[InputPointer].token_type == Token_Class.or)
            {
                Node node1 = new Node(TokenStream[InputPointer].lex);
                node.children.Add(node1);
                Match();
            }
            return node;
        }

        private Node ASS()
        {
            Node node = new Node("Assignment_Statement");
            node.children.Add(Id());
            if (TokenStream[InputPointer].token_type == Token_Class.assigment)
            {
                Node node1 = new Node(TokenStream[InputPointer].lex);
                node.children.Add(node1);
                Match();
            }
            node.children.Add(EXP());
            return node;
        }


        private Node RST()
        {
            Node node = new Node("Read_Statement");
            if (TokenStream[InputPointer].token_type == Token_Class.Read)
            {
                node.children.Add(new Node(TokenStream[InputPointer].lex));
                Match();
            }
            node.children.Add(Id());
            return node;
        }

        private Node WRST()
        {
            Node node = new Node("Write_Statement");
            if (TokenStream[InputPointer].token_type == Token_Class.Write)
            {
                node.children.Add(new Node(TokenStream[InputPointer].lex));
                Match();
            }
            node.children.Add(WRST1());
            return node;
        }

        private Node WRST1()
        {
            Node node = new Node("Statement");
            if (TokenStream[InputPointer].token_type == Token_Class.endline)
            {
                node.children.Add(new Node(TokenStream[InputPointer].lex));
                Match();
            }
            else
                node.children.Add(EXP());
            return node;
        }

        private Node DS()
        {
            Node node = new Node("Statement");
            node.children.Add(DT());
            node.children.Add(Id());
            Node temp = DS1();
            if(temp!=null)
            {
                node.children.Add(temp);
            }
            Node temp1 = DS2();
            if (temp1 != null)
            {
                node.children.Add(temp1);
            }
            return node;
        }

        private Node DS1()
        {
            Node node = new Node("Declaration_Statement");
            if (TokenStream[InputPointer].token_type == Token_Class.Comma)
            {
                node.children.Add(new Node(TokenStream[InputPointer].lex));
                Match();
                node.children.Add(Id());
                Node temp = DS2();
                if(temp!=null)
                {
                    node.children.Add(temp);
                }
            }
            else
            {
                return null;
            }
            if (TokenStream[InputPointer].token_type == Token_Class.Comma)
                node.children.Add(DS1());
            //Console.WriteLine("IP: " + TokenStream[InputPointer].lex);
            return node;
        }
        private Node DS2()
        {
            Node node = new Node("Intialization Statement");
            
            if (TokenStream[InputPointer].token_type == Token_Class.assigment)
            {
                node.children.Add(new Node(TokenStream[InputPointer].lex));
                Match();
                node.children.Add(EXP());
                return node;
            }
            return null;
        }

        private Node Id()
        {
            Node node = new Node("Idenifier");
            CheckForErrorList(TokenStream[InputPointer].token_type, Token_Class.Idenifier);
            Node temp = new Node(TokenStream[InputPointer].lex);
            node.children.Add(temp);
            Match();
            return node;
        }

        private Node Digit()
        {
            if (TokenStream[InputPointer].token_type == Token_Class.Integer)
            {
                Node node = new Node(TokenStream[InputPointer].ToString());
                return node;
            }
            return null;
        }

        private   Node DT()
        {
            Node node = new Node("Data Type");
            if (TokenStream[InputPointer].token_type == Token_Class.Int ||
                TokenStream[InputPointer].token_type == Token_Class.string_datatype ||
              TokenStream[InputPointer].token_type == Token_Class.Float)
            {
                Node temp = new Node(TokenStream[InputPointer].lex);
                node.children.Add(temp);
                Match();
            }
            return node;
        }

        public void CheckForErrorList(Token_Class x1,Token_Class x2 )
        {
            if(x1 != x2)
            {
                Console.WriteLine("ERROR LIST: " + x1 + " and " + x2);
                Errors.Error_List.Add("Expected "+ x2 +" Found " +x1 + " Lexeme number: " + InputPointer);
                return;
            }
        }

        public  TreeNode PrintParseTree(Node root)
        {
            TreeNode tree = new TreeNode("Parse Tree");
            TreeNode treeRoot = PrintTree(root);
            if (treeRoot != null)
                tree.Nodes.Add(treeRoot);
            return tree;
        }
          TreeNode PrintTree(Node root)
        {
            if (root == null || root.Name == null)
                return null;
            TreeNode tree = new TreeNode(root.Name);
            if (root.children.Count == 0)
                return tree;
            foreach (Node child in root.children)
            {
                if (child == null)
                    continue;
                tree.Nodes.Add(PrintTree(child));
            }
            return tree;
        }
    }
}
