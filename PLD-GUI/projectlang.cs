
using System;
using System.IO;
using System.Runtime.Serialization;
using com.calitha.goldparser.lalr;
using com.calitha.commons;
using System.Collections.Generic;
using System.Windows.Forms;

namespace com.calitha.goldparser
{

    [Serializable()]
    public class SymbolException : System.Exception
    {
        public SymbolException(string message) : base(message)
        {
        }

        public SymbolException(string message,
            Exception inner) : base(message, inner)
        {
        }

        protected SymbolException(SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }

    }

    [Serializable()]
    public class RuleException : System.Exception
    {

        public RuleException(string message) : base(message)
        {
        }

        public RuleException(string message,
                             Exception inner) : base(message, inner)
        {
        }

        protected RuleException(SerializationInfo info,
                                StreamingContext context) : base(info, context)
        {
        }

    }

    enum SymbolConstants : int
    {
        SYMBOL_EOF                =  0, // (EOF)
        SYMBOL_ERROR              =  1, // (Error)
        SYMBOL_WHITESPACE         =  2, // Whitespace
        SYMBOL_MINUS              =  3, // '-'
        SYMBOL_MINUSMINUS         =  4, // '--'
        SYMBOL_NUM                =  5, // '#'
        SYMBOL_LPAREN             =  6, // '('
        SYMBOL_RPAREN             =  7, // ')'
        SYMBOL_TIMES              =  8, // '*'
        SYMBOL_DIV                =  9, // '/'
        SYMBOL_COLON              = 10, // ':'
        SYMBOL_PLUS               = 11, // '+'
        SYMBOL_PLUSPLUS           = 12, // '++'
        SYMBOL_LT                 = 13, // '<'
        SYMBOL_LTEQ               = 14, // '<='
        SYMBOL_LTGT               = 15, // '<>'
        SYMBOL_EQ                 = 16, // '='
        SYMBOL_EQEQ               = 17, // '=='
        SYMBOL_GT                 = 18, // '>'
        SYMBOL_GTEQ               = 19, // '>='
        SYMBOL_ELSE               = 20, // else
        SYMBOL_FR                 = 21, // fr
        SYMBOL_IDENTIFIER         = 22, // identifier
        SYMBOL_IS                 = 23, // is
        SYMBOL_NEW_LINE           = 24, // 'new_line'
        SYMBOL_NEWLINE            = 25, // NewLine
        SYMBOL_VALUE              = 26, // value
        SYMBOL_VAR                = 27, // 'var '
        SYMBOL_WHILE              = 28, // while
        SYMBOL_ADD_EXP            = 29, // <Add_Exp>
        SYMBOL_ASSIGNMENT         = 30, // <Assignment>
        SYMBOL_COND               = 31, // <cond>
        SYMBOL_COUNTER            = 32, // <counter>
        SYMBOL_EXPRESSION         = 33, // <Expression>
        SYMBOL_FOR_LOOP           = 34, // <For_loop>
        SYMBOL_IF_CONDITION       = 35, // <If_condition>
        SYMBOL_MAIN               = 36, // <Main>
        SYMBOL_MUL_EXP            = 37, // <Mul_Exp>
        SYMBOL_NEGATE_EXP         = 38, // <Negate_Exp>
        SYMBOL_OP                 = 39, // <op>
        SYMBOL_STMT               = 40, // <Stmt>
        SYMBOL_STMT_LIST          = 41, // <Stmt_list>
        SYMBOL_VALUE_EXP          = 42, // <Value_Exp>
        SYMBOL_VAR_INITIALIZATION = 43, // <Var_initialization>
        SYMBOL_WHILE_LOOP         = 44  // <While_loop>
    };

    enum RuleConstants : int
    {
        RULE_MAIN                                                      =  0, // <Main> ::= <Stmt_list>
        RULE_STMT_LIST                                                 =  1, // <Stmt_list> ::= <Stmt>
        RULE_STMT_LIST2                                                =  2, // <Stmt_list> ::= <Stmt> <Stmt_list>
        RULE_STMT                                                      =  3, // <Stmt> ::= <Var_initialization>
        RULE_STMT2                                                     =  4, // <Stmt> ::= <Assignment>
        RULE_STMT3                                                     =  5, // <Stmt> ::= <If_condition>
        RULE_STMT4                                                     =  6, // <Stmt> ::= <For_loop>
        RULE_STMT5                                                     =  7, // <Stmt> ::= <While_loop>
        RULE_VAR_INITIALIZATION_VAR_IDENTIFIER_EQ                      =  8, // <Var_initialization> ::= 'var ' identifier '=' <Value_Exp>
        RULE_ASSIGNMENT_IDENTIFIER_EQ                                  =  9, // <Assignment> ::= identifier '=' <Value_Exp>
        RULE_IF_CONDITION_IS_LPAREN_RPAREN_COLON_NEWLINE               = 10, // <If_condition> ::= is '(' <cond> ')' ':' NewLine <Stmt_list>
        RULE_IF_CONDITION_IS_LPAREN_RPAREN_COLON_NEWLINE_NEW_LINE_ELSE = 11, // <If_condition> ::= is '(' <cond> ')' ':' NewLine <Stmt_list> 'new_line' else <Stmt_list>
        RULE_OP_LT                                                     = 12, // <op> ::= '<'
        RULE_OP_GT                                                     = 13, // <op> ::= '>'
        RULE_OP_LTGT                                                   = 14, // <op> ::= '<>'
        RULE_OP_EQEQ                                                   = 15, // <op> ::= '=='
        RULE_COND                                                      = 16, // <cond> ::= <Expression> <op> <Expression>
        RULE_FOR_LOOP_FR_NUM_NUM_COLON_NEWLINE                         = 17, // <For_loop> ::= fr <Var_initialization> '#' <cond> '#' <counter> ':' NewLine <Stmt_list>
        RULE_WHILE_LOOP_WHILE_LPAREN_RPAREN_COLON_NEWLINE              = 18, // <While_loop> ::= while '(' <Expression> ')' ':' NewLine <counter> <Stmt_list>
        RULE_COUNTER_PLUSPLUS_IDENTIFIER                               = 19, // <counter> ::= '++' identifier
        RULE_COUNTER_MINUSMINUS_IDENTIFIER                             = 20, // <counter> ::= '--' identifier
        RULE_EXPRESSION_GT                                             = 21, // <Expression> ::= <Expression> '>' <Add_Exp>
        RULE_EXPRESSION_LT                                             = 22, // <Expression> ::= <Expression> '<' <Add_Exp>
        RULE_EXPRESSION_LTEQ                                           = 23, // <Expression> ::= <Expression> '<=' <Add_Exp>
        RULE_EXPRESSION_GTEQ                                           = 24, // <Expression> ::= <Expression> '>=' <Add_Exp>
        RULE_EXPRESSION_EQEQ                                           = 25, // <Expression> ::= <Expression> '==' <Add_Exp>
        RULE_EXPRESSION_LTGT                                           = 26, // <Expression> ::= <Expression> '<>' <Add_Exp>
        RULE_EXPRESSION                                                = 27, // <Expression> ::= <Add_Exp>
        RULE_ADD_EXP_PLUS                                              = 28, // <Add_Exp> ::= <Add_Exp> '+' <Mul_Exp>
        RULE_ADD_EXP_MINUS                                             = 29, // <Add_Exp> ::= <Add_Exp> '-' <Mul_Exp>
        RULE_ADD_EXP                                                   = 30, // <Add_Exp> ::= <Mul_Exp>
        RULE_MUL_EXP_TIMES                                             = 31, // <Mul_Exp> ::= <Mul_Exp> '*' <Negate_Exp>
        RULE_MUL_EXP_DIV                                               = 32, // <Mul_Exp> ::= <Mul_Exp> '/' <Negate_Exp>
        RULE_MUL_EXP                                                   = 33, // <Mul_Exp> ::= <Negate_Exp>
        RULE_NEGATE_EXP_MINUS                                          = 34, // <Negate_Exp> ::= '-' <Value_Exp>
        RULE_NEGATE_EXP                                                = 35, // <Negate_Exp> ::= <Value_Exp>
        RULE_VALUE_EXP_VALUE                                           = 36, // <Value_Exp> ::= value
        RULE_VALUE_EXP_IDENTIFIER                                      = 37, // <Value_Exp> ::= identifier
        RULE_VALUE_EXP_LPAREN_RPAREN                                   = 38  // <Value_Exp> ::= '(' <Expression> ')'
    };

    public class MyParser
    {
        private LALRParser parser;
        ListBox list1;
        ListBox list2;
        public MyParser(string filename, ListBox list1, ListBox list2)
        {
            FileStream stream = new FileStream(filename,
                                               FileMode.Open,
                                               FileAccess.Read,
                                               FileShare.Read);
            this.list1 = list1; // outer = inner
            this.list2 = list2;
            Init(stream);
            stream.Close();
            
        }


        public MyParser(string baseName, string resourceName)
        {
            byte[] buffer = ResourceUtil.GetByteArrayResource(
                System.Reflection.Assembly.GetExecutingAssembly(),
                baseName,
                resourceName);
            MemoryStream stream = new MemoryStream(buffer);
            Init(stream);
            stream.Close();
        }

        public MyParser(Stream stream)
        {
            Init(stream);
        }

        private void Init(Stream stream)
        {
            CGTReader reader = new CGTReader(stream);
            parser = reader.CreateNewParser();
            parser.TrimReductions = false;
            parser.StoreTokens = LALRParser.StoreTokensMode.NoUserObject;

            parser.OnTokenError += new LALRParser.TokenErrorHandler(TokenErrorEvent);
            parser.OnParseError += new LALRParser.ParseErrorHandler(ParseErrorEvent);
            parser.OnTokenRead += Parser_OnTokenRead;
        }

        public void Parse(string source)
        {
            NonterminalToken token = parser.Parse(source);
            if (token != null)
            {
                Object obj = CreateObject(token);
                //todo: Use your object any way you like
            }
        }

        private void Parser_OnTokenRead(LALRParser parser, TokenReadEventArgs args)
        {
            string message = args.Token.Text + "\t" + (SymbolConstants)args.Token.Symbol.Id;
            list2.Items.Add(message);
        }

        private Object CreateObject(Token token)
        {
            if (token is TerminalToken)
                return CreateObjectFromTerminal((TerminalToken)token);
            else
                return CreateObjectFromNonterminal((NonterminalToken)token);
        }

        private Object CreateObjectFromTerminal(TerminalToken token)
        {
            switch (token.Symbol.Id)
            {
                case (int)SymbolConstants.SYMBOL_EOF :
                //(EOF)
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_ERROR :
                //(Error)
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_WHITESPACE :
                //Whitespace
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_MINUS :
                //'-'
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_MINUSMINUS :
                //'--'
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_NUM :
                //'#'
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_LPAREN :
                //'('
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_RPAREN :
                //')'
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_TIMES :
                //'*'
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_DIV :
                //'/'
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_COLON :
                //':'
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_PLUS :
                //'+'
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_PLUSPLUS :
                //'++'
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_LT :
                //'<'
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_LTEQ :
                //'<='
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_LTGT :
                //'<>'
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_EQ :
                //'='
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_EQEQ :
                //'=='
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_GT :
                //'>'
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_GTEQ :
                //'>='
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_ELSE :
                //else
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_FR :
                //fr
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_IDENTIFIER :
                //identifier
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_IS :
                //is
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_NEW_LINE :
                //'new_line'
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_NEWLINE :
                //NewLine
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_VALUE :
                //value
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_VAR :
                //'var '
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_WHILE :
                //while
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_ADD_EXP :
                //<Add_Exp>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_ASSIGNMENT :
                //<Assignment>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_COND :
                //<cond>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_COUNTER :
                //<counter>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_EXPRESSION :
                //<Expression>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_FOR_LOOP :
                //<For_loop>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_IF_CONDITION :
                //<If_condition>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_MAIN :
                //<Main>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_MUL_EXP :
                //<Mul_Exp>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_NEGATE_EXP :
                //<Negate_Exp>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_OP :
                //<op>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_STMT :
                //<Stmt>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_STMT_LIST :
                //<Stmt_list>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_VALUE_EXP :
                //<Value_Exp>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_VAR_INITIALIZATION :
                //<Var_initialization>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_WHILE_LOOP :
                //<While_loop>
                //todo: Create a new object that corresponds to the symbol
                return null;

            }
            throw new SymbolException("Unknown symbol");
        }

        public Object CreateObjectFromNonterminal(NonterminalToken token)
        {
            switch (token.Rule.Id)
            {
                case (int)RuleConstants.RULE_MAIN :
                //<Main> ::= <Stmt_list>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_STMT_LIST :
                //<Stmt_list> ::= <Stmt>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_STMT_LIST2 :
                //<Stmt_list> ::= <Stmt> <Stmt_list>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_STMT :
                //<Stmt> ::= <Var_initialization>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_STMT2 :
                //<Stmt> ::= <Assignment>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_STMT3 :
                //<Stmt> ::= <If_condition>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_STMT4 :
                //<Stmt> ::= <For_loop>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_STMT5 :
                //<Stmt> ::= <While_loop>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_VAR_INITIALIZATION_VAR_IDENTIFIER_EQ :
                //<Var_initialization> ::= 'var ' identifier '=' <Value_Exp>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_ASSIGNMENT_IDENTIFIER_EQ :
                //<Assignment> ::= identifier '=' <Value_Exp>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_IF_CONDITION_IS_LPAREN_RPAREN_COLON_NEWLINE :
                //<If_condition> ::= is '(' <cond> ')' ':' NewLine <Stmt_list>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_IF_CONDITION_IS_LPAREN_RPAREN_COLON_NEWLINE_NEW_LINE_ELSE :
                //<If_condition> ::= is '(' <cond> ')' ':' NewLine <Stmt_list> 'new_line' else <Stmt_list>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_OP_LT :
                //<op> ::= '<'
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_OP_GT :
                //<op> ::= '>'
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_OP_LTGT :
                //<op> ::= '<>'
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_OP_EQEQ :
                //<op> ::= '=='
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_COND :
                //<cond> ::= <Expression> <op> <Expression>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_FOR_LOOP_FR_NUM_NUM_COLON_NEWLINE :
                //<For_loop> ::= fr <Var_initialization> '#' <cond> '#' <counter> ':' NewLine <Stmt_list>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_WHILE_LOOP_WHILE_LPAREN_RPAREN_COLON_NEWLINE :
                //<While_loop> ::= while '(' <Expression> ')' ':' NewLine <counter> <Stmt_list>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_COUNTER_PLUSPLUS_IDENTIFIER :
                //<counter> ::= '++' identifier
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_COUNTER_MINUSMINUS_IDENTIFIER :
                //<counter> ::= '--' identifier
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_EXPRESSION_GT :
                //<Expression> ::= <Expression> '>' <Add_Exp>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_EXPRESSION_LT :
                //<Expression> ::= <Expression> '<' <Add_Exp>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_EXPRESSION_LTEQ :
                //<Expression> ::= <Expression> '<=' <Add_Exp>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_EXPRESSION_GTEQ :
                //<Expression> ::= <Expression> '>=' <Add_Exp>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_EXPRESSION_EQEQ :
                //<Expression> ::= <Expression> '==' <Add_Exp>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_EXPRESSION_LTGT :
                //<Expression> ::= <Expression> '<>' <Add_Exp>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_EXPRESSION :
                //<Expression> ::= <Add_Exp>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_ADD_EXP_PLUS :
                //<Add_Exp> ::= <Add_Exp> '+' <Mul_Exp>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_ADD_EXP_MINUS :
                //<Add_Exp> ::= <Add_Exp> '-' <Mul_Exp>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_ADD_EXP :
                //<Add_Exp> ::= <Mul_Exp>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_MUL_EXP_TIMES :
                //<Mul_Exp> ::= <Mul_Exp> '*' <Negate_Exp>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_MUL_EXP_DIV :
                //<Mul_Exp> ::= <Mul_Exp> '/' <Negate_Exp>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_MUL_EXP :
                //<Mul_Exp> ::= <Negate_Exp>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_NEGATE_EXP_MINUS :
                //<Negate_Exp> ::= '-' <Value_Exp>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_NEGATE_EXP :
                //<Negate_Exp> ::= <Value_Exp>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_VALUE_EXP_VALUE :
                //<Value_Exp> ::= value
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_VALUE_EXP_IDENTIFIER :
                //<Value_Exp> ::= identifier
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_VALUE_EXP_LPAREN_RPAREN :
                //<Value_Exp> ::= '(' <Expression> ')'
                //todo: Create a new object using the stored tokens.
                return null;

            }
            throw new RuleException("Unknown rule");
        }

        private void TokenErrorEvent(LALRParser parser, TokenErrorEventArgs args)
        {
            string message = "Token error with input: '"+args.Token.ToString()+"'";
            list2.Items.Add(message);
            //todo: Report message to UI?
        }

        private void ParseErrorEvent(LALRParser parser, ParseErrorEventArgs args)
        {
            string message = "UnexpectedToken: '" + args.UnexpectedToken.ToString() + "'";
            string _break = "=============================";
            string message2 = "ExpectedTokens: '" + args.ExpectedTokens.ToString() + "'";
            list1.Items.Add(message);
            list1.Items.Add(_break);
            list1.Items.Add(message2);
            //todo: Report message to UI?
        }


    }
}
