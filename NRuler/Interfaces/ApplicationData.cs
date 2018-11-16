using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace NRuler.Interfaces
{
    /// <summary>
    /// foamliu, 2009/04/28, 存储变量的名字和内容.
    /// </summary>
    public class ApplicationData
    {
        private Hashtable m_appData;

        public ApplicationData()
        {
            m_appData = new Hashtable();
        }

        public void Add(string varName, object value)
        {
            m_appData.Add(varName, value);
        }

        public object Get(string varName)
        {
            return m_appData[varName];
        }
    }
}
