using EventHorizon.Zone.Core.Model.Entity;
using EventHorizon.Zone.Core.Model.Player;
using EventHorizon.Zone.System.Server.Scripts.Model;
using EventHorizon.Observer.Model;

using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

public interface Testing003_TestEventObserver
    : ArgumentObserver<Testing003_TestEvent>
{
}
