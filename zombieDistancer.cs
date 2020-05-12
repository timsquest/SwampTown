using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class zombieDistancer : MonoBehaviour
{
    public GameObject target;
    public float timeBetweenSpawn;
    public float spawnDistance;
    public int terrainLayer = 9;
    private Vector3 originalPosition;
    public List<GameObject> zombiesInput;
    private List<ZombieSpawn> zombies = new List<ZombieSpawn>();
    private bool zombieHordesActive = false;
    // Start is called before the first frame update
    void Awake()
    {
        foreach (GameObject newZ in zombiesInput)
        {
            zombies.Add(new ZombieSpawn(newZ));
        }
        foreach (ZombieSpawn z in zombies)
        {
            z.zombie.SetActive(false);
            if (z.container.CompareTag("ChaserZ"))
                z.container.transform.position = target.transform.position;
            NavMeshAgent navMesh = z.zombie.GetComponent<NavMeshAgent>();
            if (navMesh)
                navMesh.enabled = true;
            navMesh.Warp(new Vector3 (z.zombie.transform.position.x, z.zombie.transform.position.y + 17f, z.zombie.transform.position.z));
            z.originalPosition = z.zombie.transform.localPosition;
            navMesh.Warp(AdjustPositionToTerrain(z));
            z.zombie.SetActive(true);
            StartCoroutine("Spawner", z);
            z.zombie.SetActive(false);
        }
        QuestManager.startEvent -= ActivateZombieHordesTemporary;
        QuestManager.startEvent += ActivateZombieHordesTemporary;
        QuestManager.stopEvent -= DeActivateZombieHordes;
        QuestManager.stopEvent += DeActivateZombieHordes;
        QuestManager.disableCollider -= ActivateZombieHordesTemporary;
        QuestManager.disableCollider += ActivateZombieHordesTemporary;
        QuestManager.enableAction -= DeActivateZombieHordes;
        QuestManager.enableAction += DeActivateZombieHordes;
        QuestManager.finalFight -= ActivateZombieHordes;
        QuestManager.finalFight += ActivateZombieHordes;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public Vector3 spherePos = new Vector3(100f,100f,100f);
    public float sphereRad = 5f;

    Vector3 AdjustPositionToTerrain(ZombieSpawn currentZombie)
    {
        Vector3 outVector = new Vector3 (0,-100,0);
        RaycastHit hit;
        Ray ray = new Ray (currentZombie.originalPosition + currentZombie.container.transform.position, -currentZombie.zombie.transform.up);
        int layerMask = 1 << terrainLayer;
        
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask)) {        
            if (hit.collider != null) {
                outVector = hit.point;
            }
        }
        return outVector;
    }

    IEnumerator Spawner(ZombieSpawn z) 
    {
        for (;;)
        {
            if (Vector3.Distance(z.zombie.transform.position, target.transform.position) > spawnDistance && z.container.CompareTag("ChaserZ"))
            {
                yield return new WaitForSeconds(timeBetweenSpawn);
            }
            if (Vector3.Distance(z.zombie.transform.position, target.transform.position) > spawnDistance)
            {
                z.zombie.SetActive(false);
                if (z.container.CompareTag("ChaserZ"))
                {
                    yield return new WaitForSeconds(timeBetweenSpawn);
                    z.container.transform.position = target.transform.position;
                    NavMeshAgent navMesh = z.zombie.GetComponent<NavMeshAgent>();
                    if (navMesh)
                    {
                        navMesh.enabled = true;
                        z.zombie.transform.localPosition = z.originalPosition;
                        navMesh.Warp(AdjustPositionToTerrain(z));
                    }
                    else
                    {
                        z.zombie.SetActive(false);
                    }
                    if (zombieHordesActive)
                        z.zombie.SetActive(true);
                }
            }
            else
            {
                if (zombieHordesActive || z.container.CompareTag("StationZ"))
                    z.zombie.SetActive(true);
            }
            yield return null;
        }
    }

     void ActivateZombieHordes()
    {
        zombieHordesActive = true;
    }

    void ActivateZombieHordesTemporary()
    {
        zombieHordesActive = true;
        StartCoroutine(TimedDeActivateHorde());
    }

    void DeActivateZombieHordes()
    {
        zombieHordesActive = false;
    }

    IEnumerator TimedDeActivateHorde()
    {
        yield return new WaitForSeconds(120f);
        DeActivateZombieHordes();
        yield return null;
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
        QuestManager.startEvent -= ActivateZombieHordes;
        QuestManager.stopEvent -= DeActivateZombieHordes;
        QuestManager.disableCollider -= ActivateZombieHordes;
        QuestManager.enableAction -= DeActivateZombieHordes;
        QuestManager.finalFight -= ActivateZombieHordes;
    }
}



public class ZombieSpawn
{
    public GameObject zombie;
    public Vector3 originalPosition;
    public GameObject container;

    public ZombieSpawn(GameObject newZombieContainer)
    {
        container = newZombieContainer;
        zombie = container.transform.GetChild(0).gameObject;
        originalPosition = zombie.transform.position;
    }
}