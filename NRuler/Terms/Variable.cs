using System;

namespace NRuler.Terms
{
    public class Variable : Term
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the Variable class.
        /// </summary>
        /// <param name="label">The label.</param>
        public Variable(String label)
            : base(label)
        {
            this.m_term_type = TermType.Variable;
            //this.m_value = label;
        }

        #endregion

        public string Name
        {
            get
            {
                return this.m_value.ToString();
            }
        }
    }

    
}
