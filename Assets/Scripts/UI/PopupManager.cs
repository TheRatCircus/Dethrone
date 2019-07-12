// Manager to handle popups (i.e. damage, status effects)
using UnityEngine;
using UnityEngine.UI;

public class PopupManager : MonoBehaviour
{
    public GameObject damagePopupPrefab;
    public GameObject canvas;

    public static PopupManager instance;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Create a popup whenever an actor is healed or damaged
    public void HealthChangePopup(float healthChange, Transform target)
    {
        Text popupText = Instantiate(damagePopupPrefab, target.position, new Quaternion(0, 0, 0, 0), canvas.transform).GetComponent<Text>();
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
