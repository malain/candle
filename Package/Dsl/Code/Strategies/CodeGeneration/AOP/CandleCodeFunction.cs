using System;
using EnvDTE;
using EnvDTE80;

namespace DSLFactory.Candle.SystemModel.CodeGeneration.CodeModel
{
    /// <summary>
    /// 
    /// </summary>
    public class CandleCodeFunction : CandleCodeElement
    {
        /// <summary>
        /// 
        /// </summary>
        protected CodeFunction _codeElement;

        /// <summary>
        /// Initializes a new instance of the <see cref="CandleCodeFunction"/> class.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="cel">The cel.</param>
        internal CandleCodeFunction(CandleCodeElement parent, CodeFunction cel) : base(parent)
        {
            _codeElement = cel;
        }

        /// <summary>
        /// Gets the code element.
        /// </summary>
        /// <value>The code element.</value>
        public CodeFunction CodeElement
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
        /// Finds the operation from contract.
        /// </summary>
        /// <param name="contract">The contract.</param>
        /// <returns></returns>
        public Operation FindOperationFromContract(ServiceContract contract)
        {
            if (contract == null)
                return null;

            return contract.Operations.Find(delegate(Operation o)
                                                {
                                                    if (o.Name == Name)
                                                    {
                                                        int i = 0;
                                                        foreach (CodeParameter arg in _codeElement.Parameters)
                                                        {
                                                            if (i >= o.Arguments.Count ||
                                                                o.Arguments[i].Name != arg.Name)
                                                                return false;
                                                            i++;
                                                        }
                                                        return true;
                                                    }
                                                    return false;
                                                });
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
        /// Replaces the body.
        /// </summary>
        /// <param name="code">The code.</param>
        public void ReplaceBody(string code)
        {
            TextPoint sp = _codeElement.GetStartPoint(vsCMPart.vsCMPartBody);
            TextPoint ep = _codeElement.GetEndPoint(vsCMPart.vsCMPartBody);
            EditPoint startpoint = sp.CreateEditPoint();
            EditPoint endpoint = ep.CreateEditPoint();
            startpoint.ReplaceText(endpoint, code, (int) vsEPReplaceTextOptions.vsEPReplaceTextAutoformat);
        }

        /// <summary>
        /// Adds the pre code.
        /// </summary>
        /// <param name="code">The code.</param>
        public void AddPreCode(string code)
        {
            TextPoint pt = _codeElement.GetStartPoint(vsCMPart.vsCMPartBody);
            EditPoint editPoint = pt.CreateEditPoint();
            editPoint.Insert(code + "\r\n");
            editPoint.SmartFormat(pt);
        }

        /// <summary>
        /// Pour éviter un bug de Visual studio TOUJOURS appeler AddPostCode AVANT addPreCode
        /// </summary>
        /// <param name="code">The code.</param>
        public void AddPostCode(string code)
        {
            TextPoint pt = _codeElement.GetEndPoint(vsCMPart.vsCMPartBody);
            EditPoint editPoint = pt.CreateEditPoint();
            editPoint.Insert(code + "\r\n");
            editPoint.SmartFormat(pt);
        }

        /// <summary>
        /// Gets the code elements.
        /// </summary>
        /// <returns></returns>
        protected override CodeElements GetCodeElements()
        {
            return _codeElement.Parameters;
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </returns>
        public override string ToString()
        {
            return
                String.Format("[function] {0}",
                              _codeElement.get_Prototype(
                                  (int)
                                  (vsCMPrototype.vsCMPrototypeType | vsCMPrototype.vsCMPrototypeParamNames |
                                   vsCMPrototype.vsCMPrototypeFullname | vsCMPrototype.vsCMPrototypeParamTypes)));
        }
    }
}