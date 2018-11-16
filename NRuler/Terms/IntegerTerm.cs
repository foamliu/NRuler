using System;

namespace NRuler.Terms
{
    public class IntegerTerm : Term
    {
        #region Constructors
        
        public IntegerTerm(String str)
            : this(Int32.Parse(str))
        {
        }

        public IntegerTerm(Int32 i)
            : base(i)
        {
            this.m_term_type = TermType.Integer;
        }

        #endregion


        public new Int32 Value
        {
            get { return (Int32)m_value; }
            set { m_value = value; }
        }

        
    }
}
