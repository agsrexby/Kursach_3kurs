using System;
using System.Collections.Generic;
using System.Linq;

namespace laba1
{
    public class SyntaxAnalyzer
    {
        private List<LexicalAnalyzer.Token> _tokens;
        private int _currentPosition;
        private List<string> _errors;
        private Stack<string> _expectedTokens = new Stack<string>();

        private LexicalAnalyzer.Token CurrentToken => 
            _currentPosition < _tokens.Count ? _tokens[_currentPosition] : null;

        public class ParseResult
        {
            public bool IsValid { get; set; }
            public List<string> Errors { get; set; } = new List<string>();
            public List<string> RecoveryMessages { get; set; } = new List<string>();
        }

        public ParseResult Parse(List<LexicalAnalyzer.Token> tokens)
        {
            _tokens = tokens ?? new List<LexicalAnalyzer.Token>();
            _currentPosition = 0;
            _errors = new List<string>();
            _expectedTokens.Clear();

            try
            {
                ParseLambdaExpression();

                // Проверка конца выражения
                if (CurrentToken != null && !IsTokenType(CurrentToken, LexicalAnalyzer.TokenType.Semicolon))
                {
                    AddError($"Ожидается ';' в конце выражения", CurrentToken.StartPos);
                    RecoveryInsert(";");
                }
            }
            catch (Exception ex)
            {
                AddError($"Критическая ошибка анализа: {ex.Message}", 
                        CurrentToken?.StartPos ?? _tokens.LastOrDefault()?.EndPos ?? 0);
            }

            return new ParseResult 
            { 
                IsValid = !_errors.Any(),
                Errors = _errors,
                RecoveryMessages = _expectedTokens.Count > 0 ? 
                    new List<string> { $"Выполнены восстановительные действия: {string.Join(", ", _expectedTokens)}" } : 
                    new List<string>()
            };
        }

        private bool IsTokenType(LexicalAnalyzer.Token token, LexicalAnalyzer.TokenType expectedType)
        {
            return token != null && token.Type == expectedType;
        }

        private void ParseLambdaExpression()
        {
            // Ожидаем: <identifier> = <lambda>
            if (!IsTokenType(CurrentToken, LexicalAnalyzer.TokenType.Identifier))
            {
                AddError($"Ожидается идентификатор", CurrentToken?.StartPos ?? 1);
                RecoveryInsert("<identifier>");
                return;
            }
            MoveNext();

            if (!IsTokenType(CurrentToken, LexicalAnalyzer.TokenType.AssignmentOperator))
            {
                AddError($"Ожидается '='", CurrentToken?.StartPos ?? 1);
                RecoveryInsert("=");
                return;
            }
            MoveNext();

            ParseLambdaBody();
        }

        private void ParseLambdaBody()
        {
            // Ожидаем: ( <params> ) -> <expression>
            if (!IsTokenType(CurrentToken, LexicalAnalyzer.TokenType.OpenParenthesis))
            {
                AddError($"Ожидается '('", CurrentToken?.StartPos ?? 1);
                RecoveryInsert("(");
                return;
            }
            MoveNext();

            ParseParameters();

            if (!IsTokenType(CurrentToken, LexicalAnalyzer.TokenType.CloseParenthesis))
            {
                AddError($"Ожидается ')'", CurrentToken?.StartPos ?? 1);
                RecoveryInsert(")");
                return;
            }
            MoveNext();

            if (!IsTokenType(CurrentToken, LexicalAnalyzer.TokenType.LambdaOperator))
            {
                AddError($"Ожидается '->'", CurrentToken?.StartPos ?? 1);
                RecoveryInsert("->");
                return;
            }
            MoveNext();

            ParseExpression();
        }

        private void ParseParameters()
        {
            // Ожидаем: <id> [, <id>]*
            if (!IsTokenType(CurrentToken, LexicalAnalyzer.TokenType.Identifier))
            {
                AddError($"Ожидается параметр", CurrentToken?.StartPos ?? 1);
                RecoveryInsert("<param>");
                return;
            }
            MoveNext();

            while (IsTokenType(CurrentToken, LexicalAnalyzer.TokenType.Comma))
            {
                MoveNext();

                if (!IsTokenType(CurrentToken, LexicalAnalyzer.TokenType.Identifier))
                {
                    AddError($"Ожидается параметр после запятой", CurrentToken?.StartPos ?? 1);
                    RecoveryInsert("<param>");
                    return;
                }
                MoveNext();
            }
        }

        private void ParseExpression()
        {
            // Ожидаем: <term> [<op> <term>]*
            if (CurrentToken == null || 
               (!IsTokenType(CurrentToken, LexicalAnalyzer.TokenType.Identifier) &&
                !IsTokenType(CurrentToken, LexicalAnalyzer.TokenType.Number) &&
                !IsTokenType(CurrentToken, LexicalAnalyzer.TokenType.OpenParenthesis)))
            {
                AddError($"Ожидается выражение", CurrentToken?.StartPos ?? 1);
                RecoveryInsert("<expression>");
                return;
            }

            // Обработка вложенных выражений
            if (IsTokenType(CurrentToken, LexicalAnalyzer.TokenType.OpenParenthesis))
            {
                MoveNext();
                ParseExpression();

                if (!IsTokenType(CurrentToken, LexicalAnalyzer.TokenType.CloseParenthesis))
                {
                    AddError($"Ожидается ')'", CurrentToken?.StartPos ?? 1);
                    RecoveryInsert(")");
                    return;
                }
                MoveNext();
            }
            else
            {
                MoveNext();
            }

            // Обработка операторов
            if (CurrentToken != null && 
               (IsTokenType(CurrentToken, LexicalAnalyzer.TokenType.ArithmeticOperator) ||
                IsTokenType(CurrentToken, LexicalAnalyzer.TokenType.ComparisonOperator)))
            {
                MoveNext();
                ParseExpression();
            }
        }

        private void RecoveryInsert(string expected)
        {
            // Метод Айронса: вставляем ожидаемый токен и продолжаем анализ
            _expectedTokens.Push(expected);
            
            // В реальной реализации можно попытаться продолжить анализ
            // с текущей позиции, пропустив ошибочный токен
            if (CurrentToken != null)
            {
                MoveNext();
            }
        }

        private void AddError(string message, int position)
        {
            _errors.Add($"Позиция {position}: {message}");
        }

        private void MoveNext()
        {
            _currentPosition++;
        }
    }
}