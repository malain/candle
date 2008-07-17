using DSLFactory.Candle.SystemModel.Strategies;
using EnvDTE;

namespace DSLFactory.Candle.SystemModel.Commands
{
    /// <summary>
    /// Importation des opérations d'un port à partir d'un fichier source
    /// </summary>
    public class ImportInterfaceHelper : IImportInterfaceHelper
    {
        /// <summary>
        /// Imports the operations.
        /// </summary>
        /// <param name="layer">The layer.</param>
        /// <param name="port">The port.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <returns>true if the import is ok</returns>
        public bool ImportOperations( SoftwareLayer layer, TypeWithOperations port, string fileName )
        {
            FileCodeModel fcm = ServiceLocator.Instance.ShellHelper.GetFileCodeModel(fileName);
            if (fcm == null)
                return false;

            foreach (CodeElement cn in fcm.CodeElements)
            {
                if (cn is CodeNamespace)
                {
                    foreach (CodeElement ci in ((CodeNamespace)cn).Members)
                    {
                        if ((ci is CodeInterface || ci is CodeClass) && layer is InterfaceLayer)
                        {
                            CodeElements members;
                            string comment;
                            if (ci is CodeInterface)
                            {
                                comment = ((CodeInterface)ci).DocComment;
                                members = ((CodeInterface)ci).Members;
                            }
                            else
                            {
                                comment = ((CodeClass)ci).DocComment;
                                members = ((CodeClass)ci).Members;
                            }
                            if (port == null)
                            {
                                port = new ServiceContract(layer.Store);
                                port.Name = ci.Name;
                                port.RootName = ci.Name;
                                port.Comment = NormalizeComment(comment);
                                ((InterfaceLayer)layer).ServiceContracts.Add((ServiceContract)port);
                            }

                            RetrieveOperations(port, members, false);
                        }
                        else if (ci is CodeClass && layer is Layer)
                        {
                            CodeClass cc = ci as CodeClass;
                            ClassImplementation clazz = port as ClassImplementation;
                            //if (cc.Access == vsCMAccess.vsCMAccessPublic)
                            {
                                if (clazz == null)
                                {
                                    clazz = new ClassImplementation(layer.Store);
                                    clazz.Name = ci.Name;
                                    clazz.RootName = ci.Name;
                                    clazz.Comment = NormalizeComment(cc.DocComment);
                                    ((Layer)layer).Classes.Add(clazz);
                                }

                                InterfaceLayer iLayer = clazz.Layer.LayerPackage.InterfaceLayer;
                                // Si il y a plusieurs interfaces, on ne fait rien car on ne sait pas laquelle prendre
                                if (iLayer != null && cc.ImplementedInterfaces.Count == 1)
                                {
                                    ServiceContract contract = clazz.Contract;
                                    if (contract == null)
                                    {
                                        string iName = cc.ImplementedInterfaces.Item(1).Name;
                                        contract = iLayer.ServiceContracts.Find(delegate(ServiceContract c) { return c.Name == iName; });
                                        if (contract == null)
                                        {
                                            contract = new ServiceContract(layer.Store);
                                            contract.Name = StrategyManager.GetInstance(clazz.Store).NamingStrategy.CreateElementName(iLayer, cc.Name);
                                            contract.RootName = cc.Name;
                                            contract.Comment = NormalizeComment(cc.DocComment);
                                            iLayer.ServiceContracts.Add(contract);
                                            RetrieveOperations(contract, cc.Members, true);
                                        }
                                        clazz.Contract = contract;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// Retrieves the operations.
        /// </summary>
        /// <param name="port">The port.</param>
        /// <param name="members">The members.</param>
        /// <param name="checkPublic">if set to <c>true</c> [check public].</param>
        private static void RetrieveOperations(TypeWithOperations port, CodeElements members, bool checkPublic)
        {
            foreach (CodeElement codeElement in members)
            {
                CodeFunction func = codeElement as CodeFunction;
                if (func == null)
                    continue;

                if (checkPublic && func.Access != vsCMAccess.vsCMAccessPublic)
                    continue;

                // Operation
                Operation op = FindOperation(port, func);
                if (op == null)
                {
                    op = new Operation(port.Store);
                    op.Name = func.Name;
                    op.Comment = NormalizeComment(func.DocComment);
                    op.Type = func.Type.AsString;
                    //if( Utils.StringStartsWith( op.Type, "system." ) )
                    //    op.Type = op.Type.Substring( 7 );
                    port.Operations.Add(op);

                    foreach (CodeParameter param in func.Parameters)
                    {
                        // Argument
                        Argument arg = new Argument(port.Store);
                        arg.Name = param.Name;
                        arg.Comment = NormalizeComment(param.DocComment);
                        arg.Type = param.Type.AsString;
                        //if( Utils.StringStartsWith( arg.Type, "system." ) )
                        //    arg.Type = arg.Type.Substring( 7 );
                        op.Arguments.Add(arg);
                    }
                }
            }
        }

        /// <summary>
        /// Finds the operation.
        /// </summary>
        /// <param name="port">The port.</param>
        /// <param name="func">The func.</param>
        /// <returns></returns>
        private static Operation FindOperation( TypeWithOperations port, CodeFunction func )
        {
            foreach( Operation op in port.Operations )
            {
                if( op.Name == func.Name && op.Type == func.Type.AsString && func.Parameters.Count == op.Arguments.Count)
                {
                    bool ok = true;
                    int i = 0;
                    foreach( CodeParameter param in func.Parameters )
                    {
                        Argument arg = op.Arguments[i];
                        if( arg.Name != param.Name || arg.Type != param.Type.AsString )
                        {
                            ok=false;
                            break;
                        }
                    }
                    if( ok )
                        return op;
                }
            }
            return null;
        }


        /// <summary>
        /// Supprime les balises 'doc' et les sauts de lignes
        /// </summary>
        /// <param name="comment">The comment.</param>
        /// <returns></returns>
        internal static string NormalizeComment( string comment )
        {
            if( comment.StartsWith( "<doc>" ) )
                comment = comment.Substring( 5 );
            if( comment.EndsWith( "</doc>" ) )
                comment = comment.Substring( 0, comment.Length-6 );

            // Suppression des sauts de lignes
            comment = comment.Replace( '\r', '\n' );    // Si il y avait un \r\n, on a maintenant un \n\n
            comment = comment.Replace( "\n\n", " " );   // On le remplace par un espace
            comment = comment.Replace( '\n', ' ' );     // Si il n'y avait qu'un \n, on aura aussi qu'un espace.
            comment = comment.Trim();

            return comment;
        }
    }
}
