using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientsManager : MonoBehaviour
{

    public Transform clientsPositionsPanel,eastSpawnPos,westSpawnPos;
    public GameObject client;

    List<Transform> clientsPosesList = new List<Transform>();
    List<bool> clientsBoolAvailble = new List<bool>();

    void Start()
    {
        for (int i = 0; i < clientsPositionsPanel.transform.childCount; i++)
        {
            clientsPosesList.Add(clientsPositionsPanel.GetChild(i).transform);
            clientsBoolAvailble.Add(true);
        }
    }

    public void GenerateNewClient(GameObject cake,string cakeCode)
    {
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

    }



}
