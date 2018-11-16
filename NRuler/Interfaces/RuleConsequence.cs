using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using NRuler.Interfaces;

namespace NRuler.Interfaces
{
    // 规则的结果
    public class RuleConsequence
    {
        #region Fields
        // 所属的规则
        private Rule m_rule;
        // 代码段
        private string m_codeSnippet;
        // 执行器
        private ConsequenceInvoker m_invoker;

        #endregion

        #region Properties

        public Rule Rule
        {
            get { return m_rule; }
            set { m_rule = value; }
        }

        public string CodeSnippet
        {
            get { return m_codeSnippet; }
            set { m_codeSnippet = value; }
        }

        #endregion

        #region Methods

        public static RuleConsequence Create(Rule rule, XmlNode node)
        {
            RuleConsequence conseq = new RuleConsequence();
            conseq.Rule = rule;
            conseq.CodeSnippet = node.InnerXml;
            return conseq;
        }

        public string ToXmlString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(string.Format("<consequence>{0}</consequence>", m_codeSnippet));
            return sb.ToString();
        }

        public void Invoke()
        {
            if (null == m_invoker)
            {
                m_invoker = new ConsequenceInvoker(this.Rule);
            }
            m_invoker.Invoke();
        }

        #endregion
    }
}
