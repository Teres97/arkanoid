using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    #region Singelton
        private static Platform _instance;
        public static Platform Instance => _instance;
        private void Awake() {
            if(_instance != null){
                Destroy(gameObject);
            }
            else{
                _instance = this; 
            }
        }
    #endregion
    float platformInitialY;
    private Camera mainCamera;

    // Update is called once per frame
    private void Start() {
        mainCamera = FindObjectOfType<Camera>();
        platformInitialY = this.transform.position.y;    
    }
    void Update()
    {
        platformMovement();
    }
    private void platformMovement()
    {
        float leftClamp = 70;
        float rightClamp = 520;
        float mousePositionPixels = Mathf.Clamp(Input.mousePosition.x,leftClamp,rightClamp);
        float MouseWorldPositionX = mainCamera.ScreenToWorldPoint(new Vector3(mousePositionPixels, 0, 0)).x;
        this.transform.position = new Vector3(MouseWorldPositionX, platformInitialY, 0);
    }

    private void OnCollisionEnter2D(Collision2D coll) {
        if(coll.gameObject.tag =="Ball"){
            Rigidbody2D ballRb = coll.gameObject.GetComponent<Rigidbody2D>();
            Vector3 hitPoint = coll.contacts[0].point;
            Vector3 platformCenter = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, 0);
            ballRb.velocity = Vector2.zero; 
            float difference = platformCenter.x - hitPoint.x;

            if(hitPoint.x<platformCenter.x){
                ballRb.AddForce(new Vector2(-(Mathf.Abs(difference * 200)), BallsManager.Instance.initialBallSpeed));
            } else{
                ballRb.AddForce(new Vector2((Mathf.Abs(difference * 200)), BallsManager.Instance.initialBallSpeed));        
            }
        }
        
    }
}
