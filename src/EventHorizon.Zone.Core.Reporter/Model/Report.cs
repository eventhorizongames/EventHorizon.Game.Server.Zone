namespace EventHorizon.Zone.Core.Reporter.Model;

using System;
using System.Collections.Generic;

public struct Report
{
    public string Id { get; }
    public DateTime Timestamp { get; }
    public IList<ReportItem> ItemList { get; private set; }

    public Report(
        string id,
        DateTime timestamp
    )
    {
        Id = id;
        Timestamp = timestamp;
        ItemList = new List<ReportItem>().AsReadOnly();
    }

    public Report AddItem(
        ReportItem item
    )
    {
        ItemList = new List<ReportItem>(
            ItemList
        )
        {
            item
        }.AsReadOnly();

        return this;
    }
}
