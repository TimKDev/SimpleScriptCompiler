using EntertainingErrors;
using SimpleScript.Parser.Nodes;
using SimpleScript.Parser.Nodes.Interfaces;

namespace SimpleScript.Parser
{
    public record ScopeVariableEntry(ValueTypes ValueType, int Lenght = 0);
    public class Scope
    {
        private readonly Dictionary<string, ScopeVariableEntry> _variables = [];
        private readonly Scope? _parentScope;
        private int currentTempVariableNumber = 0;

        public Scope(Scope? parentScope = null) => _parentScope = parentScope;

        public bool DoesVariableNameExists(string variableName)
        {
            if (_variables.ContainsKey(variableName))
            {
                return true;
            }

            if (_parentScope is not null && _parentScope.DoesVariableNameExists(variableName))
            {
                return true;
            }

            return false;
        }

        public Result<ScopeVariableEntry> GetScopeEntry(string variableName)
        {
            if (_variables.TryGetValue(variableName, out ScopeVariableEntry? value) && value != null)
            {
                return value;
            }

            if (_parentScope != null)
            {
                return _parentScope.GetScopeEntry(variableName);
            }

            return Error.Create($"Variable {variableName} not found in scope.");
        }

        public Result<ScopeVariableEntry> AddOrUpdateVariableScopeEntry(VariableDeclarationNode variableDeclarationNode)
        {
            Result<ScopeVariableEntry> scopeVariableEntryResult = GetScopeForExpression(variableDeclarationNode.InitialValue);

            if (!scopeVariableEntryResult.IsSuccess)
            {
                return scopeVariableEntryResult;
            }

            _variables[variableDeclarationNode.VariableName] = scopeVariableEntryResult.Value;

            return scopeVariableEntryResult;
        }

        public Result<ScopeVariableEntry> GetScopeForExpression(IExpression expression)
        {
            return expression switch
            {
                StringNode stringNode => new ScopeVariableEntry(StringNode.TypeName, stringNode.Value.Length),
                NumberNode => new ScopeVariableEntry(NumberNode.TypeName),
                VariableNode variableNode => EvaluateVariable(variableNode),
                AddNode addNode => EvaluateBinaryOperation(addNode),
                MultiplyNode multiplyNode => EvaluateBinaryOperation(multiplyNode),
                _ => throw new NotImplementedException()
            };
        }

        public string GetTempVariableName()
        {
            currentTempVariableNumber++;
            string result = $"temp_{currentTempVariableNumber}";
            while (DoesVariableNameExists(result))
            {
                currentTempVariableNumber++;
                result = $"temp_{currentTempVariableNumber}";
            }
            return result;
        }

        protected Result<ScopeVariableEntry> EvaluateVariable(VariableNode variableNode)
        {
            if (_variables.TryGetValue(variableNode.Name, out ScopeVariableEntry? scopeVariableEntry) && scopeVariableEntry != null)
            {
                return scopeVariableEntry;
            }
            if (_parentScope is not null)
            {
                return _parentScope.EvaluateVariable(variableNode);
            }
            return variableNode.CreateError($"Unknown Variable {variableNode.Name} cannot be used in expression");
        }

        private Result<ScopeVariableEntry> EvaluateBinaryOperation<T>(IBinaryOperation<T> node) where T : IExpression
        {
            Result<ScopeVariableEntry> variableScopeFirstArg = GetScopeForExpression(node.FirstArgument);
            Result<ScopeVariableEntry> variableScopeSecondArg = GetScopeForExpression(node.SecondArgument);

            if (variableScopeFirstArg.IsSuccess && variableScopeSecondArg.IsSuccess)
            {
                //Check for type compatibility:
                if (variableScopeFirstArg.Value.ValueType == variableScopeSecondArg.Value.ValueType)
                {
                    return new ScopeVariableEntry(variableScopeFirstArg.Value.ValueType, variableScopeFirstArg.Value.Lenght + variableScopeSecondArg.Value.Lenght);
                }

                return node.CreateError($"Types {variableScopeFirstArg.Value} and {variableScopeSecondArg.Value} are not compatible.");
            }

            return variableScopeFirstArg.Merge(variableScopeSecondArg);
        }
    }
}