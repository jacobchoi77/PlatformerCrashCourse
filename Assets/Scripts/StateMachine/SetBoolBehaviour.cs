using UnityEngine;

namespace StateMachine{
    public class SetBoolBehaviour : StateMachineBehaviour{
        public string boolName;
        public bool updateOnState;
        public bool updateOnStateMachine;
        public bool valueOnEnter, valueOnExit;

        // OnStateEnter is called before OnStateEnter is called on any state inside this state machine
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex){
            if (updateOnState){
                animator.SetBool(boolName, valueOnEnter);
            }
        }

        // OnStateUpdate is called before OnStateUpdate is called on any state inside this state machine
        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex){
            if (updateOnState){
                animator.SetBool(boolName, valueOnExit);
            }
        }

        // OnStateExit is called before OnStateExit is called on any state inside this state machine
        //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    
        //}

        // OnStateMove is called before OnStateMove is called on any state inside this state machine
        //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    
        //}

        // OnStateIK is called before OnStateIK is called on any state inside this state machine
        //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    
        //}

        // OnStateMachineEnter is called when entering a state machine via its Entry Node
        public override void OnStateMachineEnter(Animator animator, int stateMachinePathHash){
            if (updateOnStateMachine)
                animator.SetBool(boolName, valueOnEnter);
        }

        // OnStateMachineExit is called when exiting a state machine via its Exit Node
        public override void OnStateMachineExit(Animator animator, int stateMachinePathHash){
            if (updateOnStateMachine)
                animator.SetBool(boolName, valueOnExit);
        }
    }
}