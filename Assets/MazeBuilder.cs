using System.Collections;
using System.Collections.Generic;
using MazeNs;
using UnityEngine;

namespace MazeBuilderNs {
  public enum Shape {
    Cyllinder,
    Box,
    Sphere
  }

  public enum MazeType {
    HuntAndKill,
    BinaryTree
  }

  public class MazeBuilder {
    private MazeElement[,] maze;
    public int Height {get;set;}
    public int StepsLimit;
    public int Width {get;set;}
    private int actualHeight;
    private int actualWidth;

    public int ActualHeight {
      get {
        return actualHeight;
      }
    }
    public int ActualWidth {
      get {
        return actualWidth;
      } }

    public MazeElement[,] Maze {
      get {
        return maze;
      }
    }
    
    public MazeBuilder(int edgeSize) {
      Height = edgeSize;
      Width = edgeSize;
      StepsLimit = edgeSize * 3;
    }

    public MazeBuilder(int height, int width) {
      Height = height;
      Width = width;
      StepsLimit = width * 3;
    }


    public void InitMaze(Shape shape) {
      switch (shape) {
        case Shape.Cyllinder:
          initCyllinder();
          break;
        case Shape.Box:
          initBox();
          break;
        case Shape.Sphere:
          initSphere();
          break;
      }
    }

    private void initSphere() {
      Height = Height * 2 + 1;
      Width = Height;
      actualHeight = Height;
      actualWidth = Width * 2 - 2;
      maze = new MazeElement[ActualHeight, ActualWidth];
      for (int i = 0; i < ActualHeight; ++i) {
        for (int j = 0; j < Width; ++j) {
          if (i % 2 == 0 || j % 2 == 0)
            maze[i, j] = new Wall();
          else
            maze[i, j] = new Node();
        }
      }
      for (int i = 1; i < ActualHeight - 1; ++i) {
        for (int j = Width; j < ActualWidth; ++j) {
          if (i % 2 == 0 || j % 2 == 0)
            maze[i, j] = new Wall();
          else
            maze[i, j] = new Node();
        }
      }
      Debug.Log(toString());
      for (int i = 0; i < ActualHeight; ++i) {
        for (int j = 0; j < ActualWidth; ++j) {
          if (maze[i, j] != null && maze[i, j] is Node) {
            Node cell = (Node) maze[i, j];
            Wall wall;
            if (i > 1 || j < Width) {
              wall = (Wall) maze[i - 1, j];
            } else {
              wall = (Wall) maze[i - 1, Width * 2 - 2 - j];
            }
            wall.AddConnected(cell);
            cell.AddWall(wall);
            if (i < ActualHeight - 2 || j < Width) 
              wall = (Wall)maze[i + 1, j];
            else 
              wall = (Wall)maze[i + 1, Width * 2 - 2 - j];
            wall.AddConnected(cell);
            cell.AddWall(wall);
            if (j > 0) 
              wall = (Wall)maze[i, j - 1];
            else 
              wall = (Wall)maze[i, ActualWidth - 1];
            wall.AddConnected(cell);
            cell.AddWall(wall);
            if (j < ActualWidth - 2) 
              wall = (Wall)maze[i, j + 1];
            else 
              wall = (Wall)maze[i, 0];
            wall.AddConnected(cell);
            cell.AddWall(wall);
          }
        }
      }
    }
    
    private void initBox() {
      int edge = Width * 2;
      actualHeight = 3 * edge;
      actualWidth = 4 * edge;
      maze = new MazeElement[ActualHeight, ActualWidth];
      for (int i = 0; i < edge; ++i)
        for (int j = edge; j < edge * 2; ++j)
          if (i % 2 == 1 || j % 2 == 1)
            maze[i, j] = new Wall();
          else
            maze[i, j] = new Node();
      for (int i = edge; i < edge * 2; ++i) 
        for (int j = 0; j < edge * 4; ++j)
          if (i % 2 == 1 || j % 2 == 1)
            maze[i, j] = new Wall();
          else
            maze[i, j] = new Node();
      for (int i = edge * 2; i < edge * 3; ++i) 
        for (int j = edge; j < edge * 2; ++j)
          if (i % 2 == 1 || j % 2 == 1)
            maze[i, j] = new Wall();
          else
            maze[i, j] = new Node();
      Debug.Log(toString());
      for (int i = 0; i < ActualHeight; ++i)
        for (int j = 0; j < ActualWidth; ++j)
          if (maze[i, j] != null  && maze[i, j] is Node) {
            Node cell = (Node)maze[i, j];
            if (i != 0 && maze[i - 1, j] != null) {
              Wall wall = (Wall)maze[i - 1, j];
              cell.AddWall(wall);
              wall.AddConnected(cell);
            }
            if (i != ActualHeight - 1 && maze[i + 1, j] != null) {
              Wall wall = (Wall)maze[i + 1, j];
              cell.AddWall(wall);
              wall.AddConnected(cell);
            }
            if (j != 0 && maze[i, j - 1] != null) {
              Wall wall = (Wall)maze[i, j - 1];
              cell.AddWall(wall);
              wall.AddConnected(cell);
            }
            if (j != ActualWidth - 1 && maze[i, j + 1] != null) {
              Wall wall = (Wall)maze[i, j + 1];
              cell.AddWall(wall);
              wall.AddConnected(cell);
            }
          }
      //00 12
      for (int i = 0; i < edge; ++i) {
        Wall wall;
        Node cell;
        if (maze[i, edge * 2 - 1] is Node && maze[edge, edge * 3 - 1 - i] is Node)
          continue;
        if (maze[i, edge * 2 - 1] is Wall && maze[edge, edge * 3 - 1 - i] is Wall)
          continue;
        if (maze[i, edge * 2 - 1] is Wall) {
              wall = (Wall)maze[i, edge * 2 - 1];
              cell = (Node)maze[edge, edge * 3 - 1 - i];
        } else  {
              cell = (Node)maze[i, edge * 2 - 1];
              wall = (Wall)maze[edge, edge * 3 - 1 - i];
        }
        cell.AddWall(wall);
        wall.AddConnected(cell);
      }
      //00 10
      for (int i = 0; i < edge; ++i) {
        Wall wall;
        Node cell;
        if (maze[edge, i] is Node && maze[i, edge] is Node) 
          continue;
        if (maze[edge, i] is Wall && maze[i, edge] is Wall) 
          continue;
        if (maze[i, i] is Wall) {
              wall = (Wall)maze[i, edge];
              cell = (Node)maze[edge, i];
        } else  {
              cell = (Node)maze[edge, i];
              wall = (Wall)maze[i, edge];
        }
        cell.AddWall(wall);
        wall.AddConnected(cell);
      }
      //00 13
      for (int i = 0; i < edge; ++i) {
        Wall wall;
        Node cell;
            Debug.Log("i = " + i);
        if (maze[edge, edge * 4 - 1 - i] is Node && maze[0, edge + i] is Node)
          continue;
        if (maze[edge, edge * 4 - 1 - i] is Wall && maze[0, edge + i] is Wall)
          continue;
        if (maze[0, edge + i] is Wall) {
              wall = (Wall)maze[0, edge + i];
              cell = (Node)maze[edge, edge * 4 - 1 - i];
        } else  {
              cell = (Node)maze[0, edge + i];
              wall = (Wall)maze[edge, edge * 4 - 1 - i];
        }
        cell.AddWall(wall);
        wall.AddConnected(cell);
      }
      //10 13
      for (int i = 0; i < edge; ++i) {
        Wall wall;
        Node cell;
        if (maze[edge + i, edge * 4 - 1] is Node && maze[edge + i, 0] is Node)
          continue;
        if (maze[edge + i, edge * 4 - 1] is Wall && maze[edge + i, 0] is Wall)
          continue;
        if (maze[edge + i, 0] is Wall) {
              wall = (Wall)maze[edge + i, 0];
              cell = (Node)maze[edge + i, edge * 4 - 1];
        } else  {
              cell = (Node)maze[edge + i, 0];
              wall = (Wall)maze[edge + i, edge * 4 - 1];
        }
        cell.AddWall(wall);
        wall.AddConnected(cell);
      }
      //20 13
      for (int i = 0; i < edge; ++i) {
        Wall wall;
        Node cell;
        if (maze[edge * 2 - 1, edge * 4 - 1 - i] is Node && maze[edge * 3 - 1, edge + i] is Node)
          continue;
        if (maze[edge * 2 - 1, edge * 4 - 1 - i] is Wall && maze[edge * 3 - 1, edge + i] is Wall)
          continue;
        if (maze[edge * 3 - 1, edge + i] is Wall) {
              wall = (Wall)maze[edge * 3 - 1, edge + i];
              cell = (Node)maze[edge * 2 - 1, edge * 4 - 1 - i];
        } else  {
              cell = (Node)maze[edge * 3 - 1, edge + i];
              wall = (Wall)maze[edge * 2 - 1, edge * 4 - 1 - i];
        }
        cell.AddWall(wall);
        wall.AddConnected(cell);
      }
      //20 12
      for (int i = 0; i < edge; ++i) {
        Wall wall;
        Node cell;
        if (maze[edge * 2 - 1, edge * 2 + i] is Node && maze[edge * 2 + i, edge * 2 - 1] is Node)
          continue;
        if (maze[edge * 2 - 1, edge * 2 + i] is Wall && maze[edge * 2 + i, edge * 2 - 1] is Wall)
          continue;
        if (maze[edge * 2 + i, edge * 2 - 1] is Wall) {
              wall = (Wall)maze[edge * 2 + i, edge * 2 - 1];
              cell = (Node)maze[edge * 2 - 1, edge * 2 + i];
        } else  {
              cell = (Node)maze[edge * 2 + i, edge * 2 - 1];
              wall = (Wall)maze[edge * 2 - 1, edge * 2 + i];
        }
        cell.AddWall(wall);
        wall.AddConnected(cell);
      }
      //20 10
      for (int i = 0; i < edge; ++i) {
        Wall wall;
        Node cell;
        if (maze[edge * 2 - 1, edge - 1 - i] is Node && maze[edge * 2 + i, edge] is Node)
          continue;
        if (maze[edge * 2 - 1, edge - 1 - i] is Wall && maze[edge * 2 + i, edge] is Wall)
          continue;
        if (maze[edge * 2 + i, edge] is Wall) {
              wall = (Wall)maze[edge * 2 + i, edge];
              cell = (Node)maze[edge * 2 - 1, edge - 1 - i];
        } else  {
              cell = (Node)maze[edge * 2 + i, edge];
              wall = (Wall)maze[edge * 2 - 1, edge - 1 - i];
        }
        cell.AddWall(wall);
        wall.AddConnected(cell);
      }
    }

    private void initCyllinder() {
      actualWidth = 2 * Width;
      actualHeight = 2 * Height + 1;
      maze = new MazeElement[ActualHeight, ActualWidth];
      for (int i = 0; i < ActualHeight; ++i) {
        for (int j = 0; j < ActualWidth; ++j) {
          if (i % 2 == 0 || j % 2 == 0 ) {
            maze[i, j] = new Wall();
          }
          else { 
            maze[i, j] = new Node();
          }
          if (i == 0 || i == ActualHeight - 1)
            maze[i, j].State = NodeState.Border;
        }
      }
      for (int i = 1; i < ActualHeight - 1; ++i) {
        for (int j = 0; j < ActualWidth; ++j) {
          if (maze[i, j] is Node) {
            Node cell = (Node)maze[i, j];
            if (i - 1 >= 0 && maze[i - 1, j] is Wall) {
              Wall wall = (Wall)maze[i - 1, j];
              cell.AddWall(wall);
              wall.AddConnected(cell);
            }
            if (i + 1 < ActualHeight && maze[i + 1, j] is Wall) {
              Wall wall = (Wall)maze[i + 1, j];
              cell.AddWall(wall);
              wall.AddConnected(cell);
            }
            if (j - 1 >= 0 && maze[i, j - 1] is Wall) {
              Wall wall = (Wall)maze[i, j - 1];
              cell.AddWall(wall);
              wall.AddConnected(cell);
            } 
            if (j + 1 < ActualWidth && maze[i, j + 1] is Wall) {
              Wall wall = (Wall)maze[i, j + 1];
              cell.AddWall(wall);
              wall.AddConnected(cell);
            } 
          }
        }
      }
      for (int i = 0; i < ActualHeight; ++i)
        if (maze[i, ActualWidth - 1] is Node) {
              Wall wall = (Wall)maze[i, 0];
              Node cell1 = (Node)maze[i, ActualWidth - 1];
              Node cell2 = (Node)maze[i, 1];
              cell1.AddWall(wall);
              cell2.AddWall(wall);
              wall.AddConnected(cell1);
              wall.AddConnected(cell2);
        }
    }


    public void HuntAndKill() {
      System.Random rng = new System.Random();
      int x = 0;
      int y = 0;
      Node current = null;
      for (int i = ActualHeight / 2; i < ActualHeight - 1; ++i) {
        for (int j = ActualWidth / 2 + 1; j < ActualWidth - 1; ++j) {
          MazeElement start = maze[i, j];
          if (start != null && start is Node) {
            current = (Node)start;
            current.State = NodeState.Start;
            break;
          }
          Debug.Log(1);
        }
        Debug.Log(2);
        if (current != null)
          break;
      }
      //add start
      int stepsCount = 0;
      bool isFinishSet = false;
      Node last;
      while (true) {
        current.IsVisited = true;
        last = current;
        if (stepsCount < StepsLimit) {
          current = current.goToRandomNeighbour();
          ++stepsCount;
        } else {
          current = null;
          stepsCount = 0;
        }
        if (current == null) {
          if (!isFinishSet && rng.Next(0, 5) == 0) {
            last.State = NodeState.Finish;
            isFinishSet = true;
          }
          // loop through maze starting from random place
          int k = rng.Next(0, ActualHeight);
          int t = rng.Next(0, ActualWidth);
          for (; k < ActualHeight * 2 - 1; ++k) {
            for (; t < ActualWidth * 2 - 1; ++t) {
              int i = k % ActualHeight;
              int j = t % ActualWidth;
              if (maze[i, j] != null && maze[i, j] is Node) {
                if (!((Node)maze[i, j]).IsVisited && ((Node)maze[i, j]).HasVisitedNeighbour()) {
                  current = (Node)maze[i, j];
                  current.removeWallWithVisited();
                  break;
                }
              }
            }
            t = 0;
            if (current != null)
              break;
          }
        }
        if (current == null)
          break;
      }
      if (!isFinishSet && last != null) {
        last.State = NodeState.Finish;
        isFinishSet = true;
        }
      Debug.Log(toString());
    }

    public void BinaryTree() {
      System.Random rng = new System.Random();
      int x = 0;
      int y = 0;
      Node current = null;
      for (int i = ActualHeight / 2; i < ActualHeight - 1; ++i) {
        for (int j = ActualWidth / 2 + 1; j < ActualWidth - 1; ++j) {
          MazeElement start = maze[i, j];
          if (start != null && start is Node) {
            current = (Node)start;
            current.State = NodeState.Start;
            break;
          }
        }
        if (current != null)
          break;
      }
      //add finish
      bool isFinishSet = false;
      for (int i = rng.Next(0, ActualHeight / 2); i < ActualHeight - 1; ++i) {
        for (int j = rng.Next(0, ActualHeight / 2 + 1); j < ActualWidth - 1; ++j) {
          MazeElement end = maze[i, j];
          if (end != null && end is Node) {
            end.State = NodeState.Finish;
            isFinishSet = true;
            break;
          }
        }
        if (isFinishSet)
          break;
      }
      //add start
      int stepsCount = 0;
      // Node last;
      for (int posY = 0; posY < ActualHeight - 1; ++posY) {
        for (int posX = 0; posX < ActualWidth - 1; ++posX) {
          if (!(maze[posY, posX] is Node))
            continue;
          current = (Node)maze[posY, posX];
          current.IsVisited = true;
          // last = current;
          if (stepsCount < StepsLimit) {
            // !!!!!!!!!!!!!!сделать костыль для стенок слева и сверху в крайних рядах
            // choose wall to remove
            Wall right = null;
            Wall bottom = null;
            List<Wall> walls = current.getWalls();
            foreach (Wall wall in walls) {
              if (wall.State == NodeState.Border)
                continue;
              // Node connected = wall.getConnected(current);
              // if (posX + 2 < ActualWidth && maze[posY, posX + 2] != null && connected == (Node)maze[posY, posX + 2])
              if (posX + 1 < ActualWidth && maze[posY, posX + 1] == wall)
                right = wall;
              // else if (posY + 2 < ActualHeight && maze[posY + 2, posX] != null && connected == (Node)maze[posY + 2, posX])
              else if (posY + 1 < ActualHeight && maze[posY + 1, posX] == wall)
                bottom = wall;
              // for not adjacent walls
              // else if (posX > 0 && maze[posY, posX - 1] != null &&
                  // wall == (Wall)maze[posY, posX - 1])
            }
            if (right == null && bottom != null) {
              bottom.State = NodeState.Path;
            } else if (bottom == null && right != null) {
              right.State = NodeState.Path;
            } else if (right != null && bottom != null){
              if (rng.Next(0, 2) == 0) {
                bottom.State = NodeState.Path;
              } else {
                right.State = NodeState.Path;
              }
            }
            ++stepsCount;
          } else {
            current = null;
            stepsCount = 0;
          }
          if (current == null) {
            //create finishcreate finishcreate finishcreate finishcreate finishcreate finishcreate finishcreate finishcreate finishcreate finishcreate finishcreate finishcreate finishcreate finishcreate finishcreate finishcreate finishcreate finishcreate finishcreate finish
            // if (!isFinishSet && rng.Next(0, 5) == 0) {
              // last.State = NodeState.Finish;
              // isFinishSet = true;
            // }
            // loop through maze starting from random place
            int k = rng.Next(0, ActualHeight);
            int t = rng.Next(0, ActualWidth);
            for (; k < ActualHeight * 2 - 1; ++k) {
              for (; t < ActualWidth * 2 - 1; ++t) {
                int i = k % (ActualHeight - 1);
                int j = t % (ActualWidth - 1);
                if (maze[i, j] != null && maze[i, j] is Node) {
                  if (!((Node)maze[i, j]).IsVisited && ((Node)maze[i, j]).HasVisitedNeighbour()) {
                    current = (Node)maze[i, j];
                    current.removeWallWithVisited();
                    break;
                  }
                }
              }
              t = 0;
              if (current != null)
                break;
            }
          }
          if (current == null)
            break;
        }
      }
            //create finishcreate finishcreate finishcreate finishcreate finishcreate finishcreate finishcreate finishcreate finishcreate finishcreate finishcreate finishcreate finishcreate finishcreate finishcreate finishcreate finishcreate finishcreate finishcreate finish
      // if (!isFinishSet && last != null) {
        // last.State = NodeState.Finish;
        // isFinishSet = true;
        // }
      Debug.Log(toString());
    }

    public string toString() {
      string str = "";
      for (int i = 0; i < actualHeight; i++) {
        for (int j = 0; j < actualWidth; j++) {
          if (maze[i, j] == null)
            str += "░";
          else if (maze[i, j].State == NodeState.Wall)
            str += "▓";
          else
            str += "▒";
        }
        str += "\n";
      }
      return str;
    }
  }
}
