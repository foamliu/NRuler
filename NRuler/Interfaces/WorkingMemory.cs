using System;
using System.Collections;
using System.Reflection;
using NRuler.Conflicts;
using NRuler.Rete;

namespace NRuler.Interfaces
{
    /// <summary>
    /// foamliu, 2009/04/27, 在 RETE 之上进一步封装.
    /// </summary>
    public class WorkingMemory
    {
        #region Fields

        private Hashtable m_facts;
        private Agenda m_agenda;
        private RuleSet m_ruleSet;

        #endregion

        #region Properties

        public Agenda Agenda
        {
            get { return m_agenda; }
            set { m_agenda = value; }
        }

        public RuleSet RuleExecutionSet
        {
            get { return m_ruleSet; }
            set { m_ruleSet = value; }
        }

        #endregion

        #region Methods

        public WorkingMemory()
        {
            m_facts = new Hashtable();

            m_agenda = new Agenda();
            m_agenda.Name = "theAgenda";
            m_agenda.ConflictResolutionStrategy = new BasicResolver();
        }                          
        

        /// <summary>
        /// foamliu, 2009/04/24, anonymous fact.
        /// </summary>
        /// <param name="obj"></param>
        //public void AssertObject(object obj)
        //{
            
        //}

        /// <summary>
        /// foamliu, 2009/04/24, named fact.
        /// </summary>
        /// <param name="obj"></param>
        public void AssertObject(object obj, string name)
        {
            m_facts[name] = obj;

            Type type = obj.GetType();
            PropertyInfo[] props = type.GetProperties();
            foreach (PropertyInfo prop in props)
            {
                object value = prop.GetValue(obj, null);

                m_agenda.AddFact(new WME(name, "^"+prop.Name, value.ToString()));
            }
        }

        public void RetractObject(string name)
        {
            // TODO: 在agenda里面去掉.
            m_facts.Remove(name);
        }


        public void ExecuteRules()
        {
            m_agenda.Run();
        }

        #endregion
    }
}
