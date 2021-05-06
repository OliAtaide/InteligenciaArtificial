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
        bool x = item.transform.position.x == transform.position.x;
        bool z = item.transform.position.z == transform.position.z;
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
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if(Physics.Raycast(walkPoint, -transform.up, 2f, groundMask))
            walkPointSet = true;
    }

}