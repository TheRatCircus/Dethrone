// Slash: A normal swing of a bladed weapon
using UnityEngine;

namespace Dethrone.Talents
{
    public class Slash : Talent
    {
        float damage = 20;
        GameObject prefab;
        GameObject slashGameObject;
        Emissions.AreaOfEffect slashAOE;

        // Awake is called when the script instance is being loaded
        void Awake()
        {
            moduleName = "Slash";
            castAnimation.normal = (int)global::CastAnimation.SlashSabre;
            castAnimation.alt = (int)global::CastAnimation.SlashSabreAlternate;
            prefab = (GameObject)Resources.Load("Prefabs/Slash", typeof(GameObject));
            manaCost = 20;
            timing.longTelegraphTime = .25f;
            timing.shortTelegraphTime = .15f;
            timing.castingTime = .25f;
            timing.comboTime = .75f;
        }

        // This talent's effect upon starting to telegraph
        protected override void TelegraphEffect(bool repeat) => Emit(repeat);

        // This talent's effect upon being cast
        protected override void CastEffect(bool repeat)
        {
            slashAOE._aoeStatus = talentStatus;
            slashAOE.EnableAOE(true);
        }

        // This talent's emission behaviour
        protected override void Emit(bool repeat)
        {
            castPosition = owner.transform.position;
            castPosition.x += spriteRenderer.flipX ? -.5f : .5f;
            slashGameObject = Instantiate(prefab, castPosition, new Quaternion(), owner.transform);
            if (spriteRenderer.flipX)
            {
                slashGameObject.GetComponent<SpriteRenderer>().flipX = true;
            }
            slashAOE = slashGameObject.GetComponent<Emissions.Slash>();
            slashAOE.Initialize(damage, owner, repeat);
            slashAOE._aoeStatus = talentStatus;
        }

        protected override void CloseEffect()
        {
            Destroy(slashGameObject);
        }

        public override void KillTalent()
        {
            base.KillTalent();
            Destroy(slashGameObject);
        }
    }
}
