using System.Collections.Generic;

namespace laba1
{
    public class Lexer
    {
        private readonly string _input;
        private int _position;
        private readonly List<Token> _tokens;

        private static readonly Dictionary<string, (int code, string type)> Keywords = new Dictionary<string, (int, string)>
        {
            {"operation", (14, "ключевое слово")},
            {"var", (14, "ключевое слово")},
            {"int", (14, "ключевое слово")}
        };

        private static readonly Dictionary<string, (int code, string type)> Operators = new Dictionary<string, (int, string)>
        {
            {"=", (10, "оператор присваивания")},
            {"+", (12, "арифметический оператор")},
            {"-", (12, "арифметический оператор")},
            {"*", (12, "арифметический оператор")},
            {"/", (12, "арифметический оператор")},
            {"%", (12, "арифметический оператор")},
            {"->", (13, "лямбда-оператор")}
        };

        private static readonly Dictionary<char, (int code, string type)> Separators = new Dictionary<char, (int, string)>
        {
            {' ', (11, "разделитель")},
            {'\t', (11, "разделитель")},
            {'\n', (11, "разделитель")},
            {'\r', (11, "разделитель")},
            {'(', (15, "открывающая скобка")},
            {')', (15, "закрывающая скобка")},
            {'{', (15, "открывающая фигурная скобка")},
            {'}', (15, "закрывающая фигурная скобка")},
            {'[', (15, "открывающая квадратная скобка")},
            {']', (15, "закрывающая квадратная скобка")},
            {',', (15, "разделитель аргументов")},
            {';', (16, "конец оператора")},
            {':', (15, "двоеточие")},
            {'?', (15, "вопросительный знак")},
            {'.', (15, "точка")}
        };

        public Lexer(string input)
        {
            _input = input;
            _position = 0;
            _tokens = new List<Token>();
        }

        public List<Token> Tokenize()
        {
            while (_position < _input.Length)
            {
                var current = Peek();

                if (char.IsWhiteSpace(current))
                {
                    ReadWhiteSpace();
                }
                else if (char.IsLetter(current))
                {
                    ReadIdentifierOrKeyword();
                }
                else if (char.IsDigit(current))
                {
                    ReadNumber();
                }
                else if (current == '-' && Peek(1) == '>')
                {
                    ReadLambdaArrow();
                }
                else if (Operators.ContainsKey(current.ToString()))
                {
                    ReadOperator();
                }
                else if (Separators.ContainsKey(current))
                {
                    ReadSeparator();
                }
                else
                {
                    ReadInvalidCharacter();
                }
            }

            return _tokens;
        }

        private char Peek(int offset = 0)
        {
            return _position + offset < _input.Length ? _input[_position + offset] : '\0';
        }

        private char Read()
        {
            return _position < _input.Length ? _input[_position++] : '\0';
        }

        private void ReadWhiteSpace()
        {
            var start = _position;
            while (_position < _input.Length && char.IsWhiteSpace(Peek()))
            {
                _position++;
            }
            var lexeme = _input.Substring(start, _position - start);
            _tokens.Add(new Token(11, "разделитель", lexeme, $"{start + 1}-{_position}"));
        }

        private void ReadIdentifierOrKeyword()
        {
            var start = _position;
            while (_position < _input.Length && (char.IsLetterOrDigit(Peek()) || Peek() == '_'))
            {
                _position++;
            }
            var lexeme = _input.Substring(start, _position - start);

            if (Keywords.TryGetValue(lexeme, out var keywordInfo))
            {
                _tokens.Add(new Token(keywordInfo.code, keywordInfo.type, lexeme, $"{start + 1}-{_position}"));
            }
            else
            {
                _tokens.Add(new Token(2, "идентификатор", lexeme, $"{start + 1}-{_position}"));
            }
        }

        private void ReadNumber()
        {
            var start = _position;
            while (_position < _input.Length && char.IsDigit(Peek()))
            {
                _position++;
            }
            var lexeme = _input.Substring(start, _position - start);
            _tokens.Add(new Token(1, "целое число", lexeme, $"{start + 1}-{_position}"));
        }

        private void ReadLambdaArrow()
        {
            var start = _position;
            _position += 2; // Пропускаем ->
            var lexeme = _input.Substring(start, 2);
            _tokens.Add(new Token(13, "лямбда-оператор", lexeme, $"{start + 1}-{_position}"));
        }

        private void ReadOperator()
        {
            var start = _position;
            var current = Read().ToString();
            var lexeme = current;

            // Проверка на составные операторы
            if (_position < _input.Length && Operators.ContainsKey(current + Peek()))
            {
                lexeme += Read();
            }

            _tokens.Add(new Token(Operators[lexeme].code, Operators[lexeme].type, lexeme, $"{start + 1}-{_position}"));
        }

        private void ReadSeparator()
        {
            var start = _position;
            var current = Read();
            _tokens.Add(new Token(Separators[current].code, Separators[current].type, current.ToString(), $"{start + 1}-{_position}"));
        }

        private void ReadInvalidCharacter()
        {
            var start = _position;
            var current = Read();
            _tokens.Add(new Token(99, "недопустимый символ", current.ToString(), $"{start + 1}-{_position}"));
        }
    }
}
