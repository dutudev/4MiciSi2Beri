using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiciSpawner : MonoBehaviour
{

    [SerializeField] private GameObject micPrefab;
    [SerializeField] private BoxCollider2D spaceForMici;

    private ContactFilter2D _contactFilter;
    private float _timeToSpawn = 3f;
    private Vector3 _micSpawnStartLocation = new Vector3(-5.9f, 3.4f, 0f);
    private Vector3 _porkSpawn = new Vector3(-5.85f, 1.12f, 0);
    private Vector3 _kabSpawn = new Vector3(-6.2f, -0.73f, 0);
    
    // Start is called before the first frame update
    void Start()
    {
        _contactFilter.layerMask = LayerMask.GetMask("Ingredient");
    }

    // Update is called once per frame
    void Update()
    {
        if (spaceForMici.OverlapCollider(_contactFilter, new List<Collider2D>()) == 0)
        {
            _timeToSpawn -= Time.deltaTime;
        }

        if (_timeToSpawn <= 0)
        {
            _timeToSpawn = 3f;
            switch (gameObject.name)
            {
                case "MiciContainer":
                    SpawnMici();
                    break;
                case "PorkSpawner":
                    SpawnPork();
                    break;
                case "KabanosSpawner":
                    SpawnKabanos();
                    break;
            }
        }
    }

    private void SpawnMici()
    {
        for (int i = 0; i < 6; i++)
        {
            if (i % 2 == 0)
            {
                Instantiate(micPrefab, _micSpawnStartLocation + new Vector3(0f, -0.5f, 0f) * i / 2, Quaternion.identity);
            }
            else
            {
                Instantiate(micPrefab, _micSpawnStartLocation + new Vector3(0f, -0.5f, 0f) * Mathf.Floor(i / 2f)+ new Vector3(1.6f, 0, 0), Quaternion.identity);
            }
        }
    }

    private void SpawnPork()
    {
        for (int i = 0; i < 2; i++)
        {
            Instantiate(micPrefab, _porkSpawn + new Vector3(1.42f, 0f, 0f) * i, Quaternion.identity);
        }
    }

    private void SpawnKabanos()
    {
        for (int i = 0; i < 5; i++)
        {
            Instantiate(micPrefab, _kabSpawn + new Vector3(0.53f, 0f, 0f) * i, Quaternion.identity);
        }
    }
}
