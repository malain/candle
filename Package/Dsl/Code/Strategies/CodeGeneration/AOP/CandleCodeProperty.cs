using System;
using EnvDTE;
using EnvDTE80;

namespace DSLFactory.Candle.SystemModel.CodeGeneration.CodeModel
{
    /// <summary>
    /// 
    /// </summary>
    public class CandleCodeProperty : CandleCodeElement
    {
        /// <summary>
        /// 
        /// </summary>
        protected CodeProperty _codeElement;

        /// <summary>
        /// Initializes a new instance of the <see cref="CandleCodeProperty"/> class.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="cel">The cel.</param>
        internal CandleCodeProperty(CandleCodeElement parent, CodeProperty cel) : base(parent)
        {
            _codeElement = cel;
        }

        /// <summary>
        /// Gets the code element.
        /// </summary>
        /// <value>The code element.</value>
        public CodeProperty CodeElement
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
        /// Finds the property from entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public Property FindPropertyFromEntity(Entity entity)
        {
            return entity.Properties.Find(delegate(Property p) { return p.Name == Name; });
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
        /// Changes the comment.
        /// </summary>
        /// <param name="comment">The comment.</param>
        public void ChangeComment(string comment)
        {
            _codeElement.Comment = comment;
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
            return String.Format("[property] {0}.{1}", Parent.Name, Name);
        }
    }
}