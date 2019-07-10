// Impel: Throw the user's weapon in an arc
using System.Collections;
using UnityEngine;

namespace Dethrone.Talents
{
    public class Impel : Talent
    {
        float range = .5f;
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

        // Cast this talent
        public override IEnumerator Cast()
        {
            isActive = true;
            isTelegraphing = true;
            yield return new WaitForSeconds(telegraphTime);

            isTelegraphing = false;
            isCasting = true;
            CastEffect();
            impelProj.SetProjectileIsTrigger(true);
            yield return new WaitForSeconds(castingTime);

            isCasting = false;
            isActive = false;
        }

        // This talent's effect upon being cast
        protected override void CastEffect() => Emit();

        // This talent's emission behaviour
        protected override void Emit()
        {
            impelGameObject = Instantiate(prefab, targettingController.GetOrbitPoint(range), targettingController.AimRotation);
            impelProj = impelGameObject.GetComponent<Emissions.Impel>();
            impelProj.Initialize(damage, targettingController.AimDirection, 512f);
        }
    }
}
