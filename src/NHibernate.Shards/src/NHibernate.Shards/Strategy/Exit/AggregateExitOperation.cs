using System;
using System.Collections;
using NHibernate.Criterion;

namespace NHibernate.Shards.Strategy.Exit
{
	public class AggregateExitOperation : IProjectionExitOperation
	{	    
        public enum SupportedAggregations
        {
            SUM,
            MIN,
            MAX
        }

	    private SupportedAggregations supportedAggregations;

	    private string fieldName;

	    private string aggregate;
        
		public IList Apply(IList results)
		{
		    IList nonNullResults = ExitOperationUtils.GetNonNullList(results);
            switch(aggregate.ToLower())
            {
                case "max":
                    return ExitOperationUtils.GetMaxList(nonNullResults);
                case "min":
                    return ExitOperationUtils.GetMinList(nonNullResults);
                case "sum":
                    IList sumList = new ArrayList();
                    sumList.Add(GetSum(results, fieldName));
                    return sumList;
                default:
                    throw new NotSupportedException("Aggregation Projected is unsupported: " + aggregate);                    
            }
		}

        private Decimal GetSum(IList results, string fieldName)
        {
            Decimal sum = new Decimal();
            foreach(object obj in results)
            {
                double num = getNumber(obj, fieldName);
                sum += new decimal(num);
            }
            return sum;
        }

        private double getNumber(object obj,string fieldName)
        {
            return Double.Parse(ExitOperationUtils.GetPropertyValue(obj, fieldName).ToString());
        }

	    public string Aggregate
	    {
            get { return this.aggregate; }
	    }
	    
        public AggregateExitOperation(IProjection projection)
        {
            string projectionAsString = projection.ToString();
            string aggregateName = projectionAsString.Substring(0, projectionAsString.IndexOf("("));
            this.fieldName = projectionAsString.Substring(projectionAsString.IndexOf("(") + 1, projectionAsString.IndexOf(")"));
            supportedAggregations = (SupportedAggregations) Enum.Parse(supportedAggregations.GetType(), aggregateName);
        }

        
	}
}