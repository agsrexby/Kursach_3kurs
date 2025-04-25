using System;
using System.Collections.Generic;
using System.Text;

namespace laba1
{
    public class Parser
    {
        private readonly List<Token> tokens;
        private int position = 0;
        private readonly List<string> errors = new List<string>();

        public Parser(List<Token> tokens)
        {
            this.tokens = tokens;
        }

        public string Analyze()
        {
            while (!IsAtEnd())
            {
                ParseOperation();
            }

            return errors.Count == 0 ? "Ошибок не обнаружено" : string.Join(Environment.NewLine, errors);
        }

        private void ParseOperation()
        {
            Token token = Peek();

            if (token.Type == TokenType.Keyword && token.Lexeme == "operation")
            {
                Advance(); // Пропускаем 'operation'

                if (!Match(TokenType.Identifier))
                {
                    ReportError("Ожидался идентификатор после 'operation'");
                    return;
                }

                if (!Match(TokenType.OpenParen))
                {
                    ReportError("Ожидалась открывающая скобка '(' после идентификатора");
                    return;
                }

                ParseParameters();

                if (!Match(TokenType.CloseParen))
                {
                    ReportError("Ожидалась закрывающая скобка ')'");
                    return;
                }

                if (!Match(TokenType.Assignment))
                {
                    ReportError("Ожидался оператор '->'");
                    return;
                }

                if (!Match(TokenType.Identifier))
                {
                    ReportError("Ожидался идентификатор после '->'");
                    return;
                }

                if (!Match(TokenType.OpenBrace))
                {
                    ReportError("Ожидалась открывающая фигурная скобка '{'");
                    return;
                }

                while (!Check(TokenType.CloseBrace) && !IsAtEnd())
                {
                    ParseStatement();
                }

                if (!Match(TokenType.CloseBrace))
                {
                    ReportError("Ожидалась закрывающая фигурная скобка '}'");
                }
            }
            else
            {
                ReportError("Ожидалось ключевое слово 'operation'");
                Advance(); // Пропускаем токен, чтобы избежать зацикливания
            }
        }

        private void ParseParameters()
        {
            if (Check(TokenType.Keyword) && Peek().Lexeme == "var")
            {
                do
                {
                    Advance(); // Пропускаем 'var'

                    if (!Match(TokenType.Identifier))
                    {
                        ReportError("Ожидался идентификатор параметра после 'var'");
                        return;
                    }

                    if (!Match(TokenType.Equals))
                    {
                        ReportError("Ожидался символ ':' после идентификатора параметра");
                        return;
                    }

                    if (!Match(TokenType.Keyword) || Previous().Lexeme != "int")
                    {
                        ReportError("Ожидалось ключевое слово 'int' после ':'");
                        return;
                    }
                }
                while (Match(TokenType.Comma));
            }
        }

        private void ParseStatement()
        {
            if (!Match(TokenType.Identifier))
            {
                ReportError("Ожидался идентификатор в начале выражения");
                Synchronize();
                return;
            }

            if (!Match(TokenType.Equals))
            {
                ReportError("Ожидался оператор '=' после идентификатора");
                Synchronize();
                return;
            }

            ParseExpression();

            if (!Match(TokenType.Semicolon))
            {
                ReportError("Ожидалась точка с запятой ';' в конце выражения");
                Synchronize();
            }
        }

        private void ParseExpression()
        {
            ParseTerm();

            while (Match(TokenType.Operation))
            {
                ParseTerm();
            }
        }

        private void ParseTerm()
        {
            if (Check(TokenType.Identifier) || Check(TokenType.Number))
            {
                Advance();
            }
            else
            {
                ReportError("Ожидался идентификатор или число в выражении");
                Advance();
            }
        }

        private bool Match(TokenType type)
        {
            if (Check(type))
            {
                Advance();
                return true;
            }
            return false;
        }

        private bool Check(TokenType type)
        {
            if (IsAtEnd()) return false;
            return Peek().Type == type;
        }

        private Token Advance()
        {
            if (!IsAtEnd()) position++;
            return Previous();
        }

        private bool IsAtEnd()
        {
            return position >= tokens.Count;
        }

        private Token Peek()
        {
            return tokens[position];
        }

        private Token Previous()
        {
            return tokens[position - 1];
        }

        private void ReportError(string message)
        {
            Token token = Peek();
            errors.Add($"Ошибка: {message} (позиция: {token.StartPosition})");
        }

        private void Synchronize()
        {
            while (!IsAtEnd())
            {
                if (Previous().Type == TokenType.Semicolon) return;

                switch (Peek().Type)
                {
                    case TokenType.Keyword:
                    case TokenType.Identifier:
                        return;
                }

                Advance();
            }
        }
    }
}
