
namespace NRuler.Terms
{
    public class BooleanTerm : Term
    {
        public BooleanTerm(bool flag)
            : base(flag)
        {
            this.m_term_type = TermType.Boolean;
        }

        public new bool Value
        {
            get { return (bool)this.m_value; }
            set { this.m_value = value; }
        }
        


    }
}
