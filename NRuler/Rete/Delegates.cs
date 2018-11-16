using System;
using System.Collections.Generic;
using System.Text;

namespace NRuler.Rete
{
    public delegate void ActivationEventHandler(object sender, ActivationEventArgs e);

    public class ActivationEventArgs
    {
        private readonly Activation m_activation;
        private readonly bool m_activated;

        public Activation Activation
        {
            get { return this.m_activation; }            
        }

        public bool Activated
        {
            get { return this.m_activated; }
        } 

        /// <summary>
        /// 
        /// </summary>
        /// <param name="activation">The activation.</param>
        /// <param name="activated">True: activataed; false:deactivated.</param>
        public ActivationEventArgs(Activation activation, bool activated)
        {
            this.m_activation = activation;
            this.m_activated = activated;
        }

    }
}
