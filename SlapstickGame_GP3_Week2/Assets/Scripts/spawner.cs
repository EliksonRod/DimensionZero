using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using System.Collections;


public class spawner : MonoBehaviour
{
    //gameobjects
    public GameObject[] objects;
    public GameObject hazardParent;
    public GameObject hazardParent2;
    public GameObject exitOut;
    public GameObject player;

    //random spawn coordinates
    public float minSpawnX;
    public float minSpawnY;
    public float maxSpawnX;
    public float maxSpawnY;

    //timer for spawning hazards
    public float hazardTimer = 15f;

    //timer for spawning second set of hazards
    public float hazard2Timer = 35f;

    //timer for spawning exit
    public float exitTimer = 5f;

    //timer for spawning enemies
    public float timer = 5f;
    private float clock;

    private void Start()
    {
        clock = timer;
    }

    // Update is called once per frame
    void Update()
    {
        if (player.activeInHierarchy)
        {

        }
        else
        {
            StartCoroutine(deathAnim());
        }
        
        //counts down the timers
        clock -= Time.deltaTime;
        hazardTimer -= Time.deltaTime;
        hazard2Timer -= Time.deltaTime;
        exitTimer -= Time.deltaTime;

        if (clock <= 0)
        {
            //Randomly chooses what to spawn from list of objects[]
            int randomIndex = Random.Range(0, objects.Length);

            //Random range in an area where the objects will spawn
            Vector2 randomSpawnPosition = new Vector2(Random.Range(minSpawnX, maxSpawnX), Random.Range(minSpawnY, maxSpawnY));

            //does the spawning using the functions above 
            Instantiate(objects[randomIndex], randomSpawnPosition, Quaternion.identity);

            clock = timer;
        }
        if (hazardTimer <= 0)
        {
            hazardParent.SetActive(true);
        }
        if (hazard2Timer <= 0)
        {
            hazardParent2.SetActive(true);
        }
        if (exitTimer <= 0)
        {
            exitOut.SetActive(true);
        }
    }
    IEnumerator deathAnim()
    {
        yield return new WaitForSeconds(3);

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
