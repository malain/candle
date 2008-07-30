using System;
using System.Collections.Generic;
using System.Text;
using DSLFactory.Candle.SystemModel.CodeGeneration.CodeModel;
using EnvDTE;
using DSLFactory.Candle.SystemModel;

namespace DSLFactory.Candle.SystemModel.Strategies
{
    /// <summary>
    /// Changement du nom du service
    /// </summary>
    class WCFProxyVisitor : DSLFactory.Candle.SystemModel.CodeGeneration.CodeModel.ICodeModelVisitor
    {
        private ExternalServiceContract contract;

        public WCFProxyVisitor(ExternalServiceContract contract)
        {
            this.contract = contract;
        }

        public void BeginTraverse(FileCodeModel fcm)
        {
        }

        public void EndTraverse(FileCodeModel fcm)
        {
        }

        /// <summary>
        /// Changement du nom du service
        /// </summary>
        /// <param name="clazz"></param>
        public void Visit(CandleCodeClass clazz)
        {
            if (clazz.CodeElement.Bases.Count == 1)
            {
                if (clazz.CodeElement.Bases.Item(1).FullName.StartsWith("System.ServiceModel.ClientBase"))
                    clazz.CodeElement.Name = contract.Name + "ClientProxy";
            }
        }

        public void Visit(CandleCodeFunction function)
        {
        }

        public void Visit(CandleCodeParameter parm)
        {
            // attributs
        }

        public void Visit(CandleCodeEnum enumeration)
        {
            // attributs
        }

        public void Visit(CandleCodeStruct structure)
        {
            // attributs
        }

        public void Visit(CandleCodeProperty prop)
        {
            // attributs
        }

        public void Visit(CandleCodeVariable variable)
        {
            // attributs
        }

        public void Visit(CandleCodeInterface interf)
        {
            // attributs
        }

        public void Visit(CandleCodeNamespace ns)
        {
            // using
        }
    }
}
