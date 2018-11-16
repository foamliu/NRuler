using System.Collections.Generic;

namespace NRuler.Rete
{

    /// <summary>
    /// foamliu, 2008/12/10.
    /// Note: 
    ///     (1) only one;
    ///     (2) must be last.
    /// </summary>
    public class Negative_Node: ReteNode
    {
        // just like for a beta memory
        //private readonly List<Token> m_items;
        // foamliu, 2008/12/3, optimization.
        private readonly List<Token> m_items;

        // just like for a join node
        // points to the alpha memory this node is attached to
        private Alpha_Memory m_amem;
        private List<Test_At_Join_Node> m_tests;

        public List<Token> Items
        {
            get { return this.m_items; }
            //set { this.m_items = value; }
        }

        public Alpha_Memory Amem
        {
            get { return this.m_amem; }
            set { this.m_amem = value; }
        }

        public List<Test_At_Join_Node> Tests
        {
            get { return this.m_tests; }
            set { this.m_tests = value; }
        }

        public Negative_Node()
        {
            this.Type = ReteNodeType.Negative_Node;

            this.m_items = new List<Token>();
            this.m_tests = new List<Test_At_Join_Node>();
        }

        //public override string ToString()
        //{
        //    StringBuilder builder = new StringBuilder();
        //    builder.Append("N-Node ");
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
