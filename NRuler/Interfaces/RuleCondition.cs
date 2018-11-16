using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace NRuler.Interfaces
{
    // 规则的条件
    public class RuleCondition
    {
        #region Fields
        // 所属的规则
        private Rule m_rule;
        private string m_codeSnippet;

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

        public static RuleCondition Create(Rule rule, XmlNode node)
        {
            RuleCondition cond = new RuleCondition();
            cond.Rule = rule;
            cond.CodeSnippet = node.InnerXml;
            return cond;
        }

        public string ToXmlString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(string.Format("<condition>{0}</condition>", m_codeSnippet));
            return sb.ToString();
        }        

        #endregion
    }
}
