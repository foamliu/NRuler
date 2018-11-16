using System;
using System.Collections.Generic;
using NRuler.Conditions;
using NRuler.Terms;

namespace NRuler.Rete
{
    [Serializable]
    public class Production
    {
        #region Fields
        // foamliu, 2008/11/21, comment it, as it is ref-count == 0, :-)
        //private Rete m_engine;
        private string m_name;
        [NonSerialized]
        private P_Node m_p_node;

        private readonly List<Condition> m_lhs;
        // foamliu, 2008/11/21, right-hand-side as condition.        
        //                      
        private readonly List<Condition> m_rhs;
        
        // foamliu, 2008/11/21, for conflicts resolution.
        private int m_priority;
        //private DateTime m_timeStamp;

        // foamliu, 2008/11/23, agenda.
        [NonSerialized]
        private Agenda m_agenda;

        // foamliu, 2008/12/08, support variable-in-rhs scenario.
        private readonly List<Instance> m_instances;
        private readonly List<WME> m_inferredFacts;
        #endregion

        #region Properties
        public string Name
        {
            get { return this.m_name; }
            set { this.m_name = value; }
        }

        public P_Node P_Node
        {
            get { return this.m_p_node; }
            set { this.m_p_node = value; }
        }

        public Agenda Agenda
        {
            get { return this.m_agenda; }
            set { this.m_agenda = value; }
        }

        public List<Condition> LHS
        {
            get { return this.m_lhs; }            
        }

        public List<Condition> RHS
        {
            get { return this.m_rhs; }            
        }        

        // foamliu, 2008/11/21, for conflicts resolution.
        //
        public int Priority
        {
            get
            {
                return this.m_priority;
            }
            set 
            { 
                this.m_priority = value; 
            }
        }

        //public DateTime TimeStamp
        //{
        //    get
        //    {
        //        return this.m_timeStamp;
        //    }
        //}

        public bool Activated
        {
            get
            {
                //return this.P_Node.m_items.Count > 0; 
                // foamliu, 2008/11/23, for publishing (de)activation events.
                return this.P_Node.Items.Count > 0;
            }
        }

        public List<WME> InferredFacts
        {
            get
            {
                return this.m_inferredFacts;
            }
        }

        public List<Instance> Instances
        {
            get
            {
                return this.m_instances;
            }
        }

        #endregion

        #region Events

        public event ActivationEventHandler OnActivation;

        #endregion

        #region Constructors
        public Production()
        {
            this.m_lhs = new List<Condition>();
            this.m_rhs = new List<Condition>();  

            // foamliu, 2008/11/21, for conflicts resolution.
            //
            //this.m_priority = 1;
            //this.m_timeStamp = DateTime.Now;

            // foamliu, 2008/12/08, support variable-in-rhs scenario.
            this.m_instances = new List<Instance>();
            this.m_inferredFacts = new List<WME>();
        }

        // foamliu, 2008/11/21, comment it, as it is ref-count == 0, :-)

        //public Production(Rete engine)
        //{
        //    this.m_lhs = new List<Condition>();
        //    this.m_rhs = new List<ReAction>();
        //    this.m_variablenames = new Dictionary<string, Variable>();
        //    this.m_p_node = new P_Node();
        //    this.m_p_node.Production = this;

        //    this.m_engine = engine;
        //}
        #endregion

        #region Methods

        public void FireOnActivation(bool activated)
        {            
            if (null != this.OnActivation)
            {
                Activation activation = new Activation(this);
                ActivationEventArgs eventArgs = new ActivationEventArgs(activation, activated);
                this.OnActivation(this, eventArgs);
            }
        }

        /// <summary>
        /// foamliu, 2008/12/8, refresh instances cache.
        /// </summary>
        public void FireOnInstanceChange()
        {
            this.RefreshInstances();
            this.RefreshInferredFacts();
        }

        /// <summary>
        /// foamliu, 2008/12/08, support variable-in-rhs scenario.
        /// </summary>
        private void RefreshInferredFacts()
        {
            this.m_inferredFacts.Clear();

            // instances have been just updated
            foreach (Instance inst in this.m_instances)
            {
                foreach (Condition cond in this.m_rhs)
                {
                    if (cond.Type == ConditionType.Assert)
                    {
                        WME wme = new WME();
                        for (int i = 0; i < wme.Fields.Length; i++)
                        {
                            if (cond.Fields[i].TermType == TermType.Variable)
                            {
                                Variable var = cond.Fields[i] as Variable;
                                Term value = inst.GetVariableValue(var.Name);
                                wme.Fields[i] = value;
                            }
                            else
                            {
                                wme.Fields[i] = cond.Fields[i];
                            }
                        }
                        this.m_inferredFacts.Add(wme);
                    }
                }
            }
        }

        // foamliu, 2008/12/8, implement rules-with-variables and reasoner.
        // Dynamic, store it will introduce sync-up pain, create it on demand.
        private void RefreshInstances()
        {
            this.m_instances.Clear();

            for (int i = 0; i < this.m_p_node.Items.Count; i++)
            {
                this.m_instances.Add(BuildInstance(i));
            }
        }
        
        private Instance BuildInstance(int index)
        {
            Instance instance = new Instance(this);
            // avoid duplicates
            List<string> boundVars = new List<string>();

            int k = this.LHS.Count;            

            for (int i = 0; i < k; i++)
            {
                Condition cond = this.LHS[i];
                // foamliu, 2008/12/08, variables in NCC conditions are ignored.
                if (cond.Type == ConditionType.NCC)
                    continue;

                for (int j = 0; j < ReteInferenceEngine.WME_FIELD_NUM; j++)
                {
                    Term term = cond.Fields[j];

                    if (term.TermType == TermType.Variable)
                    {
                        Variable var = term as Variable;
                        if (var == null)
                            throw new ArgumentNullException("var");

                        if (!boundVars.Contains(var.Name))
                        {
                            boundVars.Add(var.Name);
                            instance.Bindings.Add(new BindingPair(var, GetTerm(i, j, index)));                            
                        }
                        
                    }
                }
            }

            return instance;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="i">condition index:    this.LHS[i]</param>
        /// <param name="j">field index:        this.LHS[i].Fields[j]</param>
        /// <param name="index">match index:    which match in p_node</param>
        /// <returns></returns>
        private Term GetTerm(int i, int j, int index)
        {
            int k = this.LHS.Count;  

            Token p = this.P_Node.Items[index];

            for (int jj = 0; jj < (k - 1) - i; jj++)
                p = p.Parent;

            //foamliu. 2008/12/10.
            if (p.WME == null)
                return null;

            return p.WME.Fields[j];
        }

        #endregion
        
    }
}
