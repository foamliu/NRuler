using System;

namespace NRuler.Common
{
    /// <summary>
    /// foamliu, 2009/04/30, 用于在命令行中显示进度.
    /// </summary>
    public class CommandLineProgressBar
    {
        // 用多少符号表示100%.
        public const int TOTALINCMDLINE = 100;
        // 用什么符号表示进度.
        public const string SYMBOL = ".";

        // 总进度
        private int m_total;
        // 上次的进度
        private int m_lastProgrss;

        public CommandLineProgressBar(int total)
        {
            m_total = total;
            m_lastProgrss = 0;

            // 画出比对进度条, 非控制台程序 Console.Out 是无法擦除重写的.
            Console.Write("[");
            for (int i = 0; i < TOTALINCMDLINE; i++)
            {
                Console.Write(" ");
            }
            Console.WriteLine("]");

            // 进度条开始
            Console.Write("[");
        }

        /// <summary>
        /// foamliu, 2009/04/30, 画进度条.
        /// </summary>
        /// <param name="progress">一定要单调增加</param>
        public void Draw(int progress)
        {
            int symbolNum = Convert.ToInt32(1.0 * progress * TOTALINCMDLINE / m_total);
            int symbolNumLast = Convert.ToInt32(1.0 * m_lastProgrss * TOTALINCMDLINE / m_total);

            for (int i = 0; i < (symbolNum - symbolNumLast); i++)
            {
                Console.Write(SYMBOL);
            }

            m_lastProgrss = progress;

            // 若结束画出结束标识
            if (progress >= m_total)
            {
                Console.WriteLine("]");
            }

        }


    }
}
