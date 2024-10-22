namespace SimpleScript.Parser
{
    //TTODO hier werden zu viele unterschiedliche Klassen vermischt. Lieber eine Klasse für String, Function, Number usw.
    public record ScopeVariableEntry(
        ValueTypes ValueType,
        int Lenght = 0,
        bool HeapAllocation = false,
        bool? IsFunction = null);
}