using System.Collections.Generic;
using NRuler.Conditions;
using NRuler.Terms;

namespace NRuler.Rete
{
    /// <summary>
    /// 
    /// The Rete Algorithm is intended to improve the speed of forward-chained rule systems 
    /// by limiting the effort required to recompute the conflict set after a rule is fired. 
    /// Its drawback is that it has high memory space requirements. 
    /// It takes advantage of two empirical observations:
    /// 
    ///     Temporal Redundancy:    The firing of a rule usually changes only a few facts, 
    ///                                 and only a few rules are affected by each of those changes. 
    ///     
    ///     Structural Similarity:  The same pattern often appears in the left-hand side of more than one rule. 
    ///     
    /// </summary>    




    // Thus facts are variable-free tuples, patterns are tuples with some variables, 
    //  and rules have as left-hand sides lists of patterns.

    public class ReteInferenceEngine
    {
        // WME take the form of triples (three-tuples)
        public const int WME_FIELD_NUM = 3;

        #region Fields

        private readonly List<WME> m_workingmemory;
        // foamliu, 2008/11/21, remove "P_Nodes", access p-nodes from productions.
        //private List<P_Node> m_p_nodes;
        private readonly Constant_Test_Node m_top_node_of_alpha_network;
        private readonly Dummy_Top_Node m_dummy_top_node;        

        #endregion

        #region Properties

        // for unit tests
        public Constant_Test_Node Top_Node_Of_Alpha_Network
        {
            get { return m_top_node_of_alpha_network; }
        }

        // for unit tests
        public Dummy_Top_Node Dummy_Top_Node
        {
            get { return m_dummy_top_node; }
        }

        // foamliu, 2008/11/21, remove "P_Nodes", access p-nodes from productions.
        // for unit tests
        //public List<P_Node> P_Nodes
        //{
        //    get { return m_p_nodes; }
        //}

        // foamliu, 2008/11/21, for unit tests
        public List<WME> WorkingMemory
        {
            get { return m_workingmemory; }
        }


        #endregion               

        #region Constructors

        public ReteInferenceEngine()
        {
            this.m_top_node_of_alpha_network = new Constant_Test_Node();
            this.m_top_node_of_alpha_network.Field_To_Test = FieldType.No_Test;
            this.m_dummy_top_node = new Dummy_Top_Node();
            // just so there will be one thing to iterate over in the join-node-right-activation procedure.            

            this.m_workingmemory = new List<WME>();

            // foamliu, 2008/11/21, remove "P_Nodes", access p-nodes from productions.
            //
            //this.m_p_nodes = new List<P_Node>();
        }

        #endregion    

        #region Methods

        public void AlphaMemoryActivation(Alpha_Memory node, WME wme)
        {
            // insert w at the head of node.items
            node.Items.Insert(0, wme);

            // for tree-based removal
            wme.Alpha_Mems.Insert(0, node);

            // for each child in node.successors do right-activation (child, w)
            foreach (ReteNode child in node.Successors)
            {
                //Join_Node_Right_Activation((Join_Node)child, w);
                // foamliu, 2008/08/26, use a general method here
                // to support both join-node and negative-node.
                RightActivation(child, wme);
            }

        }

        private Token Make_Token(ReteNode node, Token parent, WME w)
        {
            Token tok = new Token();
            tok.Parent = parent;
            tok.WME = w;
            // for tree-based removal
            tok.Node = node;
            if (parent != null)
            {
                parent.Children.Insert(0, tok);
            }
            // foamliu, 2008/08/25, we need this check for negative conditions
            if (w != null)
                w.Tokens.Insert(0, tok);
            return tok;
        }

        public void BetaMemoryLeftActivation(Beta_Memory node, Token token, WME wme)
        {
            //Token new_token = new Token();
            //new_token.Parent = t;
            //new_token.WME = w;

            Token new_token = Make_Token(node, token, wme);
            node.Items.Insert(0, new_token);

            // for each child in node.children do left-activation (child, new-token)
            foreach (ReteNode child in node.Children)
            {
                JoinNodeLeftActivation((Join_Node)child, new_token);
            }
        }

        // assumes all tests are tests for equality with some constant symbol.
        public void ConstantTestNodeActivation(Constant_Test_Node node, WME w)
        {
            if (node.Field_To_Test != FieldType.No_Test)
            {
                Term v = w.Fields[(int)node.Field_To_Test];
                if (!v.Equals(node.Thing_The_Field_Must_Equal))
                {
                    // failed the test, so don't propagate any further
                    return;
                }
            }
            if (node.Output_Memory != null)
            {
                AlphaMemoryActivation(node.Output_Memory, w);
            }
            foreach (Constant_Test_Node c in node.Children)
            {
                ConstantTestNodeActivation(c, w);
            }
        }

        public void JoinNodeRightActivation(Join_Node node, WME w)
        {
            // parent is the beta memory node
            Beta_Memory parent = (Beta_Memory)node.Parent;
            foreach (Token t in parent.Items)
            {
                //if (t is DummyTopToken)
                //{
                //    Handle_Dummy_Top_Token(node, w);
                //    continue;
                //}

                if (PerformJoinTests(node.Tests, t, w))
                {
                    foreach (ReteNode child in node.Children)
                    {
                        //if (child is Beta_Memory)
                        //{
                        //    // left-activation (child, t, w)
                        //    Beta_Memory_Left_Activation((Beta_Memory)child, t, w);
                        //}
                        //else if (child is P_Node)
                        //{
                        //    P_Node_Left_Activation((P_Node)child, t, w);
                        //}

                        // foamliu, 2008/08/26, use a general method here
                        // to support both join-node and negative-node.
                        LeftActivation(child, t, w);
                    }
                }

            }
        }

        private void HandleDummyTopToken(Join_Node j_node, WME w)
        {
            foreach (ReteNode j_child in j_node.Children)
            {
                Beta_Memory b_node = j_child as Beta_Memory;
                if (null != b_node)
                {
                    Token new_token = new Token();
                    new_token.WME = w;
                    b_node.Items.Insert(0, new_token);

                    // for each child in node.children do left-activation (child, new-token)
                    foreach (ReteNode b_child in b_node.Children)
                    {
                        JoinNodeLeftActivation((Join_Node)b_child, new_token);
                    }
                }
            }
        }

        public void JoinNodeLeftActivation(Join_Node node, Token t)
        {
            foreach (WME w in node.Amem.Items)
            {
                if (PerformJoinTests(node.Tests, t, w))
                {
                    foreach (ReteNode child in node.Children)
                    {
                        //if (child is Beta_Memory)
                        //{
                        //    // left-activation (child, t, w)
                        //    Beta_Memory_Left_Activation((Beta_Memory)child, t, w);
                        //}
                        //else if (child is P_Node)
                        //{
                        //    P_Node_Left_Activation((P_Node)child, t, w);
                        //}

                        // foamliu, 2008/08/26, use a general method here
                        // to support both join-node and negative-node.
                        // foamliu, 2008/11/21, and ncc-node and ncc-partner-node.
                        //
                        LeftActivation(child, t, w);
                    }
                }

            }
        }

        private void P_NodeLeftActivation(P_Node p_Node, Token t, WME w)
        {
            // foamliu, 2008/08/26, otherwise we cannot delete token in p-node.
            Token new_token = Make_Token(p_Node, t, w);
            //p_Node.Items.Insert(0, new_token);
            // foamliu, 2008/11/23, for publishing (de)activation events.
            p_Node.InsertToken(new_token);
        }

        public bool PerformJoinTests(List<Test_At_Join_Node> tests, Token t, WME w)
        {
            //if (t is Dummy_Top_Token)
            //    return true;

            foreach (Test_At_Join_Node this_test in tests)
            {
                Term arg1 = w.Fields[(int)this_test.Field_Of_Arg1];

                // wme2 <- the [this-test.condition-number-of-arg2]'th element in t
                // foamliu, 2008/08/20, change:
                // from: m_condition_number_of_arg2
                //   to: m_number_of_levels_up
                //Token p = t;
                //for (int i = 0; i < (this_test.Number_Of_Levels_Up-1); i++)
                //    p = p.Parent;

                //WME wme2 = p.WME;

                WME wme2 = t[(this_test.Number_Of_Levels_Up - 1)];

                if (wme2 != null)
                {
                    Term arg2 = wme2.Fields[(int)this_test.Field_Of_Arg2];
                    if (!arg1.Equals(arg2))
                        return false;
                }
            }
            return true;
        }

        private void DeleteTokenAndDescendents(Token token)
        {
            while (token.Children.Count != 0)
            {
                DeleteTokenAndDescendents(token.Children[0]);
            }

            // foamliu, 2008/11/20, ncc node.
            if (!(token.Node is NCC_Partner_Node))
            {
                //((Beta_Memory)tok.Node).Items.Remove(tok);
                // foamliu, 2008/08/26, fix the bug.
                // Otherwise we cannot delete token in p-node.
                DeleteToken(token.Node, token);
            }

            // we need this check for negative conditions.
            if (token.WME != null)
            {
                token.WME.Tokens.Remove(token);
            }

            // foamliu, 2008/11/21, more comments and fix a null-ref bug.
            // remove tok from the list tok.parent.children
            //
            if (null != token.Parent)
                token.Parent.Children.Remove(token);

            // foamliu, 2008/08/25, for negative conditions.
            if (token.Node is Negative_Node)
            {
                foreach (Negative_Join_Result jr in token.Join_Results)
                {
                    jr.WME.Negative_Join_Results.Remove(jr);
                }
            }

            // foamliu, 2008/11/20, ncc node.
            if (token.Node is NCC_Node)
            {
                foreach (Token result_tok in token.NCC_Results)
                {
                    result_tok.WME.Tokens.Remove(result_tok);
                    result_tok.Parent.Children.Remove(result_tok);
                }
            }

            if (token.Node is NCC_Partner_Node)
            {
                token.Owner.NCC_Results.Remove(token);
                if (token.Owner.NCC_Results.Count == 0)
                {
                    NCC_Partner_Node partner = token.Node as NCC_Partner_Node;
                    foreach (ReteNode child in partner.NCC_Node.Children)
                    {
                        LeftActivation(child, token.Owner, null);
                    }
                }
            }
            token = null;
        }

        private void DeleteToken(ReteNode node, Token token)
        {
            if (node is Beta_Memory)
            {
                Beta_Memory b_node = (Beta_Memory)node;
                b_node.Items.Remove(token);
            }
            else if (node is P_Node)
            {
                P_Node p_node = (P_Node)node;
                //p_node.Items.Remove(tok);
                // foamliu, 2008/11/23, for publishing (de)activation events.
                p_node.RemoveToken(token);
            }
            else if (node is Negative_Node)
            {
                Negative_Node n_node = (Negative_Node)node;
                n_node.Items.Remove(token);
            }
            else if (node is NCC_Node)
            {
                NCC_Node ncc_node = (NCC_Node)node;
                ncc_node.Items.Remove(token);
            }
        }

        private void RightActivation(ReteNode node, WME w)
        {
            Join_Node j_node;
            Negative_Node n_node;

            if ((j_node = node as Join_Node) != null)
            {
                JoinNodeRightActivation(j_node, w);
            }
            else if ((n_node = node as Negative_Node) != null)
            {
                NegativeNodeRightActivation(n_node, w);
            }
            else
            {
                System.Console.WriteLine("Right_Activation lose type: {0}", node.GetType());
            }
        }

        private void LeftActivation(ReteNode node, Token t)
        {
            LeftActivation(node, t, null);
        }

        private void LeftActivation(ReteNode node, Token t, WME w)
        {
            Beta_Memory b_node;
            P_Node p_node;
            Negative_Node n_node;
            NCC_Node ncc_node;
            NCC_Partner_Node nccp_node;

            if ((b_node = node as Beta_Memory) != null)
            {
                BetaMemoryLeftActivation(b_node, t, w);
            }
            else if ((p_node = node as P_Node) != null)
            {
                P_NodeLeftActivation(p_node, t, w);
            }
            else if ((n_node = node as Negative_Node) != null)
            {
                NegativeNodeLeftActivation(n_node, t, w);
            }
            else if ((ncc_node = node as NCC_Node) != null)
            {
                NccNodeLeftActivation(ncc_node, t, w);
            }
            else if ((nccp_node = node as NCC_Partner_Node) != null)
            {
                NccPartnerNodeLeftActivation(nccp_node, t, w);
            }
            else
            {
                System.Console.WriteLine("Left_Activation lose type: {0}", node.GetType());
            }
        }

        private Alpha_Memory BuildOrShareAlphaMemory(Condition c)
        {
            Constant_Test_Node current_node = this.m_top_node_of_alpha_network;

            for (int j = 0; j < ReteInferenceEngine.WME_FIELD_NUM; j++)
            {
                // for each constant tests in each field of c
                if (!(c.Fields[j] is Variable))
                {
                    Term sym = c.Fields[j];
                    FieldType f = (FieldType)j;
                    current_node = BuildOrShareConstantTestNode(current_node, f, sym);
                }
            }
            if (current_node.Output_Memory != null)
            {
                return current_node.Output_Memory;
            }

            Alpha_Memory am = new Alpha_Memory();
            current_node.Output_Memory = am;
            am.Successors.Clear();
            am.Items.Clear();
            // initialize am with any current WMEs
            foreach (WME w in this.m_workingmemory)
            {
                if (c.PassAllConstantTests(w))
                    AlphaMemoryActivation(am, w);
            }

            return am;
        }

        private ReteNode BuildOrShareBetaMemoryNode(ReteNode parent)
        {
            // foamliu, 2008/12/10, the first is positive, the beta is not necessary
            if (parent is Dummy_Top_Node)
            {
                return parent;
            }

            // look for an existing node to share
            foreach (ReteNode child in parent.Children)
            {
                if (child.Type == ReteNodeType.Beta_Memory)
                    return child;
            }

            Beta_Memory new_beta = new Beta_Memory();
            //new_beta.Type = ReteNodeType.Beta_Memory;
            new_beta.Parent = parent;
            // insert new at the head of the list parent.children
            parent.Children.Insert(0, new_beta);
            //new_beta.Children.Clear();
            //new_beta.Items.Clear();
            UpdateNewNodeWithMatchesFromAbove(new_beta);
            return new_beta;

        }

        private void UpdateNewNodeWithMatchesFromAbove(ReteNode new_node)
        {
            ReteNode parent = new_node.Parent;
            switch (parent.Type)
            {
                case ReteNodeType.Beta_Memory:
                    foreach (Token tok in ((Beta_Memory)parent).Items)
                    {
                        // do left-activation (new-node, tok)
                        LeftActivation(new_node, tok);
                    }
                    break;
                case ReteNodeType.Join_Node:
                    List<ReteNode> saved_list_of_children = new List<ReteNode>();
                    //saved_list_of_children.AddRange(parent.Children);

                    foreach (ReteNode node in parent.Children)
                    {
                        saved_list_of_children.Add(node);
                    }

                    parent.Children.Clear();
                    //parent.Children.Add(new_node);
                    parent.Children.Add(new_node);
                    foreach (WME w in ((Join_Node)parent).Amem.Items)
                    {
                        RightActivation(parent, w);
                    }
                    parent.Children = saved_list_of_children;
                    break;
                case ReteNodeType.Negative_Node:
                    foreach (Token tok in ((Negative_Node)parent).Items)
                    {
                        if (tok.Join_Results.Count == 0)
                            LeftActivation(new_node, tok, null);
                    }
                    break;
                case ReteNodeType.NCC_Node:
                    foreach (Token tok in ((NCC_Node)parent).Items)
                    {
                        if (tok.NCC_Results.Count == 0)
                            LeftActivation(new_node, tok, null);
                    }
                    break;
                default:
                    break;
            }
        }

        private ReteNode BuildOrShareJoinNode(ReteNode parent, Alpha_Memory am, List<Test_At_Join_Node> tests)
        {
            // look for an existing node to share
            foreach (ReteNode child in parent.Children)
            {
                Join_Node j_node = child as Join_Node;

                if (null != j_node)
                {
                    if (j_node.Amem.Equals(am) && Test_At_Join_Node.IsListEquals(j_node.Tests, tests))
                        return child;
                }
            }

            Join_Node new_j_node = new Join_Node();
            new_j_node.Parent = parent;
            // insert new at the head of the list parent.children
            parent.Children.Insert(0, new_j_node);

            // foamliu, 2008/11/29, new object, no need.
            //new_j_node.Children.Clear();
            new_j_node.Tests = tests;
            new_j_node.Amem = am;
            am.Successors.Insert(0, new_j_node);

            return new_j_node;
        }

        private Constant_Test_Node BuildOrShareConstantTestNode(Constant_Test_Node parent, FieldType field, Term term)
        {
            // look for an existing node we can share
            foreach (Constant_Test_Node child in parent.Children)
            {
                if (child.Field_To_Test == field && child.Thing_The_Field_Must_Equal.Equals(term))
                    return child;
            }
            // couldn't find a node to share, so build a new one
            Constant_Test_Node _new = new Constant_Test_Node();
            parent.Children.Add(_new);
            _new.Field_To_Test = field;
            _new.Thing_The_Field_Must_Equal = term;
            _new.Output_Memory = null;
            _new.Children.Clear();
            return _new;
        }

        private List<Test_At_Join_Node> GetJoinTestsFromCondition(Condition c, List<Condition> earlier_conds)
        {
            List<Test_At_Join_Node> result = new List<Test_At_Join_Node>();

            for (int j = 0; j < ReteInferenceEngine.WME_FIELD_NUM; j++)
            {
                if (c.Fields[j] is Variable)
                {
                    Term v = c.Fields[j];
                    FieldType f = (FieldType)j;
                    for (int i = earlier_conds.Count - 1; i >= 0; i--)
                    {
                        for (int k = 0; k < ReteInferenceEngine.WME_FIELD_NUM; k++)
                        {
                            if (earlier_conds[i].Fields[k] is Variable && earlier_conds[i].Fields[k].Equals(v))
                            {
                                FieldType f2 = (FieldType)k;
                                Test_At_Join_Node this_test = new Test_At_Join_Node();
                                this_test.Field_Of_Arg1 = f;
                                //this_test.Number_Of_Levels_Up = i;
                                //foamliu, 2008/08/20, change:
                                //from: condition-number-of-arg2
                                //  to: number-of-levels-up
                                //since the current condition index = earlier_conds.Count
                                //we have: Number_Of_Levels_Up = earlier_conds.Count - i
                                this_test.Number_Of_Levels_Up = earlier_conds.Count - i;
                                this_test.Field_Of_Arg2 = f2;
                                result.Add(this_test);
                            }
                        }
                    }
                }
            }

            return result;
        }

        public void AddWme(WME wme)
        {
            if (!this.m_workingmemory.Contains(wme))
            {
                ConstantTestNodeActivation(this.m_top_node_of_alpha_network, wme);

                // foamliu, 2008-08-14, maintain working memory
                this.m_workingmemory.Add(wme);
            }
        }

        /// <summary>
        /// foamliu, 2008/11/24, more comments.
        /// 
        /// When a WME is retracted from working memory, it must be removed from every alpha memory in which it is stored. 
        /// In addition, WME lists that contain the WME must be removed from beta memories, and activated production instances 
        /// for these WME lists must be de-activated and removed from the agenda. 
        /// </summary>
        /// <param name="w"></param>
        public void RemoveWme(WME w)
        {
            foreach (Alpha_Memory am in w.Alpha_Mems)
            {
                am.Items.Remove(w);
            }
            while (w.Tokens.Count != 0)
            {
                DeleteTokenAndDescendents(w.Tokens[0]);
            }

            // foamliu, 2008/08/25, for supporting negated conditions.
            foreach (Negative_Join_Result jr in w.Negative_Join_Results)
            {
                jr.Owner.Join_Results.Remove(jr);
                if (jr.Owner.Join_Results.Count != 0)
                {
                    foreach (ReteNode child in jr.Owner.Node.Children)
                    {
                        BetaMemoryLeftActivation((Beta_Memory)child, jr.Owner, null);
                    }
                }
            }

            // foamliu, 2008-08-14, maintain working memory
            this.m_workingmemory.Remove(w);
        }

        //public void Add_Production(List<Condition> lhs)
        //{
        //    if (lhs == null || lhs.Count==0)
        //        return;

        //    //// dummy-top-node
        //    RETE_Node current_node = this.m_dummy_top_node;
        //    List<Condition> earlier_conditions = new List<Condition>();


        //    //Condition c0 = lhs[0];
        //    //List<Test_At_Join_Node> tests = Get_Join_Tests_From_Condition(c0, earlier_conditions);
        //    //Alpha_Memory am = Build_Or_Share_Alpha_Memory(c0);
        //    //current_node = Build_Or_Share_Join_Node(current_node, am, tests);

        //    //for (int i = 1; i < lhs.Count; i++)
        //    //{
        //    //    Condition ci = lhs[i];
        //    //    current_node = Build_Or_Share_Beta_Memory_Node(current_node);
        //    //    earlier_conditions.Add(lhs[i - 1]); // c(i-1)
        //    //    tests = Get_Join_Tests_From_Condition(ci, earlier_conditions);
        //    //    am = Build_Or_Share_Alpha_Memory(ci);
        //    //    current_node = Build_Or_Share_Join_Node(current_node, am, tests);
        //    //}

        //    current_node = Build_Or_Share_Network_For_Conditions(current_node, lhs, earlier_conditions);

        //    P_Node p_node = new P_Node();
        //    current_node.Children.Add(p_node);
        //    p_node.Parent = current_node;
        //    this.P_Nodes.Add(p_node);
        //    Update_New_Node_With_Matches_From_Above(p_node);
        //}

        public void AddProduction(Production prod)
        {
            ReteNode current_node = BuildOrShareNetworkForConditions(this.m_dummy_top_node, prod.LHS, new List<Condition>());

            // foamliu, 2008/11/21, more comments.
            // build a new production node
            P_Node p_node = new P_Node(prod);
            prod.P_Node = p_node;

            // make it a child of current node            
            current_node.Children.Add(p_node);
            p_node.Parent = current_node;

            // foamliu, 2008/11/21, remove "P_Nodes", access p-nodes from productions.
            //this.P_Nodes.Add(p_node);
            UpdateNewNodeWithMatchesFromAbove(p_node);
        }

        public void RemoveProduction(Production prod)
        {
            DeleteNodeAndAnyUnusedAncestors(prod.P_Node);
        }

        private void DeleteNodeAndAnyUnusedAncestors(ReteNode node)
        {
            // foamliu, 2008/11/20, for NCC nodes, delete the partner node too
            NCC_Node ncc_node = node as NCC_Node;
            if (null != ncc_node)
            {
                DeleteNodeAndAnyUnusedAncestors(ncc_node.Partner);
            }

            // foamliu, 2008/11/20, clean up any tokens the node contains
            switch (node.Type)
            {
                case ReteNodeType.Beta_Memory:
                    Beta_Memory b_node = (Beta_Memory)node;
                    while (b_node.Items.Count != 0 && !(b_node.Items[0] is Dummy_Top_Token))
                    {
                        DeleteTokenAndDescendents(b_node.Items[0]);
                    }
                    break;
                case ReteNodeType.Negative_Node:
                    Negative_Node n_node = (Negative_Node)node;
                    while (n_node.Items.Count != 0)
                    {
                        DeleteTokenAndDescendents(n_node.Items[0]);
                    }
                    break;
                case ReteNodeType.NCC_Node:
                    while (ncc_node.Items.Count != 0)
                    {
                        DeleteTokenAndDescendents(ncc_node.Items[0]);
                    }
                    break;

                default:
                    break;
            }

            NCC_Partner_Node par_node = node as NCC_Partner_Node;
            if (null != par_node)
            {
                while (par_node.New_Result_Buffer.Count != 0)
                {
                    DeleteTokenAndDescendents(par_node.New_Result_Buffer[0]);
                }
            }

            // foamliu, 2008/11/20, for join and negative nodes, deal with the alpha memory
            // 

            Join_Node j_node = node as Join_Node;
            if (null != j_node)
            {
                j_node.Amem.Successors.Remove(node);
                if (j_node.Amem.Successors.Count == 0)
                {
                    DeleteAlphaMemory(j_node.Amem);
                }
            }
            else if (node.Type == ReteNodeType.Negative_Node)
            {
                Negative_Node n_node = (Negative_Node)node;
                n_node.Amem.Successors.Remove(node);
                if (n_node.Amem.Successors.Count == 0)
                {
                    DeleteAlphaMemory(n_node.Amem);
                }
            }

            // foamliu, 2008/11/21, fix a null-ref bug.
            if (null != node.Parent)
            {
                node.Parent.Children.Remove(node);

                if (node.Parent.Children.Count == 0)
                {
                    DeleteNodeAndAnyUnusedAncestors(node.Parent);
                }
            }

            node = null;
        }

        private void DeleteAlphaMemory(Alpha_Memory alpha_Memory)
        {
            // foamliu, 2008/11/21.
            // TODO: send to GC.
        }

        // for supporting negated conditions

        private void NegativeNodeLeftActivation(Negative_Node node, Token t, WME w)
        {
            // build and store a new token, just like a beta memory would
            Token new_token = Make_Token(node, t, w);
            node.Items.Insert(0, new_token);

            // compute the join results
            new_token.Join_Results.Clear();
            foreach (WME item in node.Amem.Items)
            {
                if (PerformJoinTests(node.Tests, new_token, item))
                {
                    Negative_Join_Result jr = new Negative_Join_Result();
                    jr.Owner = new_token;
                    jr.WME = w;
                    new_token.Join_Results.Insert(0, jr);
                    if (w != null)
                    {
                        w.Negative_Join_Results.Insert(0, jr);
                    }
                }
            }

            // if join results is empty, then inform children
            if (new_token.Join_Results.Count == 0)
            {
                foreach (ReteNode child in node.Children)
                {
                    //Beta_Memory_Left_Activation((Beta_Memory)child, new_token, null);

                    // foamliu, 2008/08/26, use a general method here
                    // to support both join-node and negative-node.
                    LeftActivation(child, new_token, null);
                }
            }
        }

        private void NegativeNodeRightActivation(Negative_Node node, WME w)
        {
            foreach (Token t in node.Items)
            {
                if (PerformJoinTests(node.Tests, t, w))
                {
                    if (t.Join_Results.Count == 0)
                    {
                        DeleteDescendentsOfToken(t);
                    }
                    Negative_Join_Result jr = new Negative_Join_Result();
                    jr.Owner = t;
                    jr.WME = w;
                    t.Join_Results.Insert(0, jr);
                    w.Negative_Join_Results.Insert(0, jr);
                }
            }
        }

        private void DeleteDescendentsOfToken(Token t)
        {
            while (t.Children.Count != 0)
            {
                DeleteTokenAndDescendents(t.Children[0]);
            }
        }

        private ReteNode BuildOrShareNetworkForConditions(ReteNode parent, List<Condition> conds, List<Condition> earlier_conds)
        {
            // foamliu, 2008/11/20.
            ReteNode current_node = parent;
            List<Condition> conds_higher_up = earlier_conds;
            List<Test_At_Join_Node> tests;
            Alpha_Memory am;
            int k = conds.Count;

            for (int i = 0; i < k; i++)
            {
                Condition ci = conds[i];

                if (ci.Type == ConditionType.Positive)
                {
                    current_node = BuildOrShareBetaMemoryNode(current_node);
                    tests = GetJoinTestsFromCondition(ci, conds_higher_up);
                    am = BuildOrShareAlphaMemory(ci);
                    current_node = BuildOrShareJoinNode(current_node, am, tests);
                }
                else if (ci.Type == ConditionType.Negative)
                {
                    tests = GetJoinTestsFromCondition(ci, conds_higher_up);
                    am = BuildOrShareAlphaMemory(ci);
                    current_node = BuildOrShareNegativeNode(current_node, am, tests);
                }
                else if (ci.Type == ConditionType.NCC)
                {
                    current_node = BuildOrShareNccNodes(current_node, (NCCCondition)ci, conds_higher_up);
                }

                conds_higher_up.Add(ci);
            }

            return current_node;
        }

        private ReteNode BuildOrShareNegativeNode(ReteNode parent, Alpha_Memory am, List<Test_At_Join_Node> tests)
        {
            // look for an existing node to share
            foreach (ReteNode child in parent.Children)
            {
                if (child.Type == ReteNodeType.Negative_Node)
                {
                    Negative_Node n_node = (Negative_Node)child;

                    if (n_node.Amem.Equals(am) && Test_At_Join_Node.IsListEquals(n_node.Tests, tests))
                        return child;
                }
            }

            Negative_Node new_n_node = new Negative_Node();
            new_n_node.Type = ReteNodeType.Negative_Node;
            new_n_node.Parent = parent;
            // insert new at the head of the list parent.children
            parent.Children.Insert(0, new_n_node);
            //new_n_node.Children.Clear();
            //new_n_node.Items.Clear();
            new_n_node.Tests = tests;
            new_n_node.Amem = am;

            am.Successors.Insert(0, new_n_node);

            return new_n_node;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="current_node"></param>
        /// <param name="condition">The NCC condition.</param>
        /// <param name="conds_higher_up"></param>
        /// <returns>Returns the NCC node</returns>
        private ReteNode BuildOrShareNccNodes(ReteNode parent, NCCCondition condition, List<Condition> earlier_conds)
        {
            ReteNode bottom_of_subnetwork = BuildOrShareNetworkForConditions(parent, condition.SubConditions, earlier_conds);

            // look for an existing node to share
            foreach (ReteNode child in parent.Children)
            {
                NCC_Node ncc_child = child as NCC_Node;
                if (null != ncc_child)
                {
                    if (ncc_child.Partner.Parent == bottom_of_subnetwork)
                        return child;
                }

            }

            NCC_Node new_ncc_partner = new NCC_Node();
            NCC_Partner_Node new_partner = new NCC_Partner_Node();
            new_ncc_partner.Parent = parent;

            // foamliu, 2008/11/21, more comments
            // insert new at the tail of the list bottom_of_subnetwork.children, so the subnetwork comes first.
            // 
            parent.Children.Add(new_ncc_partner);
            new_partner.Parent = bottom_of_subnetwork;
            bottom_of_subnetwork.Children.Insert(0, new_partner);

            //new_ncc_partner.Children.Clear();
            //new_partner.Children.Clear();

            new_ncc_partner.Partner = new_partner;
            new_partner.NCC_Node = new_ncc_partner;

            //new_ncc_partner.Items.Clear();
            //new_partner.New_Result_Buffer.Clear();
            new_partner.Number_Of_Conjuncts = condition.SubConditions.Count;

            // foamliu, 2008/11/21, more comments
            // Note: we have to inform NCC node of existing matches before informing the partner, otherwise 
            //  lots of matches would all get mixed together in the new-result-buffer.
            //
            UpdateNewNodeWithMatchesFromAbove(new_ncc_partner);
            UpdateNewNodeWithMatchesFromAbove(new_partner);

            return new_ncc_partner;
        }

        private void NccNodeLeftActivation(NCC_Node node, Token t, WME w)
        {
            // build and store a new token, just like a beta memory would
            Token new_token = Make_Token(node, t, w);
            // insert new-token at the head of node.Items
            node.Items.Insert(0, new_token);

            // get initial ncc results
            new_token.NCC_Results.Clear();

            NCC_Partner_Node partner = node.Partner as NCC_Partner_Node;

            //foreach (Token result in partner.New_Result_Buffer)
            //{
            //    partner.New_Result_Buffer.Remove(result);

            //    new_token.NCC_Results.Insert(0, result);                

            //    result.Owner = new_token;
            //}

            // foamliu, 2008/12/10, fix following error:
            //  System.InvalidOperationException : Collection was modified; 
            //  enumeration operation may not execute.

            while (partner.New_Result_Buffer.Count != 0)
            {
                Token result = partner.New_Result_Buffer[0];
                partner.New_Result_Buffer.Remove(result);
                new_token.NCC_Results.Insert(0, result);
                result.Owner = new_token;
            }


            // if no ncc results, then inform children
            if (0 == new_token.NCC_Results.Count)
            {
                foreach (ReteNode child in node.Children)
                {
                    LeftActivation(child, new_token, null);
                }
            }
        }

        private void NccPartnerNodeLeftActivation(NCC_Partner_Node partner, Token t, WME w)
        {
            NCC_Node ncc_node = partner.NCC_Node as NCC_Node;
            // build a result token <t,w>
            Token new_result = Make_Token(partner, t, w);

            // To find the appropriate owner token (into whose local memory we should put this
            //  result), first figure out what pair <owners-t,owners-w> would represent the owner. To
            //  do this, we start with the <t,w> pair we just received, and walk up the right number
            //  of links to find the pair that emerged from the join node for the preceding condition. 
            Token owners_t = t;
            WME owners_w = w;
            for (int i = 0; i < partner.Number_Of_Conjuncts; i++)
            {
                owners_w = owners_t.WME;
                owners_t = owners_t.Parent;
            }

            // Look for this owner in the NCC node's memory. If we find it, add new-result to its
            //  local memory, and propagate (deletions) to the NCC node's children.
            Token owner = ncc_node.FindOwner(owners_t, owners_w);
            if (null != owner)
            {
                owner.NCC_Results.Add(new_result);

                new_result.Owner = owner;
                DeleteDescendentsOfToken(owner);
            }
            else
            {
                // We didn't find an appropriate owner token already in the NCC node's memory.
                //  This means the subnetwork has been activated for a new match for the preceding
                //  conditions, and this new result emerged from the bottom of the subnetwork, but
                //  the NCC node hasn't been activated for the new match yet. So, we just stuff the
                //  result in our temporary buffer.
                partner.New_Result_Buffer.Insert(0, new_result);
            }

        }

        #endregion


    }
}
