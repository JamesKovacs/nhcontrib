using System.Collections.Generic;
using log4net;
using NHibernate.Annotations.NHibernate.Cfg;
using NHibernate.Mapping;

namespace NHibernate.Cfg
{
	//TODO: this should be renamed at future.
	public class Ejb3Column
	{
		private static readonly ILog log = LogManager.GetLogger(typeof (Ejb3Column));

		private Column mappingColumn;
		private bool insertable = true;
		private bool updatable = true;
		private string secondaryTableName;
		protected Dictionary<string, Join> joins;
		protected PropertyHolder propertyHolder;
		private ExtendedMappings mappings;
		private bool isImplicit;
		public static readonly int DEFAULT_COLUMN_LENGTH = 255;
		public string sqlType;
		private int length = DEFAULT_COLUMN_LENGTH;
		private int precision;
		private int scale;
		private string logicalColumnName;
		private string propertyName;
		private bool unique;
		private bool nullable = true;
		private string formulaString;
		private Formula formula;
		private Table table;


		public void setTable(Table table)
		{
			this.table = table;
		}

		public string getLogicalColumnName()
		{
			return logicalColumnName;
		}

		public string getSqlType()
		{
			return sqlType;
		}

		public int getLength()
		{
			return length;
		}

		public int getPrecision()
		{
			return precision;
		}

		public int getScale()
		{
			return scale;
		}

		public bool isUnique()
		{
			return unique;
		}

		public string getFormulaString()
		{
			return formulaString;
		}

		public string getSecondaryTableName()
		{
			return secondaryTableName;
		}

		public void setFormula(string formula)
		{
			formulaString = formula;
		}

		public bool IsImplicit
		{
			get { return isImplicit; }
		}


		public void SetInsertable(bool insertable)
		{
			this.insertable = insertable;
		}

		public void SetUpdatable(bool updatable)
		{
			this.updatable = updatable;
		}

		protected ExtendedMappings Mappings
		{
			get { return mappings; }
			set { mappings = value; }
		}

		public void SetImplicit(bool @implicit)
		{
			isImplicit = @implicit;
		}

		public void SetSqlType(string sqlType)
		{
			this.sqlType = sqlType;
		}

		public void SetLength(int length)
		{
			this.length = length;
		}

		public void SetPrecision(int precision)
		{
			this.precision = precision;
		}

		public void SetScale(int scale)
		{
			this.scale = scale;
		}

		public void SetLogicalColumnName(string logicalColumnName)
		{
			this.logicalColumnName = logicalColumnName;
		}

		public string PropertyName
		{
			get { return propertyName; }
			set { propertyName = value; }
		}

		public void SetUnique(bool unique)
		{
			this.unique = unique;
		}

		public bool IsNullable
		{
			get { return mappingColumn.IsNullable; }
		}

		public void Bind()
		{
			if (! string.IsNullOrEmpty(formulaString))
			{
				log.DebugFormat("binding formula {0}", formulaString);
				formula = new Formula();
				formula.FormulaString = formulaString;
			}
			else
			{
				InitMappingColumn(logicalColumnName, propertyName, length, precision, scale, nullable, sqlType, unique, true);
				log.DebugFormat("Binding column {0}. Unique {1}", mappingColumn.Name, unique);
			}
		}

		protected void InitMappingColumn(string columnName,
		                                 string propertyName,
		                                 int length,
		                                 int precision,
		                                 int scale,
		                                 bool nullable,
		                                 string sqlType,
		                                 bool unique,
		                                 bool applyNamingStrategy)
		{
			this.mappingColumn = new Column();
			RedefineColumnName(columnName, propertyName, applyNamingStrategy);
			this.mappingColumn.Length = length;
			if (precision > 0)
			{  
				//revelent precision
				this.mappingColumn.Precision =precision ;
				this.mappingColumn. Scale = scale ;
			}
			this.mappingColumn.IsNullable = nullable ;
			this.mappingColumn.SqlType =sqlType;
			this.mappingColumn.Unique =unique ;
		}

		protected void RedefineColumnName(string columnName, string propertyName, bool namingStrategy)
		{
			
		}
	}
}