using System.Collections.Generic;
using NRuler.Common;
using NRuler.Conflicts;

namespace NRuler.Rete
{
    public class Agenda
    {
        #region Fields
        private string m_name;
        private ReteInferenceEngine m_rete;
        private readonly List<WME> m_initialFacts;
        private readonly List<Production> m_productions;
        private readonly List<WME> m_inferredFacts;
        
        // foamliu, 2008/11/23, for supporting refraction.
        private readonly List<Production> m_activatedProductions;

        // foamliu, 2008/11/24, if a halt action is performed then done.
        private bool m_halt/* = false*/;

        // conflict resolution strategy
        private IComparer<Production> m_conflictResolutionStrategy;
        private readonly PriorityQueue<Production> m_conflictSet;
        
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name
        {
            get { return m_name; }
            set { m_name = value; }
        }

        /// <summary>
        /// Gets or sets the conflict resolution strategy.
        /// </summary>
        public IComparer<Production> ConflictResolutionStrategy
        {
            get { return this.m_conflictResolutionStrategy; }
            set { this.m_conflictResolutionStrategy = value; }
        }

        public List<WME> InitialFacts
        {
            get { return this.m_initialFacts; }
        }

        public List<WME> InferredFacts
        {
            get { return this.m_inferredFacts; }            
        }

        public int ActivatedRuleCount
        {
            get { return this.m_activatedProductions.Count; }
        }

        public ReteInferenceEngine Engine
        {
            get { return m_rete; }
        }

        //public PriorityQueue<Production> ConflictSet
        //{
        //    get { return m_conflictSet; }
        //}

        #endregion

        #region Constructors
        public Agenda()
        {
            this.m_rete = new ReteInferenceEngine();
            
            this.m_initialFacts = new List<WME>();
            this.m_inferredFacts = new List<WME>();
            this.m_productions = new List<Production>();
            this.m_conflictResolutionStrategy = new BasicResolver();
            this.m_conflictSet = new PriorityQueue<Production>(this.m_conflictResolutionStrategy);
            this.m_activatedProductions = new List<Production>();             
            
        }
        #endregion

        #region Methods

        public void AddProduction(Production prod)
        {
            prod.Agenda = this;
            prod.OnActivation += new ActivationEventHandler(ActivationEventHandler);
            this.m_productions.Add(prod);            
        }

        public void AddProductions(IList<Production> list)
        {
            foreach (Production prod in list)
            {
                this.m_productions.Add(prod);
            }
        }

        public void AddFact(WME fact)
        {
            this.m_initialFacts.Add(fact);
        }

        public void AddFacts(IList<WME> list)
        {
            this.m_initialFacts.AddRange(list);
        }

        /// <summary>
        /// foamliu, 2008/11/22, as CLIPS's corresponding command is "run".
        /// 
        /// Refraction: means rules won't fire more than once for a specific set of facts.
        /// Without refraction, expert systems would always be caught in trivial loops.
        /// That is, as soon as a rule fired, it would keep on firing on that same fact over and over again.
        /// In the real world the stimulus that causes the fireing would eventually disappear.
        /// However, in the computer world, once a fact is entered in the fact list, it stays there until explicitly removed.
        /// 
        /// If necessary, the rule can be made to fire again by retracting the fact and asserting it again.
        /// </summary>
        public void Run()
        {
            this.m_halt = false;

            Logger.Info("Start building Beta Network...");
            CommandLineProgressBar ruleProgressBar = new CommandLineProgressBar(m_productions.Count);
            // 已经加入RETE网络的规则数
            int prodNum = 0;            

            foreach (Production prod in m_productions)
            {
                this.m_rete.AddProduction(prod);

                // 显示进度
                prodNum++;
                ruleProgressBar.Draw(prodNum);
            }
            Logger.Info("Building Beta Network completed.");

            Logger.Info("Start adding facts to RETE Network...");
            CommandLineProgressBar factProgressBar = new CommandLineProgressBar(m_initialFacts.Count);
            // 已经加入RETE网络的事实数
            int wmeNum = 0;
            
            foreach (WME wme in m_initialFacts)
            {
                this.m_rete.AddWme(wme);
                // 显示进度
                wmeNum++;
                factProgressBar.Draw(wmeNum);
            }
            Logger.Info("Adding facts to RETE Network completed.");
              
            Production cur;
            while ((cur = this.m_conflictSet.Remove()) != null && !this.m_halt)
            {
                Activate(cur);
            }

        }

        /// <summary>
        /// foamliu, 2008/11/22, as CLIPS's corresponding command is "refresh".
        /// 
        /// Refresh method can be used to make the rules fire again. It places all activations that 
        /// have been already fired for a rule back on the agenda.
        /// </summary>
        public void Refresh(/*string prodName*/)
        {
            this.m_activatedProductions.Clear();
        }

        /// <summary>
        /// Execution of the Right-Hand-Side of Rules
        /// </summary>
        /// <param name="prod"></param>
        private void Activate(Production prod)
        {
            //foreach (Condition cond in prod.RHS)
            //{
            //    switch (cond.Type)
            //    {
            //        case ConditionType.Assert:
            //            Assert(cond);
            //            break;
            //        //case ConditionType.Retract:
            //        //    Retract(cond);
            //        //    break;
            //        default:
            //            Assert(cond);
            //            break;
            //    }
            //}

            if (!prod.Activated)
                return;

            foreach (WME wme in prod.InferredFacts)
            {
                Assert(wme);
            }

            this.m_activatedProductions.Add(prod);

        }        

        private void Assert(WME wme)
        {
            this.m_inferredFacts.Add(wme);
            this.m_rete.AddWme(wme);
        }

        // foamliu, 2008/11/25, the implementation is incorrent.
        // Because the wme is not the wme in rete, it hasn't token links,
        //  use it as param does not work correctly.

        private void Retract(WME wme)
        {
            this.m_inferredFacts.Remove(wme);
            this.m_rete.RemoveWme(wme);
        }

        public void ActivationEventHandler(object sender, ActivationEventArgs e)
        {
            if (e.Activated)
            {
                if (this.m_activatedProductions.Contains(e.Activation.Production))
                    return;

                this.m_conflictSet.Insert(e.Activation.Production);
            }
            //else
            //{
            //    this.m_conflictSet.Remove(e.Activation.Production);                    
            //}
        }

        // foamliu, 2008/11/24, if a halt action is performed then done.

        public void Halt()
        {
            this.m_halt = true;
        }

        //public static void Log(string msg)
        //{
        //    string line = DateTime.Now + "\t" + msg;
        //    TextWriter writer = new StreamWriter(ReteInferenceEngine.LogFileName, true);
        //    writer.WriteLine(line);
        //    writer.Close();

        //    System.Console.WriteLine(line);
        //}

        #endregion
    }  

    
}
