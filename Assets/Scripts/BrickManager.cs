using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BrickManager : MonoBehaviour
{


    #region Singelton
        private static BrickManager _instance;
        public static BrickManager Instance => _instance;
        private void Awake() {
            if(_instance != null){
                Destroy(gameObject);
            }
            else{
                _instance = this; 
            }
        }
    #endregion
    private int maxRows = 17;
    private int maxCols = 12;
    private GameObject BricksContainer;
    public Sprite[] Sprites;

    public int CurrentLevel;
    private float initialBrickSpawnPositionX = -1.96f;
    private float initialBrickSpawnPositionY = 3.325f;
    public Brick brickPrefab;

    public List<Brick> RemainingBricks {get; set;}
    public List<int[,]> LevelsData{ get; set;}
    public int InitialBricksCount { get; set; }

    public Color[] BricksColors;
    private float shiftAmount = 0.365f;

    public void LoadLevel(int level)
    {
        this.CurrentLevel = level;
        this.ClearRemainingBrick();
        this.GenerateBricks();
    }

    private void ClearRemainingBrick(){
        foreach (Brick brick in this.RemainingBricks.ToList())
        {
            Destroy(brick.gameObject);
        }
    }

    private void Start() {
        this.BricksContainer = new GameObject("BricksContainer");
        this.LevelsData = this.LoadLevelsData();
        this.GenerateBricks();
    }

    private void GenerateBricks()
    {
        this.RemainingBricks = new List<Brick>();
        int[,] currentLevelData = this.LevelsData[this.CurrentLevel];
        float currentSpawnX = initialBrickSpawnPositionX;
        float currentSpawnY = initialBrickSpawnPositionY;
        float zShift = 0;

        for(int row = 0; row < this.maxRows; row++){
            for(int col = 0; col < this.maxCols; col++){
                int BricksType = currentLevelData[row,col];

                if(BricksType>0)
                {
                    Brick newBrick = Instantiate(brickPrefab, new Vector3(currentSpawnX, currentSpawnY, 0.0f - zShift),Quaternion.identity) as Brick;
                    newBrick.Init(BricksContainer.transform, this.Sprites[BricksType - 1], this.BricksColors[BricksType], BricksType);
                
                    this.RemainingBricks.Add(newBrick);
                    zShift += 0.0001f;
                }

                currentSpawnX += shiftAmount;
                if(col + 1 == this.maxCols){
                    currentSpawnX = initialBrickSpawnPositionX;
                }
            }

            currentSpawnY -= shiftAmount; 
        }
        this.InitialBricksCount = this.RemainingBricks.Count;
    }

    private List<int[,]> LoadLevelsData(){
        TextAsset text = Resources.Load("levels") as TextAsset;

        string[] rows = text.text.Split(new string[] {Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries);
        List<int[,]> levelsData = new List<int[,]>();
        int[,] currentLevel = new int[maxRows,maxCols];
        int currentRow = 0;

        for(int row = 0; row < rows.Length; row++){
            string line = rows[row];

            if(line.IndexOf("--") == -1){
                string[] bricks = line.Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries);
                for(int col = 0; col < bricks.Length; col++){
                    currentLevel[currentRow, col] = int.Parse(bricks[col]);
                }

                currentRow++;
            }
            else{
                currentRow = 0;
                levelsData.Add(currentLevel);
                currentLevel = new int [maxRows, maxCols];

            }
        }
        return levelsData;
    }
}
