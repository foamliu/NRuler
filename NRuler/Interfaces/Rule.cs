using System.Text;
using System.Xml;

namespace NRuler.Interfaces
{
    /// <summary>
    /// foamliu, 2009/04/23, 在 RETE 之上进一步抽象.
    /// </summary>
    public class Rule
    {
        #region Fields

        private string m_name;
        // 参数列表
        private ParameterList m_parameters;
        // 条件列表
        private ConditionList m_conditions;
        // 结果
        private RuleConsequence m_consequence;
        // 所归属的规则集
        private RuleSet m_ruleSet;
        private ApplicationData m_appData;

        private ConditionListEvaluator m_eval;
        private ConsequenceInvoker m_invoker;

        #endregion

        #region Properties

        public string Name
        {
            get { return m_name; }
            set { m_name = value; }
        }

        public ParameterList ParameterList
        {
            get { return m_parameters; }
            set { m_parameters = value; }
        }

        public ConditionList ConditionList
        {
            get { return m_conditions; }
            set { m_conditions = value; }
        }

        public RuleConsequence Consequence
        {
            get { return m_consequence; }
            set { m_consequence = value; }
        }

        public RuleSet RuleSet
        {
            get { return m_ruleSet; }
            set { m_ruleSet = value; }
        }

        public ApplicationData AppData
        {
            get { return m_appData; }
            set { m_appData = value; }
        }

        #endregion

        #region Methods

        public static Rule Create(RuleSet ruleSet, XmlNode node)
        {
            Rule rule = new Rule();
            rule.RuleSet = ruleSet;
            rule.Name = node.Attributes["name"].Value;
            rule.ParameterList = ParameterList.Create(rule, node.SelectNodes("//parameter"));  // one or more
            rule.ConditionList = ConditionList.Create(rule, node.SelectNodes("//condition"));  // one or more
            rule.Consequence = RuleConsequence.Create(rule, node.SelectSingleNode("//consequence"));    // exactly one
            return rule;
        }

        public string ToXmlString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(string.Format("<rule name='{0}'>", m_name));
            sb.Append(m_parameters.ToXmlString());
            sb.Append(m_conditions.ToXmlString());
            sb.Append(m_consequence.ToXmlString());
            sb.Append("</rule>");
            return sb.ToString();
        }

        /// <summary>
        /// foamliu, 2009/04/28, 对条件列表求值.
        /// </summary>
        /// <returns></returns>
        private bool Evaluate()
        {
            //ConditionListEvaluator eval = new ConditionListEvaluator(this);
            // foamliu, 2009/04/28, 缓存避免二次编译.
            // foamliu, 2009/04/28, 懒惰初始化.
            if (null==m_eval)
            {
            m_eval = new ConditionListEvaluator(this);
            }
            return m_eval.Evaluate();
        }

        /// <summary>
        /// foamliu, 2009/04/28, 执行规则后件.
        /// </summary>
        private void Invoke()
        {
            //ConsequenceInvoker invoker = new ConsequenceInvoker(this);
            // foamliu, 2009/04/28, 缓存避免二次编译.
            // foamliu, 2009/04/28, 懒惰初始化.            

            if (null==m_invoker)
            {
            m_invoker = new ConsequenceInvoker(this);
            }
            m_invoker.Invoke();
        }

        /// <summary>
        /// foamliu, 2009/04/28, 激活这个规则.
        /// </summary>
        public void Fire()
        {
            if (this.Evaluate())
                this.Invoke();
        }

        #endregion

    }
}
