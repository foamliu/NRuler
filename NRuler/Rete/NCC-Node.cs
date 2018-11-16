using System.Collections.Generic;

namespace NRuler.Rete
{
    /// <summary>
    /// Negated Conjunctive Conditions (NCC's), also called conjunctive negations, which test for the absence
    /// of a certain combination of WMEs.
    /// </summary>
    public class NCC_Node : ReteNode
    {
        #region Fields

        //private readonly List<Token> m_items;
        // foamliu, 2008/12/3, optimization.
        private readonly List<Token> m_items;
        // points to the corresponding NCC partner node
        private ReteNode m_partner;

        #endregion

        #region Properties

        public List<Token> Items
        {
            get { return this.m_items; }
            
        }

        public ReteNode Partner
        {
            get { return this.m_partner; }
            set { this.m_partner = value; }
        }

        #endregion

        #region Constructors

        public NCC_Node()
        {
            this.Type = ReteNodeType.NCC_Node;

            this.m_items = new List<Token>();
        }

        #endregion

        public Token FindOwner(Token token, WME wme)
        {
            // foamliu, 2008/12/10.
            if (wme == null)
                return null;

            foreach (Token t in this.Items)
            {
                if (token.Equals(t.Parent) && wme.Equals(t.WME))
                    return t;
            }
            return null;
        }

        //public override string ToString()
        //{
        //    StringBuilder builder = new StringBuilder();
        //    builder.AppendFormat("ncc-node ");
        //    if (this.Children.Count != 0)
        //    {
        //        foreach (ReteNode child in this.Children)
        //        {
        //            builder.AppendFormat("({0})", child);
        //        }
        //    }
        //    return builder.ToString();
        //}
    }
}
