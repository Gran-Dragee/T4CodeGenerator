using System;
using System.Collections.Generic;
using System.CodeDom.Compiler;
using System.IO;
using System.Text;
using Microsoft.VisualStudio.TextTemplating;

namespace HerbSystem.T4
{
    [Serializable]
    public class TextTemplatingHost : ITextTemplatingEngineHost, ITextTemplatingSessionHost
    {
        #region ITextTemplatingEngineHost implementation
        public IList<string> StandardAssemblyReferences {
            get {
                return new string [] {
                    typeof(System.Uri).Assembly.Location,
                    typeof(System.Linq.Enumerable).Assembly.Location,
                    typeof(ITextTemplatingEngineHost).Assembly.Location,
                };
            }
        }

        public IList<string> StandardImports {
            get {
                return new string[] {
                    "System",
                    "System.Collections.Generic",
                    "System.Linq",
                    "System.Text"
                };
            }
        }

        private string templateFile = string.Empty;
        public string TemplateFile {
            get {
                return templateFile;
            }
        }

        public object GetHostOption(string optionName)
        {
            switch (optionName) {
                case "CacheAssemblies":
                    return true;
                default:
                    return null;
            }
        }

        public bool LoadIncludeText(string requestFileName, out string content, out string location)
        {
            location = ResolvePath(requestFileName);
            if (File.Exists(location)) {
                content = File.ReadAllText(location);
                return true;
            }

            content = string.Empty;
            return false;
        }

        public void LogErrors(CompilerErrorCollection errors)
        {
            for (var i = 0; i < errors.Count; i++) {
                Console.Error.WriteLine(string.Format("Line:{0}:{1}", errors[i].Line, errors[i].ErrorText));
            }
        }

        public AppDomain ProvideTemplatingAppDomain(string content)
        {
            return null;
        }

        public string ResolveAssemblyReference(string assemblyReference)
        {
            if (File.Exists(assemblyReference)) {
                return assemblyReference;
            }

            foreach (var stdAsmRef in StandardAssemblyReferences) {
                var dir = Path.GetDirectoryName(stdAsmRef);
                var candidate = Path.Combine(dir, string.Format("{0}.dll", assemblyReference));
                if (File.Exists(candidate)) {
                    return candidate;
                }
            }

            {
                var dir = Path.GetDirectoryName(this.GetType().Assembly.Location);
                var candidate = Path.Combine(dir, string.Format("{0}.dll", assemblyReference));
                if (File.Exists(candidate)) {
                    return candidate;
                }
            }

            return string.Empty;
        }

        public Type ResolveDirectiveProcessor(string processorName)
        {
            throw new NotImplementedException();
        }

        public string ResolveParameterValue(string directiveId, string processorName, string parameterName)
        {
            throw new NotImplementedException();
        }

        public string ResolvePath(string path)
        {
            throw new NotImplementedException();
        }

        public void SetFileExtension(string extension)
        {
            throw new NotImplementedException();
        }

        public void SetOutputEncoding(Encoding encoding, bool fromOutputDirective)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region ITextTemplatingSessionHost implementation
        public ITextTemplatingSession CreateSession()
        {
            return session;
        }

        private ITextTemplatingSession session = null;
        public ITextTemplatingSession Session {
            get {
                return session;
            }
            set {
                session = value;
            }
        }
        #endregion

        public void Initialize(string templateFile, ITextTemplatingSession session)
        {
            this.templateFile = templateFile;
            this.Session = session;
        }

        /*
        private AppDomain appDomain = null;
        public AppDomain AppDomain {
            get {
                return appDomain;
            }
        }

        private string extension = string.Empty;
        public string Extension {
            get {
                return extension;
            }
            set {
                extension = value;
            }
        }

        private Encoding encoding = Encoding.UTF8;
        public Encoding Encoding {
            get {
                return encoding;
            }
            set {
                encoding = value;
            }
        }

        public IList<string> StandardAssemblyReferences {
            get {
                return new string [] {
                    typeof(System.Uri).Assembly.Location,
                    typeof(System.Linq.Enumerable).Assembly.Location,
                    typeof(ITextTemplatingEngineHost).Assembly.Location,
                };
            }
        }

        public IList<string> StandardImports {
            get {
                return new string [] {
                    "System",
                    "System.Collections.Generic",
                    "System.Linq",
                    "System.Text"
                };
            }
        }

        public void Initialize(string templateFile, ITextTemplatingSession session, AppDomain appDomain = null)
        {
            this.templateFile = templateFile;
            this.Session = session;
            this.appDomain = appDomain;
        }

        public bool LoadIncludeText(string requestFileName, out string content, out string location)
        {
            location = ResolvePath(requestFileName);
            if (File.Exists(location)) {
                content = File.ReadAllText(location);
                return true;
            }

            content = string.Empty;
            return false;
        }

        public AppDomain ProvideTemplatingAppDomain(string content)
        {
            return AppDomain;
        }

        public string ResolveAssemblyReference(string assemblyReference)
        {
            if (File.Exists(assemblyReference)) {
                return assemblyReference;
            }

            foreach (var stdAsmRef in StandardAssemblyReferences) {
                var dir = Path.GetDirectoryName(stdAsmRef);
                var candidate = Path.Combine(dir, string.Format("{0}.dll", assemblyReference));
                if (File.Exists(candidate)) {
                    return candidate;
                }
            }

            {
                var dir = Path.GetDirectoryName(this.GetType().Assembly.Location);
                var candidate = Path.Combine(dir, string.Format("{0}.dll", assemblyReference));
                if (File.Exists(candidate)) {
                    return candidate;
                }
            }

            return string.Empty;
        }

        public string ResolvePath(string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) {
                throw new ArgumentNullException ("fileName is NULL or emptry.");
            }

            if (!File.Exists(fileName)) {
                var dir = Path.GetDirectoryName(this.TemplateFile);
                var candidate = Path.Combine(dir, fileName);
                if (File.Exists(candidate)) {
                    return candidate;
                }
            }

            return fileName;
        }

        public Type ResolveDirectiveProcessor(string processorName)
        {
            throw new Exception("Could not find directivePath.");
        }

        public string ResolveParameterValue(string directiveId, string processorName, string parameterName)
        {
            throw new NotImplementedException();
        }

        public object GetHostOption(string optionName)
        {
            object returnObject;
            switch (optionName) {
                case "CacheAssemblies":
                    returnObject = true;
                    break;
                default:
                    returnObject = null;
                    break;
            }
            return returnObject;
        }

        #if false
        public void SetFileExtension(string extension)
        {
            this.Extension = extension;
        }

        public void SetOutputEncoding(System.Text.Encoding encoding, bool fromOutputDirective)
        {
            this.Encoding = encoding;
        }
        #endif

        public void LogErrors(CompilerErrorCollection errors)
        {
            for (var i = 0; i < errors.Count; i++) {
                Console.Error.WriteLine(string.Format("Line:{0}:{1}", errors[i].Line, errors[i].ErrorText));
            }
        }
        */
    }
}
