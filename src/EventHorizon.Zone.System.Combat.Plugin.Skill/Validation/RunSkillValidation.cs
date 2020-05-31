namespace EventHorizon.Zone.System.Combat.Plugin.Skill.Validation
{
    using EventHorizon.Zone.Core.Model.Entity;
    using EventHorizon.Zone.System.Combat.Plugin.Skill.Model;
    using global::System.Collections.Generic;
    using global::System.Numerics;
    using MediatR;

    public struct RunSkillValidation : IRequest<IEnumerable<SkillValidatorResponse>>
    {
        public SkillInstance Skill { get; }
        public IList<SkillValidator> ValidatorList { get; }
        public IObjectEntity Caster { get; }
        public IObjectEntity Target { get; }
        public Vector3 TargetPosition { get; }

        public RunSkillValidation(
            SkillInstance skill,
            IList<SkillValidator> validatorList,
            IObjectEntity caster,
            IObjectEntity target,
            Vector3 targetPosition
        )
        {
            Skill = skill;
            ValidatorList = validatorList ?? new List<SkillValidator>();
            Caster = caster;
            Target = target;
            TargetPosition = targetPosition;
        }
    }
}