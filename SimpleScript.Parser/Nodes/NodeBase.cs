using EntertainingErrors;
using SimpleScript.Parser.Nodes.Interfaces;

namespace SimpleScript.Parser.Nodes
{
    public abstract class NodeBase : INodeBase
    {
        public int StartLine { get; private set; }
        public int EndLine { get; private set; }

        protected NodeBase(int startLine, int endLine)
        {
            StartLine = startLine;
            EndLine = endLine;
        }

        public Error CreateError(string message)
        {
            return CreateError(message, StartLine, EndLine);
        }

        protected static Error CreateError(string message, int startLine, int endLine)
        {
            string errorMessage = $"Error Line {startLine}: {message}";
            if (startLine != endLine)
            {
                errorMessage = $"Error Lines {startLine} - {endLine}: {message}";
            }
            return Error.Create(errorMessage);
        }
    }
}