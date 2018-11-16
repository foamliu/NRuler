using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Threading;
using NRuler.Rete;

namespace NRuler.Interfaces
{

    /// <summary>
    /// foamliu, 2009/04/23, 用于执行规则后件.
    /// </summary>
    public class RuleRuntime
    {
        #region Fields

        private WorkingMemory m_wm;

        #endregion

        #region Properties

        public WorkingMemory WorkingMemory
        {
            get { return m_wm; }
            set { m_wm = value; }
        }

        #endregion

        #region Methods

        public RuleRuntime()
        {
            
        }

        /// <summary>
        /// foamliu, 2009/04/24, for testability.
        /// </summary>
        /// <param name="rule"></param>
        public void ExecuteRule(Rule rule)
        {
            
        }

        public void ExecuteRules()
        {
            m_wm.ExecuteRules();
        }

        #endregion
    }
}
