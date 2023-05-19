using Player;
using UnityEngine;

namespace Behaviour
{
    public class IdleBehaviour : StateMachineBehaviour
    {
        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            PlayerAttack.instance.OffEffect();
        }

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (PlayerAttack.instance.inputReceived)
            {
                animator.SetTrigger($"AttackOne");
                PlayerAttack.instance.InputManager();
                PlayerAttack.instance.OnEffect();
                PlayerAttack.instance.inputReceived = false;
            }
        }

       
    }
}
