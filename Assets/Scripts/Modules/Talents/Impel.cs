using System.Collections;
using UnityEngine;

namespace Dethrone.Talents
{
    public class Impel : Talent
    {
        float range = 1.5f;
        float damage = 20f;
        GameObject prefab;
        GameObject impelGameObject;
        Emissions.Impel impelProj;

        void Awake()
        {
            moduleName = "Impel";
            castAnimation = (int)CastAnimation.Throw;
            prefab = (GameObject)Resources.Load("Prefabs/ThrowingAxe", typeof(GameObject));
            manaCost = 20f;
            telegraphTime = 0.25f;
            castingTime = 0.1f;
        }

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

        protected override void CastEffect()
        {
            Emit();
        }

        protected override void Emit()
        {
            impelGameObject = Instantiate(prefab, targettingController.GetOrbitPoint(range), targettingController.AimRotation);
            impelProj = impelGameObject.GetComponent<Emissions.Impel>();
            impelProj.Initialize(damage, targettingController.AimDirection, 512f);
        }
    }
}
