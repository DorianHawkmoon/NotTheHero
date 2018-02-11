using System;

/// <summary>
/// Interface to be implemented by classes to be stored in a heap
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IHeapItem<T> : IComparable<T> {
    int HeapIndex {
        get;
        set;
    }
}

/// <summary>
/// Heap class for classes which implements the heap interface
/// </summary>
/// <typeparam name="T">type of data for the heap</typeparam>
public class Heap<T> where T : IHeapItem<T> {
    /// <summary>
    /// array of items
    /// </summary>
    private T[] items;
    /// <summary>
    /// how many items in the heap
    /// </summary>
    private int currentItemCount;

    /// <summary>
    /// Max size for the heap
    /// </summary>
    /// <param name="maxHeapSize"></param>
    public Heap(int maxHeapSize) {
        items = new T[maxHeapSize];
    }

    /// <summary>
    /// Add an item and sort the heap
    /// </summary>
    /// <param name="item"></param>
    public void Add(T item) {
        item.HeapIndex = currentItemCount;
        items[currentItemCount] = item;
        SortUp(item);
        ++currentItemCount;
    }

    /// <summary>
    /// Remove the top of the heap
    /// </summary>
    /// <returns></returns>
    public T RemoveFirst() {
        T firstItem = items[0];
        currentItemCount--;
        items[0] = items[currentItemCount];
        items[0].HeapIndex = 0;
        SortDown(items[0]);
        return firstItem;
    }

    /// <summary>
    /// Update the sub-tree of the given node
    /// </summary>
    /// <param name="item"></param>
    public void UpdateItem(T item) {
        SortUp(item);
    }

    /// <summary>
    /// How many elements
    /// </summary>
    public int Count {
        get {
            return currentItemCount;
        }
    }

    /// <summary>
    /// If the heap contains the item
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public bool Contains(T item) {
        return Equals(items[item.HeapIndex], item);
    }

    /// <summary>
    /// Put the node one level below
    /// </summary>
    /// <param name="item"></param>
    private void SortDown(T item) {
        while (true) {
            int childIndexLeft = item.HeapIndex * 2 + 1;
            int childIndexRight = item.HeapIndex * 2 + 2;
            int swapIndex = 0;

            if (childIndexLeft < currentItemCount) {
                swapIndex = childIndexLeft;

                if (childIndexRight < currentItemCount) {
                    if (items[childIndexLeft].CompareTo(items[childIndexRight]) < 0) {
                        swapIndex = childIndexRight;
                    }
                }

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

    /// <summary>
    /// Put the node one level above
    /// </summary>
    /// <param name="item"></param>
    private void SortUp(T item) {
        int parentIndex = (item.HeapIndex - 1) / 2;

        while (true) {
            T parentItem = items[parentIndex];
            if (item.CompareTo(parentItem) > 0) {
                Swap(item, parentItem);
            } else {
                break;
            }

            parentIndex = (item.HeapIndex - 1) / 2;
        }
    }

    /// <summary>
    /// Swap two nodes position in the heap
    /// </summary>
    /// <param name="itemA"></param>
    /// <param name="itemB"></param>
    private void Swap(T itemA, T itemB) {
        items[itemA.HeapIndex] = itemB;
        items[itemB.HeapIndex] = itemA;
        int itemAIndex = itemA.HeapIndex;
        itemA.HeapIndex = itemB.HeapIndex;
        itemB.HeapIndex = itemAIndex;
    }
}