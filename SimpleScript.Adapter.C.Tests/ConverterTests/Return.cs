using EntertainingErrors;
using FluentAssertions;
using SimpleScript.Adapter.C.Tests.Helper.Extensions;
using SimpleScript.Adapter.C.Tests.Helper.Factories;
using SimpleScript.Parser;

namespace SimpleScript.Adapter.C.Tests.ConverterTests
{
    public class Return
    {
        [Fact]
        public void GivenNumberToReturn_ShouldConvertToReturnString()
        {
            var numberExpression = NumberNodeFactory.Create(45, 1, 1);
            var returnNode = ReturnNodeFactory.Create(numberExpression);
            var scope = ScopeFactory.Create([]);
            var result = ConvertReturnNodeToC.Convert(returnNode, scope);
            var expectedValue = "return 45;";
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().HaveCount(1);
            result.Value[0].Should().Be(expectedValue);
        }

        [Fact]
        public void GivenStringToReturn_ShouldConvertToReturnString()
        {
            var stringExpression = StringNodeFactory.Create("Hallo Welt", 1, 1);
            var returnNode = ReturnNodeFactory.Create(stringExpression);
            var scope = ScopeFactory.Create([]);
            var result = ConvertReturnNodeToC.Convert(returnNode, scope);
            var expectedValue = "return \"Hallo Welt\";";
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().HaveCount(1);
            result.Value[0].Should().Be(expectedValue);
        }

        [Fact]
        public void GivenAdditionOfNumbersToReturn_ShouldConvertToReturnString()
        {
            var addNode = AddNodeFactory.Create(NumberNodeFactory.Create(1, 1, 2), NumberNodeFactory.Create(2, 1, 2));
            var returnNode = ReturnNodeFactory.Create(addNode);
            var scope = ScopeFactory.Create([]);
            var result = ConvertReturnNodeToC.Convert(returnNode, scope);
            var expectedValue = "return (1 + 2);";
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().HaveCount(1);
            result.Value[0].Should().Be(expectedValue);
        }

        [Fact]
        public void GivenAdditionOfStringsToReturn_ShouldConvertToReturnString()
        {
            var addNode = AddNodeFactory.Create(StringNodeFactory.Create("Hello", 1, 2),
                StringNodeFactory.Create("World", 1, 2));
            var returnNode = ReturnNodeFactory.Create(addNode);
            var scope = ScopeFactory.Create([]);
            var result = ConvertReturnNodeToC.Convert(returnNode, scope);
            string[] expectedValue =
            [
                "char temp_1[11];",
                "strcpy(temp_1, \"Hello\");",
                "strcat(temp_1, \"World\");",
                "return temp_1;",
            ];
            AssertReturnNode(result, expectedValue);
        }

        [Fact]
        public void GivenAdditionOfInputStringsToReturn_ShouldConvertToReturnString()
        {
            var firstArg = "string_1";
            var secondArg = "string_2";
            var addNode = AddNodeFactory.Create(VariableNodeFactory.Create(firstArg, 1, 2),
                VariableNodeFactory.Create(secondArg, 1, 2));
            var returnNode = ReturnNodeFactory.Create(addNode);
            var scope = ScopeFactory.Create([], [
                FunctionArgumentFactory.Create(ArgumentType.String, firstArg, 1, 1),
                FunctionArgumentFactory.Create(ArgumentType.String, secondArg, 1, 1),
            ]);
            var result = ConvertReturnNodeToC.Convert(returnNode, scope);
            string[] expectedValue =
            [
                "char *temp_1 = (char *)malloc((strlen(string_1) + strlen(string_2) + 1) * sizeof(char));",
                "add_to_list(temp_1);",
                "strcpy(temp_1, string_1);",
                "strcat(temp_1, string_2);",
                "return temp_1;",
            ];
            AssertReturnNode(result, expectedValue);
        }

        private void AssertReturnNode(Result<string[]> result, string[] expectedValue)
        {
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().HaveCount(expectedValue.Length);
            for (var i = 0; i < expectedValue.Length; i++)
            {
                result.Value[i].AssertWithoutWhitespace(expectedValue[i]);
            }
        }
    }
}