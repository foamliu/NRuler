using System;
using System.Collections.Generic;
using System.Text;
using NRuler.Terms;

namespace NRuler.Rete
{
    /// <summary>
    /// A Variable and its bound value
    /// </summary>
    public class BindingPair
    {
        #region Fields

        private readonly Variable m_variable;
        private readonly Term m_substitutor;

        #endregion

        #region Fields

        public BindingPair(Variable variable, Term term)
        {
            this.m_variable = variable;
            this.m_substitutor = term;
        }

        #endregion

        #region Properties

        public Variable Variable
        {
            get { return this.m_variable; }
            //set { this.m_variable = value; }
        }

        public Term Value
        {
            get { return this.m_substitutor; }
            //set { this.m_substitutor = value; }
        }

        //public override String ToString()
        //{
        //    return string.Format("({0} = {1})", this.m_variable, this.m_substitutor);
        //}

        #endregion
    }

}
