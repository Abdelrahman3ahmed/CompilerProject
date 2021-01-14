using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace JASON_Compiler
{
    class TemporaryParser
    {
        public SyntaxAnalyser SA;
        public TemporaryParser()
        {
            SA = new SyntaxAnalyser();
        }
        public void DoThis()
        {
            PrintTockens();
            SA = new SyntaxAnalyser();
            SA.AssignTreeNode();   
        }
        public void PrintTockens()
        {
            for (int i = 0; i < Tiny_Compiler.Jason_Scanner.Tokens.Count; i++)
            {
                Console.WriteLine("Lexeme " + (i + 1) + " : " + Tiny_Compiler.Jason_Scanner.Tokens.ElementAt(i).lex);
                Console.WriteLine("Tocken Type : " + Tiny_Compiler.Jason_Scanner.Tokens.ElementAt(i).token_type);
            }
        }
    }
}
