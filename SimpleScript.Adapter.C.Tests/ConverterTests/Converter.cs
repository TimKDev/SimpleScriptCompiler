using FluentAssertions;
using SimpleScript.Parser.Nodes;

namespace SimpleScript.Converter.C.Tests.ConverterTests
{
    public class Converter
    {
        private readonly ConverterToCCode _sut = new();

        [Fact]
        public void ShouldConvertHelloWorldC_GivenHelloWorldAst()
        {
            ProgramNode helloWorldProgramNode = new();
            helloWorldProgramNode.ChildNodes.Add(PrintNode.Create(new StringNode("Hello Simple Script Compiler", 1, 1)).Value);

            string expectedResult = @"
                #include <stdio.h>
                int main() {
                    printf(""Hello World!\n"");
                    return 0;
                }
            ";

            string result = _sut.ConvertToCCode(helloWorldProgramNode);
            result.Should().Be(expectedResult);
        }
    }
}