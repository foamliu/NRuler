using System;
using System.Collections.Generic;
using System.Text;
using NRuler.Terms;

namespace NRuler.Conditions
{
    [Serializable]
    public class AssertCondition : Condition
    {
        
        public AssertCondition(Term id, Term attribute, Term value)
            : base("PositiveCondition", ConditionType.Assert, id, attribute, value)
        {            
        }

        public AssertCondition(string label, Term id, Term attribute, Term value)
            : base(label, ConditionType.Assert, id, attribute, value)
        {            
        }
    }
}
