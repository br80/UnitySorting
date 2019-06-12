using UnityEngine;
using System.Collections;

public class AlgorithmScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
    int numElements = 100;
    int[] arr = new int[numElements];
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

    arr = quickSort(arr);

    string str = "";
    for (int i = 0 ; i < arr.Length ; i++) {
      str += arr[i] + ", ";
    }
    Debug.Log(isSorted(arr));
    Debug.Log(str);
	}

  private bool isSorted(int[] arr) {
    for (int i = 0 ; i < arr.Length - 1 ; i++) {
      if (arr[i] > arr[i+1]) {
        return false;
      }
    }
    return true;
  }

  private int[] doSwap(int[] arr, int element1, int element2) {
    int placeholder = arr[element1];
    arr[element1] = arr[element2];
    arr[element2] = placeholder;
    return arr;
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


}
