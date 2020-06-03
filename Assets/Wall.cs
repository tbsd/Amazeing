using System.Collections;
using System.Linq;
using System.Collections.Generic;

namespace MazeNs {
  public class Wall : MazeElement {
    private List<Node> connected = new List<Node>(); 

    public Wall() {
      State = NodeState.Wall;
    }

    public void AddConnected(Node elem) {
      if (!connected.Contains(elem)) 
        connected.Add(elem);
      elem.AddWall(this);
    }

    public void AddConnected(Node elem1, Node elem2) {
      AddConnected(elem1);
      AddConnected(elem2);
    }

    public Node getConnected(Node elem) {
      List<Node> others = connected.Where(e => e != elem).ToList();
      if (others.Capacity > 0)
        return others[0];
      return null;
    }

    public override string ToString() {
      return connected.Capacity.ToString();
    }
  }
}
