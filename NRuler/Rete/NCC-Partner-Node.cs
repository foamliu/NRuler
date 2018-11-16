using System.Collections.Generic;

namespace NRuler.Rete
{
    /// <summary>
    /// An NCC partner stores a pointer to the corresponding NCC node, plus a count of the 
    /// number of conjuncts in the NCC (this is used for stripping off the last several WMEs from each
    /// subnetwork match).
    /// </summary>
    public class NCC_Partner_Node : ReteNode
    {
        #region Fields

        // points to the corresponding NCC node
        private ReteNode m_ncc_node;
        // number of conjuncts in the NCC
        private int m_number_of_conjuncts;
        // results for the match the NCC node hasn't heard about
        private readonly List<Token> m_new_result_buffer;

        #endregion

        #region Properties

        // foamliu, 2008/11/21, add some detailed comments.
        /// <summary>
        /// A pointer to the corresponding NCC node.
        /// </summary>
        public ReteNode NCC_Node
        {
            get { return m_ncc_node; }
            set { this.m_ncc_node = value; }
        }

        // foamliu, 2008/11/21, add some detailed comments.
        /// <summary>
        /// Details: this is used for stripping off the last several WMEs from each subnetwork match.
        /// </summary>
        public int Number_Of_Conjuncts
        {
            get { return m_number_of_conjuncts; }
            set { this.m_number_of_conjuncts = value; }
        }

        // foamliu, 2008/11/21, add some detailed comments.
        /// <summary>
        /// Details: used as a temporary buffer in between the time the subnetwork is activated with
        ///         a new match for the preceding conditions and the time the NCC node is activated with that match.
        ///         It stores the results (if there are any) from the subnetwork for that match.
        /// </summary>
        public List<Token> New_Result_Buffer
        {
            get { return m_new_result_buffer; }
            //set { this.m_new_result_buffer = value; }
        }

        #endregion

        #region Constructors

        public NCC_Partner_Node()
        {
            this.m_type = ReteNodeType.NCC_Partner_Node;
            this.m_new_result_buffer = new List<Token>();
        }

        #endregion

        //public override string ToString()
        //{
        //    StringBuilder builder = new StringBuilder();
        //    builder.AppendFormat("ncc-p-node ");
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