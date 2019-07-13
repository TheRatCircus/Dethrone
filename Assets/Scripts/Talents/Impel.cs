// Impel: Throw the user's weapon
using UnityEngine;

namespace Dethrone.Talents
{
    public class Impel : Talent
    {
        float damage = 20f;
        GameObject prefab;
        GameObject impelGameObject;
        Emissions.Impel impelProj;

        // Awake is called when the script instance is being loaded
        void Awake()
        {
            moduleName = "Impel";
            castAnimation = (int)global::CastAnimation.Throw;
            prefab = (GameObject)Resources.Load("Prefabs/ThrowingAxe", typeof(GameObject));
            manaCost = 20f;
            telegraphTime = 0.25f;
            castingTime = 0.1f;
        }

        // This talent's effect upon being cast
        protected override void CastEffect() => Emit();

        // This talent's emission behaviour
        protected override void Emit()
        {
            castPosition = owner.transform.position;
            castPosition.x += spriteRenderer.flipX ? -.5f : .5f;
            impelGameObject = Instantiate(prefab, castPosition, new Quaternion());
            impelProj = impelGameObject.GetComponent<Emissions.Impel>();
            impelProj.Initialize(damage, spriteRenderer.flipX ? Vector2.left : Vector2.right, 512f);
            impelProj.EnableProjectile(true);
            Destroy(impelGameObject, impelProj.Lifetime);
        }
    }
}
