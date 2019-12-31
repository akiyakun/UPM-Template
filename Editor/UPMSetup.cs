using System;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEditor;
using MiniJSON;

namespace UPM
{
    // ランチャーの基本クラス
    public class UPMSetup
    {
        // public const string PackageName = "com.sample.upm4";
        // public const string PackageRootPath = "Packages/" + PackageName + "/";

        public const string topMenu = "Tools/";

        // [InitializeOnLoadMethod]
        // static void a()
        // {
        //     UnityEngine.Debug.Log(Application.dataPath);
        //     UnityEngine.Debug.Log(System.Environment.CurrentDirectory);
        //     UnityEngine.Debug.Log(System.IO.Directory.GetCurrentDirectory());
        // }

        static string GetPackageDirectory([System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "")
        {
            // UnityEngine.Debug.Log(sourceFilePath);

            string s = Path.GetDirectoryName(Path.GetDirectoryName(sourceFilePath));
            // UnityEngine.Debug.Log(s);

            return s + "/";
        }

        [MenuItem(topMenu + "Replace assembly names", false, 0)]
        static void Replace()
        {
            string dir = GetPackageDirectory();

            // string dir = System.IO.Directory.GetCurrentDirectory();
            string file = dir + "package.json";
            string text = File.ReadAllText(file);
            Dictionary<string, object> json = Json.Deserialize(text) as Dictionary<string, object>;

            // json["name"] = PackageName;

            // text = Json.Serialize(json);
            // File.WriteAllText(file, text);

            // AssetDatabase.Refresh();
            // return;

            // UnityEngine.Debug.Log("UPM");

            string packageName = json["name"].ToString();

            //"C:\test"以下のファイルをすべて取得する
            IEnumerable<string> files =
                System.IO.Directory.EnumerateFiles(
                    dir, "*.asmdef", System.IO.SearchOption.AllDirectories);

            // ファイルを列挙する
            foreach (string f in files)
            {
                // ListBox1.Items.Add(f);
                // UnityEngine.Debug.Log(f.ToString());

                string str = File.ReadAllText(f);

                Dictionary<string, object> d = Json.Deserialize(str) as Dictionary<string, object>;

                string s = Regex.Replace(d["name"].ToString(), @"(.+\.)+", packageName + ".");
                // UnityEngine.Debug.Log(s);
                d["name"] = s;

                str = Json.Serialize(d);
                // UnityEngine.Debug.Log(json);

                File.WriteAllText(f, str);

                // Debug.Log(AssetDatabase.GetAllAssetPaths()[0]);

                // s = Regex.Replace(f, @"(.+\.)+", packageName + ".");
                // UnityEngine.Debug.Log(s);

                // .metaファイルが移動されないのでこの方法はダメ
                // File.Move(f, s);

                // Packages内のファイル移動どうやるんだろう・・・
                // var ff = f.Replace(Path.GetDirectoryName(Application.dataPath) + "/", "");
                // UnityEngine.Debug.Log(ff);

                // var dd = Path.GetDirectoryName(ff) + "/";
                // Debug.Log(dd);

                // var ss = Regex.Replace(Path.GetFileNameWithoutExtension(ff), @"(.+\.)+", packageName + ".");
                // ss = dd + ss + ".asmdef";
                // var ss = (dir + s).Replace(Path.GetDirectoryName(Application.dataPath) + "/", "");
                // UnityEngine.Debug.Log(ss);

                // var a = AssetDatabase.MoveAsset(ff, ss);
                // UnityEngine.Debug.Log(a);

                var dd = Path.GetDirectoryName(f) + "/";
                var ff = Regex.Replace(Path.GetFileNameWithoutExtension(f), @"(.+\.)+", packageName + ".");
                var ss = dd + ff + ".asmdef";
                // UnityEngine.Debug.Log(ss);

                File.Delete(Path.GetFileNameWithoutExtension(f) + ".meta");
                File.Move(f, ss);

            }

            AssetDatabase.Refresh();
        }
    }

    // /// <summary>呼び出し元を含むソースファイルの完全パスを取得します。</summary>
    // [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = false)]
    // public class CallerFilePathAttribute : Attribute
    // {
    // }

}
