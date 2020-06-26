using System.Collections;
using System;
using System.Collections.Generic;
using MazeNs;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using MazeBuilderNs;
using GlobalsNs;


public class Maze : MonoBehaviour
{
  public Shape shape;
  public MazeType type;
  public GameObject Planet;
  public GameObject Player;
  public GameObject Finish;
  public GameObject wallObject;

  void Start() {
    type = Globals.mazeType;
    Screen.orientation = ScreenOrientation.LandscapeRight;
    switch(shape) { 
      case Shape.Cyllinder:
        cylinderMaze(10, 10);
        break;
      case Shape.Box:
        boxMaze(5);
        break;
      case Shape.Sphere: 
        sphereMaze(7);
        break;
     }
    Globals.mazeType = MazeType.HuntAndKill;
  }

  void Update() {
    playerIsOnFinish();
  }

  void generateMaze(MazeBuilder mazeBuilder) {
    Debug.Log("TYPE");
    Debug.Log(type);
    switch (type) {
      case MazeType.HuntAndKill:
        mazeBuilder.HuntAndKill();
        break;
      case MazeType.BinaryTree:
        mazeBuilder.BinaryTree();
        break;
    }
  }
   
  public void playerIsOnFinish() {
    if (Finish.GetComponent<PlayerDetection>().isColliding) {
      if (shape == Shape.Cyllinder) {
        SceneManager.LoadScene("CubeScene");
      }
      if (shape == Shape.Box) {
        SceneManager.LoadScene("SphereScene");
      }
      if (shape == Shape.Sphere) {
        SceneManager.LoadScene("CylinderScene");
      }
    }
  }

  private void sphereMaze(int radius) {
    MazeBuilder mazeBuilder = new MazeBuilder(radius);
    mazeBuilder.InitMaze(Shape.Sphere);
    generateMaze(mazeBuilder);
    Debug.Log(mazeBuilder.toString());
    int topRadius = mazeBuilder.ActualHeight / 2 + 1;
    int bottomRadius = mazeBuilder.ActualHeight / 2;
    GameObject builder = Instantiate(GameObject.CreatePrimitive(PrimitiveType.Sphere), Planet.transform);
    builder.transform.rotation = Quaternion.identity;
    builder.transform.localPosition = new Vector3(0, 0.5f, 0);
    builder.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
    Quaternion posRotation = builder.transform.rotation;
    float vAngleSpace = 188f;
    drawHemisphere(mazeBuilder, topRadius, topRadius - 1, topRadius - 1, builder, (vAngleSpace * topRadius) / (topRadius + bottomRadius), 1);
    builder.transform.RotateAround(Planet.transform.localPosition, Planet.transform.right, 90f);
    builder.transform.Rotate(new Vector3(0, -90, 0));
    drawHemisphere(mazeBuilder, bottomRadius, bottomRadius, mazeBuilder.ActualHeight + bottomRadius - 1, builder, (vAngleSpace * bottomRadius) / (topRadius + bottomRadius), -1);
    GameObject.Destroy(builder);
    GameObject.Destroy(wallObject);
  }

  private void drawHemisphere(MazeBuilder mazeBuilder, int actualRadius, int xCenter, int yCenter, GameObject builder, float vAngle, int hDirection) {
    MazeElement[,] graph = mazeBuilder.Maze;
    int diametr = mazeBuilder.Height;
    float vRotationStep = vAngle / actualRadius;
    Debug.Log(mazeBuilder.toString());
    int xMax, yMax, xMin, yMin;
    xMax = xMin = xCenter;
    yMax = yMin = yCenter;
    for (int i = 0; i < actualRadius; ++i) { 
      int xInc = 1;
      int yInc = 0;
      int xPos = xMin;
      int yPos = yMin;
      int rowCount = (i * 2 + 1) * (i * 2 + 1) - (i * 2 - 1) * (i * 2 - 1);
      if (rowCount == 0)
        rowCount = 1;
      float h = 0.5f - builder.transform.localPosition.y;
      float rowLength = 2 * Mathf.PI * Mathf.Sqrt(2 * 0.5f * h - h * h);
      if (rowLength < 0.05f) 
        rowLength = 0.05f;
      float hRotationStep = hDirection * 360f / rowCount;
      float wallWidth = (rowLength / rowCount) * 0.9f;
      if (i == 0)
        wallWidth = 0.05f;
      Vector3 wallScale = new Vector3(wallWidth, .1f, 1f / actualRadius);
      do {
        MazeElement mazePart = graph[xPos, yPos];
        if (xPos == xMax && xInc == 1) {
          xInc = 0;
          yInc = 1;
        }
        if (yPos == yMax && yInc == 1) {
          xInc = -1;
          yInc = 0;
        }
        if (xPos == xMin && xInc == -1) {
          xInc = 0;
          yInc = -1;
        }
        if (yPos == yMin && yInc == -1) {
          xInc = 1;
          yInc = 0;
        }
        int xNext;
        int yNext;
        if (xMin == xMax) {
          xNext = xPos;
          yNext = yPos;
        }
        else {
          xNext = xPos + xInc;
          yNext = yPos + yInc;
        }

        if (mazePart.State == NodeState.Wall) {
          GameObject wallPart = GameObject.Instantiate(wallObject);
          wallPart.transform.SetParent(Planet.transform);
          wallPart.transform.localScale = wallScale;
          wallPart.transform.position = builder.transform.position;
          if (xNext < diametr && yNext < diametr && graph[xNext, yNext].State == NodeState.Path) {
            wallPart.transform.localScale = new Vector3(wallScale.x * 0.6f, wallScale.y, wallScale.z);
            wallPart.transform.RotateAround(Planet.transform.localPosition, Planet.transform.up, -hRotationStep * 0.3f);
          }
          wallPart.transform.rotation = builder.transform.rotation;
          wallPart.layer = LayerMask.NameToLayer ("Ignore Raycast");
        } else if (mazePart.State == NodeState.Finish) {
          Finish.transform.SetParent(Planet.transform);
          Finish.transform.localScale = wallScale * 0.9f;
          Finish.transform.position = builder.transform.position;
          Finish.transform.rotation = builder.transform.rotation;
          Finish.transform.SetParent(null);
        } else if (mazePart.State == NodeState.Start) {
            Player.transform.position = builder.transform.position;
            Player.transform.rotation = builder.transform.rotation;
          }

        builder.transform.RotateAround(Planet.transform.localPosition, Planet.transform.up, hRotationStep);
        xPos = xNext;
        yPos = yNext;
      } while (!(xPos == xMin && yPos == yMin));
      --xMin;
      --yMin;
      ++xMax;
      ++yMax;
      builder.transform.RotateAround(Planet.transform.localPosition, builder.transform.right, vRotationStep); 
    }
  }

  void boxMaze(int edgeSize) {
    MazeBuilder mazeBuilder = new MazeBuilder(edgeSize);
    mazeBuilder.InitMaze(Shape.Box);
    generateMaze(mazeBuilder);
    MazeElement[,] graph = mazeBuilder.Maze;
    int actualEdge = edgeSize * 2;
    GameObject builder = Instantiate(GameObject.CreatePrimitive(PrimitiveType.Sphere), Planet.transform);
    builder.transform.rotation = Quaternion.identity;
    Vector3 wallScale = new Vector3(1f / actualEdge, 1f / actualEdge, 1f / actualEdge);
    float posStep = 1.0f / actualEdge;
    for (int g = 0; g < 6; ++g) {
      Vector3 minPos;
      Vector3 vStep;
      Vector3 hStep;
      int hStart;
      int vStart;
      switch (g) {
        case 0:
          minPos = new Vector3(-0.5f + posStep / 2f, 0.5f + posStep / 2f, -0.5f + posStep / 2f);
          vStep = new Vector3(posStep, 0, 0);
          hStep = new Vector3(0, 0, posStep);
          vStart = 0;
          hStart = actualEdge;
          break;
        case 1:
          minPos = new Vector3(0.5f + posStep / 2f, 0.5f - posStep / 2f, -0.5f + posStep / 2f);
          builder.transform.Rotate(0, 0, -90);
          vStep = new Vector3(0, -posStep, 0);
          hStep = new Vector3(0, 0, posStep);
          vStart = actualEdge;
          hStart = actualEdge;
          break;
        case 2:
          minPos = new Vector3(0.5f - posStep / 2f, 0.5f - posStep / 2f, 0.5f + posStep / 2f);
          builder.transform.Rotate(0, -90, 0);
          vStep = new Vector3(0, -posStep, 0);
          hStep = new Vector3(-posStep, 0, 0);
          vStart = actualEdge;
          hStart = actualEdge * 2;
          break;
        case 3:
          minPos = new Vector3(-0.5f - posStep / 2f, 0.5f - posStep / 2f, 0.5f - posStep / 2f);
          builder.transform.Rotate(0, 0, -90);
          vStep = new Vector3(0, -posStep, 0);
          hStep = new Vector3(0, 0, -posStep);
          vStart = actualEdge;
          hStart = actualEdge * 3;
          break;
        case 4:
          minPos = new Vector3(-0.5f + posStep / 2f, 0.5f - posStep / 2f, -0.5f - posStep / 2f);
          builder.transform.Rotate(0, 0, -90);
          vStep = new Vector3(0, -posStep, 0);
          hStep = new Vector3(posStep, 0, 0);
          vStart = actualEdge;
          hStart = 0;
          break;
        case 5:
          minPos = new Vector3(0.5f - posStep / 2f, -0.5f - posStep / 2f, -0.5f + posStep / 2f);
          builder.transform.Rotate(-90, 0, -90);
          vStep = new Vector3(-posStep, 0, 0);
          hStep = new Vector3(0, 0, posStep);
          vStart = actualEdge * 2;
          hStart = actualEdge;
          break;
        default:
          return;
      }
      for (int i = vStart; i < vStart + actualEdge; ++i) {
        builder.transform.localPosition = minPos;
        builder.transform.localPosition += vStep * (i - vStart);
        for (int j = hStart; j < hStart + actualEdge; ++j) {
          if (graph[i, j].State == NodeState.Wall) {
            GameObject wallPart = GameObject.Instantiate(wallObject);
            wallPart.transform.SetParent(Planet.transform);
            wallPart.transform.localScale = wallScale;
            wallPart.transform.position = builder.transform.position;
            wallPart.transform.rotation = builder.transform.rotation;
            wallPart.layer = LayerMask.NameToLayer ("Ignore Raycast");
          } else if (graph[i, j].State == NodeState.Finish) {
            Finish.transform.SetParent(Planet.transform);
            Finish.transform.localScale = wallScale;
            Finish.transform.position = builder.transform.position;
            Finish.transform.rotation = builder.transform.rotation;
            Finish.transform.SetParent(null);
          } else if (graph[i, j].State == NodeState.Start) {
            Player.transform.position = builder.transform.position;
            Player.transform.rotation = builder.transform.rotation;
          }
          builder.transform.localPosition += hStep;
        }
      }
    }
    GameObject.Destroy(builder);
    GameObject.Destroy(wallObject);
  }
  
  void cylinderMaze(int mazeWidth, int mazeHeight) {
    MazeBuilder mazeBuilder = new MazeBuilder(mazeHeight, mazeWidth);
    mazeBuilder.InitMaze(Shape.Cyllinder);
    generateMaze(mazeBuilder);
    MazeElement[,] graph = mazeBuilder.Maze;
    int height = mazeBuilder.ActualHeight;
    int width = mazeBuilder.ActualWidth;
    Vector3 minX = new Vector3(-0.5f, 0, 0);
    Vector3 maxX = new Vector3(-0.5f, 0, 0);
    Vector3 minY = new Vector3(0, -1, 0);
    Vector3 maxY = new Vector3(0, 1, 0);
    Vector3 minZ = new Vector3(0, 0, 0);
    Vector3 maxZ = new Vector3(0, 0, 0);
    GameObject builder = Instantiate(GameObject.CreatePrimitive(PrimitiveType.Sphere), Planet.transform);
    Vector3 wallScale = new Vector3(16.0f / height, wallObject.transform.lossyScale.y, 1.2f * (Mathf.PI * 2 * 8 * 0.5f / width));
    Vector3 minPos = new Vector3(-0.5f, -1 + 1f / height, 0);
    Vector3 maxPos = new Vector3(-0.5f, 1 + 1f / height, 0);
    builder.transform.localPosition = minPos;
    builder.transform.rotation = Quaternion.identity;
    Quaternion initRotation = builder.transform.rotation;
    float pos = builder.transform.position.y;
    float posStep = 2.0f / height;
    float rotStep = 360.0f /  width;
    for (int i = 0; i < height; ++i) {
      for (int j = 0; j < width; ++j) {
        if(graph[i, j].State == NodeState.Wall || graph[i, j].State == NodeState.Border) {
          GameObject wallPart = GameObject.Instantiate(wallObject);
          wallPart.transform.localScale = wallScale;
          if (i == 0 || i == height - 1) 
            wallPart.transform.localScale += new Vector3(wallScale.x / 10f, 0, 0);
          wallPart.transform.SetParent(Planet.transform);
          wallPart.transform.position = builder.transform.position;
          wallPart.transform.rotation = builder.transform.rotation;
        } else if (graph[i, j].State == NodeState.Finish) {
          Finish.transform.localScale = wallScale - new Vector3(0, 0.1f, 0);
          Finish.transform.position = builder.transform.position;
          Finish.transform.rotation = builder.transform.rotation;
        } else if (i > height / 2 && j > width / 2) {
          // skip cell for player to start in cell and not in wall
        } else if (graph[i, j].State == NodeState.Start) {
            Player.transform.position = builder.transform.position;
            Player.transform.rotation = builder.transform.rotation;
          }

        builder.transform.RotateAround(Planet.transform.localPosition, Planet.transform.up, rotStep);
      }
      builder.transform.localPosition += new Vector3(0, posStep, 0);
      pos = builder.transform.localPosition.y;
    }
    GameObject.Destroy(builder);
    GameObject.Destroy(wallObject);
  }
}
