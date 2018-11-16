
namespace NRuler.Rete
{
    public class Negative_Join_Result
    {
        // the token in whose local memory this result resides
        private Token m_owner;
        // the WME that matches owner
        private WME m_wme;

        public Token Owner
        {
            get { return this.m_owner; }
            set { this.m_owner = value; }
        }

        public WME WME
        {
            get { return this.m_wme; }
            set { this.m_wme = value; }
        }
    }
}
