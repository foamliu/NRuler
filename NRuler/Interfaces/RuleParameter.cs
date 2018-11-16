using System.Text;
using System.Xml;

namespace NRuler.Interfaces
{
    public class RuleParameter
    {
        #region Fields
        private Rule m_rule;
        private string m_identifier;
        private string m_class;

        #endregion

        #region Properties

        public Rule Rule
        {
            get { return m_rule; }
            set { m_rule = value; }
        }

        public string Identifier
        {
            get { return m_identifier; }
            set { m_identifier = value; }
        }

        public string Class
        {
            get { return m_class; }
            set { m_class = value; }
        }

        #endregion

        #region Methods

        public static RuleParameter Create(Rule rule, XmlNode node)
        {
            RuleParameter para = new RuleParameter();
            para.Identifier = node.Attributes["identifier"].Value;  // exactly one
            para.Class = node.SelectSingleNode("//class").InnerText;// exactly one
            return para;
        }

        public string ToXmlString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(string.Format("<parameter identifier ='{0}'><class>{1}</class></parameter>", m_identifier, m_class));
            return sb.ToString();
        }

        #endregion
    }
}
