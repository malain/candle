using System;
using EnvDTE;
using EnvDTE80;

namespace DSLFactory.Candle.SystemModel.CodeGeneration.CodeModel
{
    /// <summary>
    /// 
    /// </summary>
    public class CandleCodeNamespace : CandleCodeElement
    {
        /// <summary>
        /// 
        /// </summary>
        protected CodeNamespace _codeElement;

        /// <summary>
        /// Initializes a new instance of the <see cref="CandleCodeNamespace"/> class.
        /// </summary>
        /// <param name="cel">The cel.</param>
        internal CandleCodeNamespace(CodeNamespace cel) : base(null)
        {
            _codeElement = cel;
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name.</value>
        public override string Name
        {
            get { return _codeElement.Name; }
        }

        /// <summary>
        /// Gets the code element.
        /// </summary>
        /// <value>The code element.</value>
        public CodeNamespace CodeElement
        {
            get { return _codeElement; }
        }

        /// <summary>
        /// Gets the code elements.
        /// </summary>
        /// <returns></returns>
        protected override CodeElements GetCodeElements()
        {
            return _codeElement.Members;
        }

        /// <summary>
        /// Adds the attribute internal.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        protected override CodeAttribute2 AddAttributeInternal(string name)
        {
            return null;
        }

        /// <summary>
        /// Gets the attributes internal.
        /// </summary>
        /// <returns></returns>
        protected override CodeElements GetAttributesInternal()
        {
            return null;
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </returns>
        public override string ToString()
        {
            return String.Format("[namespace] {0}", Name);
        }

        /// <summary>
        /// Accepts the specified visitor.
        /// </summary>
        /// <param name="visitor">The visitor.</param>
        internal override void Accept(ICodeModelVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}