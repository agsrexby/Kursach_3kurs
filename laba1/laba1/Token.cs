namespace laba1
{
    public class Token
    {
        public int Code { get; set; }
        public string Type { get; set; }
        public string Lexeme { get; set; }
        public string Position { get; set; }

        public Token(int code, string type, string lexeme, string position)
        {
            Code = code;
            Type = type;
            Lexeme = lexeme;
            Position = position;
        }
    }
}