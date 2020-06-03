using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Graph {

  public class Node : MazeElement {

    private List<Wall> walls = new List<Wall>();
    public bool IsVisited {get; set;} = false;

    public Node() {
      State = NodeState.Path;
    }

    public void AddWall(Wall wall) {
      if (!walls.Contains(wall))
        walls.Add(wall);
    }

    public Wall getWall(int i) {
      return walls[i];
    }

    public Node goToRandomNeighbour() {
      System.Random rng = new System.Random();
      int unvisetedCount = walls.Where(w => w.getConnected(this) != null && !w.getConnected(this).IsVisited).Count();
      if (unvisetedCount == 0)
        return null;
      int selected = rng.Next(0, unvisetedCount);
      Node selectedNode;
      while (true) {
        if (walls[selected].getConnected(this) != null && !walls[selected].getConnected(this).IsVisited)
          break;
        else {
          ++selected;
          if (selected == walls.Capacity)
            selected = 0;
        }
      }
    walls[selected].State = NodeState.Path;
    return walls[selected].getConnected(this);
      
    }

    public void removeWallWithVisited() {
      foreach (Wall wall in walls)
        if (wall.getConnected(this) != null && wall.getConnected(this).IsVisited) {
          wall.State = NodeState.Path;
          return;
        }
    }

    public bool HasVisitedNeighbour() {
      foreach (Wall wall in walls)
        if (wall.getConnected(this) != null && wall.getConnected(this).IsVisited)
          return true;
      return false;
    }

  }
}
