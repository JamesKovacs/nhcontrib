using System.Collections.Generic;
using NHibernate.Burrow.Util.Hql.Gold;
using NHibernate.Burrow.Util.Hql.Gold.AST;
using NUnit.Framework;

namespace NHibernate.Burrow.Util.Test.Hql
{
    [TestFixture]
    public class AstBuilderFixture
    {
        [Test]
        public void BuildSimpleTree()
        {
            Node root = AstBuilder.GetTreeRoot("select Name from Table");

            Assert.IsInstanceOfType(typeof (SelectStatement), root);
            Assert.AreEqual(2, root.ChildNodes.Count);

            Assert.IsInstanceOfType(typeof (SelectClause), root.ChildNodes[0]);
            Assert.IsInstanceOfType(typeof (FromClause), root.ChildNodes[1]);

            // Test the "Select" info
            Assert.AreEqual(1, root.ChildNodes[0].ChildNodes.Count);
            Assert.IsInstanceOfType(typeof (Identifier), root.ChildNodes[0].ChildNodes[0]);
            Assert.AreEqual("Name", ((Identifier) root.ChildNodes[0].ChildNodes[0]).Name);
        }

        [Test]
        public void FlatternProperties()
        {
            Node root = AstBuilder.GetTreeRoot("select Table.Child.FirstName from Table");

            Assert.AreEqual(1, root.Get<SelectClause>().ChildNodes.Count);
            Assert.AreEqual("Table.Child.FirstName", root.Get<SelectClause>().Get<Property>().Name);
        }

        [Test]
        public void FlatternPropertyList()
        {
            Node root = AstBuilder.GetTreeRoot("select Table.Name, LastName, Age, Gender from Table");
            SelectClause selectClause = root.Get<SelectClause>();

            IList<Node> columnList = selectClause.ChildNodes;

            // Should return 4 terminal sections
            Assert.AreEqual(4, columnList.Count);
            Assert.IsInstanceOfType(typeof (Property), columnList[0]);
            Assert.IsInstanceOfType(typeof (Identifier), columnList[1]);
            Assert.IsInstanceOfType(typeof (Identifier), columnList[2]);
            Assert.IsInstanceOfType(typeof (Identifier), columnList[3]);

            Assert.AreEqual("Table.Name", ((Property) columnList[0]).Name);
            Assert.AreEqual("LastName", ((Identifier) columnList[1]).Name);
            Assert.AreEqual("Age", ((Identifier) columnList[2]).Name);
            Assert.AreEqual("Gender", ((Identifier) columnList[3]).Name);
        }

        [Test]
        public void FromEntitiesWithAliases()
        {
            FromElement elem;

            elem = AstBuilder.GetTreeRoot("from Person p").Get<FromClause>().Get<FromElement>();
            Assert.AreEqual("Person", elem.ClassName);
            Assert.AreEqual("p", elem.Alias);

            elem = AstBuilder.GetTreeRoot("from Person as p").Get<FromClause>().Get<FromElement>();
            Assert.AreEqual("Person", elem.ClassName);
            Assert.AreEqual("p", elem.Alias);

            elem = AstBuilder.GetTreeRoot("from p in class Person").Get<FromClause>().Get<FromElement>();
            Assert.AreEqual("Person", elem.ClassName);
            Assert.AreEqual("p", elem.Alias);

            elem = AstBuilder.GetTreeRoot("from Person").Get<FromClause>().Get<FromElement>();
            Assert.AreEqual("Person", elem.ClassName);
            Assert.IsNull(elem.Alias);
        }

        [Test]
        public void GetFirstChildByType()
        {
            Node root = AstBuilder.GetTreeRoot("select Name from Table");

            Assert.IsNotNull(root.Get<SelectClause>());
            Assert.IsNotNull(root.Get<FromClause>());
        }

        [Test]
        public void InOperator()
        {
            Node root;
            Util.Hql.Gold.AST.Tuple tuple;

            AstBuilder.GetTreeRoot("from Entity where Entity.Child.Age in (1)");
            AstBuilder.GetTreeRoot("from Entity where Entity.Child.Age in ('Diego')");
            AstBuilder.GetTreeRoot("from Entity where Entity.Child.Age not in (1, 2, 3, 4)");

            root = AstBuilder.GetTreeRoot("from Entity where Entity.Child.Age in (1, 2, 3, 4)");
            tuple = (Util.Hql.Gold.AST.Tuple) root.Get<WhereClause>().Get<BinaryArithmeticOperator>().RightHandOperand;
            Assert.AreEqual(TupleFormat.CommaSeparated, tuple.Format);

            AstBuilder.GetTreeRoot("from Entity where Entity.Child.Age in (select Age from OtherEntity)");
            root = AstBuilder.GetTreeRoot("from Entity where Entity.Child in (from OtherEntity)");

            tuple = (Util.Hql.Gold.AST.Tuple) root.Get<WhereClause>().Get<BinaryArithmeticOperator>().RightHandOperand;
            Assert.AreEqual(TupleFormat.SubQuery, tuple.Format);
            Assert.IsInstanceOfType(typeof (SelectStatement), tuple.ChildNodes[0]);

            AstBuilder.GetTreeRoot("from Entity where Entity.Child in (from OtherEntity where Age = 5)");
        }

        [Test]
        public void NamedParameters()
        {
            Node root = AstBuilder.GetTreeRoot("from Person where Name = :SomeName");
            Node param = root.Get<WhereClause>().Get<BinaryArithmeticOperator>().RightHandOperand;

            Assert.IsInstanceOfType(typeof (Parameter), param);
            Assert.AreEqual(ParameterType.Named, ((Parameter) param).Type);
            Assert.AreEqual("SomeName", ((Parameter) param).Name);
        }

        [Test]
        public void ParseWhereTree()
        {
            Node root = AstBuilder.GetTreeRoot("delete from Table where a = 5 and b between 0 and 5 * 3 + -2");
            /* Should return (lines between parentheses are non-terminal nodes):
			 * - (a = 5 and b between 0 and 5 * 3 + -2)
			 *		- (a = 5)
			 *			- a
			 *			- =
			 *			- 5
			 *		- (b between 0 and 5 * 3 + -2)
			 *			- b
			 *			- between
			 *			- 0
			 *			- (5 * 3 + -2)
			 *				- (5 * 3)
			 *					- 5
			 *					- *
			 *					- 3
			 *				- (-2)
			 *					- -
			 *					- 2
			 */

            Node where = root.Get<WhereClause>();

            BinaryLogicOperator mainAnd = where.Get<BinaryLogicOperator>();
            Assert.IsNotNull(mainAnd);
            Assert.AreEqual("and", mainAnd.OperatorText);

            Assert.IsInstanceOfType(typeof (BinaryArithmeticOperator), mainAnd.LeftHandOperand);
            Assert.IsInstanceOfType(typeof (BetweenOperator), mainAnd.RightHandOperand);

            Node highOperand = ((BetweenOperator) mainAnd.RightHandOperand).HighOperand;
            Assert.IsInstanceOfType(typeof (NegateOperator), ((BinaryOperator) highOperand).RightHandOperand);

            Assert.AreEqual(2, ((BinaryOperator) highOperand).RightHandOperand.Get<IntValue>());
        }

        [Test]
        public void PositionalParameters()
        {
            Node root = AstBuilder.GetTreeRoot("from Person where Name = ?");
            Node param = root.Get<WhereClause>().Get<BinaryArithmeticOperator>().RightHandOperand;

            Assert.IsInstanceOfType(typeof (Parameter), param);
            Assert.AreEqual(ParameterType.Positional, ((Parameter) param).Type);
            Assert.IsNull(((Parameter) param).Name);
        }

        [Test]
        public void SelectFilters()
        {
            Node root;
            root = AstBuilder.GetTreeRoot("select Name from Person");
            Assert.IsNull(root.Get<SelectClause>().Filter);

            root = AstBuilder.GetTreeRoot("select distinct Name from Person");
            Assert.AreEqual(SelectFilter.Distinct, root.Get<SelectClause>().Filter);

            root = AstBuilder.GetTreeRoot("select all Name from Person");
            Assert.AreEqual(SelectFilter.All, root.Get<SelectClause>().Filter);
        }

        [Test]
        public void SimpleOperators()
        {
            // Call the builder and expect no errors
            AstBuilder.GetTreeRoot("from Entity where Name like 'John Doe'");
            AstBuilder.GetTreeRoot("from Entity where Entity.Child.Name like 'John Doe'");
            AstBuilder.GetTreeRoot("from Entity where Entity.Child.Name not like 'John Doe'");

            AstBuilder.GetTreeRoot("from Entity where not (Name = 'John Doe')");

            AstBuilder.GetTreeRoot("from Entity where Name is null");
            AstBuilder.GetTreeRoot("from Entity where Name is not null");

            AstBuilder.GetTreeRoot("delete from Table where Age between 0 and 5");
            AstBuilder.GetTreeRoot("delete from Table where Age not between 0 and 5");
        }

        [Test]
        public void SimpleWhereCondition()
        {
            Node root = AstBuilder.GetTreeRoot("from t where Name = 'John Doe'");

            WhereClause whereClause = root.Get<WhereClause>();

            Assert.IsNotNull(whereClause);
            Assert.IsNotNull(whereClause.Get<BinaryArithmeticOperator>());

            Assert.IsInstanceOfType(typeof (Identifier), whereClause.Get<BinaryArithmeticOperator>().LeftHandOperand);
            Assert.AreEqual("=", whereClause.Get<BinaryArithmeticOperator>().OperatorText);
            Assert.IsInstanceOfType(typeof (LiteralNode), whereClause.Get<BinaryArithmeticOperator>().RightHandOperand);
        }

        [Test]
        public void StringConcatenation()
        {
            // Useless parentheses are supported too!
            AstBuilder.GetTreeRoot("from Person where Password = '%' + login + '%'");
            AstBuilder.GetTreeRoot("from Person where Password = (('%' + (login + 'a')) + '%')");

            AstBuilder.GetTreeRoot("from Person where Password like login + '%'");
            AstBuilder.GetTreeRoot("from Person where Password like (('%' + (login + 'a')) + '%')");

            AstBuilder.GetTreeRoot("from Person where Password like ('test')");
        }

        [Test]
        [ExpectedException(typeof (QueryException))]
        public void ThrowWhenOnlyValueIsInWhere()
        {
            // Is a common mistake to parse successfully the following query
            AstBuilder.GetTreeRoot("from Person where 5");
        }

        [Test]
        public void UnaryNodeWithSubqueries()
        {
            AstBuilder.GetTreeRoot("delete from Entity where property1 = -(select distinct Prop1 from Entity)");
        }

        [Test]
        public void WhereWithParenthesis()
        {
            AstBuilder.GetTreeRoot("from Person where (Age = 1)");

            Node root;

            root = AstBuilder.GetTreeRoot("from Person where Age = 1 and (Name = 'Fabio' or Name = 'Dario')");

            BinaryLogicOperator mainAnd = root.Get<WhereClause>().Get<BinaryLogicOperator>();
            Assert.IsNotNull(mainAnd.LeftHandOperand);
            Assert.AreEqual("and", mainAnd.OperatorText);
            Assert.IsNotNull(mainAnd.RightHandOperand);

            root = AstBuilder.GetTreeRoot("from Person where (Age = 1 and Name = 'Fabio') or Name = 'Dario'");

            BinaryLogicOperator mainOr = root.Get<WhereClause>().Get<BinaryLogicOperator>();
            Assert.IsNotNull(mainOr.LeftHandOperand);
            Assert.AreEqual("or", mainOr.OperatorText);
            Assert.IsNotNull(mainOr.RightHandOperand);
        }

        [Test]
        public void WithoutSelect()
        {
            Node root = AstBuilder.GetTreeRoot("from Table");
            Assert.IsInstanceOfType(typeof (SelectStatement), root);

            Assert.IsNull(root.Get<SelectClause>());
            Assert.IsNotNull(root.Get<FromClause>());
        }
    }
}