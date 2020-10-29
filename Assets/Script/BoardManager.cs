using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using System;

public class BoardManager : MonoBehaviour
{
	[Serializable]
	public class Count {
		public int minimum;
		public int maximum;

		public Count (int min, int max){
			minimum = min;
			maximum = max;
		}
	}

	public int columns = 10;
	public int rows = 8;
	public Count obstacleCount = new Count(5, 8);
	public Count candyCount = new Count(1,5);
	public GameObject exit;
	public GameObject floorTile;
	public GameObject enemyTile;
	public GameObject[] obstacleTiles;
	public GameObject[] wallTiles;
	public GameObject[] candy;

	private Transform boardHolder;
	private List<Vector3> gridPositions = new List<Vector3>();

	void InitialiseList(){
		gridPositions.Clear();

		for(int x = 1; x < columns -1; x++){
			for(int y = 1; y < rows -1; y++){
				gridPositions.Add(new Vector3(x, y, 0f));
			}
		}
	}

	void BoardSetup (){
		boardHolder = new GameObject("Board").transform;
		for(int x = -1; x < columns +1; x++){
			for(int y = -1; y < rows +1; y++){
				GameObject toInstantiate = floorTile;
				if(x == -1 || x == columns){
					toInstantiate = wallTiles[4];
				} else if(y == -1 || y == rows){
					toInstantiate = wallTiles[5];
				}
				if (x == -1 && y == -1)
					toInstantiate = wallTiles[0];
				else if (x == columns && y == -1)
					toInstantiate = wallTiles[1];
				else if (x == -1 && y == rows)
					toInstantiate = wallTiles[2];
				else if (x == columns && y == rows)
					toInstantiate = wallTiles[3];

					GameObject instance = Instantiate(toInstantiate, new Vector3(x,y,0f), Quaternion.identity) as GameObject;

					instance.transform.SetParent(boardHolder);
			}
		}
	}

	Vector3 RandomPosition(){
		int randomIndex = Random.Range(0, gridPositions.Count);
		Vector3 randomPosition = gridPositions[randomIndex];
		gridPositions.RemoveAt(randomIndex);
		return randomPosition; 
	}

	void LayoutObjectAtRandom(GameObject[] tileArray, int minimum, int maximum){
		int objectCount = Random.Range(minimum, maximum +1);
		for(int i = 0; i < objectCount; i++){
			Vector3 randomPosition = RandomPosition();
			GameObject tileChoice = tileArray[Random.Range(0,tileArray.Length)];
			Instantiate (tileChoice, randomPosition, Quaternion.identity);
		}
	}

	void LayoutEnemyAtRandom(GameObject enemy, int minimum, int maximum){
		int enemyCount = Random.Range(minimum, maximum +1);
		Debug.Log(enemyCount);
		for(int i = 0; i < enemyCount; i++){
			Vector3 randomPosition = RandomPosition();
			GameObject enemyChoice = enemy;
			Instantiate (enemyChoice, randomPosition, Quaternion.identity);
		}
	}

	public void SetupScene(int level){
		BoardSetup();
		InitialiseList();
		LayoutObjectAtRandom(obstacleTiles, obstacleCount.minimum, obstacleCount.maximum);
		LayoutObjectAtRandom(candy, candyCount.minimum, candyCount.maximum);
		int enemyCount = (int)Mathf.Log(level,2f);
		Debug.Log(enemyCount);
		LayoutEnemyAtRandom(enemyTile, enemyCount, enemyCount);
		Instantiate(exit, new Vector3(columns -1, rows -1, 0F), Quaternion.identity);

	}
}
