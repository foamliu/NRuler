using System;
using System.Collections.Generic;
using System.Text;
using NRuler.Terms;

namespace NRuler.Rete
{
    /// <summary>
    /// foamliu, 2008/12/8, implement rules-with-variables and reasoner.
    /// 
    /// Or "match", for the specified query.
    /// </summary>
    [Serializable]
    public class Instance
    {
        #region Methods

        /// <summary>
        /// Variables in corresponding production occur once and only once.
        /// </summary>
        private readonly List<BindingPair> m_bindings;
        /// <summary>
        /// Corresponding production (query).
        /// </summary>
        private Production m_prod;

        #endregion

        #region Properties

        public List<BindingPair> Bindings
        {
            get
            {
                return this.m_bindings;
            }
        }

        #endregion

        #region ctors

        //public Production Production
        //{
        //    get
        //    {
        //        return this.m_prod;
        //    }
        //    set
        //    {
        //        this.m_prod = value;
        //    }
        //}

        public Instance(Production prod)
        {
            this.m_prod = prod;
            this.m_bindings = new List<BindingPair>();
        }

        #endregion

        #region Methods

        public Term GetVariableValue(string variableName)
        {
            foreach (BindingPair binding in this.m_bindings)
            {
                if (binding.Variable.Name.Equals(variableName, StringComparison.InvariantCultureIgnoreCase))
                {
                    return binding.Value;
                }
            }

            // foamliu, 2008/12/12, should never get here
            throw new ArgumentException("variableName");
        }

        #endregion
    }
}
