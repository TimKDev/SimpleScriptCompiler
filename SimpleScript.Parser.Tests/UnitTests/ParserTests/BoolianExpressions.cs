using SimpleScript.Parser.Interfaces;
using SimpleScript.Parser.Tests.Helper.Factories;

namespace SimpleScript.Parser.Tests.UnitTests.ParserTests;

public class BooleanExpressions
{
    private readonly IParser _sut = ParserFactory.Create();

    //LET isCool = TRUE 

    //LET isDev = FALSE

    //LET areNumbersEqual = x == 7 * 3

    //LET areNumbersEqual = x != 7 * 3

    //LET areNumbersEqual = x < 7 * 3

    //LET areNumbersEqual = x > 7 * 3

    //LET areNumbersEqual = x >= 7 * 3

    //LET areNumbersEqual = x <= 7 * 3
    
    //LET areStringsEqual = name == "Caro"
    
    //LET areNotStringsEqual = name != "Caro"
}