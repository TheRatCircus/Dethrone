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
            castAnimation = (int)global::CastAnimation.SabreSlash;
            prefab = (GameObject)Resources.Load("Prefabs/Slash", typeof(GameObject));
            manaCost = 20;
            telegraphTime = .25f;
            castingTime = .25f;
        }

        // This talent's effect upon starting to telegraph
        protected override void TelegraphEffect() => Emit();

        // This talent's effect upon being cast
        protected override void CastEffect()
        {
            slashAOE._aoeStatus = talentStatus;
            slashAOE.EnableAOE(true);
        }

        // This talent's emission behaviour
        protected override void Emit()
        {
            castPosition = owner.transform.position;
            castPosition.x += spriteRenderer.flipX ? -.5f : .5f;
            slashGameObject = Instantiate(prefab, castPosition, new Quaternion(), owner.transform);
            if (spriteRenderer.flipX)
            {
                slashGameObject.GetComponent<SpriteRenderer>().flipX = true;
            }
            slashAOE = slashGameObject.GetComponent<Emissions.Slash>();
            slashAOE.Initialize(damage, owner);
            slashAOE._aoeStatus = talentStatus;
            Destroy(slashGameObject, telegraphTime + castingTime);
        }
    }
}
