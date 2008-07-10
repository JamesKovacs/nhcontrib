using System;
using System.Linq.Expressions;

namespace NHibernate.Linq.Visitors
{
    /// <summary>
    /// Preprocesses an expression tree replacing binary boolean expressions with unary expressions.
    /// </summary>
    public class BinaryBooleanReducer : ExpressionVisitor
    {
        //this class simplifies this:
        //   timesheet.Entries.Any() == true
        //to this:
        //   timesheet.Entries.Any()

        private Expression ProcessBinaryExpression(Expression exprToCompare, Expression exprToReturn, ExpressionType nodeType)
        {
            BooleanConstantFinder visitor = new BooleanConstantFinder();
            visitor.Visit(exprToCompare);

            if (visitor.Constant.HasValue)
            {
                bool value = (nodeType == ExpressionType.Equal) ? visitor.Constant.Value : !visitor.Constant.Value;

                if (value)
                {
                    return exprToReturn;
                }
                else
                {
                    if (exprToReturn.NodeType == ExpressionType.Not)
                        return ((UnaryExpression)exprToReturn).Operand;

                    return Expression.Not(exprToReturn);
                }
            }

            return null;
        }

        protected override Expression VisitBinary(BinaryExpression expr)
        {
            Expression e = ProcessBinaryExpression(expr.Left, expr.Right, expr.NodeType);
            if (e != null) return e;

            e = ProcessBinaryExpression(expr.Right, expr.Left, expr.NodeType);
            if (e != null) return e;

            return base.VisitBinary(expr);
        }

        class BooleanConstantFinder : ExpressionVisitor
        {
            private bool _isNestedBinaryExpression;

            public bool? Constant { get; private set; }

            protected override Expression VisitConstant(ConstantExpression c)
            {
                if (c.Type == typeof(bool) && !_isNestedBinaryExpression)
                    Constant = (bool)c.Value;
                return c;
            }

            protected override Expression VisitBinary(BinaryExpression b)
            {
                _isNestedBinaryExpression = true;
                return b;
            }
        }
    }
}
