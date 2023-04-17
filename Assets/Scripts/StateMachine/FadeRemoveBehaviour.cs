using UnityEngine;

namespace StateMachine{
    public class FadeRemoveBehaviour : StateMachineBehaviour{
        public float fadeTime = 0.5f;
        public float fadeDelay;
        private float _timeElapsed;
        private float _fadeDelayElapsed;
        private SpriteRenderer _spriteRenderer;
        private GameObject _objToRemove;
        private Color _startColor;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex){
            _timeElapsed = 0f;
            _spriteRenderer = animator.GetComponent<SpriteRenderer>();
            _startColor = _spriteRenderer.color;
            _objToRemove = animator.gameObject;
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex){
            if (fadeDelay > _fadeDelayElapsed){
                _fadeDelayElapsed += Time.deltaTime;
            }
            else{
                _timeElapsed += Time.deltaTime;
                var newAlpha = _startColor.a * (1 - _timeElapsed / fadeTime);
                _spriteRenderer.color = new Color(_startColor.r, _startColor.g, _startColor.b, newAlpha);
                if (_timeElapsed > fadeTime){
                    Destroy(_objToRemove);
                }
            }
        }
    }
}