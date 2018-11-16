using System;
using System.Collections.Generic;
using System.Text;

namespace NRuler.Conditions
{
    public enum ConditionType
    {
        /// <summary>
        /// Positive
        /// </summary>
        Positive,
        /// <summary>
        /// Negative
        /// </summary>
        Negative,
        /// <summary>
        /// NCC
        /// </summary>
        NCC,
        /// <summary>
        /// Assert
        /// </summary>
        Assert,
        /// <summary>
        /// Retract
        /// </summary>
        Retract,
    }
}
