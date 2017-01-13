using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnPoint : MonoBehaviour {

	[Tooltip("What is being spawned")]
	public GameObject toSpawn;

	[Tooltip("Maximum number of spawned objects")]
	public int max;

	[Tooltip("Rate at which objects are spawned")]
	public float rate;

	[Tooltip("Random range to offset spawned x-position")]
	public float randomStart;

	[Tooltip("Spawn at the start of scene.")]
	public bool spawnImmediately;

	//Cached variables.
	private float time = 0.0f;
	private List<GameObject> spawned;
	private Vector3 origin;

	void Awake()
	{
		//Instantiate list.
		spawned = new List<GameObject> ();

		//Spawn object immediately, if selected.
		if (spawnImmediately) {
			Spawn ();
		}
	}
		
	void Update () {
		time += Time.deltaTime;

		//Spawn.
		if (time >= rate) {
			Spawn ();
			time = 0.0f;
		}

		//Check if all spawned objects still exist.
		//Makes space if spawned objects are destroyed.
		if (spawned.Count == max) {
			for (int i = spawned.Count - 1; i >= 0; i--) {
				if (spawned [i] == null) {
					spawned.RemoveAt (i);
				}
			}
		}
	}

	//Create new instance of the object.
	void Spawn()
	{
		if (toSpawn && spawned.Count < max) 
		{
			//modify the start position by the given offset.
			origin = transform.position;
			origin.x += Random.Range (-randomStart, randomStart);

			//Instantiate at the given position.
			spawned.Add ((GameObject)Instantiate(toSpawn, origin, Quaternion.identity));
		}
	}
}
