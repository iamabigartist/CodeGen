using UnityEditor;
using UnityEngine;
namespace Labs.Lab0_TryEditorSingleton.Editor
{
    public class InAppSSConfigWindow : EditorWindow
    {
        [MenuItem( "InAppSS/ConfigWindow" )]
        static void ShowWindow()
        {
            var window = GetWindow<InAppSSConfigWindow>();
            window.titleContent = new("InAppSSConfigWindow");
            window.Show();
        }

        InApplicationClassObject instance;

        void OnEnable()
        {
            instance = InApplicationClassObject.instance;
        }

        void OnGUI()
        {
            instance.value = EditorGUILayout.IntField( "value", instance.value );

            if (GUILayout.Button( "Minus Value" ))
            {
                instance.value--;
            }
        }
    }
}
