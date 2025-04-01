using System;
using System.Collections.Generic;
using System.Text;

namespace laba1
{
    public class LexicalAnalyzer
    {
        public enum TokenType
        {
            Identifier = 1,          // Идентификатор
            Number = 2,              // Число
            AssignmentOperator = 3,   // Оператор присваивания =
            LambdaOperator = 4,       // Лямбда-оператор ->
            ArithmeticOperator = 5,    // Арифметические операторы + - * /
            ComparisonOperator = 6,    // Операторы сравнения < > == !=
            LogicalOperator = 7,      // Логические операторы && || !
            OpenParenthesis = 8,       // Открывающая скобка (
            CloseParenthesis = 9,      // Закрывающая скобка )
            Comma = 10,               // Запятая ,
            Semicolon = 11,            // Точка с запятой ;
            Whitespace = 12,           // Пробельные символы
            Invalid = 99               // Недопустимый символ
        }

        public class Token
        {
            public TokenType Type { get; set; }
            public string TypeName { get; set; }  // Добавлено новое свойство для имени типа
            public string Value { get; set; }
            public int StartPos { get; set; }
            public int EndPos { get; set; }

            public Token(TokenType type, string typeName, string value, int startPos, int endPos)
            {
                Type = type;
                TypeName = typeName;
                Value = value;
                StartPos = startPos;
                EndPos = endPos;
            }
        }

        public List<Token> Analyze(string input)
        {
            List<Token> tokens = new List<Token>();
            int currentPos = 0;
            int startPos = 0;
            StringBuilder currentToken = new StringBuilder();

            while (currentPos < input.Length)
            {
                char currentChar = input[currentPos];
                startPos = currentPos;

                // Пропускаем пробелы
                if (char.IsWhiteSpace(currentChar))
                {
                    currentPos++;
                    continue;
                }

                // Идентификаторы и ключевые слова
                if (char.IsLetter(currentChar) || currentChar == '_')
                {
                    currentToken.Clear();
                    while (currentPos < input.Length && (char.IsLetterOrDigit(input[currentPos]) || input[currentPos] == '_'))
                    {
                        currentToken.Append(input[currentPos]);
                        currentPos++;
                    }
                    tokens.Add(new Token(TokenType.Identifier, "Идентификатор", currentToken.ToString(), startPos + 1, currentPos));
                    continue;
                }

                // Числа
                if (char.IsDigit(currentChar))
                {
                    currentToken.Clear();
                    while (currentPos < input.Length && char.IsDigit(input[currentPos]))
                    {
                        currentToken.Append(input[currentPos]);
                        currentPos++;
                    }
                    tokens.Add(new Token(TokenType.Number, "Число", currentToken.ToString(), startPos + 1, currentPos));
                    continue;
                }

                // Лямбда-оператор ->
                if (currentChar == '-' && currentPos + 1 < input.Length && input[currentPos + 1] == '>')
                {
                    tokens.Add(new Token(TokenType.LambdaOperator, "Лямбда-оператор", "->", startPos + 1, currentPos + 2));
                    currentPos += 2;
                    continue;
                }

                // Операторы
                if ("+-*/%&|^<>!".IndexOf(currentChar) >= 0)
                {
                    // Проверяем двухсимвольные операторы (==, !=, <=, >=, &&, ||)
                    if (currentPos + 1 < input.Length && "=!&|".IndexOf(currentChar) >= 0 && input[currentPos + 1] == currentChar ||
                        "=<>".IndexOf(input[currentPos + 1]) >= 0)
                    {
                        tokens.Add(new Token(TokenType.ComparisonOperator, "Оператор сравнения", 
                                            input.Substring(currentPos, 2), startPos + 1, currentPos + 2));
                        currentPos += 2;
                    }
                    else
                    {
                        tokens.Add(new Token(TokenType.ArithmeticOperator, "Арифметический оператор", 
                                            currentChar.ToString(), startPos + 1, currentPos + 1));
                        currentPos++;
                    }
                    continue;
                }

                // Скобки и другие символы
                switch (currentChar)
                {
                    case '(':
                        tokens.Add(new Token(TokenType.OpenParenthesis, "Открывающая скобка", "(", startPos + 1, currentPos + 1));
                        currentPos++;
                        break;
                    case ')':
                        tokens.Add(new Token(TokenType.CloseParenthesis, "Закрывающая скобка", ")", startPos + 1, currentPos + 1));
                        currentPos++;
                        break;
                    case ',':
                        tokens.Add(new Token(TokenType.Comma, "Разделитель", ",", startPos + 1, currentPos + 1));
                        currentPos++;
                        break;
                    case ';':
                        tokens.Add(new Token(TokenType.Semicolon, "Конец оператора", ";", startPos + 1, currentPos + 1));
                        currentPos++;
                        break;
                    case '=':
                        tokens.Add(new Token(TokenType.AssignmentOperator, "Оператор присваивания", "=", startPos + 1, currentPos + 1));
                        currentPos++;
                        break;
                    default:
                        tokens.Add(new Token(TokenType.Invalid, "Недопустимый символ", currentChar.ToString(), startPos + 1, currentPos + 1));
                        currentPos++;
                        break;
                }
            }

            return tokens;
        }
    }
}