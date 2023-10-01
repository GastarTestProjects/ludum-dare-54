using System;
using UnityEngine;
using UnityEngine.AI;

namespace _Game.Scripts.Enemies
{
    public class EnemyBehaviour
    {
        public enum State
        {
            Idle,
            Chasing,
            Attacking,
            Dead
        }
        
        public State CurrentState { get; private set; }

        private float distanceToAttack = 2f;
        // private bool attacking;
        // private float attackDuration = 0.5f;
        // private float prevAttackTime;
        
        private NavMeshAgent enemyAgent;
        private Action _onAttack;

        public EnemyBehaviour(NavMeshAgent enemyAgent, Action onAttack)
        {
            _onAttack = onAttack;
            this.enemyAgent = enemyAgent;
        }

        public void SetState(State state)
        {
            CurrentState = state;
        }
        
        public void Tick(Vector3 destination)
        {
            switch (CurrentState)
            {
                case State.Idle:
                    break;
                case State.Chasing:
                    if (Vector3.Distance(destination, enemyAgent.transform.position) <= distanceToAttack)
                    {
                        SetState(State.Attacking);
                    }
                    else
                    {
                        enemyAgent.SetDestination(destination);
                    }
                    break;
                case State.Attacking:
                    if (Vector3.Distance(destination, enemyAgent.transform.position) > distanceToAttack)
                    {
                        SetState(State.Chasing);
                    }
                    else
                    {
                        StartAttack();
                    }
                    break;
                case State.Dead:
                    break;
            }
        }

        private void StartAttack()
        {
            _onAttack?.Invoke();
        }
    }
}