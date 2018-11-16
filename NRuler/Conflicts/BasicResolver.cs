using System;
using System.Collections.Generic;
using System.Text;
using NRuler.Rete;

namespace NRuler.Conflicts
{
    // foamliu, 2008/11/21.
    //  1.Priority (0 is highest).
    //  2.Timestamp (First In First Served, earlier is higher).

    // In Agenda, we pick Maximum.
    public class BasicResolver : IComparer<Production>
    {
        public int Compare(Production prod1, Production prod2)
        {
            return Math.Sign(prod1.Priority - prod2.Priority);
        }

    }
}
