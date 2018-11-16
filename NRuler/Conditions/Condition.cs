using System;
using System.Collections.Generic;
using System.Text;
using NRuler.Rete;
using NRuler.Terms;

namespace NRuler.Conditions
{
    [Serializable]
    public abstract class Condition
    {
        #region Fields
        protected string m_label;
        // variables are allowed in Conditions
        protected Term[] m_fields = new Term[ReteInferenceEngine.WME_FIELD_NUM];
        // the field is variable
        //protected bool[] m_isvar = new bool[Rete.WME_FIELD_NUM];

        protected ConditionType m_type;

        #endregion

        #region Properties

        public string Label
        {
            get { return this.m_label; }
            set { this.m_label = value; }
        }

        public Term[] Fields
        {
            get { return this.m_fields; }
            set { this.m_fields = value; }
        }

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>The id.</value>
        public Term Id
        {
            get 
            { 
                return m_fields[0]; 
            }
            set
            {                
                m_fields[0] = value;
            }
        }

        /// <summary>
        /// Gets or sets the attribute.
        /// </summary>
        /// <value>The attribute.</value>
        public Term Attribute
        {
            get 
            { 
                return m_fields[1]; 
            }
            set
            {
                m_fields[1] = value;
            }
        }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        public Term Value
        {
            get 
            { 
                return m_fields[2]; 
            }
            set
            {                
                m_fields[2] = value;
            }
        }

        public ConditionType Type
        {
            get { return this.m_type; }
            set { this.m_type = value; }
        }        

        #endregion

        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        //public Condition()
        //    : this("condition")

        //{
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="label">The label.</param>
        public Condition(string label)
        {
            this.m_label = label;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="label">The label.</param>
        /// <param name="id">The id.</param>
        /// <param name="attribute">The attribute.</param>
        /// <param name="value">The value.</param>
        public Condition(string label, Term id, Term attribute, Term value)
            : this(label)
        {
            this.Id = id;
            this.Attribute = attribute;
            this.Value = value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="label">The label.</param>
        /// <param name="conditionType">Type of the condition.</param>
        /// <param name="id">The id.</param>
        /// <param name="attribute">The attribute.</param>
        /// <param name="value">The value.</param>
        public Condition(string label, ConditionType conditionType, Term id, Term attribute, Term value)
            : this(label, id, attribute, value)
        {
            this.m_type = conditionType;
        }        

        #endregion

        #region Methods

        public override string ToString()
        {
            return string.Format("{0} {1} {2}", m_fields[0], m_fields[1], m_fields[2]);
        }

        public bool PassAllConstantTests(WME w)
        {
            for (int j = 0; j < ReteInferenceEngine.WME_FIELD_NUM; j++)
            {
                if (!(this.Fields[j] is Variable))
                {
                    if (!w.Fields[j].Equals(this.Fields[j]))
                        return false;
                }
            }
            return true;
        }

        #endregion
    }
}
