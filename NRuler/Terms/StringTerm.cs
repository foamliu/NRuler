using System;

namespace NRuler.Terms
{
    /// <summary>
    /// A term for strings.
    /// </summary>
    public class StringTerm : Term
    {

        public StringTerm(String s)
            : base(s)
        {
            this.m_term_type = TermType.String;
        }        

        public new string Value 
        { 
            get { return (string)m_value; } 
            set { m_value = value; } 
        } 
    }
}
