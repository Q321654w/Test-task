using System.Collections;

namespace UnityEngine.Tilemaps.ScriptableSkills
{
    [CreateAssetMenu(menuName = "uMMORPG Skill/Circle Slash Damage", order = 999)]
    public class CircleSlashSkill : DamageSkill
    {
        public LinearFloat duration = new LinearFloat { baseValue = 4f };
        public LinearInt attacksCount = new LinearInt() { baseValue = 10 };

        public override bool CheckTarget(Entity caster)
        {
            caster.target = caster;
            return true;
        }

        public override bool CheckDistance(Entity caster, int skillLevel, out Vector2 destination)
        {
            destination = caster.transform.position;
            return true;
        }

        public override void Apply(Entity caster, int skillLevel, Vector2 direction)
        {
            caster.StartCoroutine(SlashIteration(caster, skillLevel));
        }

        private IEnumerator SlashIteration(Entity caster, int skillLevel)
        {
            var durationTime = duration.Get(skillLevel);
            var attackCount = attacksCount.Get(skillLevel);
            var range = castRange.Get(skillLevel);

            var frequency = durationTime / attackCount;

            var i = 0;

            while (i < attackCount)
            {
                var center = caster.transform.position;
                var colliders = Physics2D.OverlapCircleAll(center, range);
                foreach (Collider2D co in colliders)
                {
                    var candidate = co.GetComponentInParent<Entity>();
                    if (candidate != null && caster.CanAttack(candidate))
                    {
                        caster.combat.DealDamageAt(candidate, caster.combat.damage + damage.Get(skillLevel));
                    }
                }

                i++;
                yield return new WaitForSeconds(frequency);
            }
        }
    }
}