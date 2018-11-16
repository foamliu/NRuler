
namespace NRuler.Terms
{
    public interface IEvaluator
    {
        bool Evaluate(Term subject, Term obj);
    }

    public class Evaluator
    {
        public static new IEvaluator Equals = new Equals();
    }

    public class Equals : IEvaluator
    {
        public bool Evaluate(Term subject, Term obj)
        {
            return subject.Equals(obj);
        }

        public override string ToString()
        {
            return "=";
        }
    }

    public class NotEquals : IEvaluator
    {
        public bool Evaluate(Term subject, Term obj)
        {
            return !subject.Equals(obj);
        }

        public override string ToString()
        {
            return "!=";
        }
    }

    public class GreaterThan : IEvaluator
    {
        public bool Evaluate(Term subject, Term obj)
        {
            return subject.Equals(obj);
        }

        public override string ToString()
        {
            return ">";
        }
    }

    public class GreaterThanOrEquals : IEvaluator
    {
        public bool Evaluate(Term subject, Term obj)
        {
            return subject.Equals(obj);
        }

        public override string ToString()
        {
            return ">=";
        }
    }

    public class LessThan : IEvaluator
    {
        public bool Evaluate(Term subject, Term obj)
        {
            return subject.Equals(obj);
        }

        public override string ToString()
        {
            return "<";
        }
    }

    public class LessThanOrEquals : IEvaluator
    {
        public bool Evaluate(Term subject, Term obj)
        {
            return subject.Equals(obj);
        }

        public override string ToString()
        {
            return "<=";
        }
    }
}
