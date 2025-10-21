using System.Collections;
using System.Threading;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using static UnityEngine.Rendering.DebugUI;
public class spawner : MonoBehaviour
{
    //gameobjects
    public GameObject[] Collectibles;
    public GameObject[] Hazards;
    public GameObject LevelExit;
    public GameObject PlayerObject;

    public TextMeshProUGUI levelTimerDisplay;

    //random spawn coordinates
    public float minSpawnX;
    public float maxSpawnX;
    public float minSpawnY;
    public float maxSpawnY;

    public float levelTimer = 60;
    int currentTime;

    //timer for spawning hazards
    public float hazardSpawnCooldown = 15f;

    //timer for spawning collectibles
    public float collectibleSpawnCooldown = 1.5f;
    float collectibleSpawnTimer;

    private void Start()
    {
        //currentTime = levelTimer;
        StartCoroutine(ActivateHazard());

    }

    // Update is called once per frame
    void Update()
    {
        if (!PlayerObject.activeInHierarchy)
        {
            StartCoroutine(deathAnim());
        }

        if (levelTimerDisplay != null)
            levelTimerDisplay.text = currentTime.ToString();

        //counts down the timers
        if (levelTimer > 0)
        {
            levelTimer -= 1 * Time.deltaTime;
            currentTime = Mathf.RoundToInt(levelTimer);
        }
        collectibleSpawnTimer -= Time.deltaTime;

        //Spawns collectibles after timer reaches 0
        if (collectibleSpawnTimer <= 0)
        {
            collectibleSpawnTimer = collectibleSpawnCooldown;

            //Randomly chooses what to spawn from list of objects[]
            int randomIndex = Random.Range(0, Collectibles.Length);

            //Random range in an area where the objects will spawn
            Vector2 randomSpawnPosition = new Vector2(Random.Range(minSpawnX, maxSpawnX), Random.Range(minSpawnY, maxSpawnY));

            //does the spawning using the functions above 
            Instantiate(Collectibles[randomIndex], randomSpawnPosition, Quaternion.identity);
        }

        //Activates level exit when timer reaches 0
        if (currentTime <= 0)
        {
            LevelExit.SetActive(true);
        }
    }

    //Activates hazards after a set cooldown time
    IEnumerator ActivateHazard()
    {
        yield return new WaitForSeconds(hazardSpawnCooldown);

        for (int i = 0; i < Hazards.Length; i++)
        {
            Hazards[i].SetActive(true);
            yield return new WaitForSeconds(hazardSpawnCooldown);
        }
    }

    //Plays death animation then reloads the scene
    IEnumerator deathAnim()
    {
        yield return new WaitForSeconds(3);

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
