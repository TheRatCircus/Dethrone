using System.Collections;
using System.Collections.Generic;
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
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DamagePopup(float damage, Transform target)
    {
        Text popupText = Instantiate(damagePopupPrefab, target.position, new Quaternion(0, 0, 0, 0), canvas.transform);
        popupText.text = (-damage).ToString();
    }
}
