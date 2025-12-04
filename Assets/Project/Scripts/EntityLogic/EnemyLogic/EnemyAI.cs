using Project.Scripts.PlayerLogic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [Header("Navigation")]
    private Transform player;
    private NavMeshAgent agent;

    [Header("Combat")] 
    [SerializeField] private float speed;
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float attackCooldown = 1f;
    [SerializeField] private int damage = 10;
    [SerializeField] private ParticleSystem hit;
    
    [Header("Detection")]
    [SerializeField] private float detectionRange = 15f;
    
    private float lastAttackTime;
    private bool isAttacking = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        
        agent.speed = speed;
        agent.stoppingDistance = attackRange - 0.2f;
    }
    public void SetPlayer(Transform _player)
    {
        player = _player;
    }
    public void GetPoints(int value)
    {
        player.transform.GetComponent<PlayerComponent>().IncreasePoints(value);
    }

    void Update()
    {
        if (player == null) return;
        
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        
        if (distanceToPlayer <= detectionRange)
        {
            ChasePlayer();
            
            if (distanceToPlayer <= attackRange && CanAttack())
            {
                Attack();
            }
        }
        else
        {
            agent.isStopped = true;
        }
    }
    
    void ChasePlayer()
    {
        agent.isStopped = false;
        agent.SetDestination(player.position);
    }
    
    void Attack()
    {
        isAttacking = true;
        lastAttackTime = Time.time;
        
        agent.isStopped = true;
        
        PlayerComponent playerHealth = player.GetComponent<PlayerComponent>();
        if (playerHealth != null)
        {
            hit.gameObject.SetActive(true);
            hit.Play();
            playerHealth.DecreaseHp(damage);
        }
        
        Invoke(nameof(ResumeMovement), 0.5f);
    }
    
    void ResumeMovement()
    {
        isAttacking = false;
        agent.isStopped = false;
    }
    
    bool CanAttack()
    {
        return !isAttacking && Time.time >= lastAttackTime + attackCooldown;
    }
}