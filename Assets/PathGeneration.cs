using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathGeneration : MonoBehaviour {

    string objectTyp;
    FloorGeneration floorRef; 
    public int pathIdx;
    public int xPos { get; private set; }
    public int yPos { get; private set; }

    // Use this for initialization
    void Awake() {
        ///Constant
        objectTyp = "Way";
        floorRef = transform.parent.GetComponent<FloorGeneration>();
        xPos = Mathf.RoundToInt(transform.position.x);
        yPos = Mathf.RoundToInt(transform.position.y);

        ///State
        pathIdx = 0;
    }

    public void GenPath() {

        List<string> spawnDirection = new List<string>();
        int xPos = Mathf.RoundToInt(transform.position.x);
        int yPos = Mathf.RoundToInt(transform.position.y);
        PathGeneration exitColl;

        exitColl = floorRef.GetPath(xPos + 1, yPos); 
        if (exitColl == null && xPos < 15) {
            spawnDirection.Add("E");
        }
        exitColl = floorRef.GetPath(xPos - 1, yPos);
        if (exitColl == null && xPos > 0) {
            spawnDirection.Add("W");
        }
        exitColl = floorRef.GetPath(xPos, yPos + 1);
        if (exitColl == null && yPos < 15) {
            spawnDirection.Add("S");
        }
        exitColl = floorRef.GetPath(xPos, yPos - 1);
        if (exitColl == null && yPos > 0) {
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
                if (nextOne.xPos == 0) floorRef.hasExitW = true;
                if (nextOne.yPos == 0) floorRef.hasExitN = true;
                if (nextOne.xPos == 15) floorRef.hasExitE = true;
                if (nextOne.yPos == 15) floorRef.hasExitS = true;
            }
            nextOne.CheckCollision();
        }
    }

    void CheckCollision() {
        PathGeneration exitIDOther;

        exitIDOther = floorRef.GetPath(xPos + 1, yPos);
        if (exitIDOther != null) {
            floorRef.SetLinked(pathIdx, exitIDOther.pathIdx, true);
        }
        exitIDOther = floorRef.GetPath(xPos - 1, yPos);
        if (exitIDOther != null) {
            floorRef.SetLinked(pathIdx, exitIDOther.pathIdx, true);
        }
        exitIDOther = floorRef.GetPath(xPos, yPos - 1);
        if (exitIDOther != null) {
            floorRef.SetLinked(pathIdx, exitIDOther.pathIdx, true);
        }
        exitIDOther = floorRef.GetPath(xPos, yPos + 1);
        if (exitIDOther != null) {
            floorRef.SetLinked(pathIdx, exitIDOther.pathIdx, true);
        }


    }
}
