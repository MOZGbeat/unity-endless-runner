using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileGenerator : MonoBehaviour {

	public GameObject[] tilePrefabs_1;
	public GameObject[] tilePrefabs_2;
	public GameObject tileStartPrefabs_1;
	public GameObject tileStartPrefabs_2;
    GameObject[] tilePrefabs;
	private List<GameObject> activeTiles = new List<GameObject>();
	private float spawnPos = 200;
	private float tileLength = 200;

	[SerializeField] private Transform player;
	private int startTiles = 6;

	void Start () 
	{
		if (PlayerPrefs.GetInt("usedMap") == 0)
		{
			tileStartPrefabs_1.SetActive(true);
			tileStartPrefabs_2.SetActive(false);
            tilePrefabs = tilePrefabs_1;
		}
		else if (PlayerPrefs.GetInt("usedMap") == 1)
		{
			tilePrefabs = tilePrefabs_2;
            tileStartPrefabs_1.SetActive(false);
            tileStartPrefabs_2.SetActive(true);
        }

		SpawnTile(0);
        for (int i = 0; i < startTiles - 1; i++)
        {
			SpawnTile(Random.Range(1, tilePrefabs.Length));
        }
	}
	
	void Update () 
	{
		if (player.position.z - 200 > spawnPos - (startTiles * tileLength))
        {
			SpawnTile(Random.Range(1, tilePrefabs.Length));
			DeleleTile();
        }
	}
	private void SpawnTile(int tileIndex)
	{
		GameObject nextTile = Instantiate(tilePrefabs[tileIndex], transform.forward * spawnPos, transform.rotation);
		activeTiles.Add(nextTile);
		spawnPos += tileLength;
    }
	private void DeleleTile()
    {
		Destroy(activeTiles[0]);
		activeTiles.RemoveAt(0);
    }
}
