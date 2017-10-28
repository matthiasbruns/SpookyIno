using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour {
    ///Constant
    string objectTyp = "Level";
    public GameObject FloorTile;

    ///State
    List<int> EmptyX = new List<int>();
    List<int> EmptyY = new List<int>();
    List<FloorGeneration> floorList = new List<FloorGeneration>();

    ///Balancing
    //Amount of floor tiles to be created
    int Amount;
    public float spawnInterval;

    ///StartGenerating
    FloorGeneration startFloor;


    // Use this for initialization
    void Start() {
        Amount = 1999;
        spawnInterval = 10f;

        FloorGeneration startFloor = Instantiate(FloorTile, new Vector3(0, 0, 1), Quaternion.identity, transform).GetComponent<FloorGeneration>();
        floorList.Add(startFloor);
        startFloor.StartRoom();

        EmptyX.Add(Mathf.RoundToInt(startFloor.transform.position.x));
        EmptyY.Add(Mathf.RoundToInt(startFloor.transform.position.y));

        StartCoroutine(SpawnNextFloor(spawnInterval));

    }

    public IEnumerator SpawnNextFloor(float time) {
        yield return new WaitForSeconds(time);
        if (EmptyX.Count >= Amount) yield break;

        int lastX = EmptyX[EmptyX.Count - 1];
        int lastY = EmptyY[EmptyY.Count - 1];
        int nextX = lastX;
        int nextY = lastY;


        FloorGeneration oldObj = GetFloor(lastX, lastY);

        string dir = oldObj.GetRandomExitDir();
        if (dir == "N") nextY--;
        if (dir == "S") nextY++;
        if (dir == "E") nextX++;
        if (dir == "W") nextX--;
        //show_debug_message("skip: " + string(lastX) + " " + string(lastY));
        //show_debug_message("nextOld: " + string(nextX) + " " + string(nextY));
        //show_debug_message("nextNew: " + string(EmptyX[positionI - 1]) + " " + string(EmptyY[positionI - 1]));
        FloorGeneration newObj = GetFloor(nextX, nextY);
        if (newObj != null && newObj.GetProtected()) yield break; 

        /*var dir=""
        if (nextX>lastX) dir="W";
        if (nextX<lastX) dir="E";
        if (nextY>lastY) dir="N";
        if (nextY<lastY) dir="S";*/

        if (newObj == null) {
            newObj = Instantiate(FloorTile, new Vector3(nextX * 16, nextY * 16, 1), Quaternion.identity, transform).GetComponent<FloorGeneration>();
            floorList.Add(newObj);
            floorList[floorList.Count - 1].lastX = nextX;
            floorList[floorList.Count - 1].lastY = nextY;
        }
        newObj.ClearFloor();
        newObj.GenExit();

        EmptyX.Add(nextX);
        EmptyY.Add(nextY);

    }

    public FloorGeneration GetFloor(int xPos, int yPos) {
        xPos *= 16;
        yPos *= 16;
        foreach (FloorGeneration f in floorList) {
            if (Mathf.RoundToInt(f.transform.position.x) == xPos && Mathf.RoundToInt(f.transform.position.y) == yPos) {
                return f;
            }
        }
        return null;
    }

}
