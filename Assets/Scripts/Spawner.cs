using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GM gameManager;
    //used to set the parent of the spawned objects so that they properly scale with screen size
    public GameObject gameCanvas;

    //holds all trash and laundry preabs in Unity that can be spawned
    public GameObject[] trashLaundrySpawns;
    //holds all dish prefabs in Unity that can be spawned
    public GameObject[] dishSpawns;

    //how long the game will wait until the first spawn
    float startWait;
    //least amount of time that the game will wait to spawn the next item
    float leastWait;
    //most amount of time that the game will wait to spawn the next item
    float mostWait;

    //list of the Maximum X values where the trash and laundry can be spawned
    public List<float> trashLaundrySpawnMaxsX;
    //list of the Minimum X values where the trash and laundry can be spawned
    public List<float> trashLaundrySpawnMinsX;

    //list of the Maximum Y values where the trash and laundry can be spawned
    public List<float> trashLaundrySpawnMaxsY;
    //list of the Minimum Y values where the trash and laundry can be spawned
    public List<float> trashLaundrySpawnMinsY;

    //List of all the Vector2 values where the dishes can spawn
    public List<Vector2> dishSpawnPoints;

    //always true, used in a while loop for the spawner
    bool stopTrashLaundry;

    //used to pull a spawned item from the trashLaundrySpawns array
    int randomTrashLaundry;
    //used to pull a spawned item from the dishSpawns array
    int randomDish;

    //used to pull a random 
    int randomDishSpawnPoint;
    //random number between 0 and 4 that chooses which Min/Max X/Y values to use
    //each "quadrant" (although there are 5) has max and min values set by invisible game objects
    int randomQuadrant;
    //determintes whether dishes or trash/laundry will spawn next
    int randomType;

    //all invisible game objects that are used to set the randomQuadrant
    public GameObject OneTopLeft;
    public GameObject OneBottomRight;
    public GameObject TwoTopLeft;
    public GameObject TwoBottomRight;
    public GameObject ThreeTopLeft;
    public GameObject ThreeBottomRight;
    public GameObject FourTopLeft;
    public GameObject FourBottomRight;
    public GameObject FiveTopLeft;
    public GameObject FiveBottomRight;

    //invisible game objects that determine the 4 possible spawn points of the dishes
    public GameObject OneDish;
    public GameObject TwoDish;
    public GameObject ThreeDish;
    public GameObject FourDish;

    void Start()
    {
        //coroutine is started
        StartCoroutine(waitSpawner());

        leastWait = 15;
        mostWait = 60;

        //"qaudrant" 1
        trashLaundrySpawnMaxsX.Add(OneBottomRight.transform.position.x);
        trashLaundrySpawnMinsX.Add(OneTopLeft.transform.position.x);
        trashLaundrySpawnMaxsY.Add(OneTopLeft.transform.position.y);
        trashLaundrySpawnMinsY.Add(OneBottomRight.transform.position.y);

        //"qaudrant" 2
        trashLaundrySpawnMaxsX.Add(TwoBottomRight.transform.position.x);
        trashLaundrySpawnMinsX.Add(TwoTopLeft.transform.position.x);
        trashLaundrySpawnMaxsY.Add(TwoTopLeft.transform.position.y);
        trashLaundrySpawnMinsY.Add(TwoBottomRight.transform.position.y);

        //"qaudrant" 3
        trashLaundrySpawnMaxsX.Add(ThreeBottomRight.transform.position.x);
        trashLaundrySpawnMinsX.Add(ThreeTopLeft.transform.position.x);
        trashLaundrySpawnMaxsY.Add(ThreeTopLeft.transform.position.y);
        trashLaundrySpawnMinsY.Add(ThreeBottomRight.transform.position.y);

        //"qaudrant" 4
        trashLaundrySpawnMaxsX.Add(FourBottomRight.transform.position.x);
        trashLaundrySpawnMinsX.Add(FourTopLeft.transform.position.x);
        trashLaundrySpawnMaxsY.Add(FourTopLeft.transform.position.y);
        trashLaundrySpawnMinsY.Add(FourBottomRight.transform.position.y);

        //"qaudrant" 5
        trashLaundrySpawnMaxsX.Add(FiveBottomRight.transform.position.x);
        trashLaundrySpawnMinsX.Add(FiveTopLeft.transform.position.x);
        trashLaundrySpawnMaxsY.Add(FiveTopLeft.transform.position.y);
        trashLaundrySpawnMinsY.Add(FiveBottomRight.transform.position.y);

        //4 possible dish spawn points
        dishSpawnPoints.Add(OneDish.transform.position);
        dishSpawnPoints.Add(TwoDish.transform.position);
        dishSpawnPoints.Add(ThreeDish.transform.position);
        dishSpawnPoints.Add(FourDish.transform.position);
    }

    IEnumerator waitSpawner()
    {
        //sets start wait time between 0 and 14 seconds
        startWait = Random.Range(0, 15);
        //tells coroutine to wait for the startWait number of seconds
        yield return new WaitForSeconds(startWait);

        while (!stopTrashLaundry)
        {
            //makes sure no more than 4 dishes spawn
            if (gameManager.dishNumber < 4)
            {
                //chooses a the number 1 or 2
                //1 will make trash/laundry spawn
                //2 will make a dish spawn
                randomType = Random.Range(1, 3);
            }
            else
            {
                //if gameManager.dishNumber > 4, trash/laundry will be spawned
                randomType = 1;
            }

            //trash/laundry spawn
            if (randomType == 1)
            {
                //used to choose a random "quadrant" number
                randomQuadrant = Random.Range(0, trashLaundrySpawnMaxsX.Count);
                //used to choose a random piece of trash or laundry number
                randomTrashLaundry = Random.Range(0, trashLaundrySpawns.Length);

                //the Vector2 spawn position of the trash/laundry is a random X/Y based on the randomQuadrant chosen
                Vector2 trashSpawnPosition = new Vector2(Random.Range(trashLaundrySpawnMinsX[randomQuadrant], trashLaundrySpawnMaxsX[randomQuadrant]), Random.Range(trashLaundrySpawnMinsY[randomQuadrant], trashLaundrySpawnMaxsY[randomQuadrant]));

                //the trash/laundry piece is instantiated at the trashSpawnPosition
                //the parent is set to the gameCanvas.transform
                var newtrash = Instantiate(trashLaundrySpawns[randomTrashLaundry], trashSpawnPosition, Quaternion.identity, gameCanvas.transform);
                newtrash.transform.parent = gameCanvas.transform;

            }
            //dish spawn
            else if (randomType == 2)
            {
                //used to choose a random dish number
                randomDish = Random.Range(0, dishSpawns.Length);
                //used to choose of the 4 dish spawn points
                randomDishSpawnPoint = Random.Range(0, dishSpawnPoints.Count);

                //the Vector2 spawn position of the dish using randomDishSpawnPoint
                Vector2 dishSpawnPosition = dishSpawnPoints[randomDishSpawnPoint];

                //remove whichever spawn point was chosen so that more than one dish does not spawn in the same position
                dishSpawnPoints.RemoveAt(randomDishSpawnPoint);

                //the trash/laundry piece is instantiated at the trashSpawnPosition
                //the parent is set to the gameCanvas.transform
                var newdish = Instantiate(dishSpawns[randomDish], dishSpawnPosition, Quaternion.identity, gameCanvas.transform);
                newdish.transform.parent = gameCanvas.transform;

                //increase dish number
                gameManager.dishNumber++;
            }
            //sets spawnWait to a random int between leastWait and mostWait - 1
            float spawnWait = Random.Range(leastWait, mostWait);
            //pauses the coroutine for spawnWait seconds
            yield return new WaitForSeconds(spawnWait);
        }

    }

    //run via the game manager
    //parameter trashNumber set based on how long the app has been closed
    //same code as waitSpawner(), but with no wait between spawns, all spawns happen at runtime
    public void appReOpened(int trashNumber)
    {
        //runs the Start() method
        this.Start();

        //runs spawning [trashNumber] times
        for (int i = 0; i < trashNumber; i++)
        {
            if (gameManager.dishNumber < 4)
            {
                randomType = Random.Range(1, 3);
            }
            else
            {
                randomType = 1;
            }

            if (randomType == 1)
            {
                randomQuadrant = Random.Range(0, trashLaundrySpawnMaxsX.Count - 1);
                randomTrashLaundry = Random.Range(0, trashLaundrySpawns.Length);

                Vector2 trashSpawnPosition = new Vector2(Random.Range(trashLaundrySpawnMinsX[randomQuadrant], trashLaundrySpawnMaxsX[randomQuadrant]), Random.Range(trashLaundrySpawnMinsY[randomQuadrant], trashLaundrySpawnMaxsY[randomQuadrant]));
                Instantiate(trashLaundrySpawns[randomTrashLaundry], trashSpawnPosition, Quaternion.identity, gameCanvas.transform);
            }
            else if (randomType == 2)
            {
                randomDish = Random.Range(0, dishSpawns.Length);

                randomDishSpawnPoint = Random.Range(0, dishSpawnPoints.Count - 1);
                Vector2 dishSpawnPosition = dishSpawnPoints[randomDishSpawnPoint];
                dishSpawnPoints.RemoveAt(randomDishSpawnPoint);
                var newdish = Instantiate(dishSpawns[randomDish], dishSpawnPosition, Quaternion.identity, gameCanvas.transform);
                gameManager.dishNumber++;
            }
        }
    }
}








