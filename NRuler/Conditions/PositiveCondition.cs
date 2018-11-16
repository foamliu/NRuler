using System;
using System.Collections.Generic;
using System.Text;
using NRuler.Terms;

namespace NRuler.Conditions
{
    [Serializable]
    public class PositiveCondition : Condition
    {
        public PositiveCondition(Term id, Term attribute, Term value)
            : base("PositiveCondition", ConditionType.Positive, id, attribute, value)
        {            
        }

        public PositiveCondition(string label, Term id, Term attribute, Term value)
            : base(label, ConditionType.Positive, id, attribute, value)
        {            
        }


    }
}
