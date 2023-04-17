using System.Reflection;
using Actions;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIManager : MonoBehaviour{
    public GameObject damageTextPrefab;
    public GameObject healthTextPrefab;
    public Canvas gameCanvas;

    private void Awake(){
        gameCanvas = FindObjectOfType<Canvas>();
    }

    private void OnEnable(){
        CharacterActions.Damaged += CharacterTookDamage;
        CharacterActions.Healed += CharacterHealed;
    }

    private void OnDisable(){
        CharacterActions.Damaged -= CharacterTookDamage;
        CharacterActions.Healed -= CharacterHealed;
    }

    private void CharacterTookDamage(GameObject character, int damageReceived){
        if (Camera.main == null) return;
        var spawnPosition = Camera.main.WorldToScreenPoint(character.transform.position);
        var tmpText = Instantiate(damageTextPrefab, spawnPosition, Quaternion.identity, gameCanvas.transform)
            .GetComponent<TMP_Text>();
        tmpText.text = damageReceived.ToString();
    }

    private void CharacterHealed(GameObject character, int healthRestored){
        if (Camera.main == null) return;
        var spawnPosition = Camera.main.WorldToScreenPoint(character.transform.position);
        var tmpText = Instantiate(healthTextPrefab, spawnPosition, Quaternion.identity, gameCanvas.transform)
            .GetComponent<TMP_Text>();
        tmpText.text = healthRestored.ToString();
    }

    public void OnExit(InputAction.CallbackContext context){
        if (!context.started) return;
        // @formatter:off
        #if(UNITY_EDITOR || DEVELOPMENT_BUILD)
            Debug.Log(name + " : " + GetType() + " : " +MethodBase.GetCurrentMethod()?.Name);
        #endif
        #if(UNITY_EDITOR)
            EditorApplication.isPlaying = false;
        #elif(UNITY_STANDALONE)
            Application.Quit();
        #elif(UNITY_WEBGL)
            SceneManager.LoadScene("QuitScene")
        #endif
    }
}