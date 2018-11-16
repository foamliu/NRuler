using System;

namespace NRuler.Terms
{
    public class NullTerm : Term
    {
        public NullTerm()
        {            
            this.m_term_type = TermType.Null;
            this.m_value = null;
        }

        public new object Value
        {
            get { return null; }
            set { if (value != null) throw new NullReferenceException("Value must be null."); }
        }

        public override int GetHashCode()
        {
            return 0;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return true;
            NullTerm other = obj as NullTerm;
            if (other != null)
            {
                return true;
            }
            return false;
        }

    }
}
