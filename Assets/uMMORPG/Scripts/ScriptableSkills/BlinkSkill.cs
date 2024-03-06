using UnityEngine.AI;

namespace UnityEngine.Tilemaps.ScriptableSkills
{
    [CreateAssetMenu(menuName = "uMMORPG Skill/Blink", order = 999)]
    public class Blink : ScriptableSkill
    {
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
            var range = castRange.Get(skillLevel);

            var position = (Vector2)caster.transform.position;
            
            var movementDirection = caster.movement.GetVelocity().normalized;
            if (movementDirection == Vector2.zero)
                movementDirection = direction;
            
            var offset = 0f;
            if (caster.TryGetComponent<CircleCollider2D>(out var circleCollider))
                offset = circleCollider.radius;
            
            var destination = position + movementDirection * range;

            if (NavMesh2D.Raycast(position, position + movementDirection * (range + offset), out var hit,
                    NavMesh.AllAreas))
                destination = hit.position - offset * movementDirection;

            caster.movement.Warp(destination);
        }
    }
}