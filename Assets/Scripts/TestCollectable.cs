using Unity.VisualScripting;
using UnityEngine;

public class TestCollectable : MonoBehaviour
{
    [SerializeField] GameObject gameManager;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            SaveSystem.saveData.collectables++;
            gameManager.GetComponent<SaveSystem>().SaveGame(SaveSystem.saveData);
            Debug.Log("you have found " + SaveSystem.saveData.collectables + " collectables so far");
            Destroy(this.gameObject);
        }
    }
}
