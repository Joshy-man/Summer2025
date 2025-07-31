using UnityEngine;
public class ObjectSpawner : MonoBehaviour
{
public enum ObjectType { Enemy }
    public GameObject[] objectPrefabs;

// Start is called once before the first execution of Update after the MonoBehaviour is created
void Start()

 {
    GameController.OnReset += LevelChange;
 }
 

 
private void LevelChange()
    {
        DestroyAllSpawnedObjects();
    }
private void DestroyAllSpawnedObjects()

 {
 }
}