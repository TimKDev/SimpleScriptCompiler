using EntertainingErrors;
using SimpleScript.Parser.Nodes;
using SimpleScript.Parser.Nodes.Interfaces;

namespace SimpleScript.Parser
{
    public class Scope
    {
        private readonly Dictionary<string, ScopeVariableEntry> _variables = [];
        private readonly Scope? _parentScope;
        private int currentTempVariableNumber;

        public Scope(Scope? parentScope = null)
        {
            _parentScope = parentScope;
        }

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
            Result<ScopeVariableEntry> scopeVariableEntryResult =
                GetScopeForExpression(variableDeclarationNode.InitialValue);

            if (!scopeVariableEntryResult.IsSuccess)
            {
                return scopeVariableEntryResult;
            }

            _variables[variableDeclarationNode.VariableName] = scopeVariableEntryResult.Value;

            return scopeVariableEntryResult;
        }

        public Result<ScopeVariableEntry> AddVariableScopeEntry(InputNode inputNode)
        {
            ScopeVariableEntry scopeVariableEntryResult = new(ValueTypes.String, InputNode.CharLength);
            _variables[inputNode.VariableName] = scopeVariableEntryResult;

            return scopeVariableEntryResult;
        }

        public Result<ScopeVariableEntry> AddVariableScopeEntry(FunctionNode functionNode)
        {
            ScopeVariableEntry scopeVariableEntryResult = new(ValueTypes.Function, 0);
            _variables[functionNode.Name] = scopeVariableEntryResult;

            return scopeVariableEntryResult;
        }

        public Result<ScopeVariableEntry> AddVariableScopeEntry(FunctionArgumentNode functionArgumentNode)
        {
            ScopeVariableEntry scopeVariableEntryResult =
                new(functionArgumentNode.ArgumentType == ArgumentType.Int ? ValueTypes.Number : ValueTypes.String, 0,
                    true);
            _variables[functionArgumentNode.ArgumentName] = scopeVariableEntryResult;

            return scopeVariableEntryResult;
        }

        public Result<ScopeVariableEntry> GetScopeForExpression(IExpression expression)
        {
            return expression switch
            {
                StringNode stringNode => new ScopeVariableEntry(StringNode.TypeName,
                    stringNode.Value.Length + 1), // + 1 is necessary to also include the \0 Null Character! 
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

        private Result<ScopeVariableEntry> EvaluateVariable(VariableNode variableNode)
        {
            if (_variables.TryGetValue(variableNode.Name, out var scopeVariableEntry))
            {
                return scopeVariableEntry;
            }

            return _parentScope is not null
                ? _parentScope.EvaluateVariable(variableNode)
                : variableNode.CreateError($"Unknown Variable {variableNode.Name} cannot be used in expression");
        }

        //This function also works for multiplication, because it is only needed for strings and for strings there is no multiplication allowed.
        private Result<ScopeVariableEntry> EvaluateBinaryOperation<T>(IBinaryOperation<T> node) where T : IExpression
        {
            var variableScopeFirstArg = GetScopeForExpression(node.FirstArgument);
            var variableScopeSecondArg = GetScopeForExpression(node.SecondArgument);

            if (!variableScopeFirstArg.IsSuccess || !variableScopeSecondArg.IsSuccess)
            {
                return variableScopeFirstArg.Merge(variableScopeSecondArg);
            }

            var firstArg = variableScopeFirstArg.Value;
            var secondArg = variableScopeSecondArg.Value;
            //Check for type compatibility:
            if (firstArg.ValueType == secondArg.ValueType)
            {
                //For numbers the Length is calculated, but it has no meaning!
                return new ScopeVariableEntry(firstArg.ValueType, firstArg.Lenght + secondArg.Lenght - 1,
                    firstArg.HeapAllocation ||
                    secondArg.HeapAllocation); // -1 because the null character is only needed once for each string. 
            }

            return node.CreateError($"Types {firstArg} and {secondArg} are not compatible.");
        }
    }
}