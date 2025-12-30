using UnityEngine;

public class AttackStateController : StateMachineBehaviour
{
    // 해당 애니메이션 상태에 들어갈 때 실행
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // 공격 시작 시 bool 값을 true로 세팅
        animator.SetBool("IsAttacking", true);
    }

    // 해당 애니메이션 상태에서 나갈 때 실행 (중요!)
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // 공격 종료 시 bool 값을 false로 세팅
        animator.SetBool("IsAttacking", false);

        // ★ 핵심: 공격 중에 쌓여있던 'hit' 트리거를 여기서 제거합니다.
        // 이렇게 하면 공격이 끝나자마자 억지로 맞지 않습니다.
        animator.ResetTrigger("GetHit");
    }
}