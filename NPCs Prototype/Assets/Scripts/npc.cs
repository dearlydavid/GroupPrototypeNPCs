using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class npc : MonoBehaviour
{
    public GameObject[] waypoints; // list of empty game objects to use to navigate to
    private NavMeshAgent myAgent; // the navMesh for the npc
    private int currentWaypoint; // index of the waypoint list to check where the npc is currently going to
    private bool talking; // used to check if the npc is currently talking
    public GameObject QuestText; // text box to show the npc's request
    public Animator animator; //animator for the dialogue boxes

    public bool isMover = false;

    public quest[] questList;
    private int questIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        //setting up navmesh agent and turning off box tracker by default
        myAgent = GetComponent<NavMeshAgent>();
        myAgent.destination = waypoints[currentWaypoint].transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // if the quest has been given but not completed
        if (!questList[questIndex].awarded && questList[questIndex].questActive)
        {
            // disable collected items and keep track of how many have been collected with check
            int check = 0;
            foreach (QuestItem item in questList[questIndex].itemList)
            {
                if (item.itemCollected)
                {
                    item.gameObject.SetActive(false);
                    check++;
                }
            }

            // if enough items from itemList have been collected, set it so that the quest can be turned in
            if (check >= questList[questIndex].numRequired)
            {
                questList[questIndex].checkStatus();
            }
        }

        // if not talking and at current waypoint, cycle to next waypoint
        if (Vector3.Distance(myAgent.destination, transform.position) <= 1 && !talking && isMover)
        {
            currentWaypoint++;
            if (currentWaypoint >= waypoints.Length)
            {
                currentWaypoint = 0;
            }

            if (isMover) { myAgent.destination = waypoints[currentWaypoint].transform.position; }
            
        }
    }

    //triggered when the playe is standing in the talking trigger box for this npc
    void OnTriggerStay(Collider other)
    {
        print("enter");
        if (other.tag == "Player")
        {
            // makes npc stand still
            if (isMover) { myAgent.destination = transform.position; }
            
            // checks that the npc has not already done these things
            if (!talking)
            {
                // play talking animation, marks npcs current state, and shows thier dialouge text
                GetComponentInChildren<Animator>().SetTrigger("Talk");
                questList[questIndex].checkStatus();
                QuestText.GetComponent<Text>().text = questList[questIndex].currentText;
                QuestText.SetActive(true);
                talking = true;
                popOpen();
            }

            if (questList[questIndex].awarded)
            {
                if (questList[questIndex].currentIdx == 1)
                {
                    questList[questIndex].giveReward();
                    QuestText.GetComponent<Text>().text = questList[questIndex].currentText;
                }

                if (Input.GetKey(KeyCode.Space))
                {
                    if (questIndex < questList.Length - 1)
                    {
                        questIndex++;
                    }
                    else
                    {
                        questList[questIndex].currentIdx = questList[questIndex].currentIdx + 1;
                    }
                    questList[questIndex].checkStatus();
                    QuestText.GetComponent<Text>().text = questList[questIndex].currentText;
                }
            }

            // turn npc to look at player
            transform.LookAt(other.transform);

        }
    }

    // once player has left the talking zone collider turn off text, change state, and end the talking animation/idle
    private void OnTriggerExit(Collider other)
    {
        if (talking)
        {
            GetComponentInChildren<Animator>().SetTrigger("StopTalking");
            talking = false;
            QuestText.SetActive(false);
            popDown();
        }
    }

    void popOpen()
    {
        animator.SetBool("isOpen", true);
    }

    void popDown()
    {
        animator.SetBool("isOpen", false);
    }
}
