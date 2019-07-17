using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientsManager : MonoBehaviour
{

    public Transform clientsPositionsPanel,eastSpawnPos,westSpawnPos;
    public GameObject client;

    public List<Client> clientsList = new List<Client>();

    public CashierManager cashMan;

    public float patienceTime = 60;

    List<Transform> clientsPosesList = new List<Transform>();
    List<bool> clientsBoolAvailble = new List<bool>();


    void Start()
    {
        if (MainMenuManager.isPlayerConnected) patienceTime = 300;
        for (int i = 0; i < clientsPositionsPanel.transform.childCount; i++)
        {
            clientsPosesList.Add(clientsPositionsPanel.GetChild(i).transform);
            clientsBoolAvailble.Add(true);
        }
    }

    public void GenerateNewClient(GameObject cake,string cakeCode)
    {
        bool isThereAnyPlaceAvailble = false;
        foreach (bool availblePlaces in clientsBoolAvailble)
        {
            if (availblePlaces){
                isThereAnyPlaceAvailble = true;
                break;
            }
        }
        if (!isThereAnyPlaceAvailble) return;

        int randClientPosIndex = 0 ;
        GameObject newClient = 
            Instantiate (client,(Random.Range(0,2)%2 ==0 )? eastSpawnPos.position : westSpawnPos.position,Quaternion.identity,transform);

        do
        {
            randClientPosIndex = Random.Range(0, clientsPosesList.Count);
        } while (!clientsBoolAvailble[randClientPosIndex]);

        clientsBoolAvailble[randClientPosIndex] = false;

        Client newClientScript = newClient.GetComponent<Client>();

        newClientScript.SetClientsValue(clientsPosesList[randClientPosIndex].position);
        newClientScript.GenerateCakeInBull(cake, cakeCode);
        newClientScript.WalkToPoint(clientsPosesList[randClientPosIndex].position);
        newClientScript.clientMan = this;
        clientsList.Add(newClientScript);

    }

    public void InvokeCards(string bulleCakeCode)
    {
        cashMan.cakeCode = bulleCakeCode;
        cashMan.cardMan.ShowCadPanel();
        cashMan.cardMan.GenerateRandomCards();
    }


}
