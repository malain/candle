using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Xml.Serialization;
using Microsoft.VisualStudio.Modeling;

namespace DSLFactory.Candle.SystemModel.Commands.Reverse
{
    /// <summary>
    /// Importation des interfaces d'une assembly
    /// </summary>
    public class ReverseInterfaces
    {
        private readonly InterfaceLayer _layer;

        /// <summary>
        /// Permet d'importer une assembly dans la couche modèle
        /// </summary>
        /// <param name="layer">Couche interface</param>
        public ReverseInterfaces(InterfaceLayer layer)
        {
            if (layer == null)
                throw new ArgumentNullException("layer");

            // Namespace initial
            this._layer = layer;
        }

        /// <summary>
        /// Récupération des classes d'une assembly
        /// </summary>
        /// <param name="fullPath">The full path.</param>
        /// <param name="selectedClasses">The selected classes.</param>
        public void AddAssembly(string fullPath, IList selectedClasses)
        {
            using (AssemblyResolver resolver = new AssemblyResolver(fullPath))
            {
                using (Transaction transaction = _layer.Store.TransactionManager.BeginTransaction("Import models"))
                {
                    transaction.Context.ContextInfo.Add("ReverseInProgress", true);

                    DataLayer modelsLayer = _layer.SoftwareComponent.DataLayer;

                    // 1er passage : Création des modéles 
                    foreach (Type clazz in selectedClasses)
                    {
                        ServiceContract port = new ServiceContract(_layer.Store);
                        port.Name = clazz.Name;
                        port.RootName = clazz.Name;
                        _layer.ServiceContracts.Add(port);

                        foreach (MethodInfo method in clazz.GetMethods(BindingFlags.Public | BindingFlags.Instance))
                        {
                            Operation op = new Operation(port.Store);
                            port.Operations.Add(op);
                            op.Name = method.Name;
                            string typeName = GetTypeElement(method, method.ReturnType);
                            op.Type = typeName;
                            op.IsCollection = IsListe(method.ReturnType);
                            if (!Utils.StringStartsWith(op.Type, "System.") && !Utils.StringStartsWith(op.Type, "Microsoft."))
                            {
                                if (typeName != null)
                                {
                                    bool exists;
                                    ClassNameInfo cni = new ClassNameInfo(typeName);
                                    Entity target = (Entity)modelsLayer.AddTypeIfNotExists(cni.Namespace, cni.ClassName, false, out exists);
                                    if (!exists)
                                    {
                                        CreateEntity(target, method.ReturnType);
                                    }
                                    op.Type = target.Name;
                                }
                            }
                            // TODO a revoir
                            if (op.Type == "System.Void")
                                op.Type = "void";

                            foreach (ParameterInfo parm in method.GetParameters())
                            {
                                Argument arg = new Argument(port.Store);
                                arg.Name = parm.Name;
                                typeName = GetTypeElement(parm, parm.ParameterType);
                                arg.Type = typeName;
                                if (!Utils.StringStartsWith(arg.Type, "System.") && !Utils.StringStartsWith(arg.Type, "Microsoft."))
                                {
                                    if (typeName != null)
                                    {
                                        ClassNameInfo cni = new ClassNameInfo(typeName);
                                        bool exists;
                                        Entity target = (Entity)modelsLayer.AddTypeIfNotExists(cni.Namespace, cni.ClassName, false, out exists);
                                        if (!exists)
                                        {
                                            CreateEntity(target, parm.ParameterType);
                                        }
                                        arg.Type = target.Name;
                                    }
                                }

                                // TODO a revoir
                                if (arg.Type == "System.Void")
                                    arg.Type = "void";

                                arg.IsCollection = IsListe(parm.ParameterType);
                                arg.Direction = parm.IsOut ? ArgumentDirection.Out : ArgumentDirection.In;
                                op.Arguments.Add(arg);
                            }
                        }                        
                    }
                    transaction.Commit();
                }
            }
        }

        /// <summary>
        /// Création d'un modèle de type classe
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="clazz">Clr Type initial</param>
        private void CreateEntity(Entity model, Type clazz)
        {
            // Héritage
            if (clazz.BaseType != null && !(clazz.BaseType.FullName.StartsWith("System.")))
            {
                model.SuperClass = _layer.SoftwareComponent.FindGlobalType(clazz.BaseType.FullName) as Entity;
            }

            // Propriétés de la classe
            foreach (PropertyInfo prop in clazz.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
            {
                if (prop.CanRead)
                {
                    string xmlName = "";
                    try
                    {
                        object[] attributes = prop.GetCustomAttributes(typeof(XmlElementAttribute), false);
                        if (attributes.Length > 0)
                        {
                            XmlElementAttribute attr = attributes[0] as XmlElementAttribute;
                            xmlName = attr.ElementName;
                        }
                    }
                    catch { }

                    // Création d'une association
                    if (prop.PropertyType.IsClass && prop.PropertyType.Assembly == clazz.Assembly)
                    {
                        string typeName = GetTypeElement(prop, prop.PropertyType);
                        if (typeName != null)
                        {
                            Entity target = _layer.SoftwareComponent.FindGlobalType(typeName) as Entity;
                            if (target != null)
                            {
                                Association association = model.AddAssociationTo(target);
                                association.SourceRoleName = prop.Name;
                                association.SourceMultiplicity = IsListe(prop.PropertyType) ? Multiplicity.OneMany : Multiplicity.One;
                                association.XmlName = xmlName;
                                continue;
                            }
                        }

                        typeName = "$error:unable to determine the type.";
                    }

                    // Création d'un attribut
                    DSLFactory.Candle.SystemModel.Property p = new DSLFactory.Candle.SystemModel.Property(_layer.Store);
                    p.Type = prop.PropertyType.FullName;
                    p.Name = prop.Name;
                    //p.IsPrimitive = IsTypePrimitive(prop.PropertyType);
                    p.XmlName = xmlName;
                    model.Properties.Add(p);
                }
            }
        }


        /// <summary>
        /// Détermine si le type est une liste
        /// </summary>
        /// <param name="type">Type clr</param>
        /// <returns>vrai si c'est une liste</returns>
        private bool IsListe(Type type)
        {
            return (type.IsArray ||
                type.GetInterface("ICollection") != null ||
                type.GetInterface("IList") != null);
        }

        /// <summary>
        /// Détermine le type de l'élément d'une liste
        /// </summary>
        /// <param name="member">The member.</param>
        /// <param name="t">The t.</param>
        /// <returns></returns>
        private string GetTypeElement(ICustomAttributeProvider member, Type t)
        {
            string result = null;
            if (t.IsGenericType)
            {
                // TODO si il y en a plusieurs
                Type[] args = t.GetGenericArguments();
                result = args[0].FullName;
            }
            else
            {
                if (IsListe(t))
                {
                    t = t.GetElementType();
                    if (t != null && t != typeof(object))
                        result = t.FullName;
                    else
                    {
                        try
                        {
                            object[] attrs = member.GetCustomAttributes(typeof(System.Xml.Serialization.XmlArrayItemAttribute), false);
                            if (attrs.Length == 1)
                                result = ((XmlArrayItemAttribute)attrs[0]).Type.FullName;
                        }
                        catch { }
                    }
                }
            }

            if (result == null)
                result = t.FullName;

            if (result.Length > 0 && result[result.Length - 1] == '&')
                return result.Substring(0, result.Length - 1);

            return result;
        }
    }
}

 
