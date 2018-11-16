using System;
using System.Collections.Generic;
using System.Text;

namespace NRuler.Rete
{
    public class Dummy_Top_Node : Beta_Memory
    {
        public Dummy_Top_Node():base()
        {
            m_items.Add(new Dummy_Top_Token());
        }
    }
}
