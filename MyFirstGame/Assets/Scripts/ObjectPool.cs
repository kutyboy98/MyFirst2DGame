using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField]
    private GameObject[] MonsterPrefabs;
    private List<GameObject> objects = new List<GameObject>();
    public GameObject GetObject<T>(T type)
    {
        foreach (var obj in objects)
        {
            if (obj.name == type.ToString() && !obj.activeInHierarchy)
            {
                if (obj.tag.Equals("Crime")) obj.GetComponent<SpriteRenderer>().sortingOrder = 1;
                obj.SetActive(true);
                return obj;
            }
        }
        foreach (var monsterPrefab in MonsterPrefabs)
        {
            if (monsterPrefab.name == type.ToString())
            {
                var obj = Instantiate(monsterPrefab);
                obj.name = type.ToString();
                objects.Add(obj);
                return obj;
            }
        }
        return null;
    }    

    public void ReleaseObject(GameObject gameObject)
    {
        gameObject.SetActive(false);        
    }
}
