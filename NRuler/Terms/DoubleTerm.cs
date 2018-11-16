using System;

namespace NRuler.Terms
{
    public class DoubleTerm : Term
    {
        public DoubleTerm(String s) : this(Double.Parse(s))
        {
        }
        
        public DoubleTerm(double d) : base(d)
        {
            this.m_term_type = TermType.Double;
        }
        
        public DoubleTerm(Single d) : base(d)
        {
            this.m_term_type = TermType.Double;
        }
       
        public DoubleTerm(decimal d) : base(Convert.ToDouble(d))
        {
            this.m_term_type = TermType.Double;
        }

        public new double Value
        {
            get { return (double)this.m_value; }
            set { this.m_value = value; }
        }


    }
}
