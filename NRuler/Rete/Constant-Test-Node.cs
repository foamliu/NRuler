using System;
using System.Collections.Generic;
using System.Text;
using NRuler.Terms;

namespace NRuler.Rete
{
    public class Constant_Test_Node
    {
        private FieldType m_field_to_test;
        private Term m_thing_the_field_must_equal;
        private Alpha_Memory m_output_memory;
        private readonly List<Constant_Test_Node> m_children;

        public FieldType Field_To_Test
        {
            get { return this.m_field_to_test; }
            set { this.m_field_to_test = value; }
        }

        public Term Thing_The_Field_Must_Equal
        {
            get { return this.m_thing_the_field_must_equal; }
            set { this.m_thing_the_field_must_equal = value; }
        }

        public Alpha_Memory Output_Memory
        {
            get { return this.m_output_memory; }
            set { this.m_output_memory = value; }
        }

        public List<Constant_Test_Node> Children
        {
            get { return this.m_children; }            
        }

        public Constant_Test_Node()
        {
            this.m_children = new List<Constant_Test_Node>();
        }

        //public override string ToString()
        //{
        //    if (this.Field_To_Test == FieldType.No_Test)
        //        return "";

        //    StringBuilder builder = new StringBuilder();
        //    builder.AppendFormat("{0}={1} ", this.Field_To_Test, this.Thing_The_Field_Must_Equal);
        //    //if (this.Children.Count != 0)
        //    //{                
        //    //    foreach (Constant_Test_Node child in this.Children)
        //    //    {
        //    //        builder.AppendFormat("({0})", child);
        //    //    }                
        //    //}
        //    return builder.ToString();
        //}

    }
}
