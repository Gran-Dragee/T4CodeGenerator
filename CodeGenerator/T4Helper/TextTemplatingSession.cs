using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Microsoft.VisualStudio.TextTemplating;

namespace HerbSystem.T4
{
    /// <summary>
    /// テンプレート側で扱う変数を渡すための機構
    /// 例:
    ///     var session = new TextTemplatingSession();
    ///     session["param1"] = "A";
    ///     session["param2"] = "B";
    ///     engine.Initialize("path", session);
    /// </summary>
    [Serializable]
    public class TextTemplatingSession : Dictionary<string, Object>, ITextTemplatingSession, ISerializable
    {
        public Guid Id {
            get;
            private set;
        }

        public TextTemplatingSession()
        {
            this.Id = Guid.NewGuid();
        }

        private TextTemplatingSession(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            this.Id = (Guid)info.GetValue("Id", typeof(Guid));
        }

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("Id", this.Id);
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var o = obj as TextTemplatingSession;
            return o != null && o.Equals(this);
        }

        public bool Equals(Guid other)
        {
            return other.Equals(this.Id);
        }

        public bool Equals(ITextTemplatingSession other)
        {
            return other != null && other.Id == this.Id;
        }
    }
}
