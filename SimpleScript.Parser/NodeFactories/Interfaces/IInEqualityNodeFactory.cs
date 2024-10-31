using EntertainingErrors;
using SimpleScript.Lexer;
using SimpleScript.Parser.NodeFactories.Interfaces;
using SimpleScript.Parser.Nodes;

namespace SimpleScript.Parser.NodeFactories;

public interface IInEqualityNodeFactory : IBinaryNodeFactory<InEqulityNode>;