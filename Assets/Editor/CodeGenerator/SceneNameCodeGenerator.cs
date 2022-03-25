using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.ShortcutManagement;
using UnityEngine;
namespace DefaultCompany.Test
{
    // classes dont need to be static when you are using InitializeOnLoad
    [InitializeOnLoad]
    public class SceneNameCodeGenerator
    {
        static SceneNameCodeGenerator instance;
        CodeGeneratorCommon common = new();
        static CodeGeneratorCommon Com => instance.common;

        const string FileName = "SceneNames";
        static string FilePath => string.Format( CodeGeneratorCommon.FilePathFormat, CodeGeneratorCommon.DirPath, FileName );

        // static constructor
        static SceneNameCodeGenerator()
        {
            if (instance != null) return;

            instance = new();
            instance.common = new();
            //subscripe to event
            // EditorApplication.update += UpdateSceneNames;
            // get tags
            Com.names = GetNewName();

            // write file
            if (!File.Exists( FilePath ))
            {
                WriteCodeFile();
            }
        }

        static List<string> GetNewName()
        {
            return EditorBuildSettings.scenes.Select( x => x.path ).ToList();
        }

        [Shortcut( "Gen/RefreshSceneName", KeyCode.F1, ShortcutModifiers.Alt | ShortcutModifiers.Control )]
        // update method that has to be called every frame in the editor
        static void UpdateSceneNames()
        {

            if (Application.isPlaying) return;

            if (EditorApplication.timeSinceStartup < Com.nextCheckTime) return;

            Com.nextCheckTime = EditorApplication.timeSinceStartup + CodeGeneratorCommon.CheckIntervalSec;

            var newNames = GetNewName();

            if (Com.SomethingHasChanged( Com.names, newNames ))
            {
                Com.names = newNames;
                WriteCodeFile();
            }
        }

        // writes a file to the project folder
        static void WriteCodeFile()
        {
            Com.WriteCodeFile( FilePath, builder =>
            {
                WrappedInt indentCount = 0;
                builder.AppendIndentLine( indentCount, Com.AutoGenTemplate );
                builder.AppendIndentLine( indentCount, Com.NameSpaceTemplate );

                using (new CurlyIndent( builder, indentCount ))
                {
                    builder.AppendIndentFormatLine( indentCount, "public static class {0}", FileName );

                    using (new CurlyIndent( builder, indentCount ))
                    {
                        foreach (string name in Com.names)
                        {
                            // ex) assets/scenes/menu.unity -> menu
                            var tail = name.Substring( name.LastIndexOf( '/' ) + 1 );
                            var result = tail.Substring( 0, tail.LastIndexOf( '.' ) );
                            builder.AppendIndentFormatLine( indentCount, "public const string {0} = @\"{1}\";", Com.MakeIdentifier( result ), Com.EscapeDoubleQuote( name ) );
                        }
                    }
                }
            } );
        }
    }
}
