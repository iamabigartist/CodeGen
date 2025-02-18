﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
namespace DefaultCompany.Test
{
    public class CodeGeneratorCommon
    {
        public List<string> names = new();

        public double nextCheckTime = 0.0;


        // directory at witch auto-generated scripts are placed
        public const string DirPath = "Assets/Scripts/AutoGenerated";

        // file header format
        public const string AutoGenFormat =
            @"//-----------------------------------------------------------------------
// This file is AUTO-GENERATED.
// Changes for this script by hand might be lost when auto-generation is run.
// (Generated date: {0})
//-----------------------------------------------------------------------
";
        public const string FilePathFormat = "{0}/{1}.gen.cs";

        public const float CheckIntervalSec = 3f;

        public const string StringPrefix = "Str";

        // header
        public string AutoGenTemplate => string.Format( AutoGenFormat, DateTime.Now.ToString( "yyyy/MM/dd HH:mm:ss" ) );

        // namespace
        public string NameSpaceTemplate => string.Format( "namespace {0}.{1}", PlayerSettings.companyName, PlayerSettings.productName );


        // writes a file to the project folder
        public void WriteCodeFile(string path, Action<StringBuilder> callback)
        {
            Debug.Assert( callback != null );

            if (!Directory.Exists( DirPath ))
            {
                Directory.CreateDirectory( DirPath );
            }

            try
            {
                // Always create a new file because overwriting to existing file may generate mal-formatted script.
                // for instance, when the number of tags is reduced, last tag will be remain after the last curly brace in the file.
                using (FileStream stream = File.Open( path, FileMode.Create, FileAccess.Write ))
                {
                    using (StreamWriter writer = new StreamWriter( stream ))
                    {
                        StringBuilder builder = new StringBuilder();
                        callback( builder );
                        writer.Write( builder.ToString() );
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogException( e );

                // if we have an error, it is certainly that the file is screwed up. Delete to be save
                //if (File.Exists(path))
                //{
                //    File.Delete(path);
                //}
            }

            AssetDatabase.ImportAsset( path );
            // AssetDatabase.Refresh();
        }

        // check if names are changed
        public bool SomethingHasChanged(List<string> a, List<string> b)
        {
            if (a.Count != b.Count)
            {
                return true;
            }
            else
            {
                // loop thru all new tags and compare them to the old ones
                for (int i = 0; i < a.Count; i++)
                {
                    if (!string.Equals( a[i], b[i] ))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public string MakeIdentifier(string str)
        {
            var result = Regex.Replace( str, "[^a-zA-Z0-9]", "_" );

            if ('0' <= result[0] && result[0] <= '9')
            {
                result = result.Insert( 0, "_" );
            }

            return result;
        }

        public string EscapeDoubleQuote(string str)
        {
            return str.Replace( "\"", "\"\"" );
        }
    }
}
