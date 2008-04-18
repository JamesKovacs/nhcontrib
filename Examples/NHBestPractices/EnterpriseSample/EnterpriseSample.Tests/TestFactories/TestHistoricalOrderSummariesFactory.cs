using System.Collections.Generic;
using EnterpriseSample.Core.Domain;

namespace EnterpriseSample.Tests.TestFactories
{
    class TestHistoricalOrderSummariesFactory
    {
        public List<HistoricalOrderSummary> CreateHistoricalOrderSummaries() {
            List<HistoricalOrderSummary> summaries = new List<HistoricalOrderSummary>();
            summaries.Add(Summary1);
            summaries.Add(Summary2);
            summaries.Add(Summary3);
            summaries.Add(Summary4);
            return summaries;
        }

        private HistoricalOrderSummary Summary1 {
            get {
                return new HistoricalOrderSummary("Milk", 3);
            }
        }

        private HistoricalOrderSummary Summary2 {
            get {
                return new HistoricalOrderSummary("Pizza", 74);
            }
        }

        private HistoricalOrderSummary Summary3 {
            get {
                return new HistoricalOrderSummary("Homebrew", 128);
            }
        }

        private HistoricalOrderSummary Summary4 {
            get {
                return new HistoricalOrderSummary("Lettuce", 1);
            }
        }
    }
}
