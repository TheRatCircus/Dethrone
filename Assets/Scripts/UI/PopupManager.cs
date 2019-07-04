// Manager to handle popups (i.e. damage, status effects)
using UnityEngine;
using UnityEngine.UI;

public class PopupManager : MonoBehaviour
{
    Text damagePopupPrefab;
    Text[] damagePopup;

    public GameObject canvas;

    // Start is called before the first frame update
    void Start()
    {
        damagePopupPrefab = (Text)Resources.Load("Prefabs/DamagePopup", typeof(Text));

        Health[] healths = (Health[])Resources.FindObjectsOfTypeAll(typeof(Health));
        foreach (Health health in healths)
        {
            health.OnHealthChangeEvent += HealthChangePopup;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Create a popup whenever an actor is healed or damaged
    public void HealthChangePopup(float healthChange, Transform target)
    {
        Text popupText = Instantiate(damagePopupPrefab, target.position, new Quaternion(0, 0, 0, 0), canvas.transform);
        popupText.text = (-healthChange).ToString();
        if (healthChange == 0)
        {
            popupText.color = Color.grey;
        }
        else if (healthChange < 0)
        {
            popupText.color = Color.red;
        }
        else
        {
            popupText.color = Color.green;
        }
    }
}
