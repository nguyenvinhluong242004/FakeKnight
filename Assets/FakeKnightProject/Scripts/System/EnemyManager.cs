using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] public static EnemyManager instance;
    [SerializeField] private GameObject teleport;
    public List<GameObject> pooledSlimes;
    public List<GameObject> pooledDevils;
    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    void Start()
    {
        pooledSlimes = new List<GameObject>();
        pooledDevils = new List<GameObject>();
    }

    public void initEnemy(int _id)
    {
        if (_id == 0)
        {
            for (int i = 0; i < pooledSlimes.Count; i++)
            {
                GameObject obj = pooledSlimes[i];
                if (obj != null && !obj.activeInHierarchy)
                {
                    obj.transform.position = teleport.transform.position;
                    obj.SetActive(true);
                    pooledSlimes.RemoveAt(i);
                    return;
                }
            }
        }
        else if (_id == 1)
        {
            for (int i = 0; i < pooledDevils.Count; i++)
            {
                GameObject obj = pooledDevils[i];
                if (obj != null && !obj.activeInHierarchy)
                {
                    obj.transform.position = teleport.transform.position;
                    obj.SetActive(true);
                    pooledDevils.RemoveAt(i);
                    return;
                }
            }
        }
    }
    IEnumerator InvokeWithDelayInit(float delay, int _id)
    {
        yield return new WaitForSeconds(delay);
        initEnemy(_id);
    }

    public void addPooled(GameObject obj)
    {
        int _id = obj.GetComponent<EnemyLevel1>().id;
        if (_id == 0)
        {
            pooledSlimes.Add(obj);
        }
        else if (_id == 1)
        {
            pooledDevils.Add(obj);
        }
        StartCoroutine(InvokeWithDelayInit(2f, _id));
    }
}
