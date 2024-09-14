using EntertainingErrors;
using SimpleScript.Parser;
using SimpleScript.Parser.Nodes;

namespace SimpleScript.Adapter.C
{
    public class ConvertReturnNodeToC
    {
        public static Result<string[]> Convert(ReturnNode returnNode, Scope mainScope)
        {
            //Auswertung der Return Expression sollte eigentlich analog laufen wie bei Print.
            //Zusätzlich müssen alle Variablendeklarationen mit String und Length 0 (Heapallocations) die nicht returned werden, mit free freigegeben werden, 
            //bevor die Funktion returned. Falls eine String Variable mit Length 0 returned wird, muss diese in die Linked List gegeben werden mit add_to_list().
            //Hierfür sollte eine eigene Klasse mit potentiellen Helperfunctions geschrieben werden. 
            throw new NotImplementedException();
        }
    }
}
