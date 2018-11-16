using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using NRuler.Rete;
using NRuler.Conditions;
using NRuler.Terms;

namespace NRuler
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WME[] wmes = new WME[] {
                new WME("B1", "^on", "B2"),     // w1
                new WME("B1", "^on", "B3"),     // w2
                new WME("B1", "^color", "red"), // w3
                new WME("B2", "^on", "table"),  // w4
                new WME("B2", "^left-of", "B3"),// w5
                new WME("B2", "^color", "blue"),// w6
                new WME("B3", "^left-of", "B4"),// w7
                new WME("B3", "^on", "table"),  // w8
                new WME("B3", "^color", "red"), // w9
            };

            Production prod = new Production();
            prod.Name = "find-stack-of-two-blocks-to-the-left-of-a-red-block";
            Variable x = new Variable("x");
            Variable y = new Variable("y");
            Variable z = new Variable("z");
            Variable w = new Variable("w");

            prod.LHS.Add(new PositiveCondition("C1", x, "^on", y));
            prod.LHS.Add(new PositiveCondition("C2", y, "^left-of", z));

            NCCCondition neg = new NCCCondition();
            neg.SubConditions.Add(new PositiveCondition("C3", z, "^color", "red"));
            neg.SubConditions.Add(new PositiveCondition("C4", z, "^on", w));
            prod.LHS.Add(neg);

            ReteInferenceEngine rete = new ReteInferenceEngine();
            rete.AddProduction(prod);

            foreach (WME wme in wmes)
            {
                rete.AddWme(wme);
            }

            System.Console.WriteLine("Completed.");

        }


    }
}
