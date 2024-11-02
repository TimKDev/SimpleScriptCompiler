using EntertainingErrors;
using SimpleScript.Parser.Nodes;
using SimpleScript.Parser.Nodes.Interfaces;

namespace SimpleScript.Parser
{
    public class Scope
    {
        //TODO Design Problem: Wie kann ich garantieren, dass der Adapter für diese global verfügbaren Methoden eine Implementierung bereitstellt?
        private readonly Dictionary<string, ScopeVariableEntry> _variables = new()
        {
            { "ToNumber", new ScopeVariableEntry(ValueTypes.Number, IsFunction: true) }
        };

        private readonly Scope? _parentScope;
        private int _currentTempVariableNumber;

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

        public Result<ScopeVariableEntry> AddVariableScopeEntry(FunctionNode functionNode, ReturnType returnValue)
        {
            var convertedReturnValue = returnValue switch
            {
                ReturnType.Int => ValueTypes.Number,
                ReturnType.String => ValueTypes.String,
                ReturnType.Void => ValueTypes.Void,
                _ => throw new NotImplementedException(),
            };
            ScopeVariableEntry scopeVariableEntryResult = new(convertedReturnValue, 0, IsFunction: true);
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
                BooleanNode => new ScopeVariableEntry(ValueTypes.Boolean),
                VariableNode variableNode => EvaluateVariable(variableNode),
                AddNode addNode => EvaluateBinaryOperation(addNode),
                MinusNode minusNode => EvaluateBinaryOperation(minusNode),
                MultiplyNode multiplyNode => EvaluateBinaryOperation(multiplyNode),
                EqualityNode equalityNode => EvaluateBinaryOperation(equalityNode, ValueTypes.Boolean),
                InEqualityNode inEqualityNode => EvaluateBinaryOperation(inEqualityNode, ValueTypes.Boolean),
                SmallerNode smallerNode => EvaluateBinaryOperation(smallerNode, ValueTypes.Boolean),
                SmallerOrEqualNode smallerOrEqualNode =>
                    EvaluateBinaryOperation(smallerOrEqualNode, ValueTypes.Boolean),
                GreaterNode greaterNode => EvaluateBinaryOperation(greaterNode, ValueTypes.Boolean),
                GreaterOrEqualNode greaterOrEqualNode =>
                    EvaluateBinaryOperation(greaterOrEqualNode, ValueTypes.Boolean),
                FunctionInvocationNode functionInvocationNode => EvaluateFunctionInvocation(functionInvocationNode),
                _ => throw new NotImplementedException()
            };
        }

        private Result<ScopeVariableEntry> EvaluateFunctionInvocation(FunctionInvocationNode functionInvocationNode)
        {
            if (_variables.TryGetValue(functionInvocationNode.FunctionName, out var scopeVariableEntry))
            {
                return scopeVariableEntry;
            }

            return _parentScope is not null
                ? _parentScope.EvaluateFunctionInvocation(functionInvocationNode)
                : functionInvocationNode.CreateError(
                    $"Unknown Variable {functionInvocationNode.FunctionName} cannot be used in expression");
        }

        public string GetTempVariableName()
        {
            _currentTempVariableNumber++;
            string result = $"temp_{_currentTempVariableNumber}";
            while (DoesVariableNameExists(result))
            {
                _currentTempVariableNumber++;
                result = $"temp_{_currentTempVariableNumber}";
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
        private Result<ScopeVariableEntry> EvaluateBinaryOperation<T>(IBinaryOperation<T> node,
            ValueTypes? typeOfBinaryOperation = null) where T : IExpression
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
                return new ScopeVariableEntry(typeOfBinaryOperation ?? firstArg.ValueType,
                    firstArg.Lenght + secondArg.Lenght - 1,
                    firstArg.HeapAllocation ||
                    secondArg.HeapAllocation); // -1 because the null character is only needed once for each string. 
            }

            return node.CreateError($"Types {firstArg.ValueType} and {secondArg.ValueType} are not compatible.");
        }
    }
}