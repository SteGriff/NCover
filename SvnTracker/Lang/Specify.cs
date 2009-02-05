using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using MiscUtil;
using NUnit.Framework;

namespace SvnTracker.Lang
{
    public static class Specify
    {
        public static void AssertFalse(params Expression<Func<bool>>[] funcs)
        {
            Assert(false, funcs);
        }

        public static void AssertFalse<T>(this T subject, params Expression<Func<T, bool>>[] funcs)
        {
            Assert(subject, false, funcs);
        }

        public static void Assert(params Expression<Func<bool>>[] funcs)
        {
            Assert(true, funcs);
        }

        public static void Assert<T>(this T subject, params Expression<Func<T,bool>> [] funcs)
        {
            Assert(subject, true, funcs);
        }
        
        public static void Assert(bool success, params Expression<Func<bool>>[] funcs)
        {
            var bodies = new Expression[funcs.Length];
            for(int i = 0; i < funcs.Length; i++)
            {
                bodies[i] = funcs[i].Body;
            }
            Assert(success, success, new ParameterExpression[funcs.Length], bodies);
        }

        private static void Assert<T>(this T subject, bool success, params Expression<Func<T,bool>> [] funcs)
        {
            var bodies = new Expression[funcs.Length];
            var parameters = new ParameterExpression[funcs.Length];
            for(int i = 0; i < funcs.Length; i++)
            {
                bodies[i] = funcs[i].Body;
                parameters[i] = funcs[i].Parameters[0];
            }
            Assert(subject, success, parameters, bodies);
        }

        private static void Assert<T>(this T subject, bool success, ParameterExpression [] parameters, Expression [] expressions)
        {
            var sb = new StringBuilder();
            bool failed = false;
            for (int i = 0; i < expressions.Length; i++)
            {
                var expression = expressions[i];
                var exps = new List<Expression>();
                Expand(expression, exps);
                foreach (var exp in exps)
                {
                    var sbb = new StringBuilder();
                    if (Eval(parameters[i], exp, subject, sbb) != success)
                    {
                        failed = true;
                        sb.Append(sbb.ToString());
                    }
                }
            }
            string msg = sb.ToString();
            if (failed)
            {
                throw new AssertionException("\r\n" + subject + " fails these constraints:\r\n\r\n" + msg);
            }
        }

        private static void Expand(Expression body, IList<Expression> expantion)
        {
            if (body.NodeType != ExpressionType.AndAlso)
            {
                expantion.Add(body);
                return;
            }
            var binExp = (BinaryExpression)body;
            Expand(binExp.Left, expantion);
            Expand(binExp.Right, expantion);
        }

        private static bool Eval<T>(ParameterExpression parameterExpression, Expression body, T subject, StringBuilder sb)
        {
            var evaluator = new ExpressionEvaluator(subject, parameterExpression);
            Step reval;
            try
            {
                reval = evaluator.Visit(body);
                sb.AppendLine(reval.Key);
                return (bool) reval.Value;
            }
            catch (IndexOutOfRangeException e)
            {
                var result = new StringBuilder("BANG!\r\n" + e.Message);

                if (evaluator.Subject is ICollection)
                {
                    result.AppendLine("(" + evaluator.Subject + ".Count == " + ((ICollection)evaluator.Subject).Count + ")");
                }
                evaluator.Result(result.ToString());
                return false;
            }
            catch (TargetInvocationException e)
            {
                var result = new StringBuilder("BANG!\r\n" + e.InnerException.Message);
                    
                if (e.InnerException is ArgumentOutOfRangeException && subject is ICollection)
                {
                    result.AppendLine("(" + evaluator.Subject + ".Count == " + ((ICollection)evaluator.Subject).Count + ")");
                }
                evaluator.Result(result.ToString());
                return false;
            }
            catch (Exception e)
            {
                throw new Exception("While evaluating " + body + "\r\n" + e.Message, e);
            }
            finally
            {
                int i = 1;
                foreach (string step in evaluator.evaluationSteps)
                {
                    sb.AppendLine("\t" + i++ + ": " + step);
                }
                //if (reval != null && reval.Value != null)
                //{
                //    sb.AppendLine("\t" + i + ": " + reval.Value);
                //}
                sb.AppendLine();
            }
        }

        public class Step
        {
            public Step(string key, object value)
            {
                Key = key;
                Value = value;
            }

            public string Key { get; set; }
            public object Value { get; set; }
        }

        class ExpressionEvaluator
        {
            private readonly object m_Parameter;
            private readonly Expression m_ParamExp;
            public readonly List<string> evaluationSteps = new List<string>();

            public ExpressionEvaluator(object parameter, Expression paramExp)
            {
                m_Parameter = parameter;
                m_ParamExp = paramExp;
            }

            public Step Visit(Expression exp)
            {
                if (exp is TypeBinaryExpression)
                    return Visit((TypeBinaryExpression) exp);
                if (exp is UnaryExpression)
                    return Visit((UnaryExpression) exp);
                if (exp is NewArrayExpression)
                    return Visit((NewArrayExpression)exp);
                if (exp is ConstantExpression)
                    return Visit((ConstantExpression)exp);
                if (exp is MethodCallExpression)
                    return Visit((MethodCallExpression) exp);
                if (exp is MemberExpression)
                    return Visit((MemberExpression)exp);
                if (exp is BinaryExpression)
                    return Visit((BinaryExpression) exp);
                if (exp is ParameterExpression)
                    return Visit((ParameterExpression)exp);

                return new Step(exp.ToString(), exp.ToString());
            }
            
            Step Visit(NewArrayExpression exp)
            {
                Array array = Array.CreateInstance(exp.Type, exp.Expressions.Count);
                for (int i = 0; i < exp.Expressions.Count; i++)
                {
                    array.SetValue(Visit(exp.Expressions[i]), i);
                }
                return new Step(exp.ToString(), array);
            }
            
            Step Visit(TypeBinaryExpression exp)
            {
                Step step = Visit(exp.Expression);
                Subject = step.Value;
                switch (exp.NodeType)
                {
                    case ExpressionType.TypeIs:
                        Step(Display(Subject) + " is " + exp.TypeOperand);
                        return new Step(step.Key + " is " + exp.TypeOperand,
                                        Result(exp.TypeOperand.IsAssignableFrom(Subject.GetType())));
                }
                throw new NotSupportedException(exp.NodeType.ToString());
            }

            Step Visit(UnaryExpression exp)
            {
                Step step = Visit(exp.Operand);
                Subject = step.Value;
                switch (exp.NodeType)
                {
                    case ExpressionType.Not:
                        if (step.Value is bool)
                        {
                            return new Step("!" + step.Key, !(bool)step.Value);
                        }
                        return new Step("~" + step.Key, ~(long)step.Value);
                    case ExpressionType.Negate:
                    case ExpressionType.NegateChecked:
                        return new Step("-" + step.Key, -(double)step.Value);
                    case ExpressionType.UnaryPlus:
                        return new Step("+" + step.Key, step.Value);
                    case ExpressionType.Convert:
                        try
                        {
                            var type = exp.Type;
                            var value = step.Value;
                            object castedObject = Cast(value, type);
                            return new Step("((" + exp.Type.Name + ")" + step.Key + ")", castedObject);
                        } 
                        catch (TargetInvocationException e)
                        {
                            if (e.InnerException.Message.Contains("Specified cast is not valid"))
                            {
                                return new Step("((" + exp.Type.Name + ")" + step.Key + ")", Convert.ChangeType(step.Value, exp.Type));
                            }
                        }
                        break;
                    case ExpressionType.TypeAs:
                        Step(Display(Subject) + " as " + exp.Type);
                        return new Step(step.Key + " as " + exp.Type,
                                        Result(exp.Type.IsAssignableFrom(Subject.GetType()) ? Cast(Subject, exp.Type) : null));
                }
                throw new Exception("Unkonw  typeo" + exp.NodeType);
            }

            private object Cast(object value, Type type)
            {
                try
                {
                    MethodInfo castMethod = GetType().GetMethod("Cast").MakeGenericMethod(type);
                    return castMethod.Invoke(null, new[] {value});
                }
                catch (TargetInvocationException e)
                {
                    if (e.InnerException.Message.Contains("cast is not valid"))
                    {
                        return Convert.ChangeType(value, type);
                    }
                    throw;
                }
            }

            Step Visit(BinaryExpression binExp)
            {
                Step left = Visit(binExp.Left);
                Step right = Visit(binExp.Right);
                var subject = left.Value;
                var arg = right.Value;
                MethodInfo method = null;
                Subject = subject;
                switch (binExp.NodeType)
                {
                    case ExpressionType.Modulo:
                        Step(Display(Subject) + " % " + Display(arg));
                        return new Step(left.Key + " % " + right.Key, Result(((double)Cast(Subject, typeof(double)) % (double)(Cast(arg,typeof(double))))));
                    case ExpressionType.ExclusiveOr:
                        Step(Display(Subject) + " ^ " + Display(arg));
                        return new Step(left.Key + " ^ " + right.Key, Result(Evaluate(Subject, arg, binExp.NodeType)));
                    case ExpressionType.Power:
                        Step(Display(Subject) + " p^ " + Display(arg));
                        return new Step(left.Key + " p^ " + right.Key, Result(Evaluate(Subject, arg, binExp.NodeType)));
                    case ExpressionType.RightShift:
                        Step(Display(Subject) + " >> " + Display(arg));
                        return new Step(left.Key + " >> " + right.Key, Result(Evaluate(Subject, arg, binExp.NodeType)));
                    case ExpressionType.LeftShift:
                        Step(Display(Subject) + " << " + Display(arg));
                        return new Step(left.Key + " << " + right.Key, Result(Evaluate(Subject, arg, binExp.NodeType)));
                    case ExpressionType.Multiply:
                    case ExpressionType.MultiplyChecked:
                        Step(Display(Subject) + " * " + Display(arg));
                        return new Step(left.Key + " * " + right.Key, Result(Evaluate(Subject, arg, binExp.NodeType)));
                    case ExpressionType.Divide:
                        Step(Display(Subject) + " / " + Display(arg));
                        return new Step(left.Key + " / " + right.Key, Result(Evaluate(Subject, arg, binExp.NodeType)));
                    case ExpressionType.Add:
                    case ExpressionType.AddChecked:
                        Step(Display(Subject) + " + " + Display(arg));
                        return new Step(left.Key + " + " + right.Key, Result(Evaluate(Subject, arg, binExp.NodeType)));
                    case ExpressionType.Subtract:
                    case ExpressionType.SubtractChecked:
                        Step(Display(Subject) + " - " + Display(arg));
                        return new Step(left.Key + " - " + right.Key, Result(Evaluate(Subject, arg, binExp.NodeType)));
                    case ExpressionType.AndAlso:
                        Step(Display(Subject) + " && " + Display(arg));
                        return new Step(left.Key + " && " + right.Key, Result((bool) Subject && (bool)arg));
                    case ExpressionType.OrElse:
                        Step(Display(Subject) + " || " + Display(arg)); 
                        return new Step(left.Key + " || " + right.Key, Result((bool)Subject || (bool)arg)); 
                    case ExpressionType.Or:
                        Step(Display(Subject) + " | " + Display(arg));
                        return new Step(left.Key + " | " + right.Key, Result(Evaluate(Subject, arg, binExp.NodeType)));
                    case ExpressionType.And:
                        Step(Display(Subject) + " & " + Display(arg));
                        return new Step(left.Key + " & " + right.Key, Result(Evaluate(Subject, arg, binExp.NodeType)));
                    case ExpressionType.GreaterThan:
                        Step(Display(Subject) + " > " + arg);
                        return new Step(left.Key + " > " + right.Key, Result(Evaluate(Subject, arg, binExp.NodeType)));
                    case ExpressionType.GreaterThanOrEqual:
                        Step(Display(Subject) + " >= " + arg);
                        return new Step(left.Key + " >= " + right.Key, Result(Evaluate(Subject, arg, binExp.NodeType)));
                    case ExpressionType.LessThan:
                        Step(Display(Subject) + " < " + arg);
                        return new Step(left.Key + " < " + right.Key, Result(Evaluate(Subject, arg, binExp.NodeType)));
                    case ExpressionType.LessThanOrEqual:
                        Step(Display(Subject) + " <= " + arg);
                        return new Step(left.Key + " <= " + right.Key, Result(Evaluate(Subject, arg, binExp.NodeType)));
                    case ExpressionType.NotEqual:
                        Step(Display(Subject) + " != " + Display(arg));
                        return new Step(left.Key + " != " + right.Key, Result(Evaluate(Subject, arg, binExp.NodeType)));
                    case ExpressionType.Equal:
                        Step(Display(Subject) + " == " + Display(arg));
                        //Line up strings so it's obvious what the difference is
                        return new Step(left.Key + " == " + (left.Value is string && right.Value is string? "\r\n" : "") + right.Key, Result(Evaluate(Subject, arg, binExp.NodeType)));
                    case ExpressionType.ArrayIndex:
                        Step(Display(Subject) + "[" + Display(arg) + "]");
                        if (arg is int)     new Step(left.Key + "[" + right.Key + "]", Result(((Array)Subject).GetValue((int)arg)));
                        if (arg is long)    new Step(left.Key + "[" + right.Key + "]", Result(((Array)Subject).GetValue((long)arg)));
                        if (arg is int[])   new Step(left.Key + "[" + right.Key + "]", Result(((Array)Subject).GetValue((int[])arg)));
                        if (arg is long[])  new Step(left.Key + "[" + right.Key + "]", Result(((Array)Subject).GetValue((long[])arg)));                            
                        break;
                    default:                    
                        method = binExp.Method;
                        break;
                }

                if (method == null)
                {
                    throw new Exception("Don't know how to handle a " + binExp.NodeType);
                }

                if (method.IsStatic)
                {
                    Step(method.Name + "(" + arg + ")");                
                    return new Step(method.Name + "(" + right.Key + ")", Result(method.Invoke(null, new [] { Subject, arg})));
                }
                Step(Display(Subject) + "." + method.Name + "(" + arg + ")");
                return new Step(left.Key + "." + method.Name + "(" + right.Key + ")", Result(method.Invoke(Subject, new[] { arg })));
            }

            private static object Evaluate(object subject, object arg, ExpressionType subtract)
            {
                string methodName = subtract.ToString().Replace("Checked","");
                switch (subtract)
                {
                    case ExpressionType.ExclusiveOr: 
                        methodName = "Xor"; 
                        break;
                }
                return typeof(Operator).GetMethod(methodName).MakeGenericMethod(subject.GetType()).Invoke(null, new[] { subject, arg });
            }

            Step Visit(MethodCallExpression exp)
            {
                var step = Visit(exp.Object);
                Subject = step.Value;
               
                if (Subject == null && !exp.Method.IsStatic)
                {
                    Step("" + Display(Subject) + "." + exp.Method.Name + "(...)");
                    throw new TargetInvocationException(new Exception(Display(Subject) + " is null."));
                }

                var argValues = new List<object>();
                var argSteps = new List<Step>();
                foreach (var arg in exp.Arguments)
                {
                    var stepArg = Visit(arg);
                    argValues.Add(stepArg.Value);
                    argSteps.Add(stepArg);
                }
                Subject = step.Value;

                //Indexer
                if (exp.Method.Name.StartsWith("get_"))
                {
                    Step(Display(Subject) + "[" + ToDisplayString(argValues) + "]");
                    return new Step(step.Key + "[" + ToDisplayString(argSteps) + "]", Result(exp.Method.Invoke(Subject, argValues.ToArray())));                    
                }
                Step(Display(Subject) + "." + exp.Method.Name + "(" + ToDisplayString(argValues) + ")");
                return new Step(step.Key + "." + exp.Method.Name + "(" + ToDisplayString(argSteps) + ")", Result(exp.Method.Invoke(Subject, argValues.ToArray())));
            }

            Step Visit(MemberExpression exp)
            {
                var step = Visit(exp.Expression);
                Subject = step.Value;
                if (exp.Member is FieldInfo)
                {
                    var field = (FieldInfo) exp.Member;
                    Step("" + Display(Subject) + "." + exp.Member.Name);
                    return new Step(step.Key + "." + exp.Member.Name, Result(field.GetValue(Subject)));
                }

                var prop = (PropertyInfo) exp.Member;
                Step("" + Display(Subject) + "." + exp.Member.Name);
                return new Step(step.Key + "." + exp.Member.Name, Result(prop.GetGetMethod().Invoke(Subject, new object[] { })));
            }

            public string Display(object o)
            {
                if (o == m_Parameter) return m_ParamExp.ToString();
                if (o == null) return "null";

                //Anonymous type
                if (o.GetType().Name.StartsWith("<"))
                {
                    return "_";
                }
                if (o is String)
                {
                    return "\"" + o + '"';
                }
                if (o is char)
                {
                    return "'" + o + "'";
                }

                //Is toString defined on this type?
                if (!HasToStringDefined(o))
                {
                    string name = o.GetType().Name;
                    return (Char.ToLower(name[0])) + name.Substring(1);
                }

                return o.ToString();
            }

            private static bool HasToStringDefined(object o)
            {
                return (o is string || o.ToString().Length < 20);
                //return o.GetType().GetMethod("ToString", BindingFlags.DeclaredOnly) != null;
            }

            private Step Visit(ConstantExpression c)
            {
                return new Step(Display(c.Value),c.Value);
            }

#pragma warning disable 168
            Step Visit(ParameterExpression p)
#pragma warning restore 168
            {
                return new Step(Display(m_Parameter), m_Parameter);
            }

            private object m_tryStep;
            public object Subject { get; set; }
            private void Step(string from)
            {
                m_tryStep = from;
            }

            public object Result(object to)
            {
                evaluationSteps.Add(m_tryStep + " ==> " + Display(to));
                return to;
            }

            private string ToDisplayString(IEnumerable arguments)
            {
                var sb = new StringBuilder();
                foreach (object arg in arguments)
                {
                    if (sb.Length != 0)
                    {
                        sb.Append(", ");
                    }
                    sb.Append(Display(arg));
                }
                return sb.ToString();
            }
            private static string ToDisplayString(IEnumerable<Step> arguments)
            {
                var sb = new StringBuilder();
                foreach (Step arg in arguments)
                {
                    if (sb.Length != 0)
                    {
                        sb.Append(", ");
                    }
                    sb.Append(arg.Key);
                }
                return sb.ToString();
            }

            // ReSharper disable UnusedMemberInPrivateClass
            public static T Cast<T>(object o)
            // ReSharper restore UnusedMemberInPrivateClass
            {
                return (T)o;
            }
        }
    }

    [TestFixture]
    public class SpecExtentionsSpec
    {
#pragma warning disable 184
        // ReSharper disable RedundantToStringCall

        [Test]
        public void TestFailures()
        {
            const string str = "Fred";            
            str.AssertFalse(
                s=>(s as object) == null,
                s=>s is int,
                s=>s.ToString() == "Frd",
                s=>s[3] == 'e',
                s=>s.Length > 5,
                s => "Something really close and long" 
                  == "Something reallly close and long"
                );
        }

        class X : IComparable<X>
        {
            public string Name { get; set; }
            public int CompareTo(X other)
            {
                return other.Name.CompareTo(Name);
            }
        }

        [Test]
        public void TestSuccess()
        {
            var x = new X {Name = "Fred"};
            var other = new X {Name = "Fred"};
            x.Assert(z=>z.CompareTo(other) == 0);


            /*
             * could we do this?
             * var query = from word in words
            where word.Length > 4
            select word.ToUpper(); 

             * **/


            const string str = "Fred";
            str.Assert(
                s => (s as object) == "Fred",
                s => s is string,
                s => s.ToString() == "Fred",
                s => !false,
                s => s.Length - 2 == 2,
                s => s[3] == 'd',
                s => s.Length > 2
                );
        }

        [Test]
        public void TestSuccessAndStyle() {
            const string str = "Fred";
                str.Assert(s => (s as object) == "Fred"
                && s is string
                && s.ToString() == "Fred"
                && !false
                && s.Length - 2 == 2
                && s[3] == 'd'
                && s.Length > 2
                );        
        }

        [Test]
        public void OperatorTests()
        {
            Console.WriteLine(" " + (1 / 2));
            1.Assert(n => n + 1 == 2
                && n - 1 == 0
                && n % 2 == 1
                && (n ^ 1) == 0
                && n * 2 == 2
                && n / 2 == 0
                && ((double)n) / 2 == 0.5
                && n != 2
                );
        }
        

        // ReSharper restore RedundantToStringCall
#pragma warning restore 184

    }
}
