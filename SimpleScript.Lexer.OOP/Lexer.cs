namespace SimpleScript.Lexer.OOP;

public class Lexer
{
    public TokenizedProgram Tokenize(string source)
    {
        var result = new TokenizedProgram();
        if (source is ['"', .., '"'])
        {
            var stringValue = source[1..^1];
            result.AddTokens([new StringToken(stringValue, 1)]);
        }

        return result;
    }
}

public static class EnumerableExtensions
{
    public static void ForEach<T>(this IEnumerable<T> source, Action<T, int> action)
    {
        var index = 0;
        foreach (var item in source)
        {
            action(item, index);
            index++;
        }
    }
}

public class SourceProgram
{
    private readonly string _source;

    public SourceProgram(string source)
    {
        _source = source;
    }

    public string[] SplitByLines() => _source.Split('\n');
}

public class StringSplitter
{
    public UntypedToken[] SplitString(SourceProgram source)
    {
        var result = new List<UntypedToken>();
        var isInString = false;
        var currentToken = string.Empty;
        source
            .SplitByLines()
            .ForEach((lineOfCode, lineNumber) =>
            {
                foreach (var currentChar in lineOfCode)
                {
                    if (isInString)
                    {
                        currentToken += currentChar;
                    }

                    if (currentChar == '"')
                    {
                        if (isInString)
                        {
                            result.Add(new UntypedToken(currentToken, lineNumber));
                        }

                        isInString = !isInString;
                    }
                }
            });

        return result.ToArray();
    }
}

public class UntypedToken
{
    public int Line { get; }
    public string Value { get; }

    public UntypedToken(string value, int line)
    {
        Line = line;
        Value = value;
    }
}