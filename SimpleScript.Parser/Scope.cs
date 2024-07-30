using EntertainingErrors;
using SimpleScript.Parser.Nodes;
using SimpleScript.Parser.Nodes.Interfaces;

namespace SimpleScript.Parser
{
    public class Scope
    {
        private Dictionary<string, ValueTypes> _variables = [];
        private Scope? _parentScope;

        public Scope(Scope? parentScope = null)
        {
            _parentScope = parentScope;
        }

        public Result<ValueTypes> GetValueType(string variableName)
        {
            if (_variables.TryGetValue(variableName, out ValueTypes value))
            {
                return value;
            }

            if (_parentScope != null)
            {
                return _parentScope.GetValueType(variableName);
            }

            return Error.Create($"Variable {variableName} not found in scope.");
        }

        public Result<ValueTypes> AddVariable(VariableDeclarationNode variableDeclarationNode)
        {
            Result<ValueTypes> scopeVariableEntryResult = GetScopeForExpression(variableDeclarationNode.InitialValue);

            if (!scopeVariableEntryResult.IsSuccess)
            {
                return scopeVariableEntryResult;
            }

            _variables[variableDeclarationNode.VariableName] = scopeVariableEntryResult.Value;

            return scopeVariableEntryResult;
        }

        protected Result<ValueTypes> EvaluateVariable(VariableNode variableNode)
        {
            if (_variables.TryGetValue(variableNode.Name, out ValueTypes scopeVariableEntry))
            {
                return scopeVariableEntry;
            }
            if (_parentScope is not null)
            {
                return _parentScope.EvaluateVariable(variableNode);
            }
            return variableNode.CreateError($"Unknown Variable {variableNode.Name} cannot be used in expression");
        }

        private Result<ValueTypes> EvaluateBinaryOperation<T>(IBinaryOperation<T> node) where T : IExpression
        {
            Result<ValueTypes> variableScopeFirstArg = GetScopeForExpression(node.FirstArgument);
            Result<ValueTypes> variableScopeSecondArg = GetScopeForExpression(node.SecondArgument);

            if (variableScopeFirstArg.IsSuccess && variableScopeSecondArg.IsSuccess)
            {
                if (variableScopeFirstArg.Value == variableScopeSecondArg.Value)
                {
                    return variableScopeFirstArg.Value;
                }

                return node.CreateError($"Types {variableScopeFirstArg.Value} and {variableScopeSecondArg.Value} are not compatible.");
            }

            return variableScopeFirstArg.Merge(variableScopeSecondArg);
        }

        private Result<ValueTypes> GetScopeForExpression(IExpression expression)
        {
            return expression switch
            {
                StringNode => StringNode.TypeName,
                NumberNode => NumberNode.TypeName,
                VariableNode variableNode => EvaluateVariable(variableNode),
                AddNode addNode => EvaluateBinaryOperation(addNode),
                MultiplyNode multiplyNode => EvaluateBinaryOperation(multiplyNode),
                _ => throw new NotImplementedException()
            };
        }
    }
}