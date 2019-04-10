using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestItem : MonoBehaviour
{
    [HideInInspector]
    public bool itemCollected;
    public quest questGiver;

    public bool good;

    // if the item is collided with, the quest is active, and the playe has not already turned in the quest set the quest item to be collected
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && questGiver.questActive && !questGiver.awarded)
        {
            itemCollected = true;
        }
        
    }
}
