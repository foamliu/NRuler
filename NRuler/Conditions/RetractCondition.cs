using System;
using System.Collections.Generic;
using System.Text;
using NRuler.Terms;

namespace NRuler.Conditions
{
    public class RetractCondition: Condition
    {
        public RetractCondition(Term id, Term attribute, Term value)
            : base("PositiveCondition", ConditionType.Retract, id, attribute, value)
        {            
        }

        public RetractCondition(string label, Term id, Term attribute, Term value)
            : base(label, ConditionType.Retract, id, attribute, value)
        {            
        }
    }
}
