using System.Collections.Generic;

namespace NRuler.Rete
{
    /// <summary>
    /// The Production Node.
    /// </summary>
    public class P_Node : ReteNode
    {
        #region Fields
        private readonly Production m_prod;
        private readonly List<Token> m_items;
        #endregion

        #region Properties
        //public Production Production
        //{
        //    get { return this.m_prod; }
        //    //set { this.m_prod = value; }
        //}

        public List<Token> Items
        {
            get { return this.m_items; }            
        }        

        //public int InferredFactsCount
        //{
        //    get { return m_items.Count; }
        //}
        #endregion

        #region Constructors
        public P_Node()
        {
            this.m_type = ReteNodeType.P_Node;
            this.m_items = new List<Token>();
        }

        public P_Node(Production prod):this()
        {
            this.m_prod = prod;
        }
        #endregion

        #region Methods

        // foamliu, 2008/11/23, for publishing (de)activation events.

        public void InsertToken(Token tok)
        {
            this.m_items.Insert(0, tok);

            // foamliu, 2008/12/8, refresh instances cache.
            this.m_prod.FireOnInstanceChange();

            // 0 -> 1
            if (m_items.Count == 1)
            {
                this.m_prod.FireOnActivation(true);                
            }
        }

        public void RemoveToken(Token tok)
        {
            this.m_items.Remove(tok);

            // foamliu, 2008/12/8, refresh instances cache.
            this.m_prod.FireOnInstanceChange();

            // 1 -> 0
            if (m_items.Count == 0)
            {
                this.m_prod.FireOnActivation(false);     
            }
        }

        //private string Tokens2String()
        //{
        //    StringBuilder builder = new StringBuilder();
        //    foreach (Token tok in this.m_items)
        //    {
        //        builder.Append("(");
        //        for (Token p = tok; p != null && !(p is Dummy_Top_Token); p = p.Parent)
        //        {
        //            builder.AppendFormat("{0} and ", p);
        //        }
        //        builder.Append(")\r\n");
        //    }
        //    return builder.ToString();
        //}

        //public override string ToString()
        //{
        //    StringBuilder builder = new StringBuilder();
        //    //builder.AppendFormat("P-Node [{0}]", Tokens2String());
        //    builder.AppendFormat("{0}", Tokens2String());
        //    return builder.ToString();
        //}

        #endregion

    }
}
