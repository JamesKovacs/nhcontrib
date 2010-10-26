using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.UserTypes;
using NHibernate.SqlTypes;
using System.Data;
using System.Data.Common;

namespace NHibernate.Envers.Entities
{
    /**
     * A hibernate type for the {@link RevisionType} enum.
     * @author Simon Duduica, port of Envers omonyme class by Adam Warski (adam at warski dot org)
     */
    public class RevisionTypeType: IUserType {
        private static readonly SqlType[] SQL_TYPES = { new SqlType(DbType.Byte) };

        public SqlType[] SqlTypes { 
            get {
                return SQL_TYPES;
            }
        }

        public System.Type ReturnedType {
            get{
                return typeof(RevisionType);
            }
        }

        public object NullSafeGet(IDataReader resultSet, String[] names, Object owner){
            if( resultSet.Read())
            {
                try{
                    Byte representation = (Byte) resultSet[names[0]];            
                    return RevisionType.FromRepresentation(representation);
                }
                catch ( IndexOutOfRangeException){
                    return null;
                }
            }
            return null;
        }

        public void NullSafeSet(IDbCommand cmd, Object value, int index){
            if (null == value) {
                cmd.Parameters[index] = null;
            } else {
                ((IDbDataParameter)cmd.Parameters[index]).Value = value;
                //DbParameter param = DbParameter;
                //cmd.Parameters[index] = param.Value();
            }
        }

        public Object DeepCopy(Object value){
            return value;
        }

        public bool IsMutable
        {
            get { return false; }
        }

        public Object Assemble(object cached, Object owner){
            return cached;
        }

        public object Disassemble(Object value){
            return value;
        }

        public Object Replace(Object original, Object target, Object owner){
            return original;
        }

        public int GetHashCode(Object x){
            return x.GetHashCode();
        }

        public bool Equals(Object x, Object y){
            //noinspection ObjectEquality
            if (x == y) {
                return true;
            }

            if (null == x || null == y) {
                return false;
            }

            return x.Equals(y);
        }
    }
}
