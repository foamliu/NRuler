using System.Collections.Generic;

namespace NRuler.Rete
{
    /// <summary>
    /// A  beta memory node stores a list of the tokens it contains, plus a list of its children (other nodes 
    ///     in the beta part of the network). 
    /// </summary>
    
    public class Beta_Memory : ReteNode
    {
        #region Fields

        //private readonly List<Token> m_items;
        // foamliu, 2008/12/3, optimization.
        protected readonly List<Token> m_items;

        #endregion

        #region Properties

        public List<Token> Items
        {
            get { return this.m_items; }
        }

        #endregion

        #region Constructors

        public Beta_Memory()
        {
            this.m_type = ReteNodeType.Beta_Memory;
            this.m_items = new List<Token>();
        }

        #endregion

        //private string Tokens2String()
        //{
        //    StringBuilder builder = new StringBuilder();
        //    foreach (Token tok in this.Items)
        //    {
        //        builder.Append("(");
        //        for (Token p = tok; p != null && !(p is Dummy_Top_Token); p = p.Parent)
        //        {
        //            builder.AppendFormat("{0} and ", p);
        //        }
        //        builder.Append(")");
        //    }
        //    return builder.ToString();
        //}

        //public override string ToString()
        //{
        //    StringBuilder builder = new StringBuilder();
        //    builder.AppendFormat("B-Node [{0}]", Tokens2String());
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
