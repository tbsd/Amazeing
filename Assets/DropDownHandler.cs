using UnityEngine;
using MazeBuilderNs;
using GlobalsNs;
using UnityEngine.UI;

public class DropDownHandler : MonoBehaviour {
  public Dropdown dropdown;

  public void HandleInputData(int val) {
    // Globals globals = new Globals();
    switch (val) {
      case 0:
        Globals.mazeType = MazeType.HuntAndKill;
        Debug.Log("case 0");
        break;
      case 1:
        Globals.mazeType = MazeType.BinaryTree;
        Debug.Log("case 1");
        break;
    }
  }
}
