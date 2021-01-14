using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public enum Token_Class
{
    Begin, Call, Declare, End, Do, Else, EndIf, EndUntil, EndWhile, If, Integer,
    Parameters, Procedure, Program, Read, Real, Set, Then, Until, While, Write,
    Dot, Semicolon, Comma, LParanthesis, RParanthesis, EqualOp, LessThanOp,
    GreaterThanOp, NotEqualOp, PlusOp, MinusOp, MultiplyOp, DivideOp,
    Idenifier, Constant, and, or, assigment, String, comment, Int, RePEat,
    endline, Return, LeftBraces, RightBraces, notequal, main, Float, string_datatype, equal_compersion, elseif
}
namespace JASON_Compiler
{


    public class Token
    {
        public string lex;
        public Token_Class token_type;
    }

    public class Scanner
    {
        public List<Token> Tokens = new List<Token>();
        Dictionary<string, Token_Class> ReservedWords = new Dictionary<string, Token_Class>();
        Dictionary<string, Token_Class> Operators = new Dictionary<string, Token_Class>();

        public Scanner()
        {
            ReservedWords.Add("IF", Token_Class.If);
            ReservedWords.Add("BEGIN", Token_Class.Begin);
            ReservedWords.Add("CALL", Token_Class.Call);
            ReservedWords.Add("DECLARE", Token_Class.Declare);
            ReservedWords.Add("END", Token_Class.End);
            ReservedWords.Add("DO", Token_Class.Do);
            ReservedWords.Add("ELSE", Token_Class.Else);
            ReservedWords.Add("ENDIF", Token_Class.EndIf);
            ReservedWords.Add("ENDUNTIL", Token_Class.EndUntil);
            ReservedWords.Add("ENDWHILE", Token_Class.EndWhile);
            ReservedWords.Add("INTEGER", Token_Class.Integer);
            ReservedWords.Add("PARAMETERS", Token_Class.Parameters);
            ReservedWords.Add("PROCEDURE", Token_Class.Procedure);
            ReservedWords.Add("PROGRAM", Token_Class.Program);
            ReservedWords.Add("READ", Token_Class.Read);
            ReservedWords.Add("REAL", Token_Class.Real);
            ReservedWords.Add("SET", Token_Class.Set);
            ReservedWords.Add("THEN", Token_Class.Then);
            ReservedWords.Add("UNTIL", Token_Class.Until);
            ReservedWords.Add("WHILE", Token_Class.While);
            ReservedWords.Add("WRITE", Token_Class.Write);
            ReservedWords.Add("INT", Token_Class.Int);
            ReservedWords.Add("REPEAT", Token_Class.RePEat);
            ReservedWords.Add("RETURN", Token_Class.Return);
            ReservedWords.Add("ENDL", Token_Class.endline);
            ReservedWords.Add("MAIN", Token_Class.main);
            ReservedWords.Add("STRING", Token_Class.string_datatype);
            ReservedWords.Add("FLOAT", Token_Class.Float);
            ReservedWords.Add("ELSEIF", Token_Class.elseif);

            Operators.Add(".", Token_Class.Dot);
            Operators.Add(";", Token_Class.Semicolon);
            Operators.Add(",", Token_Class.Comma);
            Operators.Add("(", Token_Class.LParanthesis);
            Operators.Add(")", Token_Class.RParanthesis);
            Operators.Add("<", Token_Class.LessThanOp);
            Operators.Add(">", Token_Class.GreaterThanOp);
            Operators.Add("!", Token_Class.NotEqualOp);
            Operators.Add("+", Token_Class.PlusOp);
            Operators.Add("-", Token_Class.MinusOp);
            Operators.Add("*", Token_Class.MultiplyOp);
            Operators.Add("/", Token_Class.DivideOp);
            Operators.Add("&&", Token_Class.and);
            Operators.Add("||", Token_Class.or);
            Operators.Add(":=", Token_Class.assigment);
            Operators.Add("comment", Token_Class.comment);
            Operators.Add("{", Token_Class.LeftBraces);
            Operators.Add("}", Token_Class.RightBraces);
            Operators.Add("<>", Token_Class.notequal);
            Operators.Add("==", Token_Class.equal_compersion);
        }

        public void StartScanning(string SourceCode)
        {
            for (int i = 0; i < SourceCode.Length; i++)
            {
                int j = i;
                int y = i;
                char CurrentChar = SourceCode[i];
                string CurrentLexeme = CurrentChar.ToString();
                string str;
                if (CurrentChar == ' ' || CurrentChar == '\r' || CurrentChar == '\n')
                    continue;
                if (CurrentChar >= 'A' && CurrentChar <= 'Z' || CurrentChar >= 'a' && CurrentChar <= 'z') //if you read a character
                {
                    for (y = i + 1; y < SourceCode.Length; y++)
                    {

                        if (SourceCode[y] >= 'A' && SourceCode[y] <= 'Z' || SourceCode[y] >= 'a' && SourceCode[y] <= 'z' || SourceCode[y] >= '0' && SourceCode[y] <= '9')
                        {
                            CurrentLexeme += SourceCode[y].ToString();
                        }
                        else
                        {
                            FindTokenClass(CurrentLexeme.ToUpper());
                            i = y - 1;
                            break;
                        }
                    }
                }
                else if (CurrentChar == '.')
                {
                    FindTokenClass(CurrentLexeme.ToUpper());
                }
                //number 
                else if (CurrentChar >= '0' && CurrentChar <= '9')
                {
                    str = "";
                    while (CurrentChar == '.' || CurrentChar >= '0' && CurrentChar <= '9')
                    {
                        //CurrentLexeme = "";
                        if (j > SourceCode.Length - 1)
                            break;
                        else
                        {
                            if (SourceCode[j] == '.' || SourceCode[j] >= '0' && SourceCode[j] <= '9')
                            {
                                str += SourceCode[j];
                            }
                            j++;
                            if (j < SourceCode.Length)
                                CurrentChar = SourceCode[j];
                        }
                    }
                    i = j - 1;
                    CurrentLexeme = str;
                    FindTokenClass(CurrentLexeme.ToUpper());
                }
                //assigment statement
                else if (CurrentChar == ':' && i < SourceCode.Length - 1 && SourceCode[i + 1] == '=')
                {
                    CurrentLexeme += "=";
                    i++;
                    FindTokenClass(CurrentLexeme.ToUpper());
                }
                //equal equal
                else if (CurrentChar == '=' && i < SourceCode.Length - 1 && SourceCode[i + 1] == '=')
                {

                    CurrentLexeme += "=";
                    i++;
                    FindTokenClass(CurrentLexeme.ToUpper());
                }
                //not equal 
                else if (CurrentChar == '<' && i < SourceCode.Length - 1 && SourceCode[i + 1] == '>')
                {
                    CurrentLexeme += '>';
                    i++;
                    FindTokenClass(CurrentLexeme.ToUpper());
                }
                //anding
                else if (CurrentChar == '&' && i < SourceCode.Length - 1 && SourceCode[i + 1] == '&')
                {
                    CurrentLexeme += "&";
                    i++;
                    FindTokenClass(CurrentLexeme.ToUpper());
                }
                //or
                else if (CurrentChar == '|' && i < SourceCode.Length - 1 && SourceCode[i + 1] == '|')
                {
                    CurrentLexeme += "|";
                    i++;
                    FindTokenClass(CurrentLexeme.ToUpper());
                }

                else if (SourceCode[i] == '/' && i < SourceCode.Length - 1 && SourceCode[i + 1] == '*')
                {
                    bool found = true;
                    str = "";
                    str += "/*";
                    j += 2;
                    while (SourceCode[j] != '*' && j < SourceCode.Length - 1 && SourceCode[j + 1] != '/')
                    {
                        CurrentChar = SourceCode[j];
                        if (!(SourceCode[j] == '*' && SourceCode[j + 1] == '/'))
                        {
                            str += CurrentChar;
                        }
                        j++;
                        if (j >= SourceCode.Length)
                        {
                            Errors.Error_List.Add(CurrentLexeme.ToUpper());
                            found = false;
                            break;
                        }
                    }
                    if (found == true)
                    {
                        if (SourceCode[j] == ';' || SourceCode[j] == '\n')
                        {

                            CurrentLexeme = str;
                            i = j - 1;
                            FindTokenClass(CurrentLexeme);
                        }
                        else
                        {
                            str += "*/";
                            CurrentLexeme = str;
                            i = j + 1;
                            FindTokenClass(CurrentLexeme.ToUpper());
                        }
                    }
                }

                else if (CurrentChar == '{' || CurrentChar == '}' || CurrentChar == '(' || CurrentChar == ')')
                {
                    FindTokenClass(CurrentLexeme);
                }
                //operator 
                else if (CurrentChar == '+' || CurrentChar == '-' || CurrentChar == '*' || CurrentChar == '/' || CurrentChar == '>' || CurrentChar == '<' || CurrentChar == ',' || CurrentChar == ';' || CurrentChar == '.')
                {
                    FindTokenClass(CurrentLexeme.ToUpper());
                }
                //string
                else if (CurrentChar == '"')
                {
                    bool out_range = false;
                    str = "";
                    j++;
                    str += '"';
                    while (SourceCode[j] != '"' && SourceCode[j] != ';' && SourceCode[j] != '\n')
                    {
                        CurrentChar = SourceCode[j];
                        str += CurrentChar;
                        j++;
                        if (j >= SourceCode.Length)
                        {
                            out_range = true;
                            Errors.Error_List.Add(str);
                            i = j;
                            break;
                        }
                        //if (SourceCode[j] == '"' || SourceCode[j] == ';' || SourceCode[j] == '\n')
                        //break;

                    }
                    if (!out_range)
                    {
                        if (SourceCode[j] == ';' || SourceCode[j] == '\n')
                        {
                            CurrentLexeme = str;
                            i = j - 1;
                            FindTokenClass(CurrentLexeme);
                        }
                        else
                        {
                            str += '"';
                            CurrentLexeme = str;
                            i = j;
                            FindTokenClass(CurrentLexeme);
                        }
                    }
                }
                else
                {
                    FindTokenClass(CurrentLexeme.ToUpper());
                }
            }

            Tiny_Compiler.TokenStream = Tokens;
        }
        void FindTokenClass(string Lex)
        {
            Token_Class TC;
            Token Tok = new Token();
            Tok.lex = Lex;
            //Is it a reserved word?

            if (ReservedWords.ContainsKey(Lex))
            {
                Tok.token_type = ReservedWords[Lex];
            }
            //Is it an identifier?
            else if (is_identifier(Lex))
            {
                TC = Token_Class.Idenifier;
                Tok.token_type = TC;
            }
            //Is it a Constant?
            else if (is_constant(Lex))
            {
                Tok.token_type = Token_Class.Constant;
            }
            //Is it an operator?
            else if (is_operator(Lex))
            {
                Tok.token_type = Operators[Lex];
            }
            //is it an string
            else if (is_string(Lex))
            {
                Tok.token_type = Token_Class.String;
            }
            //it is an comment 
            else if (is_comment(Lex))
            {
                Tok.token_type = Operators["comment"];
            }
            else
            {
                Errors.Error_List.Add(Lex);
                return;
            }
            Tokens.Add(Tok);
        }

        bool is_identifier(string lex)
        {
            bool isValid = true;
            if (ReservedWords.ContainsKey(lex))
            {
                isValid = false;
            }
            else if (lex[0] >= 'A' && lex[0] <= 'Z' || lex[0] >= 'a' && lex[0] <= 'z') //if you read a character
            {
                for (int j = 0; j < lex.Length; j++)
                {
                    if ((lex[j] >= 'A' && lex[j] <= 'Z') || (lex[j] >= 'a' && lex[j] <= 'z') || (lex[j] >= '0' && lex[j] <= '9'))
                    {
                    }
                    else
                    {
                        isValid = false;
                        break;
                    }
                }

            }
            else
            {
                isValid = false;
            }

            return isValid;
        }
        bool is_constant(string lex)
        {
            bool isValid = false;
            int temp = 0;
            if (lex[0] >= '0' && lex[0] <= '9')
            {
                isValid = true;
            }
            for (int i = 1; i < lex.Length; i++)//check if error in number 
            {
                if (lex[i] == '.')
                {
                    temp++;
                    if (temp > 1)
                    {
                        isValid = false;
                        break;
                    }
                }
            }
            return isValid;
        }
        bool is_operator(string lex)
        {
            for (int i = 0; i < Operators.Count; i++)
            {
                if (Operators.Keys.Contains(lex))
                {
                    return true;
                }
            }
            return false;
        }

        bool is_comment(string lex)
        {
            if (lex[0] == '/' && lex[1] == '*' && lex[lex.Length - 2] == '*' && lex[lex.Length - 1] == '/')
            {
                return true;
            }
            return false;
        }
        bool is_string(string lex)
        {
            if (lex[0] == '"' && lex[lex.Length - 1] == '"')
            {
                return true;
            }
            return false;
        }

    }
}
