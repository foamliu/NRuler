using System;

namespace NRuler.Terms
{
    public class DateTimeTerm : Term
    {
        public DateTimeTerm(DateTime time)
            : base(time)
        {
            this.m_term_type = TermType.DateTime;
        }

        public new DateTime Value
        {
            get { return (DateTime)this.m_value; }
            set { this.m_value = value; }
        }
    }
}
