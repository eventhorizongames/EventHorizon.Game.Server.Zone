using System.Collections;
using System.Collections.Generic;
using EventHorizon.Game.Server.Zone.Editor.Model;
using MediatR;

namespace EventHorizon.Game.Server.Zone.Editor.Load
{
    public class LoadEditorScriptListEvent : IRequest<IList<EditorScript>>
    {
        
    }
}