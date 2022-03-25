using System;
using UnityEditor;
using UnityEditor.ShortcutManagement;
using UnityEngine;
namespace Labs.Lab0_TryEditorSingleton.Editor
{
    [FilePath( "Managers/InApplicationClassObject.ssmanager", FilePathAttribute.Location.ProjectFolder )]
    public class InApplicationClassObject : ScriptableSingleton<InApplicationClassObject>
    {

        public event Action UpdateViewer;
        public int value;

        [Shortcut( "InApplicationClassObject/LogValue" )]
        static void AddValue()
        {
            instance.value++;
            instance.Save( true );
            instance.UpdateViewer?.Invoke();
        }

        [InitializeOnLoadMethod]
        static void LogValue()
        {
            // Debug.Log( instance.value );
        }

        public string FilePath => GetFilePath();

    }
}
