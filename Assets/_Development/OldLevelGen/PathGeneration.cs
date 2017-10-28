using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathGeneration : MonoBehaviour {

    FloorGeneration floorRef;
    LevelGenerator levelRef;
    public int pathIdx;
    public int xPos, yPos;

    // Use this for initialization
    void Awake() {
        ///Constant
        floorRef = transform.parent.GetComponent<FloorGeneration>();
        levelRef = floorRef.transform.parent.GetComponent<LevelGenerator>();
        xPos = 0;
        yPos = 0;

        ///State
        pathIdx = 0;
    }

    public void GenPath() {

        List<string> spawnDirection = new List<string>();
        PathGeneration exitColl;

        exitColl = floorRef.GetPath(xPos + 1, yPos);
        if (exitColl == null && xPos < levelRef.floorPathRatio - 1 && xPos > 0) {
            spawnDirection.Add("E");
        }
        exitColl = floorRef.GetPath(xPos - 1, yPos);
        if (exitColl == null && xPos > 0 && xPos < levelRef.floorPathRatio - 1) {
            spawnDirection.Add("W");
        }
            exitColl = floorRef.GetPath(xPos, yPos + 1);
        if (exitColl == null && yPos < levelRef.floorPathRatio - 1 && yPos > 0) {
            spawnDirection.Add("S");
        }
        exitColl = floorRef.GetPath(xPos, yPos - 1);
        if (exitColl == null && yPos > 0 && yPos < levelRef.floorPathRatio - 1) {
            spawnDirection.Add("N");
        }

        if (spawnDirection.Count > 0) {
            int countDirectionTemp = Random.Range(0, spawnDirection.Count);
            PathGeneration nextOne = null;

            switch (spawnDirection[countDirectionTemp]) {
                case ("E"): {
                        nextOne = floorRef.CreateExit(xPos + 1, yPos, pathIdx);
                        floorRef.instGenPath.Add(nextOne);
                        break;
                    }
                case ("W"): {
                        nextOne = floorRef.CreateExit(xPos - 1, yPos, pathIdx);
                        floorRef.instGenPath.Add(nextOne);
                        break;
                    }
                case ("S"): {
                        nextOne = floorRef.CreateExit(xPos, yPos + 1, pathIdx);
                        floorRef.instGenPath.Add(nextOne);
                        break;
                    }
                case ("N"): {
                        nextOne = floorRef.CreateExit(xPos, yPos - 1, pathIdx);
                        floorRef.instGenPath.Add(nextOne);
                        break;
                    }
                default: {
                        break;
                    }
            }

            if (nextOne != null) {
                if (nextOne.transform.position.x == 0) floorRef.hasExitW = true;
                if (nextOne.transform.position.y == 0) floorRef.hasExitN = true;
                if (nextOne.transform.position.x == levelRef.floorPathRatio - 1) floorRef.hasExitE = true;
                if (nextOne.transform.position.y == levelRef.floorPathRatio - 1) floorRef.hasExitS = true;
            }
            nextOne.CheckCollision();
        }
    }

    void CheckCollision() {
        PathGeneration exitIDOther;

        exitIDOther = floorRef.GetPath(Mathf.RoundToInt(transform.position.x) + 1, Mathf.RoundToInt(transform.position.y));
        if (exitIDOther != null) {
            floorRef.SetLinked(pathIdx, exitIDOther.pathIdx, true);
        }
        exitIDOther = floorRef.GetPath(Mathf.RoundToInt(transform.position.x) - 1, Mathf.RoundToInt(transform.position.y));
        if (exitIDOther != null) {
            floorRef.SetLinked(pathIdx, exitIDOther.pathIdx, true);
        }
        exitIDOther = floorRef.GetPath(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y) - 1);
        if (exitIDOther != null) {
            floorRef.SetLinked(pathIdx, exitIDOther.pathIdx, true);
        }
        exitIDOther = floorRef.GetPath(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y) + 1);
        if (exitIDOther != null) {
            floorRef.SetLinked(pathIdx, exitIDOther.pathIdx, true);
        }

    }
}
