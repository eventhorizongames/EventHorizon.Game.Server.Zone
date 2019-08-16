/// <summary>
/// Name: Interaction_TestInteraction.csx
/// 
/// This script is just a test script, writes to the log.
/// 
/// Player: IObjectEntity
/// Target: IObjectEntity
/// Data: IDictionary<string, object>
/// Services: { 
///     Mediator: IMediator; 
///     Random: IRandomNumberGenerator; 
///     DateTime: IDateTimeService; 
///     I18n: I18nLookup; 
/// }
/// </summary>

System.Console.WriteLine($"I am here: Player: {Player.Id}  |  Target: {Target.Id}");