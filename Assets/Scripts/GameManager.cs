using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Singelton
        private static GameManager _instance;
        public static GameManager Instance => _instance;
        private void Awake() {
            if(_instance != null){
                Destroy(gameObject);
            }
            else{
                _instance = this; 
            }
        }
    #endregion
    public bool isGameStarted {get; set;}

    private void Start() {
        Screen.SetResolution(540, 960, false);
    }
}
