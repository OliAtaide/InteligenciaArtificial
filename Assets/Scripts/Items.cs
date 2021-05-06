using UnityEngine;

public class Items : MonoBehaviour
{
    public GameObject item;
    private void Update()
    {
        if (GameObject.FindGameObjectsWithTag("Item").Length < 6)
        {
            float randomX = Random.Range(-8f, 8f);
            float randomZ = Random.Range(-6, 6);

            Vector3 position = new Vector3(randomX, 0.2f, randomZ);

            Instantiate(item, position, item.transform.rotation);
        }
    }
}