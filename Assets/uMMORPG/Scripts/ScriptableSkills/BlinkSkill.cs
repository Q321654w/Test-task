namespace UnityEngine.Tilemaps.ScriptableSkills
{
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
                var offset = 0f;
                if (caster.TryGetComponent<CircleCollider2D>(out var circleCollider))
                    offset = circleCollider.radius;

                var maxRange = range;
                var transform = caster.transform;
                var lookDirection = caster.lookDirection;
                var startPoint = (Vector2)transform.position;
                var hits = Physics2D.CircleCastAll(startPoint, offset, lookDirection, range);

                foreach (var hit in hits)
                {
                    if (!hit.transform.gameObject.TryGetComponent<TilemapCollider2D>(out _))
                        continue;
                    
                    Debug.Log($"hit point {hit.point}");
                    
                    var distanceVector = hit.point - (Vector2)transform.position;
                    var newMaxRange = Mathf.Abs(Vector2.Dot(lookDirection,distanceVector)) - offset;
                    maxRange = Mathf.Min(newMaxRange, maxRange);
                }

                var destination = (Vector2)transform.position + caster.lookDirection * maxRange;
                caster.movement.Warp(destination);
            }
        }
    }
}