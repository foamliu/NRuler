using System;
using System.Collections.Generic;
using System.Text;
using NRuler.Terms;

namespace NRuler.Conditions
{
    public class NCCCondition : Condition
    {
        #region Fields
        /// <summary>
        /// Subconditions for an NCC node
        /// </summary>
        protected List<Condition> m_sub_conditions;

        #endregion

        #region Properties

        public List<Condition> SubConditions
        {
            get { return this.m_sub_conditions; }

        }

        #endregion

        #region Constructors

        public NCCCondition()
            : base("NCCCondition")
        {
            this.m_type = ConditionType.NCC;
            this.m_sub_conditions = new List<Condition>();
        }

        public NCCCondition(Term id, Term attribute, Term value)
            : this("NCCCondition", id, attribute, value)
        {            
            //this.m_sub_conditions = new List<Condition>();
        }
                
        public NCCCondition(string label, Term id, Term attribute, Term value)
            : base(label, ConditionType.NCC, id, attribute, value)
        {            
            this.m_sub_conditions = new List<Condition>();
            this.m_sub_conditions.Add(new PositiveCondition(id, attribute, value));
        }

        #endregion

    }
}
