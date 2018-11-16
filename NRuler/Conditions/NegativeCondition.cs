using System;
using System.Collections.Generic;
using System.Text;
using NRuler.Terms;

namespace NRuler.Conditions
{
    /// <summary>
    /// foamliu, 2008/12/10.
    /// Note: 
    ///     (1) only one;
    ///     (2) must be last.
    /// </summary>
    public class NegativeCondition : Condition
    {
        public NegativeCondition(Term id, Term attribute, Term value)
            : base("NegativeCondition", ConditionType.Negative, id, attribute, value)
        {            
        }
                
        public NegativeCondition(string label, Term id, Term attribute, Term value)
            : base(label, ConditionType.Negative, id, attribute, value)
        {            
        }

    }
}
