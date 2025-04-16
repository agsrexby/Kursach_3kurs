using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace laba1
{
    public class Parser
    {
        private readonly List<Token> _tokens;
        private int _position;
        private Token Current => _position < _tokens.Count ? _tokens[_position] : null;
        private readonly StringBuilder _errors;

        public Parser(List<Token> tokens)
        {
            _tokens = tokens;
            _position = 0;
            _errors = new StringBuilder();
        }

        public string Analyze()
        {
            while (_position < _tokens.Count)
            {
                ParseStatement();
            }

            return _errors.Length == 0 ? "Ошибок не обнаружено" : _errors.ToString();
        }

        private void ParseStatement()
        {
            if (Match("идентификатор", "operation"))
            {
                if (!Match("оператор присваивания", "="))
                {
                    Error("Ожидался знак '=' после 'operation'");
                    return;
                }

                ParseLambda();

                if (!Match("конец оператора", ";"))
                {
                    Error("Ожидалась точка с запятой ';' в конце лямбда-выражения");
                }
            }
            else
            {
                Error($"Неизвестное выражение: '{Current?.Lexeme}'");
                _position++;
            }
        }

        private void ParseLambda()
        {
            if (!Match("открывающая скобка", "("))
            {
                Error("Ожидалась '(' после '='");
                return;
            }

            bool expectingIdentifier = true;
            while (!Match("закрывающая скобка", ")"))
            {
                if (expectingIdentifier)
                {
                    if (!Match("идентификатор"))
                    {
                        Error("Ожидался идентификатор параметра");
                        return;
                    }
                    expectingIdentifier = false;
                }
                else
                {
                    if (!Match("разделитель аргументов", ","))
                    {
                        Error("Ожидалась ',' между параметрами");
                        return;
                    }
                    expectingIdentifier = true;
                }

                if (_position >= _tokens.Count)
                {
                    Error("Ожидалась закрывающая скобка ')'");
                    return;
                }
            }

            if (!Match("лямбда-оператор", "->"))
            {
                Error("Ожидался лямбда-оператор '->'");
                return;
            }

            ParseExpression();
        }

        private void ParseExpression()
        {
            ParseTerm();
            while (Match("арифметический оператор", "+") || Match("арифметический оператор", "-"))
            {
                ParseTerm();
            }
        }

        private void ParseTerm()
        {
            ParseFactor();
            while (Match("арифметический оператор", "*") || Match("арифметический оператор", "/"))
            {
                ParseFactor();
            }
        }

        private void ParseFactor()
        {
            if (Match("открывающая скобка", "("))
            {
                ParseExpression();
                if (!Match("закрывающая скобка", ")"))
                {
                    Error("Ожидалась закрывающая скобка ')'");
                }
            }
            else if (!Match("идентификатор") && !Match("целое число"))
            {
                Error("Ожидался идентификатор или целое число");
            }
        }

        private bool Match(string type, string lexeme = null)
        {
            if (Current != null && Current.Type == type && (lexeme == null || Current.Lexeme == lexeme))
            {
                _position++;
                return true;
            }
            return false;
        }

        private void Error(string message)
        {
            var pos = Current != null ? Current.Position : "конец";
            _errors.AppendLine($"[Ошибка в позиции {pos}] {message}");
        }
    }
}
