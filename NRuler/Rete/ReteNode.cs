using System.Collections.Generic;

namespace NRuler.Rete
{    
    /// <summary>
    /// Nodes in beta network of rete
    /// </summary>
    
    public class ReteNode
    {
        #region Fields

        protected ReteNodeType m_type;
        protected List<ReteNode> m_children;        
        protected ReteNode m_parent;

        #endregion

        #region Properties

        public ReteNodeType Type
        {
            get { return this.m_type; }
            set { this.m_type = value; }
        }

        public List<ReteNode> Children
        {
            get { return this.m_children; }
            set { this.m_children = value; }
        }

        public ReteNode Parent
        {
            get { return this.m_parent; }
            set { this.m_parent = value; }
        }       
 
        #endregion

        #region ctors

        public ReteNode()
        {
            this.m_children = new List<ReteNode>();
        }

        //public override string ToString()
        //{
        //    if (this.Type == ReteNodeType.Beta_Memory)
        //    {
        //        Beta_Memory b_node = (Beta_Memory)this;
        //        return b_node.ToString();
        //    }
        //    else if (this.Type == ReteNodeType.Join_Node)
        //    {
        //        Join_Node j_node = (Join_Node)this;
        //        return j_node.ToString();
        //    }
        //    else if (this.Type == ReteNodeType.P_Node)
        //    {
        //        P_Node p_node = (P_Node)this;
        //        return p_node.ToString();
        //    }

        //    return base.ToString();                
        //}

        #endregion

    }
}
