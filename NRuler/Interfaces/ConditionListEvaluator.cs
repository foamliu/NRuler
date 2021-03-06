﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.CodeDom.Compiler;
using System.IO;
using NRuler.Common;

namespace NRuler.Interfaces
{
    /// <summary>
    /// foamliu, 2009/04/28, 对条件列表求值.
    /// </summary>
    public class ConditionListEvaluator
    {
        #region Fields
        // 对象实例
        private object m_instance;

        private Rule m_rule;

        #endregion

        #region Constructors

        public ConditionListEvaluator(Rule rule)
        {
            m_rule = rule;
            ConstructInstance(rule);
        }

        #endregion

        #region Methods

        /// <summary>
        /// foamliu, 2009/04/27.
        /// 
        /// 参照MSDN：
        /// http://msdn.microsoft.com/en-us/library/microsoft.csharp.csharpcodeprovider.aspx
        /// </summary>
        /// <param name="items"></param>
        private void ConstructInstance(Rule rule)
        {
            CodeDomProvider provider = null;
            provider = CodeDomProvider.CreateProvider("CSharp");

            if (provider != null)
            {
                CompilerParameters cp = new CompilerParameters();
                //cp.ReferencedAssemblies.Add("system.dll");                

                // foamliu, 2009/04/28, 避免硬编码 Assembly 路径.

                AppDomain MyDomain = AppDomain.CurrentDomain;
                Assembly[] AssembliesLoaded = MyDomain.GetAssemblies();
                foreach (Assembly MyAssembly in AssembliesLoaded)
                {
                    //Console.WriteLine("Loaded: {0}", MyAssembly.Location);

                    cp.ReferencedAssemblies.Add(MyAssembly.Location);
                }

                //cp.ReferencedAssemblies.Add(@"E:\MyProjects\NRuler\bin\Common.dll");
                //cp.ReferencedAssemblies.Add(@"E:\MyProjects\NRuler\bin\RuleEngine.exe");
                //cp.ReferencedAssemblies.Add(@"E:\MyProjects\NRuler\bin\UnitTests.dll");
                cp.GenerateExecutable = false;
                cp.GenerateInMemory = true;

                StringBuilder code = new StringBuilder();
                code.AppendLine("using System;");
                code.AppendLine("namespace NRuler.Interfaces");
                code.AppendLine("{");
                code.AppendFormat("    public class _Evaluator");
                code.AppendLine();
                code.AppendLine("    {");

                for (int i = 0; i < rule.ConditionList.List.Count; i++)
                {
                    //code.AppendFormat("        public bool _EvaluateCondition{0}(NRuler.Interfaces.ApplicationData appData)", i);
                    // foamliu, 2009/04/28, 使得代码更灵活 (不怕 ApplicationData 名字空间修改).
                    code.AppendFormat("        public bool _EvaluateCondition{0}({1} appData)", i, rule.AppData.GetType().FullName);
                    code.AppendLine();
                    code.AppendLine("        {");
                    foreach (RuleParameter para in rule.ParameterList.List)
                    {
                        code.AppendFormat("            {0} {1} = ({0})appData.Get(\"{1}\");", para.Class, para.Identifier);
                        code.AppendLine();
                    }
                    code.AppendFormat("            return {0};", rule.ConditionList[i].CodeSnippet);
                    code.AppendLine();
                    code.AppendLine("        }\n");
                }

                code.AppendLine("    }");
                code.AppendLine("}");

                //Logger.Info(code.ToString());

                CompilerResults cr = provider.CompileAssemblyFromSource(cp, code.ToString());
                if (cr.Errors.HasErrors)
                {
                    Logger.Error("Error Compiling the Codes: ");
                    foreach (CompilerError err in cr.Errors)
                    {
                        Logger.Error("{0}\n", err.ErrorText);
                    }
                }
                Assembly assembly = cr.CompiledAssembly;
                m_instance = assembly.CreateInstance("NRuler.Interfaces._Evaluator");

            }

        }

        /// <summary>
        /// foamliu, 2009/04/28, 对表达式列表求值.
        /// </summary>
        /// <returns></returns>
        public bool Evaluate()
        {
            bool bResult;
            for (int i = 0; i < m_rule.ConditionList.List.Count; i++)
            {
                RuleCondition cond = m_rule.ConditionList[i];
                MethodInfo mi = m_instance.GetType().GetMethod("_EvaluateCondition" + i);
                bResult = (bool)mi.Invoke(m_instance, new object[] { m_rule.AppData });
                if (!bResult)
                    return false;
            }
            return true;
        }

        #endregion
    }
}
