using UnityEngine;
using System.Collections;

public class SwapScript : MonoBehaviour {

  public SortScript sortManager;

  private GameObject block1;
  private GameObject block2;

  private Vector3 block1Dest1;
  private Vector3 block1Dest2;
  private Vector3 block1Dest3;

  private Vector3 block2Dest1;
  private Vector3 block2Dest2;
  private Vector3 block2Dest3;

  private int currentPhase = -1;

  public float blockSpeed = 3f;

  public bool isPlayerSwap = false;


  public void StartSwap(GameObject obj1, GameObject obj2) {
    block1 = obj1;
    block2 = obj2;

    block1Dest1 = block1.transform.position + new Vector3(0,1,0);
    block1Dest2 = new Vector3(block2.transform.position.x, block1.transform.position.y + 1, 0);
    block1Dest3 = block2.transform.position;

    block2Dest1 = block2.transform.position + new Vector3(0,-1,0);
    block2Dest2 = new Vector3(block1.transform.position.x, block2.transform.position.y - 1, 0);
    block2Dest3 = block1.transform.position;

    currentPhase = 0;

  }


	// Update is called once per frame
	void Update () {
    if (currentPhase == 0) {
      if (Vector3.Distance(block1Dest1, block1.transform.position) > 0) {
        block1.transform.position = Vector3.MoveTowards(block1.transform.position, block1Dest1, blockSpeed * Time.deltaTime);
        block2.transform.position = Vector3.MoveTowards(block2.transform.position, block2Dest1, blockSpeed * Time.deltaTime);
      } else {
        currentPhase++;
      }
    } else if (currentPhase == 1) {
      if (Vector3.Distance(block1Dest2, block1.transform.position) > 0) {
        block1.transform.position = Vector3.MoveTowards(block1.transform.position, block1Dest2, blockSpeed * Time.deltaTime);
        block2.transform.position = Vector3.MoveTowards(block2.transform.position, block2Dest2, blockSpeed * Time.deltaTime);
      } else {
        currentPhase++;
      }
    } else if (currentPhase == 2) {
      if (Vector3.Distance(block1Dest3, block1.transform.position) > 0) {
        block1.transform.position = Vector3.MoveTowards(block1.transform.position, block1Dest3, blockSpeed * Time.deltaTime);
        block2.transform.position = Vector3.MoveTowards(block2.transform.position, block2Dest3, blockSpeed * Time.deltaTime);
      } else {
        currentPhase = -1;
        if (isPlayerSwap) sortManager.PlayerSwapDone();
        else sortManager.SwapDone();
      }
    }
	}
}
