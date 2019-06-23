using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DamagePopup : MonoBehaviour
{
    public Text popupText;

    private Vector2 currentVelocity;

    // Use this for initialization
    void Start()
    {
        currentVelocity = Vector2.zero;
        popupText = this.GetComponent<Text>();
        StartCoroutine("Fade");
    }

    private void Update()
    {
        Vector2 risePos = transform.position;
        risePos.y += 1.0f;
        transform.position = Vector2.SmoothDamp(transform.position, risePos, ref currentVelocity, 1.0f);
    }

    IEnumerator Fade()
    {
        yield return new WaitForSeconds(1.0f);
        
        while (popupText.color.a > 0)
        {
            Color popupColor = popupText.color;
            popupColor.a -= Time.deltaTime / 1.0f;
            popupText.color = popupColor;
            
            yield return null;
        }
    }
}
