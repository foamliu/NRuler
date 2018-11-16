﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace NRuler.Interfaces
{
    public class ConditionList
    {
        #region Fields
        // 所属的规则
        private Rule m_rule;
        private List<RuleCondition> m_list;

        #endregion

        #region Properties

        public RuleCondition this[int index]
        {
            get
            {
                return m_list[index];
            }
        }

        public Rule Rule
        {
            get { return m_rule; }
            set { m_rule = value; }
        }

        public List<RuleCondition> List
        {
            get { return m_list; }
        }

        #endregion

        #region Methods

        public ConditionList()
        {
            m_list = new List<RuleCondition>();
        }

        public static ConditionList Create(Rule rule, XmlNodeList nodeList)
        {
            ConditionList condList = new ConditionList();
            condList.Rule = rule;
            foreach (XmlNode node in nodeList)
            {
                condList.List.Add(RuleCondition.Create(rule, node));
            }
            return condList;
        }

        public string ToXmlString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (RuleCondition cond in m_list)
            {
                sb.Append(cond.ToXmlString());
            }
            return sb.ToString();
        }

        #endregion
    }
}
