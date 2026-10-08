using UnityEngine;
using System.Collections;

public class CoroutineDemo : MonoBehaviour
{
    [Header("Configuración")]
    [SerializeField] private float startDelay = 2f;
    [SerializeField] private float preparationTime = 1f;
    [SerializeField] private float delayBetweenSpawns = 3f;
    [SerializeField] private int totalSpawns = 5;


    private int repeatCounter = 0;
    Coroutine machineCoroutine;

    private void Start()
    {      

        machineCoroutine = StartCoroutine(StartSpawnerCoroutine());
    }

    private IEnumerator StartSpawnerCoroutine()
    {
        Debug.Log("Corrutina iniciada");

        yield return new WaitForSeconds(startDelay);
        Debug.Log("Pasaron 2 segundos");

        yield return new WaitForSeconds(preparationTime);
        Debug.Log("Paso 1 segundo más");

        yield return SpawnEnemies();

        Debug.Log("Corrutina finalizada");
    }

    private IEnumerator SpawnEnemies()
    {
        Debug.Log("Spawn start");

        for (int i = 0; i <= totalSpawns; i++)
        {
            Debug.Log($"Fade value: {i * 20}%");
            yield return new WaitForSeconds(delayBetweenSpawns);
        }

        Debug.Log("Spawn complete");
    }
}
