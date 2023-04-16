using TMPro;
using UnityEngine;

public class HealthText : MonoBehaviour{
    public Vector3 moveSpeed = new(0, 75, 0);
    public float timeToFade = 1f;
    private RectTransform textTransform;
    private TextMeshProUGUI _textMeshProUGUI;
    private float timeElapsed = 0f;
    private Color startColor;

    private void Awake(){
        textTransform = GetComponent<RectTransform>();
        _textMeshProUGUI = GetComponent<TextMeshProUGUI>();
        startColor = _textMeshProUGUI.color;
    }

    private void Update(){
        textTransform.position += moveSpeed * Time.deltaTime;
        timeElapsed += Time.deltaTime;
        if (timeElapsed < timeToFade){
            var fadeAlpha = startColor.a * (1 - timeElapsed / timeToFade);
            _textMeshProUGUI.color = new Color(startColor.r, startColor.g, startColor.b, fadeAlpha);
        }
        else{
            Destroy(gameObject);
        }
    }
}