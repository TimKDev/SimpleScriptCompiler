namespace SimpleScript.Adapter.C;

public static class HelperFunctions
{
    public static string AddToGlobalFreeList (string argumentName) => $"add_to_list({argumentName})";
}