using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour {
    ///Constant
    public GameObject FloorTile;

    ///State
    List<int> EmptyX = new List<int>();
    List<int> EmptyY = new List<int>();
    List<FloorGeneration> floorList = new List<FloorGeneration>();

    ///Balancing
    //Amount of floor tiles to be created
    int Amount;
    public float spawnInterval;
    public int floorPathRatio = 8;

    // Use this for initialization
    void Awake() {
        Amount = 1999;
        spawnInterval = 10f;

        EmptyX.Add(0);
        EmptyY.Add(0);

        FloorGeneration startFloor = Instantiate(FloorTile, new Vector3(0, 0, 1), Quaternion.identity, transform).GetComponent<FloorGeneration>();
        floorList.Add(startFloor);
        startFloor.StartRoom();
        
        SpawnNextFloor();
    }

    public void SpawnNextFloor() {
        if (EmptyX.Count >= Amount) return;

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
        if (newObj != null && newObj.GetProtected()) return; 

        /*var dir=""
        if (nextX>lastX) dir="W";
        if (nextX<lastX) dir="E";
        if (nextY>lastY) dir="N";
        if (nextY<lastY) dir="S";*/

        if (newObj == null) {
            newObj = Instantiate(FloorTile, new Vector3(nextX * floorPathRatio, nextY * floorPathRatio, 1), Quaternion.identity, transform).GetComponent<FloorGeneration>();
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
        xPos *= floorPathRatio;
        yPos *= floorPathRatio;
        foreach (FloorGeneration f in floorList) {
            if (Mathf.RoundToInt(f.transform.position.x) == xPos && Mathf.RoundToInt(f.transform.position.y) == yPos) {
                return f;
            }
        }
        return null;
    }

}
