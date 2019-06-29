using System.Collections;
using UnityEngine;

namespace Dethrone.Talents
{
    public class Slash : Talent
    {
        float range = 1.5f;
        float damage = 20f;
        GameObject prefab;
        GameObject slashGameObject;
        Emissions.Slash slashAOE;

        void Awake()
        {
            moduleName = "Slash";
            castAnimation = (int)CastAnimation.None;
            prefab = (GameObject)Resources.Load("Prefabs/Slash", typeof(GameObject));
            manaCost = 20f;
            telegraphTime = 0.25f;
            castingTime = 0.25f;
        }

        public override IEnumerator Cast()
        {
            isActive = true;
            CastEffect();
            isTelegraphing = true;
            talentController.SetStatus(true, true, false);
            slashAOE.SetStatus(true, true, false);
            yield return new WaitForSeconds(telegraphTime);

            isTelegraphing = false;
            isCasting = true;
            talentController.SetStatus(true, false, true);
            slashAOE.SetStatus(true, false, true);
            slashAOE.SetAOEIsTrigger(true);
            yield return new WaitForSeconds(castingTime);

            isCasting = false;
            isActive = false;
            slashAOE.SetStatus(false, false, false);
            slashAOE.DestroyAOE();
            talentController.SetStatus(false, false, false);
        }

        protected override void CastEffect()
        {
            Emit();
        }

        protected override void Emit()
        {
            slashGameObject = Instantiate(prefab, targettingController.GetOrbitPoint(range), targettingController.AimRotation, targettingController.transform);
            slashAOE = slashGameObject.GetComponent<Emissions.Slash>();
            slashAOE.Initialize(damage);
        }

    }

}