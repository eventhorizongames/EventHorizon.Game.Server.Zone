/// <summary>
/// Caster - 
/// Target - 
/// Data: { propertyName: string; valueProperty: string; cost: number }
/// </summary>
var propertyName = (string)Data["propertyName"];
var valueProperty = (string)Data["valueProperty"];
var cost = (long)Data["cost"];

var casterPropertyValue = Caster.GetProperty<dynamic>(propertyName)[valueProperty];

return casterPropertyValue >= cost;