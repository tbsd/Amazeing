namespace MazeNs {
  public enum WallDirection {
    Up,
    Right,
    Down,
    Left
  }
  
  public enum NodeState {
      Path,
      Wall,
      Border,
      Start,
      Finish
  }

  public class MazeElement {
    public NodeState State {get; set;}
  }
}
