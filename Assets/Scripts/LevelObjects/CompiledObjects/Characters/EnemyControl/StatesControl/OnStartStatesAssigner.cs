
using System.Collections;
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
        private WaitForSeconds CoroutineWaiter;

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
            Enemies[0].SelectBehaviourState(EnemyBehaviour.BehaviourStateType.Dispersion);
            Enemies[1].SelectBehaviourState(EnemyBehaviour.BehaviourStateType.Idle);
            Enemies[2].SelectBehaviourState(EnemyBehaviour.BehaviourStateType.Idle);
            Enemies[3].SelectBehaviourState(EnemyBehaviour.BehaviourStateType.Idle);
            Enemies[1].ChangeBehaviourStateEvent += EnemyReleased;
            ProgressManager.PointCollectedEvent += PointCollected;
            UpdateWaiter();
            EnemyRealizationCoroutine = StartCoroutine(ReleaseNextEnemy());
        }

        private void EnemyReleased(EnemyBehaviour.BehaviourStateType i)
        {
            Enemies[FreeEnemiesCount].ChangeBehaviourStateEvent -= EnemyReleased;
            FreeEnemiesCount++;
            if (FreeEnemiesCount >= 4)
            {
                StopCoroutine(EnemyRealizationCoroutine);
            }
            else
            {
                Enemies[FreeEnemiesCount].ChangeBehaviourStateEvent += EnemyReleased;
            }
        }
        private void UpdateWaiter()
        {
            CoroutineWaiter = new WaitForSeconds(LevelManager.Instance_.GlobalConsts_.PlayerAFKTime);
        }
        private void PointCollected(int count) =>
            UpdateWaiter();
        private IEnumerator ReleaseNextEnemy()
        {
            while (true)
            {
                yield return CoroutineWaiter;
                Enemies[FreeEnemiesCount].SelectBehaviourState(EnemyBehaviour.BehaviourStateType.Dispersion);
                UpdateWaiter();
            }
        }
    }
}
