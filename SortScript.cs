using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class SortScript : MonoBehaviour {

  public GameObject blockPrefab;
  private int numBlocks = 10;

  GameObject[] blocks;
  GameObject[] playerBlocks;

  Queue<int[]> swapQueue;

  SwapScript swapScript;
  public SwapScript playerSwapScript;

  private bool gameOver = false;

  public Text text;


	// Use this for initialization
	void Start () {
    float startX = -numBlocks / 2f;
    blocks = new GameObject[numBlocks];
    playerBlocks = new GameObject[numBlocks];

    swapQueue = new Queue<int[]>();
    swapScript = gameObject.GetComponent<SwapScript>();
    swapScript.sortManager = this;

    // Make int arr and shuffle
    int[] arr = new int[numBlocks];
    for (int i = 0 ; i < arr.Length ; i++) {
      arr[i] = i;
    }
    // shuffle
    for (int i = arr.Length - 1 ; i >= 0 ; i--) {
      int randomIndex = Mathf.FloorToInt(Random.value * i);
      int placeholder = arr[i];
      arr[i] = arr[randomIndex];
      arr[randomIndex] = placeholder;
    }

    for (int i = 0 ; i < numBlocks ; i++) {
      float colorVal = 1f * arr[i] / numBlocks;
      blocks[i] = createBlock(new Vector3(startX + i, 3, 0), arr[i], new Color (0, colorVal, colorVal), false);
      playerBlocks[i] = createBlock(new Vector3(startX + i, 0, 0), arr[i], new Color (colorVal, colorVal, colorVal), true);
      playerBlocks[i].GetComponent<BlockScript>().val = arr[i];
      playerBlocks[i].GetComponent<BlockScript>().index = i;
    }

    arr = bubbleSort(arr);
    int[] swap = swapQueue.Dequeue();
    startBlockSwap(swap[0], swap[1]);


	}


  private bool isSwapping = false;
  public int playerBlockIndex1 = -1;
  public int playerBlockIndex2 = -1;

  public void PlayerBlockClicked(int index) {
    if (isSwapping || gameOver) return;
    if (playerBlockIndex1 == -1) {
      playerBlockIndex1 = index;
    } else {
      playerBlockIndex2 = index;

      // Do swap
      playerSwapScript.StartSwap(playerBlocks[playerBlockIndex1], playerBlocks[playerBlockIndex2]);
      playerBlocks[playerBlockIndex1].GetComponent<BlockScript>().index = playerBlockIndex2;
      playerBlocks[playerBlockIndex2].GetComponent<BlockScript>().index = playerBlockIndex1;
      GameObject placeholder = playerBlocks[playerBlockIndex1];
      playerBlocks[playerBlockIndex1] = playerBlocks[playerBlockIndex2];
      playerBlocks[playerBlockIndex2] = placeholder;

      if (playerBlocksSorted() && !gameOver) {
        if (swapQueue.Count > 0) text.text = "You Win!";
        gameOver = true;
      }
      playerBlockIndex1 = -1;
      playerBlockIndex2 = -1;
    }
  }

  private bool playerBlocksSorted() {
    for (int i = 0 ; i < playerBlocks.Length - 1; i++) {
      BlockScript block1 = playerBlocks[i].GetComponent<BlockScript>();
      BlockScript block2 = playerBlocks[i+1].GetComponent<BlockScript>();
      if (block1.val > block2.val) return false;
    }
    return true;
  }

  public void PlayerSwapDone() {
    isSwapping = false;
  }

  private GameObject createBlock(Vector3 position, int index, Color color, bool isClickable) {
    GameObject obj = (GameObject) Instantiate(blockPrefab, position, Quaternion.identity);
    obj.GetComponent<MeshRenderer>().material.color = color;
    obj.GetComponent<BlockScript>().index = index;
    if (isClickable) {
      obj.GetComponent<BlockScript>().clickable = true;
      obj.GetComponent<BlockScript>().sortScript = this;
    }

    return obj;
  }

  private int[] bubbleSort(int[] arr) {
    while (true) {
      bool hasSwapped = false;

      for (int i = 0 ; i < arr.Length - 1 ; i++) {
        if (arr[i] > arr[i + 1]) {
          arr = doSwap(arr, i, i+1);
          hasSwapped = true;
        }
      }

      if (!hasSwapped) {
        return arr;
      }
    }
  }

  private int[] doSwap(int[] arr, int element1, int element2) {
    if (element1 == element2) return arr;
    swapQueue.Enqueue(new int[2]{element1, element2});
    int placeholder = arr[element1];
    arr[element1] = arr[element2];
    arr[element2] = placeholder;
    return arr;
  }

  private int[] quickSort(int[] arr) {
    return quickSortHelper(arr, 0, arr.Length - 1);
  }

  private int[] quickSortHelper(int[] arr, int start, int end) {
    if (end - start <= 0) {
      return arr;
    } else {
      int pivot = arr[end];
      int swapIndex = 0;
      for (int i = 0 ; i < end ; i++) {
        if (arr[i] <= pivot) {
          // DO SWAP
          arr = doSwap(arr, swapIndex, i);
          swapIndex++;
        }
      }
      arr = doSwap(arr, swapIndex, end);
      arr = quickSortHelper(arr, start, swapIndex - 1);
      arr = quickSortHelper(arr, swapIndex + 1, end);
      return arr;
    }
  }

  private bool isSorted(int[] arr) {
    for (int i = 0 ; i < arr.Length - 1 ; i++) {
      if (arr[i] > arr[i+1]) {
        return false;
      }
    }
    return true;
  }

  private void startBlockSwap(int block1, int block2) {
    swapScript.StartSwap(blocks[block1], blocks[block2]);
    GameObject placeholder = blocks[block1];
    blocks[block1] = blocks[block2];
    blocks[block2] = placeholder;
  }
  public void SwapDone() {
    if (swapQueue.Count > 0) {
      int[] swap = swapQueue.Dequeue();
      startBlockSwap(swap[0], swap[1]);
    } else if (!gameOver) {
      gameOver = true;
      text.text = "You Lose";
    }
  }

}
