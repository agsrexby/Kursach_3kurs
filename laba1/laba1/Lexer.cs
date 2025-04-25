using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace laba1
{
    public enum TokenType
    {
        Keyword,
        Identifier,
        Number,
        Whitespace,
        OpenParen,
        CloseParen,
        OpenBrace,
        CloseBrace,
        Equals,
        Comma,
        Assignment,
        Operation,
        Semicolon,
        Newline,
        Invalid
    }

    public class Lexer
    {
        private static readonly string[] Keywords = { "operation", "var", "int" };

        public static List<Token> Tokenize(string expr)
        {
            List<Token> tokens = new List<Token>();

            if (string.IsNullOrEmpty(expr))
                return tokens;

            int start = 0;
            int end = 0;
            int length = expr.Length;

            while (end < length)
            {
                start = end;
                char current = expr[end];
                TokenType type = TokenType.Invalid;

                if (char.IsLetter(current))
                {
                    while (end < length &&
                           (char.IsLetterOrDigit(expr[end]) || expr[end] == '_'))
                    {
                        end++;
                    }
                    type = TokenType.Identifier;

                    string value = expr.Substring(start, end - start);
                    foreach (var keyword in Keywords)
                    {
                        if (value == keyword)
                        {
                            type = TokenType.Keyword;
                            break;
                        }
                    }
                }
                else if (char.IsDigit(current))
                {
                    while (end < length &&
                           (char.IsDigit(expr[end]) || expr[end] == '.'))
                    {
                        end++;
                    }
                    type = TokenType.Number;
                }
                else
                {
                    switch (current)
                    {
                        case ' ':
                            type = TokenType.Whitespace;
                            end++;
                            break;
                        case '(':
                            type = TokenType.OpenParen;
                            end++;
                            break;
                        case ')':
                            type = TokenType.CloseParen;
                            end++;
                            break;
                        case '{':
                            type = TokenType.OpenBrace;
                            end++;
                            break;
                        case '}':
                            type = TokenType.CloseBrace;
                            end++;
                            break;
                        case '=':
                            type = TokenType.Equals;
                            end++;
                            break;
                        case ',':
                            type = TokenType.Comma;
                            end++;
                            break;
                        case '-':
                            if (end + 1 < length && expr[end + 1] == '>')
                            {
                                type = TokenType.Assignment;
                                end += 2;
                            }
                            else
                            {
                                type = TokenType.Operation;
                                end++;
                            }

                            break;
                        case '+':
                        case '*':
                        case '/':
                            type = TokenType.Operation;
                            end++;
                            break;
                        case ';':
                            type = TokenType.Semicolon;
                            end++;
                            break;
                        case '\n':
                        case '\r':
                            type = TokenType.Newline;
                            end++;
                            break;
                        default:
                            type = TokenType.Invalid;
                            end++;
                            break;
                    }
                }

                string tokenValue = expr.Substring(start, end - start);
                tokens.Add(new Token(tokenValue, start + 1, end, type));
            }

            return tokens;
        }

        public static string TokenTypeToString(TokenType type)
        {
            switch (type)
            {
                case TokenType.Keyword: return "Ключевое слово";
                case TokenType.Identifier: return "Идентификатор";
                case TokenType.Number: return "Число";
                case TokenType.Whitespace: return "Пробел";
                case TokenType.OpenParen: return "Открывающая скобка";
                case TokenType.CloseParen: return "Закрывающая скобка";
                case TokenType.OpenBrace: return "Открывающая фигурная скобка";
                case TokenType.CloseBrace: return "Закрывающая фигурная скобка";
                case TokenType.Equals: return "Оператор присваивания";
                case TokenType.Comma: return "Запятая";
                case TokenType.Assignment: return "Лямбда-оператор";
                case TokenType.Operation: return "Арифметическая операция";
                case TokenType.Semicolon: return "Точка с запятой";
                case TokenType.Newline: return "Новая строка";
                default: return "Неизвестный";
            }
        }
    }
}
