namespace laba1
{
    public class Token
    {
        public string Lexeme { get; set; }          // Сам текст лексемы
        public int StartPosition { get; set; }      // Позиция начала в тексте
        public int EndPosition { get; set; }        // Позиция конца в тексте (необязательная, но может быть полезна)
        public TokenType Type { get; set; }         // Тип токена (из enum)

        public Token(string lexeme, int start, int end, TokenType type)
        {
            Lexeme = lexeme;
            StartPosition = start;
            EndPosition = end;
            Type = type;
        }
    }
}