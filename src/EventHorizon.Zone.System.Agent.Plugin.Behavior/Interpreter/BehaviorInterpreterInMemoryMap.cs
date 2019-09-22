using System.Collections.Generic;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.Api;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.Interpreters;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.Model;

namespace EventHorizon.Zone.System.Agent.Plugin.Behavior.Interpreter
{
    public class BehaviorInterpreterInMemoryMap : BehaviorInterpreterMap
    {
        readonly Dictionary<BehaviorNodeType, BehaviorInterpreter> _interpreterMap;

        public BehaviorInterpreterInMemoryMap(
            ActionBehaviorInterpreter actionInterpreter,
            ConditionBehaviorInterpreter conditionInterpreter
        )
        {
            var prioritySelectorInterpreter = new PrioritySelectorInterpreter();
            var concurrentSelectorInterpreter = new ConcurrentSelectorInterpreter();
            var sequenceSelectorInterpreter = new SequenceSelectorInterpreter();

            _interpreterMap = new Dictionary<BehaviorNodeType, BehaviorInterpreter>()
            {
                {
                    BehaviorNodeType.PRIORITY_SELECTOR, prioritySelectorInterpreter
                },
                {
                    BehaviorNodeType.CONCURRENT_SELECTOR, concurrentSelectorInterpreter
                },
                {
                    BehaviorNodeType.SEQUENCE_SELECTOR, sequenceSelectorInterpreter
                },
                {
                    BehaviorNodeType.CONDITION, conditionInterpreter
                },
                {
                    BehaviorNodeType.ACTION, actionInterpreter
                }
            };
        }

        public BehaviorInterpreter InterperterByType(
            BehaviorNodeType type
        )
        {
            // TODO: Add validation check?
            return _interpreterMap[type];
        }
    }
}