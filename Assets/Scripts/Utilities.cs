using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

// Row is Y
// Col is X

public static class Vector2Extensions {
    // Convert to absolute values
    public static Vector2 Abs (this Vector2 vec) {
        return new Vector2(Mathf.Abs(vec.x), Mathf.Abs(vec.y));
    }

    // Convert to vector 3
    public static Vector3 AsVector3 (this Vector2 vec) {
        return new Vector3(vec.x, vec.y, 0);
    }

    // Convert to integer values
    public static int ToInt (this float point) {
        int newPoint = (int)point;
        return newPoint;
    }
}

// Helpers for 2D arrays
public static class MatrixExtensions {

    // Retrieve a cell
    public static T Get<T>(this T[,] matrix, Vector2 vec) {
        return (T)matrix.GetValue(vec.x.ToInt(), vec.y.ToInt());
    }

    // Set a cell
    public static void Set<T>(this T[,] matrix, T obj, Vector2 vec) {
        matrix.SetValue(obj, vec.x.ToInt(), vec.y.ToInt());
    }

    // Swap two cells
    public static void Swap<T>(this T[,] matrix, Vector2 vec1, Vector2 vec2) {
        T first = matrix.Get(vec1);
        T second = matrix.Get(vec2);

        matrix.Set(first, vec2);
        matrix.Set(second, vec1);
    }

    // Retrieve specified row
    public static T[] GetRow<T>(this T[,] matrix, int row) {
        int length = matrix.GetLength(0);
        T[] result = new T[length];

        for (int i = 0; i < length; i++) {
            result[i] = matrix[i, row];
        }

        return result;
    }

    // Set entire row specified
    public static void SetRow<T>(this T[,] matrix, int row, T[] newRow)
    {
    int length = matrix.GetLength(1);

    for (int i = 0; i < length; i++)
        matrix[row, i] = newRow[i];
    }

    // Retrieve specified column
    public static T[] GetCol<T>(this T[,] matrix, int col) {
        int length = matrix.GetLength(1);
        T[] result = new T[length];

        for (int i = 0; i < length; i++) {
            result[i] = matrix[col, i];
        }

        return result;
    }

    // Set entire column specified
    public static void SetCol<T>(this T[,] matrix, int col, T[] newCol)
    {
    int length = matrix.GetLength(1);

    for (int i = 0; i < length; i++)
        matrix.SetValue(newCol[i], col, i);
    }

    // Move all not-null slots to bottom of 2D array
    public static void ShiftValuesDown<T>(this T[,] matrix) {
        int cols = matrix.GetLength(0);
        for (int i = 0; i < cols; i++) {
            T[] col = matrix.GetCol(i);
            int length = col.Length;
            col = col.Where(c => c != null).ToArray();
            Array.Resize<T>(ref col, length);
            // PrintIndexAndValues(col);
            matrix.SetCol(i, col);
        }
    }

    // Replace 2D array with specified
    public static void ReplaceWith<T>(this T[,] matrix, T[,] next) {
        matrix = next;
    }

    // Print an array row/col value with index
    public static void PrintIndexAndValues<T>(T[] arr)  {
        string str = "";
        for (int i = 0; i < arr.Length; i++)
        {
            str += $"[{i}]: {arr[i]} ";
            
        }

        Debug.Log(str);
    }
    // Print the entire array
    public static void Print2DArray<T>(this T[,] matrix) {
        int rows = matrix.GetLength(1);
        for (int i = 0; i < rows; i++) {
            T[] row = matrix.GetRow(i);
            PrintIndexAndValues(row);
        }
    }
}