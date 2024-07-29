using EntertainingErrors;
using SimpleScript.Parser.Nodes.Interfaces;

namespace SimpleScript.Parser.Nodes
{
    public class VariableNode : NodeBase, IExpression, IMultiplyable, IPrintableNode, IAddable
    {
        public string Name { get; private set; }
        public ValueTypes Type { get; init; }
        public VariableNode(string name, int startLine, int endLine) : base(startLine, endLine)
        {
            Name = name;
        }

        public ValueTypes GetValueType(Scope variableScope)
        {

        }
    }

    public class Scope
    {
        private Dictionary<string, ScopeVariableEntry> _variables = [];
        private Scope? _parentScope;

        public Scope(Scope? parentScope = null)
        {
        }

        public Result AddVariable(VariableDeclarationNode variableDeclarationNode)
        {
            Result<ScopeVariableEntry> scopeVariableEntryResult = variableDeclarationNode.InitialValue switch
            {
                StringNode stringNode => new ScopeVariableEntry(stringNode, StringNode.TypeName),
                NumberNode numberNode => new ScopeVariableEntry(numberNode, NumberNode.TypeName),
                VariableNode variableNode => EvaluateVariable(variableNode),
                AddNode addNode =>
                _ => throw new NotImplementedException()
            };

            if (!scopeVariableEntryResult.IsSuccess)
            {
                return scopeVariableEntryResult.Convert();
            }

            _variables[variableDeclarationNode.VariableName] = scopeVariableEntryResult.Value;

            return Result.Success();
        }

        protected Result<ScopeVariableEntry> EvaluateVariable(VariableNode variableNode)
        {
            if (_variables.TryGetValue(variableNode.Name, out ScopeVariableEntry? scopeVariableEntry) && scopeVariableEntry is not null)
            {
                return scopeVariableEntry;
            }
            if (_parentScope is not null)
            {
                return _parentScope.EvaluateVariable(variableNode);
            }
            return variableNode.CreateError($"Unknown Variable {variableNode.Name} cannot be used in expression");
        }

        //public Result ChangeVariableValue(string variableName, IExpression newValue)
        //{
        //    //Überprüfe, ob die Variable bereits definiert ist => Sonst Throw Error (dies kann per Definition in meiner Sprache nicht passieren, da man immer LET verwendet!)
        //    //Werte rekursiv die Expression aus, um den aktuellen Wert ohne Variablen zu bekommen => Alle Teile der Expression müssen definiert sein
        //    //Bei dieser Auswertum wird auch der Typ der Variablen bestimmt und es wird überprüft, dass alle verwendeten Variablen bereits deklariert wurden und das die Typen in den Operationen zusammenpassen. 
        //}

        public Result<ValueTypes> GetValueType(string variableName)
        {
            return ValueTypes.String;
        }
    }

    public class ScopeVariableEntry
    {
        public IExpression Value { get; set; }
        public ValueTypes ValueType { get; set; }

        public ScopeVariableEntry(IExpression value, ValueTypes valueType)
        {
            Value = value;
            ValueType = valueType;
        }
    }
}