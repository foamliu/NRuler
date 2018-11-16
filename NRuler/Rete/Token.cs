using System.Collections.Generic;

namespace NRuler.Rete
{
    public class Token
    {
        #region Fields

        // points to the higher token, for items 1...i-1
        private Token m_parent;
        // gives item i
        private WME m_wme;
        // points to the memory this token is in
        private ReteNode m_node;

        // the ones with parent=this token
        //private readonly List<Token> m_children;
        // foamliu, 2008/12/3, optimization.
        private readonly List<Token> m_children;

        // foamliu, 2008/08/25, used only on tokens in negative nodes        
        // foamliu, 2008/12/3, optimization.
        private readonly List<Negative_Join_Result> m_join_results;

        // foamliu, 2008/11/19, similiar to join-results but for NCC nodes
        //private readonly List<Token> m_ncc_results;
        // foamliu, 2008/12/3, optimization.
        private readonly List<Token> m_ncc_results;

        // foamliu, 2008/11/19, on tokens in NCC partners: token in whose local memory this result resides
        private Token m_owner;

        #endregion

        #region Properties

        public Token Parent
        {
            get { return this.m_parent; }
            set { this.m_parent = value; }
        }

        public WME WME
        {
            get { return this.m_wme; }
            set { this.m_wme = value; }
        }

        public ReteNode Node
        {
            get { return this.m_node; }
            set { this.m_node = value; }
        }

        public List<Token> Children
        {
            get { return this.m_children; }           
        }

        public List<Negative_Join_Result> Join_Results
        {
            get { return this.m_join_results; }            
        }

        public List<Token> NCC_Results
        {
            get { return this.m_ncc_results; }            
        }

        public Token Owner
        {
            get { return this.m_owner; }
            set { this.m_owner = value; }
        }
        
        /// <summary>
        /// foamliu, 2009/04/27, 为了简便 Rete.PerformJoinTests.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public WME this[int number]
        {
            get
            {
                Token p = this;
                for (int i = 0; i < number; i++)
                    p = p.Parent;
                return p.WME;
            }
        }

        #endregion

        #region Constructors

        public Token()
        {
            this.m_children = new List<Token>();
            // foamliu, 2008/08/25, used only on tokens in negative nodes
            this.m_join_results = new List<Negative_Join_Result>();

            // foamliu, 2008/11/20, fix a null-ref bug.
            this.m_ncc_results = new List<Token>();

        }

        #endregion

        public override string ToString()
        {
            return string.Format("{0}", m_wme);
        }
    }

}
