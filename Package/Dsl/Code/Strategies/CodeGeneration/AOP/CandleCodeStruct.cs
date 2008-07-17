using System;
using EnvDTE;
using EnvDTE80;

namespace DSLFactory.Candle.SystemModel.CodeGeneration.CodeModel
{
    /// <summary>
    /// 
    /// </summary>
    public class CandleCodeStruct : CandleCodeElement
    {
        /// <summary>
        /// 
        /// </summary>
        protected CodeStruct _codeElement;

        /// <summary>
        /// Initializes a new instance of the <see cref="CandleCodeStruct"/> class.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="cel">The cel.</param>
        internal CandleCodeStruct(CandleCodeElement parent, CodeStruct cel) : base(parent)
        {
            _codeElement = cel;
        }

        /// <summary>
        /// Gets the code element.
        /// </summary>
        /// <value>The code element.</value>
        public CodeStruct CodeElement
        {
            get { return _codeElement; }
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
        /// Accepts the specified visitor.
        /// </summary>
        /// <param name="visitor">The visitor.</param>
        internal override void Accept(ICodeModelVisitor visitor)
        {
            visitor.Visit(this);
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
            return _codeElement.AddAttribute(name, String.Empty, null) as CodeAttribute2;
        }

        /// <summary>
        /// Gets the attributes internal.
        /// </summary>
        /// <returns></returns>
        protected override CodeElements GetAttributesInternal()
        {
            return _codeElement.Attributes;
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </returns>
        public override string ToString()
        {
            return String.Format("[struct] {0}", Name);
        }
    }
}