using System;
using System.Collections.Generic;
using NRuler.Terms;

namespace NRuler.Rete
{
    /// <summary>
    /// Working Memory Entity
    /// </summary>
    [Serializable]
    public class WME
    {
        #region Fields            

        // foamliu, 2008/11/21, name.
        private string m_name;

        // no variables are allowed in WMEs
        private readonly Term[] m_fields = new Term[ReteInferenceEngine.WME_FIELD_NUM];
        // the ones containing this WME
        [NonSerialized]
        //private List<Alpha_Memory> m_alpha_mems;
        // foamliu, 2008/12/3, optimization.
        private List<Alpha_Memory> m_alpha_mems;

        // the ones with wme=this WME
        [NonSerialized]
        // foamliu, 2008/12/3, optimization.
        private List<Token> m_tokens = new List<Token>();
        // foamliu, 2008/08/25, for supporting negated conditions
        [NonSerialized]
        // foamliu, 2008/12/3, optimization.
        private List<Negative_Join_Result> m_negative_join_results = new List<Negative_Join_Result>();

        #endregion

        #region Properties

        public string Name
        {
            get { return this.m_name; }
            set { this.m_name = value; }
        }

        public Term[] Fields
        {
            get { return this.m_fields; }            
        }

        public List<Alpha_Memory> Alpha_Mems
        {
            get 
            {
                if (null == this.m_alpha_mems)
                    this.m_alpha_mems = new List<Alpha_Memory>();
                return this.m_alpha_mems; 
            }
            
        }

        public List<Token> Tokens
        {
            get 
            {
                if (null == this.m_tokens)
                    this.m_tokens = new List<Token>();
                return this.m_tokens; 
            }
            
        }

        public List<Negative_Join_Result> Negative_Join_Results
        {
            get 
            {
                if (null == this.m_negative_join_results)
                    this.m_negative_join_results = new List<Negative_Join_Result>();
                return this.m_negative_join_results; 
            }
            
        }

        #endregion

        #region Constructors

        public WME(string label)
        {
            this.m_name = label;
        }

        public WME(string label, Term v1, Term v2, Term v3)
            : this(label)
        {
            this.m_fields[(int)FieldType.Identifier] = v1;
            this.m_fields[(int)FieldType.Attribute] = v2;
            this.m_fields[(int)FieldType.Value] = v3;
        }

        public WME(Term v1, Term v2, Term v3)
            : this("WME", v1, v2, v3)
        {
        }

        /// <summary>
        /// Initializes a new instance of the WME class.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="attr"></param>
        /// <param name="value"></param>
        public WME(string id, string attr, string value)
        {
            this.m_fields[(int)FieldType.Identifier] = new Term(id);
            this.m_fields[(int)FieldType.Attribute] = new Term(attr);
            this.m_fields[(int)FieldType.Value] = new Term(value);           
        }
        
        public WME(): this(null, null, null)
        {
        }

        #endregion

        #region Methods

        public override string ToString()
        {
            return string.Format("{0} {1} {2}", m_fields[0], m_fields[1], m_fields[2]);
        }

        public override bool Equals(object obj)
        {
            //Check for null and compare run-time types.
            if (obj == null || GetType() != obj.GetType()) return false;
            
            WME p = (WME)obj;

            for (int j = 0; j < ReteInferenceEngine.WME_FIELD_NUM; j++)
            {
                if (!this.Fields[j].Equals(p.Fields[j]))
                    return false;
            }

            return true;
        }

        public override int GetHashCode()
        {
            return Fields[0].GetHashCode() + Fields[1].GetHashCode() + Fields[2].GetHashCode();
        }

        #endregion
    }
   
}
