namespace SimpleScript.Adapter.Abstractions
{
    public interface ICompiler
    {
        bool Compile(string fileName, string code);
    }
}
