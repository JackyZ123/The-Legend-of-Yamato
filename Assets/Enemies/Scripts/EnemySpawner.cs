using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [System.Serializable]
    public class EnemyData{
        [SerializeField]
        public GameObject enemy;
        [SerializeField]
        public int minNum;
        [SerializeField]
        public int maxNum;
    }

    public LevelLoader levelLoader;

    public EnemyData[] enemyChoices;

    private void Start() {
        if (!levelLoader){
            GameObject.Find("Level Manager").GetComponent<LevelLoader>();
        }
    }

    public void SpawnEnemy(int difficulty){
        Vector2 location;
    }
}
