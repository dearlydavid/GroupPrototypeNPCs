using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class quest : MonoBehaviour
{
    //[HideInInspector]
    public bool questActive;
    //[HideInInspector]
    public bool awarded;

    [Tooltip("Needs to be either 3 or 4 at this point; 3 if another quest follows, 4 if there is no following quest")]
    [TextArea(3,10)]
    public string[] dialogue;

    [HideInInspector]
    public int currentIdx = 0;

    [Tooltip("List of possible items that can complete the quest")]
    public QuestItem[] itemList;
    [Tooltip("Should be between 1 and the length of the Item List")]
    public int numRequired;

    [HideInInspector]
    public string currentText;

    public QuestItem reward;

    void Start()
    {
        currentText = dialogue[currentIdx];
        reward.gameObject.SetActive(false);
    }

    public void checkStatus()
    {
        if (!questActive)
        {
            questActive = true;
        }
        else if (questActive && !awarded)
        {
            currentIdx = 1;
            int check = 0;

            foreach (QuestItem item in itemList)
            {
                if (item.itemCollected)
                {
                    item.gameObject.SetActive(false);
                    GetComponent<AudioSource>().Play();
                    check++;
                }
            }

            if (check >= numRequired)
            {
                awarded = true;
            }
        }
        setText();
    }

    public void giveReward()
    {
        if(reward != null)
        {

            foreach (QuestItem item in itemList)
            {
                if (!item.good)
                {
                    reward.good = false;
                }
            }


            if(reward.good = false)
            {
                currentIdx = 3;
            }
            else
            {
                currentIdx = 2;
            }

            reward.gameObject.SetActive(true);
            reward.gameObject.GetComponent<Transform>().SetPositionAndRotation(GameObject.FindGameObjectWithTag("Player").transform.position, Quaternion.identity);
        }
        setText();
    }

    public void setText()
    {
        currentText = dialogue[currentIdx];
    }
}
