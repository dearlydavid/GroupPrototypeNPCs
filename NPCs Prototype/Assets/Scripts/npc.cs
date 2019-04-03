using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class npc : MonoBehaviour
{
    public GameObject[] waypoints;
    private NavMeshAgent myAgent;
    private int currentWaypoint;
    private bool talking;
    public GameObject QuestText;
    private bool questItemFound;
    public QuestItem questItem;
    public bool questActive;

    // Start is called before the first frame update
    void Start()
    {
        myAgent = GetComponent<NavMeshAgent>();
        myAgent.destination = waypoints[currentWaypoint].transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (!questItemFound && questItem.itemCollected)
        {
            questItemFound = true;
            Destroy(questItem.gameObject);
        }
        if (Vector3.Distance(myAgent.destination, transform.position) <= 1 && !talking)
        {
            currentWaypoint++;
            if(currentWaypoint >= waypoints.Length)
            {
                currentWaypoint = 0;
            }
            myAgent.destination = waypoints[currentWaypoint].transform.position;
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            myAgent.destination = transform.position;
            if (!talking)
            {
                if (questItemFound)
                {
                    QuestText.GetComponent<Text>().text = "Thanks, thats just what I wanted!";
                }
                questActive = true;
                GetComponentInChildren<Animator>().SetTrigger("Talk");
                talking = true;
                QuestText.SetActive(true);
            }
            transform.LookAt(other.transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (talking)
        {
            GetComponentInChildren<Animator>().SetTrigger("StopTalking");
            talking = false;
            QuestText.SetActive(false);
        }
    }


}
