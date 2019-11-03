using System.Collections.Generic;

namespace EventHorizon.Zone.Core.Reporter.Model
{
    public struct Report
    {
        public string Id { get; }
        public IList<ReportItem> ItemList { get; private set; }

        public Report(
            string id
        )
        {
            this.Id = id;
            this.ItemList = new List<ReportItem>().AsReadOnly();
        }

        public Report AddItem(
            ReportItem item
        )
        {
            var itemList = new List<ReportItem>(
                this.ItemList
            );
            itemList.Add(
                item
            );
            this.ItemList = itemList.AsReadOnly();
            return this;
        }
    }
}