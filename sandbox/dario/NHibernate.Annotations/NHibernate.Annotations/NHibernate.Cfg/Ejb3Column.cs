using System;
using System.Collections.Generic;
using System.Persistence;
using log4net;
using NHibernate.Annotations;
using NHibernate.Annotations.Cfg;
using NHibernate.Annotations.NHibernate.Cfg;
using NHibernate.Mapping;
using NHibernate.Util;

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
        protected IDictionary<string, Join> joins;
        protected IPropertyHolder propertyHolder;
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

        public void SetFormula(string formula)
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
            if (!string.IsNullOrEmpty(formulaString))
            {
                log.DebugFormat("binding formula {0}", formulaString);
                formula = new Formula();
                formula.FormulaString = formulaString;
            }
            else
            {
                InitMappingColumn(logicalColumnName, propertyName, length, precision, scale, nullable, sqlType, unique,
                                  true);
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
                this.mappingColumn.Precision = precision;
                this.mappingColumn.Scale = scale;
            }
            this.mappingColumn.IsNullable = nullable;
            this.mappingColumn.SqlType = sqlType;
            this.mappingColumn.Unique = unique;
        }

        protected void RedefineColumnName(string columnName, string propertyName, bool applyNamingStrategy)
        {
            if (applyNamingStrategy)
            {
                if (StringHelper.IsEmpty(columnName))
                {
                    if (propertyName != null)
                    {
                        mappingColumn.Name = mappings.NamingStrategy.PropertyToColumnName(propertyName);
                    }
                    //Do nothing otherwise
                }
                else
                {
                    mappingColumn.Name = mappings.NamingStrategy.ColumnName(columnName);
                }
            }
            else
            {
                if (StringHelper.IsNotEmpty(columnName)) mappingColumn.Name = columnName;
            }
        }

        public Column MappingColumn
        {
            get { return mappingColumn; }
            set { mappingColumn = value; }
        }

        public string Name
        {
            get { return mappingColumn.Name; }
        }

        public bool IsInsertable
        {
            get { return insertable; }
        }

        public bool IsUpdatable
        {
            get { return updatable; }
        }

        public void SetNullable(bool nullable)
        {
            if (mappingColumn != null)
            {
                mappingColumn.IsNullable = nullable;
            }
            else
            {
                this.nullable = nullable;
            }
        }

        public void SetJoins(IDictionary<string, Join> joins)
        {
            this.joins = joins;
        }

        public IPropertyHolder PropertyHolder
        {
            get { return propertyHolder; }
            set { propertyHolder = value; }
        }

        public void linkWithValue(SimpleValue value)
        {
            if (formula != null)
            {
                value.AddFormula(formula);
            }
            else
            {
                MappingColumn.Value = value;
                value.AddColumn(MappingColumn);
                value.Table.AddColumn(MappingColumn);
                AddColumnBinding(value);
                table = value.Table;
            }
        }

        protected void AddColumnBinding(SimpleValue value)
        {
            string logicalColumnName = mappings.NamingStrategy.LogicalColumnName(this.logicalColumnName, propertyName);
            mappings.AddColumnBinding(logicalColumnName, MappingColumn, value.Table);
        }

        public Table Table
        {
            get
            {
                if (table != null) return table; //association table
                if (IsSecondary)
                {
                    return Join.Table;
                }
                else
                {
                    return propertyHolder.Table;
                }
            }
        }

        public bool IsSecondary
        {
            get
            {
                if (propertyHolder == null)
                {
                    throw new AssertionFailure("Should not call getTable() on column wo persistent class defined");
                }

                if (StringHelper.IsNotEmpty(secondaryTableName))
                {
                    return true;
                }

                return false;
            }
        }

        public Join Join
        {
            get
            {
                Join join;
                joins.TryGetValue(secondaryTableName, out join);

                if (join == null)
                    throw new AnnotationException("Cannot find the expected secondary table: no " + secondaryTableName +
                                                  " available for " + propertyHolder.ClassName);

                return join;
            }
        }

        public void ForceNotNull()
        {
            mappingColumn.IsNullable = true;
        }

        public void SetSecondaryTableName(string secondaryTableName)
        {
            this.secondaryTableName = secondaryTableName;
        }

        public static Ejb3Column[] BuildColumnFromAnnotation(
            ColumnAttribute[] anns,
            FormulaAttribute formulaAnn, Nullability nullability, IPropertyHolder propertyHolder,
            IPropertyData inferredData,
            IDictionary<string, Join> secondaryTables,
            ExtendedMappings mappings)
        {
            Ejb3Column[] columns;

            if (formulaAnn != null)
            {
                Ejb3Column formulaColumn = new Ejb3Column();
                formulaColumn.SetFormula(formulaAnn.Value);
                formulaColumn.SetImplicit(false);
                formulaColumn.Mappings = mappings;
                formulaColumn.PropertyHolder = propertyHolder;
                formulaColumn.Bind();
                columns = new Ejb3Column[] {formulaColumn};
            }
            else
            {
                columns = BuildColumnFromAnnotation(anns,
                                                    nullability,
                                                    propertyHolder,
                                                    inferredData,
                                                    secondaryTables,
                                                    mappings);
            }
            return columns;
        }

        private static Ejb3Column[] BuildColumnFromAnnotation(ColumnAttribute[] anns, Nullability nullability,
                                                              IPropertyHolder propertyHolder, IPropertyData inferredData,
                                                              IDictionary<string, Join> secondaryTables,
                                                              ExtendedMappings mappings)
        {
            ColumnAttribute[] actualCols = anns;
            ColumnAttribute[] overriddenCols = propertyHolder.GetOverriddenColumn(StringHelper.Qualify(propertyHolder.Path, inferredData.PropertyName));

            if (overriddenCols != null)
            {
                //check for overridden first
                if (anns != null && overriddenCols.Length != anns.Length)
                    throw new AnnotationException("AttributeOverride.column() should override all columns for now");

                actualCols = overriddenCols.Length == 0 ? null : overriddenCols;
                log.DebugFormat("Column(s) overridden for property {0}", inferredData.PropertyName);
            }

            if (actualCols == null)
                return BuildImplicitColumn(inferredData, secondaryTables, propertyHolder, nullability, mappings);

            int length = actualCols.Length;
            Ejb3Column[] columns = new Ejb3Column[length];
            for (int index = 0; index < length; index++)
            {
                ColumnAttribute col = actualCols[index];
                String sqlType = col.ColumnDefinition.Equals("") ? null : col.ColumnDefinition;
                Ejb3Column column = new Ejb3Column();
                column.SetImplicit(false);
                column.SetSqlType(sqlType);
                column.SetLength(col.Length);
                column.SetPrecision(col.Precision);
                column.SetScale(col.Scale);
                column.SetLogicalColumnName(col.Name);
                column.PropertyName = BinderHelper.GetRelativePath(propertyHolder, inferredData.PropertyName);
                column.SetNullable(col.Nullable); //TODO force to not null if available? This is a (bad) user choice.
                column.SetUnique(col.Unique);
                column.SetInsertable(col.Insertable);
                column.SetUpdatable(col.Updatable);
                column.SetSecondaryTableName(col.Table);
                column.PropertyHolder = propertyHolder;
                column.SetJoins(secondaryTables);
                column.Mappings = mappings;
                column.Bind();
                columns[index] = column;
            }
            return columns;
        }

        private static Ejb3Column[] BuildImplicitColumn(IPropertyData inferredData,
                                                        IDictionary<string, Join> secondaryTables,
                                                        IPropertyHolder propertyHolder, Nullability nullability,
                                                        ExtendedMappings mappings)
        {
            Ejb3Column[] columns;
            columns = new Ejb3Column[1];
            Ejb3Column column = new Ejb3Column();
            column.SetImplicit(false);
            //not following the spec but more clean
            if (nullability != Nullability.ForcedNull  && inferredData.ClassOrElement.IsPrimitive
                    && !inferredData.Property.GetType().IsArray ) //TODO: IsArray in this way ???
            {
                column.SetNullable(false);
            }
            column.SetLength(DEFAULT_COLUMN_LENGTH);
            column.PropertyName = BinderHelper.GetRelativePath(propertyHolder, inferredData.PropertyName);
            column.PropertyHolder = propertyHolder;
            column.SetJoins(secondaryTables);
            column.Mappings = mappings;
            column.Bind();
            columns[0] = column;
            return columns;
        }

        public static void checkPropertyConsistency(Ejb3Column[] columns, String propertyName)
        {
            int nbrOfColumns = columns.Length;
            if (nbrOfColumns > 1)
            {
                for (int currentIndex = 1; currentIndex < nbrOfColumns; currentIndex++)
                {
                    if (columns[currentIndex].IsInsertable != columns[currentIndex - 1].IsInsertable)
                    {
                        throw new AnnotationException("Mixing insertable and non insertable columns in a property is not allowed: " + propertyName);
                    }
                    if (columns[currentIndex].IsNullable != columns[currentIndex - 1].IsNullable)
                    {
                        throw new AnnotationException("Mixing nullable and non nullable columns in a property is not allowed: " + propertyName);
                    }
                    if (columns[currentIndex].IsUpdatable != columns[currentIndex - 1].IsUpdatable)
                    {
                        throw new AnnotationException("Mixing updatable and non updatable columns in a property is not allowed: " + propertyName);
                    }
                    if (!columns[currentIndex].Table.Equals(columns[currentIndex - 1].Table))
                    {
                        throw new AnnotationException("Mixing different tables in a property is not allowed: " + propertyName);
                    }
                }
            }
        }


        public void AddIndex(Index index, bool inSecondPass)
        {
            if (index == null) return;
            String indexName = index.Name;
            AddIndex(indexName, inSecondPass);
        }

        void AddIndex(String indexName, bool inSecondPass)
        {
            IndexOrUniqueKeySecondPass secondPass = new IndexOrUniqueKeySecondPass(indexName, this, mappings, false);
            if (inSecondPass)
            {
                secondPass.DoSecondPass(mappings.Classes);
            }
            else
            {
                mappings.AddSecondPass(secondPass);
            }

        }


        void AddUniqueKey(String uniqueKeyName, bool inSecondPass)
        {
            IndexOrUniqueKeySecondPass secondPass = new IndexOrUniqueKeySecondPass(uniqueKeyName, this, mappings, true);
            if (inSecondPass)
            {
                secondPass.DoSecondPass(mappings.Classes);
            }
            else
            {
                mappings.AddSecondPass(secondPass);
            }
        }
    }


}