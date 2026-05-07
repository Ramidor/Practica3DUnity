using UnityEngine;

public class ZombieAttackState : StateMachineBehaviour
{
    Enemy enemy;

    [Header("Cerebro Difuso: Rabia")]
    [Tooltip("Eje X: Vida del Zombie (Ej: 100 a 0). Eje Y: Velocidad de Animación (Ej: 1 a 3).")]
    public AnimationCurve velocidadAtaqueFuzzy;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Conectamos con el script Enemy de tu zombie para poder leer su vida
        enemy = animator.GetComponent<Enemy>();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (enemy != null)
        {
            float vidaDelZombie = enemy.HP;
            if (vidaDelZombie <= 0)
            {
                animator.speed = 1f;
                return;
            }

            float multiplicadorRabia = velocidadAtaqueFuzzy.Evaluate(vidaDelZombie);
            multiplicadorRabia = Mathf.Clamp(multiplicadorRabia, 0.5f, 5f);
            animator.speed = multiplicadorRabia;

            // --- LA SOLUCIÓN AL BUG ---
            // "stateInfo.normalizedTime" va de 0 (empieza la animación) a 1 (termina la animación)
            // Si la animación ya va por el 95% o más, apagamos el ataque para que vuelva a perseguir
            if (stateInfo.normalizedTime >= 0.95f)
            {
                animator.SetBool("isAttacking", false);
            }
        }
    }
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // IMPORTANTÍSIMO: Cuando el zombie deje de atacar o muera, devolvemos la velocidad del Animator a 1
        // para que no corra la animación de morir a cámara rápida.
        animator.speed = 1f;
    }
}