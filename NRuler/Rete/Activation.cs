
namespace NRuler.Rete
{
    public class Activation
    {
        private readonly Production m_prod;

        public Production Production
        {
            get
            {
                return this.m_prod;
            }
        }

        public Activation(Production prod)
        {
            this.m_prod = prod;
        }
    }
}
