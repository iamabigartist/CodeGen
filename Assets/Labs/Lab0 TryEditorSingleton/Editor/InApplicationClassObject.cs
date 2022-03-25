using UnityEditor;
using UnityEditor.ShortcutManagement;
using UnityEngine;
namespace Labs.Lab0_TryEditorSingleton.Editor
{
    [FilePath( "SOManager", FilePathAttribute.Location.ProjectFolder )]
    public class InApplicationClassObject : ScriptableSingleton<InApplicationClassObject>
    {

        public int value;

        [InitializeOnLoadMethod]
        static void AddValue()
        {
            instance.value++;
        }

        [Shortcut( "InApplicationClassObject/LogValue" )]
        static void LogValue()
        {
            Debug.Log( instance.value );
        }
    }
}
