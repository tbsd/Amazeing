using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIButtonsHandler : MonoBehaviour {
    public Button BoxBtn;
    public Button CylinderBtn;
    public Button SphereBtn;
    public Button CameraBtn;
    public CameraScript camera;
    
    // Start is called before the first frame update
    void Start() {
        BoxBtn.onClick.AddListener(delegate() { startLevel(0); });
        CylinderBtn.onClick.AddListener(delegate() { startLevel(1); });
        SphereBtn.onClick.AddListener(delegate() { startLevel(2); });
        CameraBtn.onClick.AddListener(delegate() { camera.restorePositon(); });
    }

    // Update is called once per frame
    void Update() {
    }

    void startLevel(int lvl) {
        if (lvl == 0) {
          SceneManager.LoadScene("CubeScene");
        }
        if (lvl == 1) {
          SceneManager.LoadScene("CylinderScene");
        }

        if (lvl == 2) {
          SceneManager.LoadScene("SphereScene");
        }
    }
}
