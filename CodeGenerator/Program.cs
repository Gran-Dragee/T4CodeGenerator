using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using HerbSystem.T4;
using Mono.TextTemplating;

namespace CodeGenerator
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            var directoryInfo = new DirectoryInfo("res/json_schema");
            var schemaFiles = directoryInfo.GetFiles("*.json");
            var fileNames = schemaFiles.Select(f => f.FullName).ToArray();
            var classNames = schemaFiles.Select(f => f.Name).Select(name => name.Split(new string[]{".json"}, System.StringSplitOptions.None)[0]).ToArray();

            if (!Directory.Exists("output")) {
                Directory.CreateDirectory("output");
            } else {
                Directory.Delete("output", true);
                Directory.CreateDirectory("output");
            }

            var schemaTT = File.ReadAllText("res/templates/SchemaClass.tt");

            for (var i = 0; i < fileNames.Length; i++) {
                var json = MiniJSON.Json.Deserialize(File.ReadAllText(fileNames[i])) as Dictionary<string, object>;

                var session = new TextTemplatingSession();
                session["ClassName"] = classNames[i];
                session["Json"] = json;

                var host = new TextTemplatingHost();
                host.Initialize("res/templates/SchemaClass.tt", session);

                var engine = new TemplatingEngine();
                var result = engine.ProcessTemplate(schemaTT, host);

                var streamWriter = new StreamWriter(string.Format("output/{0}.cs", classNames[i]), false);
                streamWriter.Write(result);
                streamWriter.Close();
            }
        }
    }
}