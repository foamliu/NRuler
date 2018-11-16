using System.Collections.Generic;

namespace NRuler.Rete
{
    /// <summary>
    /// An alpha memory stores a list of the WMEs it contains, plus a list of its successors (join nodes attached to it).
    /// </summary>
    public class Alpha_Memory
    {

        #region Fields

        //private readonly List<WME> m_items;
        // foamliu, 2008/12/3, optimization.
        private readonly List<WME> m_items;

        // join-nodes attached to it
        //private readonly List<ReteNode> m_successors;
        // foamliu, 2008/12/3, optimization.
        private readonly List<ReteNode> m_successors;

        #endregion

        #region Properties

        public List<WME> Items
        {
            get { return this.m_items; }            
        }

        public List<ReteNode> Successors
        {
            get { return this.m_successors; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the Alpha_Memory class
        /// </summary>
        public Alpha_Memory()
        {
            this.m_items = new List<WME>();
            this.m_successors = new List<ReteNode>();
        }

        #endregion

        //public override string ToString()
        //{
        //    StringBuilder builder = new StringBuilder();
        //    builder.Append("[");
        //    foreach (WME w in this.Items)
        //    {
        //        builder.AppendFormat("{0}, ", w);
        //    }
        //    builder.Append("]");
        //    return builder.ToString();            
        //}

    }
}
