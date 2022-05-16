namespace EventHorizon.Zone.Core.Observable;

using System;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
public class ObservableEvent : Attribute { }
