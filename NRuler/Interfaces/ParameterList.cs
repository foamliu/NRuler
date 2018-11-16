using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace NRuler.Interfaces
{
    public class ParameterList
    {
        #region Fields

        private List<RuleParameter> m_list;

        #endregion

        #region Properties

        public RuleParameter this[int index]
        {
            get
            {
                return m_list[index];
            }
        }

        public List<RuleParameter> List
        {
            get { return m_list; }
        }

        #endregion

        #region Constructors

        public ParameterList()
        {
            m_list = new List<RuleParameter>();
        }

        #endregion

        #region Methods

        public static ParameterList Create(Rule rule, XmlNodeList nodeList)
        {
            ParameterList paraList = new ParameterList();
            foreach (XmlNode node in nodeList)
            {
                paraList.List.Add(RuleParameter.Create(rule, node));
            }
            return paraList;
        }

        public string ToXmlString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (RuleParameter para in m_list)
            {
                sb.Append(para.ToXmlString());
            }
            return sb.ToString();
        }

        #endregion        
    }
}
