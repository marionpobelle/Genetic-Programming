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
    public int teamIndex;
    AgentData data;
    NavMeshAgent navMesh;

    float currentHP;
    public bool isAlive;
    Agent target;

    public float TotalDamageInflicted = 0.0f;
    public int KillAmount = 0;
    public float LifeTime = 0.0f;

    public static event Action OnHit;
    public static event Action OnDeath;
    [SerializeField] float overlapingSphereRadius;


    /// <summary>
    /// Called when the agent is instantiated.
    /// </summary>
    public void SetupAgent(int teamIndex, Material teamMaterial)
    {
        Debug.LogWarning("TODO : SET TEAM INDEX");
        agentRenderer.material = teamMaterial;
    }

    /// <summary>
    /// Called when the fight starts.
    /// </summary>
    public void InitAgent(Vector3 initialPosition)
    {
        transform.position = initialPosition;
    }

    public float ComputeScore()
    {
        Debug.Log("TODO : COMPUTE SCORE");

        //function should be something like :
        // return kills * killscoreMultiplier + percentageAliveInGame * aliveScoreMultiplier + totalInflictedDamage * damageScoreMultiplier;

        return 0;
    }

    /// <summary>
    /// Sets up the agent stats.
    /// </summary>
    private void Awake()
    {
        data = new AgentData();
    }

    /// <summary>
    /// Called each frame.
    /// </summary>
    private void Update()
    {
        
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
    /// <param name="allActiveAgents"></param>
    //TODO: List of all active agents for distance comparison if nobody is in the radius
    Agent GetClosestEnemyAgent(Agent[] allActiveAgents)
    {
        Collider[] allObjectsOverlapingWithAgent = Physics.OverlapSphere(transform.position, overlapingSphereRadius);
        Agent closestEnemy = null;
        //Arbitrary large value
        float currentMinDist = 1000;
        //If we found objects in 
        if(allObjectsOverlapingWithAgent.Length != 0)
        {
            foreach (Collider overlapingCollider in allObjectsOverlapingWithAgent)
            {
                if (overlapingCollider.gameObject.GetComponent<Agent>() == null)
                {
                    continue;
                }
                else
                {
                    if (overlapingCollider.gameObject.GetComponent<Agent>().teamIndex == teamIndex)
                    {
                        continue;
                    }
                    else
                    {
                        float distanceToCurrentEnemy = Distance(this.gameObject, overlapingCollider.gameObject);
                        if (distanceToCurrentEnemy < currentMinDist)
                        {
                            currentMinDist = distanceToCurrentEnemy;
                            closestEnemy = overlapingCollider.gameObject.GetComponent<Agent>();
                        }
                    }
                }

            }
        }
        //If there were only obstacles or allies in the radius || radius did not find any object
        if( closestEnemy == null ) 
        {
            foreach(Agent agent in allActiveAgents)
            {
                if (agent.teamIndex == teamIndex)
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
        return closestEnemy;
    }

    /// <summary>
    /// Computes the euclidian distance between two game objects.
    /// </summary>
    /// <param name="a"></param>
    /// /// <param name="b"></param>
    float Distance(GameObject a, GameObject b)
    {
        return (a.transform.position - b.transform.position).magnitude;
    }
}
