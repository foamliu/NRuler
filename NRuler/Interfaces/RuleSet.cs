using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using NRuler.Rete;

namespace NRuler.Interfaces
{
    public class RuleSet
    {
        #region Fields
        private List<Rule> m_list;

        #endregion

        #region Properties

        public Rule this[int index]
        {
            get
            {
                return m_list[index];
            }
        }

        public List<Rule> List
        {
            get { return m_list; }
        }

        public int Count
        {
            get { return m_list.Count; }
        }

        #endregion

        #region Methods

        public RuleSet()
        {
            m_list = new List<Rule>();
        }

        public void Add(Rule rule)
        {
            m_list.Add(rule);
        }

        public void Remove(Rule rule)
        {
            m_list.Remove(rule);
        }

        public static RuleSet LoadXml(string xml)
        {
            RuleSet ruleSet = new RuleSet();
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            XmlNodeList nodeList = doc.SelectNodes("//rule-set/rule");   // one or more           

            foreach (XmlNode node in nodeList)
            {
                ruleSet.Add(Rule.Create(ruleSet, node));
            }

            return ruleSet;
        }

        public static RuleSet Load(string filename)
        {
            string xml;
            RuleSet ruleSet;

            using (TextReader reader = new StreamReader(filename))
            {
                xml = reader.ReadToEnd();
                ruleSet = RuleSet.LoadXml(xml);
            }

            return ruleSet;
        }

        public string ToXmlString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<rule-set>");
            foreach (Rule rule in m_list)
            {
                sb.Append(rule.ToXmlString());
            }
            sb.Append("</rule-set>");
            return sb.ToString();
        }

        public List<Production> ToProductionList()
        {
            List<Production> list = new List<Production>();
            foreach (Rule rule in m_list)
            {
                Production prod = new Production();
                prod.Name = rule.Name;
                foreach (RuleCondition cond in rule.ConditionList.List)
                {
                    //prod.LHS.Add(new NRuler.Conditions.PositiveCondition(
                }
                list.Add(prod);
            }
            return list;
        }

        #endregion
    }
}
