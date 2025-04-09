using UnityEngine;

public class ChairInteraction : MonoBehaviour
{
    public Transform sitPoint; 

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                player.IsNearChair = true;
                player.currentChair = this;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                player.IsNearChair = false;
                player.currentChair = null;
            }
        }
    }
}