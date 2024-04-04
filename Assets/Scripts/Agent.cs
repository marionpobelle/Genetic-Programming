using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class Agent : MonoBehaviour
{
    [Header("Score Settings")]
    [SerializeField] float killScoreMultiplier = 100;
    [SerializeField] float damageScoreMultiplier = 1;
    [SerializeField] float aliveScoreMultiplier = 1;
    [SerializeField] Renderer agentRenderer;
    [SerializeField] NavMeshAgent navMeshAgent;
    [SerializeField] float overlapingSphereRadius;
    public int teamIndex;
    public AgentData Data;

    float currentHP;
    public bool IsAlive = true;
    Agent target;

    public float TotalDamageInflicted = 0.0f;
    public int KillAmount = 0;

    float nextAllowedAttackTime = 0;
    float deathTime = -1;

    public static event Action OnHit;
    public static event Action OnDeath;


    /// <summary>
    /// Called when the agent is instantiated.
    /// </summary>
    public void SetupAgent(int teamIndex, Material teamMaterial, AgentData newData)
    {
        this.teamIndex = teamIndex;
        agentRenderer.material = teamMaterial;
        Data = newData;
    }

    /// <summary>
    /// Called when the fight starts.
    /// </summary>
    public void InitAgent(Vector3 initialPosition)
    {
        IsAlive = true;
        gameObject.SetActive(true);
        TotalDamageInflicted = 0;
        KillAmount = 0;
        deathTime = -1;
        navMeshAgent.enabled = false;
        initialPosition.x = (teamIndex == 0 || teamIndex == 2) ? Mathf.Abs(initialPosition.x) : -Mathf.Abs(initialPosition.x);
        //initialPosition.z = (teamIndex == 0 || teamIndex == 1) ? Mathf.Abs(initialPosition.z) : -Mathf.Abs(initialPosition.z);
        transform.position = initialPosition;
        navMeshAgent.enabled = true;
        currentHP = Data.MaxHP;
    }

    public void UpdateData(AgentData newData, float buildFitness)
    {
        Data = new AgentData(newData, buildFitness);
        navMeshAgent.speed = Data.Speed;
    }

    public float ComputeScore()
    {
        return //KillAmount * killScoreMultiplier
            + ((deathTime < 0) ? 100 : CombatManager.Instance.ComputeDeathLifetimePercentage(deathTime)) * aliveScoreMultiplier
            + TotalDamageInflicted * damageScoreMultiplier;
    }

    private void Update()
    {
        if (!CombatManager.Instance.IsFightRunning)
            return;

        if (target == null || !target.IsAlive)
        {
            target = GetClosestEnemyAgent();

            if (target == null)
                return;
        }

        if (IsTargetInAttackRange(target))
        {
            navMeshAgent.SetDestination(transform.position);
            AttackBehaviour();
        }
        else
        {
            //switch target if another one is closer
            Agent newPotentialTarget = GetClosestEnemyAgent();

            if (newPotentialTarget != null)
            {
                target = newPotentialTarget;
            }

            navMeshAgent.SetDestination(target.transform.position);
        }
    }

    void AttackBehaviour()
    {
        //TODO KILL CALLBACK

        if (Time.time > nextAllowedAttackTime)
        {
            target.OnAttacked(this);
            nextAllowedAttackTime = Time.time + .05f;
        }
    }

    bool DodgeAttack(float precision, float evasiveness)
    {
        float random = UnityEngine.Random.Range(0f, 1f);

        float dodgeChance = (Mathf.Sqrt(evasiveness) / (Mathf.Sqrt(evasiveness) * 1.4f * Mathf.Sqrt(precision))) * 1.25f;

        return random < dodgeChance;
    }

    private void OnAttacked(Agent attackingAgent)
    {
        if (DodgeAttack(attackingAgent.Data.Precision, Data.Evasiveness))
        {
            //  Debug.Log("Dodged the attack !");
            return;
        }
        //BUG: Draw event stuff
        OnHit?.Invoke();

        float inflictedDamage = 0;

        inflictedDamage = Mathf.Max(1, attackingAgent.Data.Attack / (Data.Defense + 1));

        //Debug.Log($"Agent got hit for {inflictedDamage}", this);

        attackingAgent.TotalDamageInflicted += inflictedDamage;

        currentHP -= inflictedDamage;

        if (currentHP <= 0)
        {
            attackingAgent.KillAmount++;
            IsAlive = false;
            OnDeath?.Invoke();
            gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Assign an enemy agent as a current target for this agent.
    /// </summary>
    /// <param name="targetToAssign"></param>
    void AssignTarget(Agent targetToAssign)
    {
        target = targetToAssign;
    }

    /// <summary>
    /// Get the closest enemy agent to this agent.
    /// </summary>
    Agent GetClosestEnemyAgent()
    {
        Collider[] allObjectsOverlapingWithAgent = Physics.OverlapSphere(transform.position, overlapingSphereRadius);
        Agent closestEnemy = null;
        //Arbitrary large value
        float currentMinDist = float.MaxValue;

        //If we found objects in 
        foreach (Collider overlapingCollider in allObjectsOverlapingWithAgent)
        {
            Agent agent = overlapingCollider.GetComponent<Agent>();

            if (agent == null)
            {
                continue;
            }
            else
            {
                if (agent.teamIndex == teamIndex)
                {
                    continue;
                }
                else
                {
                    float distanceToCurrentEnemy = Distance(this.gameObject, overlapingCollider.gameObject);
                    if (distanceToCurrentEnemy < currentMinDist)
                    {
                        currentMinDist = distanceToCurrentEnemy;
                        closestEnemy = agent;
                    }
                }
            }
        }

        //If there were only obstacles or allies in the radius || radius did not find any object
        //List < List < Agent >> teams = new List<List<Agent>>();
        if (closestEnemy == null)
        {
            for (int i = 0; i < CombatManager.Instance.teams.Count; i++)
            {
                if (i == teamIndex) continue;
                else
                {
                    foreach (Agent agent in CombatManager.Instance.teams[i])
                    {
                        if (agent.IsAlive == false)
                        {
                            continue;
                        }
                        else
                        {
                            float distanceToCurrentEnemy = Distance(this.gameObject, agent.gameObject);
                            if (distanceToCurrentEnemy < currentMinDist)
                            {
                                currentMinDist = distanceToCurrentEnemy;
                                closestEnemy = agent;
                            }
                        }
                    }
                }
            }

        }
        return closestEnemy;
    }

    /// <summary>
    /// Computes the euclidian distance between two game objects.
    /// </summary>
    /// <param name="a"></param>
    /// /// <param name="b"></param>
    float Distance(GameObject a, GameObject b)
    {
        return (a.transform.position - b.transform.position).sqrMagnitude;
    }

    bool IsTargetInAttackRange(Agent target)
    {
        return (target.transform.position - transform.position).magnitude <= Data.AttackDistance;
    }
}
