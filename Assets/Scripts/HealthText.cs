using TMPro;
using UnityEngine;

[RequireComponent(typeof(RectTransform), typeof(TextMeshProUGUI))]
public class HealthText : MonoBehaviour{
    public Vector3 moveSpeed = new(0, 75, 0);
    public float timeToFade = 1f;
    private RectTransform _textTransform;
    private TextMeshProUGUI _textMeshProUGUI;
    private float _timeElapsed;
    private Color _startColor;

    private void Awake(){
        _textTransform = GetComponent<RectTransform>();
        _textMeshProUGUI = GetComponent<TextMeshProUGUI>();
        _startColor = _textMeshProUGUI.color;
    }

    private void Update(){
        _textTransform.position += moveSpeed * Time.deltaTime;
        _timeElapsed += Time.deltaTime;
        if (_timeElapsed < timeToFade){
            var fadeAlpha = _startColor.a * (1 - _timeElapsed / timeToFade);
            _textMeshProUGUI.color = new Color(_startColor.r, _startColor.g, _startColor.b, fadeAlpha);
        }
        else{
            Destroy(gameObject);
        }
    }
}