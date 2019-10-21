using EventHorizon.Zone.System.Server.Scripts.Events.Load;
using EventHorizon.Zone.System.Server.Scripts.System;

var randomMaxValue = Data.Get<int>("RandomMaxValue");
var randomValue = Services.Random.Next(
    randomMaxValue
).ToString();

var nowString = Services.DateTime.Now.ToString();

var locale = Data.Get<string>("Locale");
var i18nKey = Data.Get<string>("I18nKey");
var i18nValue = Services.I18n.Lookup(
    locale,
    i18nKey
);

await Services.Mediator.Send(
    new LoadServerScriptsCommand()
);

return new SystemServerScriptResponse(
    true,
    $"Random: {randomValue} | DateTime: {nowString} | I18n: {i18nValue}"
);