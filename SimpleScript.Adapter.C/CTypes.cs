namespace SimpleScript.Adapter.C
{
    //public class FunctionDeclarationInC
    //{
    //    private CType ReturnType;
    //    private string FunctionName;
    //    private readonly CFunctionArgument[] FunctionArguments;
    //    private string[] Body;
    //    public functionDeclarationInC(string functionName, CType returnType, CFunctionArgument[] functionArguments, string[] body)
    //    {
    //        ReturnType = returnType;
    //        FunctionArguments = functionArguments;
    //        Body = body;
    //        FunctionName = functionName;
    //    }
    //    public static FunctionDeclarationInC Create(FunctionNode functionNode)
    //    {
    //        //function Name und Arg können direkt übersetzt werden
    //        CFunctionArgument[] functionArgs = functionNode.Arguments.Select(CFunctionArgument.Create).ToArray();
    //        //Body wird analog ausgewertet wie eine Main Function, wobei sich der Scope in der ersten Version nur aus den Argument Parametern der Funktion zusammensetzt
    //        Scope functionScope = new();

    //    }


    //}
    public enum CTypes
    {
        Integer,
        String
    }
}



