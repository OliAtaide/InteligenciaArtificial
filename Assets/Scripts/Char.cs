using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class Char : MonoBehaviour {
    
    public NavMeshAgent agent;
    public Animator animator;
    public GameObject item;
    public Vector3 walkPoint;
    public bool walkPointSet, itemIsInRange;
    public float walkPointRange;
    public LayerMask groundMask;
    int itemCount;
    public TextMeshProUGUI countUI;

    private void Awake() {
        item = GameObject.FindWithTag("Item");
        agent = GetComponent<NavMeshAgent>();
        Debug.Log(agent.path);
        animator = GetComponent<Animator>();
    }

    private void Update() {
        if(itemIsInRange && item != null) GotoItem();
        else Walking();
    }

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Item"){
            item = other.gameObject;
            itemIsInRange = true;
        }
        else if(other.tag == "Doll"){
            itemIsInRange = false;
            SearchWalkPoint();
        }
    }

    private bool reachedItem(){
        bool x = transform.position.x - item.transform.position.x <= 0.5f;
        bool z = transform.position.z - item.transform.position.z <= 0.5f;
        return x && z;
    }

    private void GotoItem(){
        if(reachedItem()) GetItem();
        else {
            agent.SetDestination(item.transform.position);
            agent.speed = 3;
            animator.ResetTrigger("Run");
            animator.SetTrigger("Walk");
        }
    }

    private void GetItem(){
        Destroy(item);
        itemIsInRange = false;

        itemCount++;
        countUI.text = itemCount.ToString();

        animator.ResetTrigger("Walk");
        animator.SetTrigger("Run");
        agent.speed = 5;
    }

    private void Walking(){
        if(!walkPointSet) SearchWalkPoint();
        else {
            animator.ResetTrigger("Walk");
            animator.SetTrigger("Run");
            agent.SetDestination(walkPoint);
        }

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        if(distanceToWalkPoint.magnitude < 1.0f)
            walkPointSet = false;
    }

    private void SearchWalkPoint(){
        float randomX = Random.Range(-9f, 9f);
        float randomZ = Random.Range(-7, 7);

        walkPoint = new Vector3(randomX, transform.position.y, randomZ);
        
        bool isGrounded = Physics.Raycast(walkPoint, -transform.up, 2f, groundMask);
        

        if(isGrounded) walkPointSet = true;
    }

}