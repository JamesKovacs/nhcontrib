using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Dialect.Schema;

namespace NHibernate.Tool.Db2hbm
{
    ///<author>
    /// Felice Pollano (felice@felicepollano.com)
    ///</author>
    public class TypeConverter
    {
        class CandidateType
        {
            public string Type { get; set; }
            public int SatisfiedRestrictions { get; set; }
        }
        cfg.db2hbmconf config;
        public TypeConverter(cfg.db2hbmconf config )
        {
            this.config = config;
        }
        public  string GetNHType(IColumnMetadata cInfo)
        {
            List<CandidateType> found = new List<CandidateType>();
            var comp = config.typemapping.Where(t => string.Compare(t.dbtype, cInfo.TypeName, true) == 0);
            foreach (var candidate in comp)
            {
                CandidateType ct = new CandidateType();
                ct.Type = candidate.nhtype;
                ct.SatisfiedRestrictions += SatisfyLen(cInfo, candidate);
                ct.SatisfiedRestrictions += SatisfyPrecision(cInfo, candidate);
                ct.SatisfiedRestrictions += SatisfyScale(cInfo, candidate);
                found.Add(ct);
            }
            if (found.Count() > 0)
                return found.OrderByDescending(t=>t.SatisfiedRestrictions).First().Type;
            //logger.Warn(string.Format("No NHibernate type defined for dbtype:{0} len:{1}", cInfo.TypeName, cInfo.ColumnSize));
            return null;
        }

        private int SatisfyScale(IColumnMetadata cInfo, cfg.db2hbmconfSqltype candidate)
        {
            return 0; 
        }

        private int SatisfyPrecision(IColumnMetadata cInfo, cfg.db2hbmconfSqltype candidate)
        {
            if (null == candidate.precision)
                return 0;
            else
            {
                int hi, lo;
                GetHiLo(candidate.length, out hi, out lo);
                if (cInfo.NumericalPrecision >= lo && cInfo.NumericalPrecision <= hi)
                    return 1;
                else
                    return 0;
            }
        }

        private int SatisfyLen(IColumnMetadata cInfo, cfg.db2hbmconfSqltype candidate)
        {
            if (null == candidate.length)
                return 0;
            else
            {
                int hi, lo;
                GetHiLo(candidate.length,out hi,out lo);
                if (cInfo.ColumnSize >= lo && cInfo.ColumnSize <= hi)
                    return 1;
                else
                    return 0;
            }
        }

        private void GetHiLo(string p, out int hi, out int lo)
        {
            if (-1 == p.IndexOf("-"))
            {
                int.TryParse(p, out lo);
                hi = lo;
            }
            else
            {
                string[] tokens = p.Split('-');
                int.TryParse(tokens[0], out lo);
                if (tokens[1].Trim() == "*")
                {
                    hi = int.MaxValue;
                }
                else
                {
                    int.TryParse(tokens[1], out hi);
                }
            }
        }
    }
}
