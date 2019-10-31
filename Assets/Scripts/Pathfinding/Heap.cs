using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Heap<T> where T : IHeapItem<T> {

    // Heap optimization to make the A* calculations speed up dramatically

    // Formulas to remember:
    // Finding the parent node: (n-1)/2
    // Finding the left child: 2n+1
    // Finding the right child: 2n+2

    // defining needed variables
    T[] items;
    int currentCount;

    // public constructor
    public Heap(int maxHeapSize) {
        items = new T[maxHeapSize];
    }

    // Adding a new item to the heap
    public void AddNewT(T item) {
        // setting the index for the new item
        item.HeapIndex = currentCount;
        items[currentCount] = item;

        // sorting the item up so that it's in the correct place in the heap
        SortItemUp(item);
        currentCount++;
    }

    // removing the first element to move to the closed hashset
    public T RemoveFirst() {
        // identifying the first item and reducing the current count
        T firstItem = items[0];
        currentCount--;

        // moving the bottom item up to the top
        items[0] = items[currentCount];
        items[0].HeapIndex = 0;

        // Sorting that item down to put it in the proper place for the heap
        SortItemDown(items[0]);

        // returning the first item to to be moved to the closed hash set
        return firstItem;
    }

    // a check to see if the heap has an item
    public bool Contains(T item) {
        return Equals(items[item.HeapIndex], item);
    }

    // a simple function to update an item in the heap, sorting it up to find its proper place
    public void UpdateItem(T item) {
        SortItemUp(item);
    }

    // a getter to send out the current count in the heap
    public int Count {
        get {
            return currentCount;
        }
    }

    // Sorting the selected item up in the heap
    void SortItemUp(T item) {
        // finding the current parent's heap index
        int parentIndex = (item.HeapIndex - 1) / 2;

        // running an infinite loop that I will break out of to find the proper spot in the heap
        while (true) {
            // instantiating the parent item
            T parentItem = items[parentIndex];

            // if the item has a smaller value than it's parent, swap the item up
            if (item.CompareTo(parentItem) > 0) {
                Swap(item, parentItem);
            }
            else {
                return;
            }

            // resetting the parent item after a swap
            parentIndex = (item.HeapIndex - 1) / 2;
        }
    }

    // Sorting an item down through the heap to find it's proper home
    void SortItemDown(T item) {

        // running an infinite loop that I will break out of
        while (true) {
            // instantiate the children items and set the initial value of the swap index
            int childIndexLeft = item.HeapIndex * 2 + 1;
            int childIndexRight = item.HeapIndex * 2 + 2;
            int swapIndex = 0;

            // if the child's index is lower than the current count, set the swap index to that
            if (childIndexLeft < currentCount) {
                swapIndex = childIndexLeft;

                // if the right child's index is also lower than the current count, 
                // compare the two children indexes and set the swapIndex to the lower of the two
                if (childIndexRight < currentCount) {
                    if (items[childIndexLeft].CompareTo(items[childIndexRight]) < 0) {
                        swapIndex = childIndexRight;
                    }
                }

                // Finally, if the swap index is lower than the item's index, swap those two items
                if (item.CompareTo(items[swapIndex]) < 0) {
                    Swap(item, items[swapIndex]);
                } else {
                    return;
                }
            } else {
                return;
            }
        }
    }

    // this is the method that will swap two items in the heap
    void Swap(T itemA, T itemB) {
        // Swap the two items
        items[itemA.HeapIndex] = itemB;
        items[itemB.HeapIndex] = itemA;

        // Swap the two heap indexes
        int itemAIndex = itemA.HeapIndex;
        itemA.HeapIndex = itemB.HeapIndex;
        itemB.HeapIndex = itemAIndex;
    }
}

// the public interface so that elements outside can access the heap
public interface IHeapItem<T> : IComparable<T> {
    // it will be able to get and set
    int HeapIndex {
        get;
        set;
    }
}
