
using System.Collections;
using System.Linq;
using PlannedRout.GameScoreManagment;
using PlannedRout.LevelManagment;
using UnityEngine;

namespace PlannedRout.LevelObjects.Characters
{
    public sealed class OnStartStatesAssigner : MonoBehaviour
    {
        [SerializeField] private string EnemyTag_Red;
        [SerializeField] private string EnemyTag_Blue;
        [SerializeField] private string EnemyTag_Pink;
        [SerializeField] private string EnemyTag_Orange;

        private EnemyBehaviour[] Enemies;
        private int FreeEnemiesCount = 1;

        private Coroutine EnemyRealizationCoroutine;

        private void Awake()
        {
            LevelManager.LevelInitializedEvent += ReferredInitialization;
        }
        private void ReferredInitialization()
        {
            LevelManager.LevelInitializedEvent -= ReferredInitialization;
            Enemies = new EnemyBehaviour[4]
            {
                GameObject.FindGameObjectWithTag(EnemyTag_Red).GetComponent<EnemyBehaviour>(),
                GameObject.FindGameObjectWithTag(EnemyTag_Blue).GetComponent<EnemyBehaviour>(),
                GameObject.FindGameObjectWithTag(EnemyTag_Pink).GetComponent<EnemyBehaviour>(),
                GameObject.FindGameObjectWithTag(EnemyTag_Orange).GetComponent<EnemyBehaviour>()
            };
            StartCoroutine(ReferredStart());
        }
        private IEnumerator ReferredStart()
        {
            yield return new WaitForEndOfFrame();
            ActivateEnemiesRealization();
            ProgressManager.PointCollectedEvent += PointCollected;
            EnemyRealizationCoroutine = StartCoroutine(ReleaseNextEnemy());
        }
        private void ActivateEnemiesRealization()
        {
            Enemies[0].SelectBehaviourState(EnemyBehaviour.BehaviourStateType.Dispersion);
            Enemies[1].SelectBehaviourState(EnemyBehaviour.BehaviourStateType.Idle);
            Enemies[2].SelectBehaviourState(EnemyBehaviour.BehaviourStateType.Idle);
            Enemies[3].SelectBehaviourState(EnemyBehaviour.BehaviourStateType.Idle);
            Enemies[1].ChangeBehaviourStateEvent += EnemyReleased;
        }

        private void EnemyReleased(EnemyBehaviour.BehaviourStateType i)
        {
            Enemies[FreeEnemiesCount].ChangeBehaviourStateEvent -= EnemyReleased;
            FreeEnemiesCount++;
            if (FreeEnemiesCount >= 4)
            {
                StopRealization();
            }
            else
            {
                Enemies[FreeEnemiesCount].ChangeBehaviourStateEvent += EnemyReleased;
            }
        }
        private void UpdateWaiter()
        {
            if(EnemyRealizationCoroutine!=null)
                StopCoroutine(EnemyRealizationCoroutine);
            EnemyRealizationCoroutine = StartCoroutine(ReleaseNextEnemy());
        }
        private void PointCollected(int count) =>
            UpdateWaiter();
        private IEnumerator ReleaseNextEnemy()
        {
            while (true)
            {
                yield return new WaitForSeconds(LevelManager.Instance_.GlobalConsts_.PlayerAFKTime);
                Enemies[FreeEnemiesCount].SelectBehaviourState(EnemyBehaviour.BehaviourStateType.Dispersion);
            }
        }
        private void StopRealization()
        {
            StopCoroutine(EnemyRealizationCoroutine);
            ProgressManager.PointCollectedEvent -= PointCollected;
        }
    }
}
