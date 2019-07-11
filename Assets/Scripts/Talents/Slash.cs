// Slash: A normal swing of a bladed weapon
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

        // Awake is called when the script instance is being loaded
        void Awake()
        {
            moduleName = "Slash";
            castAnimation = (int)global::CastAnimation.None;
            prefab = (GameObject)Resources.Load("Prefabs/Slash", typeof(GameObject));
            manaCost = 20;
            telegraphTime = .25f;
            castingTime = .25f;
        }

        // Cast this talent
        public override IEnumerator Cast()
        {
            isActive = true;
            CastEffect();
            isTelegraphing = true;
            slashAOE.SetStatus(true, true, false);
            yield return new WaitForSeconds(telegraphTime);

            isTelegraphing = false;
            isCasting = true;
            slashAOE.SetStatus(true, false, true);
            slashAOE.EnableAOE(true);
            yield return new WaitForSeconds(castingTime);

            isCasting = false;
            isActive = false;
            slashAOE.SetStatus(false, false, false);
            slashAOE.DestroyAOE();
        }

        // This talent's effect upon being cast
        protected override void CastEffect()
        {
            Emit();
        }

        // This talent's emission behaviour
        protected override void Emit()
        {
            slashGameObject = Instantiate(prefab, targettingController.GetOrbitPoint(range), targettingController.AimRotation, targettingController.transform);
            slashAOE = slashGameObject.GetComponent<Emissions.Slash>();
            slashAOE.Initialize(damage, owner);
        }
    }
}
