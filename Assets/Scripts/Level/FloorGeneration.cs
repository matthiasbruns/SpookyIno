using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LinkTuple = System.Tuple<int, int>;
using Tuple = System.Tuple;

public class FloorGeneration : MonoBehaviour {

    //Constant
    public GameObject PathTile;
    LevelGenerator levelRef;
    public int xPos { get; private set; }
    public int yPos { get; private set; }
    public int lastX, lastY;

    //State
    public List<PathGeneration> instPath = new List<PathGeneration>();
    public List<PathGeneration> instGenPath = new List<PathGeneration>();
    Dictionary<LinkTuple, bool> link = new Dictionary<LinkTuple, bool>();
    public bool babyCover;

    public bool hasExitN, hasExitS, hasExitW, hasExitE;

    // Use this for initialization
    void Awake() {
        levelRef = transform.parent.GetComponent<LevelGenerator>();
        babyCover = true;
        StartCoroutine(StopProtect(30f));
    }

    public IEnumerator StopProtect(float time) {
        yield return new WaitForSeconds(time);
        babyCover = false;
    }

    public void StartRoom() {
        int xTemp = 0;
        int yTemp = 0;

        hasExitN = false;
        hasExitS = false;
        hasExitW = false;
        hasExitE = false;

        int countExit = Random.Range(3, 6); // min 2 weil 1 ([0]) bereits durch spawn belegt var i = 1 in for schleife weil index stimmen muss

        instPath.Add(CreateExit(levelRef.floorPathRatio/2, levelRef.floorPathRatio / 2, 0));

        for (int i = 1; i < countExit; i += 1) {
            int switchExit = Random.Range(0, 4);
            if (switchExit == 0) { xTemp = levelRef.floorPathRatio - 1; hasExitE = true; }
            if (switchExit == 1) { yTemp = levelRef.floorPathRatio - 1; hasExitS = true; }
            if (switchExit == 2) { xTemp = 0; hasExitW = true; }
            if (switchExit == 3) { yTemp = 0; hasExitN = true; }

            if (switchExit == 0 || switchExit == 2) {
                do {
                    yTemp = Random.Range(0, levelRef.floorPathRatio);
                }

                while (GetPath(xTemp, yTemp) != null);
            } else {
                do {
                    xTemp = Random.Range(0, levelRef.floorPathRatio);
                }

                while (GetPath(xTemp, yTemp) != null);
            }


            instPath.Add(CreateExit(xTemp, yTemp, i));
        }
        GenPath();

        levelRef.SpawnNextFloor();
    }

    void GenPath() {

        for (int j = 0; j < 200; j++) {
            ClearRoom();
            int interval = instPath.Count;
            for (int i = 0; i < interval; i++) {
                instPath[i].GenPath();
            }

            for (int i = 0; i < instGenPath.Count; i++) {
                instGenPath[i].GenPath();
            }

            CheckLinked();
            CheckLinked();

            if (IsGood()) {
                levelRef.SpawnNextFloor();
                break;
            }
        }

    }

    void ClearRoom() {
        link.Clear();
        
        foreach (PathGeneration path in instGenPath) {
            Destroy(path.gameObject);
        }
        instGenPath.Clear();

        int numPathsIdxs = NumExit();

        for (int xx = 0; xx < numPathsIdxs; xx += 1) {
            for (int yy = xx; yy < numPathsIdxs; yy += 1) {
                SetLinked(xx, yy, xx == yy);
            }
        }



    }

    public void ClearFloor() {
        ClearRoom();

        foreach (PathGeneration path in instPath) {
            Destroy(path.gameObject);
        }
        instPath.Clear();
    }

    bool IsGood() {

        int numPathsIdxs = NumExit();

        for (int xx = 0; xx < numPathsIdxs - 1; xx += 1)  //-1 weil letzte Abfrage Unnötig
        {
            for (int yy = xx; yy < numPathsIdxs; yy += 1) {
                if (IsLinked(xx, yy) == false) {
                    return false;
                }
            }
        }
        return true;

    }

    public void GenExit() {

        List<PathGeneration> exitTempN = new List<PathGeneration>();
        List<PathGeneration> exitTempS = new List<PathGeneration>();
        List<PathGeneration> exitTempW = new List<PathGeneration>();
        List<PathGeneration> exitTempE = new List<PathGeneration>();
        //List<PathGeneration> exitTemp = new List<PathGeneration>();

        hasExitN = false;
        hasExitS = false;
        hasExitW = false;
        hasExitE = false;

        /*if (dir=="N") {xTemp=irandom_range(0, 15); yTemp=0; hasExitN=true;};
        if (dir=="S") {xTemp=irandom_range(0, 15); yTemp=15; hasExitS=true;};
        if (dir=="E") {xTemp=15; yTemp=irandom_range(0, 15); hasExitE=true;};
        if (dir=="W") {xTemp=0; yTemp=irandom_range(0, 15); hasExitW=true;};

        instPath[0] = floor_createExit(id, xTemp, yTemp, 0);*/

        int nextExit = 0;
        for (int i = 0; i < 4; i++) {
            FloorGeneration f = null;
            if (i == 0) f = levelRef.GetFloor(xPos + 1, yPos);
            if (i == 1) f = levelRef.GetFloor(xPos - 1, yPos);
            if (i == 2) f = levelRef.GetFloor(xPos, yPos + 1);
            if (i == 3) f = levelRef.GetFloor(xPos, yPos - 1);
            if (f == null) continue;

            bool added = false;

            for (int j = 0; j < levelRef.floorPathRatio; j++) {
                PathGeneration path = null;
                if (i == 0) path = GetPath(0, j);
                if (i == 1) path = GetPath(levelRef.floorPathRatio - 1, j);
                if (i == 2) path = GetPath(j, 0);
                if (i == 3) path = GetPath(j, levelRef.floorPathRatio - 1);
                if (path == null) continue;

                if (i == 0) { exitTempE.Add(CreateExit(levelRef.floorPathRatio - 1, j, nextExit)); added = true; hasExitE = true; }
                if (i == 1) { exitTempW.Add(CreateExit(0, j, nextExit)); added = true; hasExitW = true; }
                if (i == 2) { exitTempS.Add(CreateExit(j, levelRef.floorPathRatio - 1, nextExit)); added = true; hasExitS = true; }
                if (i == 3) { exitTempN.Add(CreateExit(j, 0, nextExit)); added = true; hasExitN = true; }
            }
            if (added) nextExit++;
        }

        if (hasExitN == true) ShufflePath(exitTempN);
        if (hasExitS == true) ShufflePath(exitTempS);
        if (hasExitE == true) ShufflePath(exitTempE);
        if (hasExitW == true) ShufflePath(exitTempW);
        //ShufflePath(exitTemp);

        GenPath();
    }

    public PathGeneration GetPath(int xPos, int yPos) {

        foreach (PathGeneration R in instPath) {
            if (R.transform.position.x == xPos && R.transform.position.y == yPos) {
                return R;
            }
        }
        foreach (PathGeneration R in instGenPath) {
            if (R.transform.position.x == xPos && R.transform.position.y == yPos) {
                return R;
            }
        }
        return null;
    }

    public PathGeneration CreateExit(int xPath, int yPath, int pathValue) {


        PathGeneration path = Instantiate(PathTile, new Vector3(xPos + xPath, yPos + yPath, 0), Quaternion.identity, transform).GetComponent<PathGeneration>();
        //  instance_create_depth(this.x + (sprite_get_width(spr_path) * xExit), this.y + (sprite_get_height(spr_path) * yExit), -1, obj_room_exit);

        path.pathIdx = pathValue;

        return path;
    }

    int NumExit() {

        int numPathsIdxs = 0;
        foreach (PathGeneration R in instPath) {
            if (R.pathIdx + 1 > numPathsIdxs) numPathsIdxs = R.pathIdx + 1;
        }

        return numPathsIdxs;

    }

    public void SetLinked(int i, int j, bool newInput) {
        
        link[Tuple.Create(i, j)] = newInput;
        link[Tuple.Create(j, i)] = newInput;

    }

    bool IsLinked(int i, int j) {

        return link[Tuple.Create(i, j)] || link[Tuple.Create(j, i)];
    }

    void CheckLinked() {

        // max is the max size or -1 if none found
        int maxPathIdx = NumExit();
        for (int i = 0; i < maxPathIdx; i++) {
            for (int j = 0; j < maxPathIdx; j++) {
                if (IsLinked(i, j)) {
                    for (int k = 0; k < maxPathIdx; k++) {
                        if (IsLinked(j, k)) {
                            SetLinked(i, k, true);
                        }
                    }
                }
            }
        }


    }

    void ShufflePath(List<PathGeneration> list) {
        for (int i = 0; i < list.Count; i++) {
            PathGeneration temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
        int potExits = Random.Range(1, list.Count + 1);
        for (int i = 0; i < potExits; i++) {
            instPath.Add(list[i]);
        }
    }

    public string GetRandomExitDir() {
        List<string> spawnDirection = new List<string>();

        if (hasExitE == true) {
            spawnDirection.Add("E");
        }
        if (hasExitW == true) {
            spawnDirection.Add("W");
        }
        if (hasExitS == true) {
            spawnDirection.Add("S");
        }
        if (hasExitN == true) {
            spawnDirection.Add("N");
        }

        if (spawnDirection.Count > 0) {
            int spawnDirectionTemp = Random.Range(0, spawnDirection.Count);

            return spawnDirection[spawnDirectionTemp];

        }
        return null;
    }

    public bool GetProtected() {
        /*
        if (player_getFloor(obj_player) == this) {
            return true;
        }
        return false;
        */
        return babyCover;
    }
}
