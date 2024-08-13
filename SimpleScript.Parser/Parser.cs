using EntertainingErrors;
using SimpleScript.Lexer;
using SimpleScript.Parser.Interfaces;
using SimpleScript.Parser.NodeFactories.Interfaces;
using SimpleScript.Parser.Nodes;

namespace SimpleScript.Parser
{
    public class Parser : IParser
    {
        private readonly IBodyNodeFactory _bodyNodeFactory;

        public Parser(IBodyNodeFactory bodyNodeFactory)
        {
            this._bodyNodeFactory = bodyNodeFactory;
        }

        public Result<ProgramNode> ParseTokens(List<Token> inputTokens)
        {
            Result<BodyNode> mainBody = _bodyNodeFactory.Create(inputTokens);

            return mainBody.IsSuccess ? new ProgramNode(mainBody.Value) : mainBody.Errors;
        }
    }
}
