using System.Collections.Generic;
using UnityEngine;

public class RandomTrapManager : MonoBehaviour
{
    [Header("Các vị trí trap có thể xuất hiện")]
    [SerializeField] private TrapAmbushPoint[] trapPoints;

    private void Awake()
    {
        SelectRandomTrapPoint();
    }

    private void SelectRandomTrapPoint()
    {
        if (trapPoints == null || trapPoints.Length == 0)
        {
            Debug.LogWarning(
                "RandomTrapManager chưa có Trap Point nào.",
                this
            );

            return;
        }

        List<TrapAmbushPoint> validPoints =
            new List<TrapAmbushPoint>();

        foreach (TrapAmbushPoint point in trapPoints)
        {
            if (point != null)
            {
                validPoints.Add(point);
                point.gameObject.SetActive(false);
            }
        }

        if (validPoints.Count == 0)
        {
            Debug.LogWarning(
                "Danh sách Trap Point không có object hợp lệ.",
                this
            );

            return;
        }

        int randomIndex = Random.Range(0, validPoints.Count);

        TrapAmbushPoint selectedPoint =
            validPoints[randomIndex];

        selectedPoint.gameObject.SetActive(true);

        Debug.Log(
            $"Trap ngẫu nhiên được chọn tại: {selectedPoint.name}"
        );
    }
}