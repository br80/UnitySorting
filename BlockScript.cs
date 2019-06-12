using UnityEngine;
using System.Collections;

public class BlockScript : MonoBehaviour {

  public int index;
  public int val;
  public bool clickable;
  public SortScript sortScript;

  void OnMouseDown() {
    if (clickable) {
      sortScript.PlayerBlockClicked(index);
    }
  }

}
