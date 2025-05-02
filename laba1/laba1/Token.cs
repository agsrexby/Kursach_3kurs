namespace laba1;

public enum TokenType
{
    Identifier,
    Assign,
    OpenParen,
    CloseParen,
    Comma,
    LambdaArrow,
    Operator,
    Semicolon,
    Whitespace,
    Unknown
}

public class Token
{
    public TokenType Type { get; set; }
    public string Value { get; set; }
    public int Position { get; set; }

    public Token(TokenType type, string value, int position)
    {
        Type = type;
        Value = value;
        Position = position;
    }

    public override string ToString() => $"{Type}('{Value}') at {Position}";
}