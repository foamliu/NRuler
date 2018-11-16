using System;
using System.Collections.Generic;

namespace NRuler.Rete
{
    /// <summary>
    /// specifies the locations of the two fields whose values must be 
    ///     equal in order for some variable to be bound consistently.
    /// </summary>
    public class Test_At_Join_Node
    {
        // one of the three fields in the WME (in the alpha memory)
        private FieldType m_field_of_arg1;
        // namely number-of-levels-up
        // foamliu, 2008/08/20, change:
        // from: m_condition_number_of_arg2
        //   to: m_number_of_levels_up
        private int m_number_of_levels_up;
        // a field from a WME that matches some earlier condition in the production 
        //  i.e., part of the token in the beta memory.
        private FieldType m_field_of_arg2;

        public FieldType Field_Of_Arg1
        {
            get { return this.m_field_of_arg1; }
            set { this.m_field_of_arg1 = value; }
        }

        public int Number_Of_Levels_Up
        {
            get { return this.m_number_of_levels_up; }
            set { this.m_number_of_levels_up = value; }
        }

        public FieldType Field_Of_Arg2
        {
            get { return this.m_field_of_arg2; }
            set { this.m_field_of_arg2 = value; }
        }

        public override bool Equals(Object obj)
        {
            //Check for null and compare run-time types.
            if (obj == null || GetType() != obj.GetType()) return false;
            Test_At_Join_Node p = (Test_At_Join_Node)obj;
            return p.Field_Of_Arg1 == this.Field_Of_Arg1
                && p.Field_Of_Arg2 == this.Field_Of_Arg2
                && p.Number_Of_Levels_Up == this.Number_Of_Levels_Up;
        }

        public override int GetHashCode()
        {
            return (int)(this.Field_Of_Arg1) * (int)(this.Field_Of_Arg2) * (int)(this.Number_Of_Levels_Up);
        }

        public static bool IsListEquals(List<Test_At_Join_Node> list1, List<Test_At_Join_Node> list2)
        {
            foreach (Test_At_Join_Node n in list1)
            {
                if (!list2.Contains(n))
                    return false;
            }
            foreach (Test_At_Join_Node n in list2)
            {
                if (!list1.Contains(n))
                    return false;
            }
            return true;
        }

    }
}
