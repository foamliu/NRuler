using System.Collections.Generic;

namespace NRuler.Rete
{
    public class Join_Node : ReteNode
    {
        #region Fields
        // points to the alpha memory this node is attached to
        private Alpha_Memory m_amem;
        //private List<Test_At_Join_Node> m_tests;
        private List<Test_At_Join_Node> m_tests;

        #endregion

        #region Properties

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

        #endregion

        #region ctors

        public Join_Node()
        {
            this.Type = ReteNodeType.Join_Node;
            this.m_tests = new List<Test_At_Join_Node>();
        }

        //public override string ToString()
        //{
        //    StringBuilder builder = new StringBuilder();
        //    builder.Append("j-node ");
        //    if (this.Children.Count != 0)
        //    {
        //        foreach (ReteNode child in this.Children)
        //        {
        //            builder.AppendFormat("({0})", child);
        //        }
        //    }
        //    return builder.ToString();
        //}

        #endregion

    }    
}
