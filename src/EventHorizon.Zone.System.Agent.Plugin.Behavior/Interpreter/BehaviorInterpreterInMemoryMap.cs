namespace EventHorizon.Zone.System.Agent.Plugin.Behavior.Interpreter
{
    using global::System.Collections.Generic;

    using EventHorizon.Zone.System.Agent.Plugin.Behavior.Api;
    using EventHorizon.Zone.System.Agent.Plugin.Behavior.Interpreters;
    using EventHorizon.Zone.System.Agent.Plugin.Behavior.Model;

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
            return _interpreterMap[type];
        }
    }
}
