using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelSpawnManager : MonoBehaviour
{
    public GameObject barrelPrefab;
    public int nrOfBarrels = 20;
    public GameObject player;
    public float spawnRange = 10;
    public List<GameObject> barrels;
    public float destroyDistance = 10;
    
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        SpawnBarrels();
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < barrels.Count; i++)
        {
            if (Vector3.Distance(player.transform.position, barrels[i].transform.position) >
                destroyDistance)
            {
                Destroy(barrels[i].gameObject);
                barrels.RemoveAt(i);
                SpawnBarrel();
            }
        }
    }

    void SpawnBarrels()
    {
        for (int i = 0; i < nrOfBarrels; i++)
        {
            SpawnBarrel();
        }
    }

    void SpawnBarrel()
    {
        Quaternion barrelRotation = new Quaternion(Random.Range(0f, 360f),Random.Range(0f,
            360f),Random.Range(0f, 360f),Random.Range(0f, 360f));
        var barrel = Instantiate(barrelPrefab, player.transform.position +
                                               GenerateSpawnPos(), barrelRotation);
        barrel.GetComponent<Rigidbody>().AddForce(0, 80, 0, ForceMode.Impulse);
        barrels.Add(barrel);
    }
    
    private Vector3 GenerateSpawnPos()
    {
        float spawnPosX = Random.Range(-spawnRange, spawnRange);
        float spawnPosY = Random.Range(-spawnRange, spawnRange);
        
        Vector3 randomPos = new Vector3(x:spawnPosX, y:1, z:spawnPosY);
        return randomPos;
    }
}
