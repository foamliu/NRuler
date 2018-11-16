using System;

namespace NRuler.Terms
{
    [Serializable]
    public class Term
    {
        /// <summary>
        /// The Term Type
        /// </summary>
        protected TermType m_term_type;

        protected object m_value;

        public object Value
        {
            get { return this.m_value; }
            //set { this.m_value = value; }
        }

        /// <summary>
        /// Gets or sets the type of the term.
        /// </summary>
        /// <value>The type of the term.</value>
        public TermType TermType
        {
            get { return this.m_term_type; }
            set { this.m_term_type = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(Object obj)
        {
            Term other = obj as Term; 
            if (other != null) 
            { 
                return m_value.Equals(other.Value); 
            } 
            return base.Equals(obj); 
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            if (m_value == null) 
            { 
                return 0; 
            }
            return m_value.GetHashCode();
        }

        /// <summary>
        /// 
        /// </summary>
        public Term()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="val"></param>
        public Term(object val)
        {
            this.m_value = val;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="type"></param>
        public Term(object value, TermType type)
            : this(value)
        {
            this.m_term_type = type;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.Value.ToString();
        }

        #region Implicit Conversions

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static implicit operator Term(String value)
        {
            return new Term(value, TermType.String);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static implicit operator Term(char value)
        {
            return new Term(value, TermType.String);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static implicit operator Term(Boolean value)
        {
            return new Term(value, TermType.Boolean);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static implicit operator Term(Double value)
        {
            return new Term(value, TermType.Double);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static implicit operator Term(Single value)
        {
            return new Term(value, TermType.Double);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static implicit operator Term(Decimal value)
        {
            return new Term(value, TermType.Double);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static implicit operator Term(Int32 value)
        {
            return new Term(value, TermType.Integer);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static implicit operator Term(DateTime value)
        {
            return new Term(value, TermType.DateTime);
        }


        #endregion


    }
}
