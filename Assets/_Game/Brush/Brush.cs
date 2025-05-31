using System;
using _Game;
using _Game.Card;
using DG.Tweening;
using UnityEngine;

public class Brush : MonoBehaviour
{
    public float moveDuration = 0.5f;
    public float riseHeight = 1f;
    public float rotationDuration = 0.5f;

    public enum Edge
    {
        Min,
        Max,
        Center
    }

    public Edge edgeToAlign = Edge.Min;

    public bool isUsed = false;
    
    private bool isMoving = false;
    private bool movement = false;
    Card component;

    private void OnMouseDown()
    {
        if (G.coreLoop.status == CoreStatus.opening)
        {
            movement = !movement;
            Debug.Log(movement);
        }

    }

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        
        if (Input.GetMouseButtonDown(0) && !isMoving && movement && Physics.Raycast(ray, out RaycastHit hit))
        {
            if(hit.collider.gameObject.TryGetComponent(out component))
            {
                if (component.isFaceUp)
                {
                    Debug.DrawLine(ray.origin, hit.point, Color.red, 2f);
                    AnimateMoveTo(hit.point);
                    component.SetCard(G.deckShuffler.GenerateOneCard().GetComponent<Card>());
                    isUsed = true;
                }
            }

        }
    }

    void AnimateMoveTo(Vector3 targetPosition)
    {
        Collider col = GetComponent<Collider>();
        if (col == null) return;

        Bounds bounds = col.bounds;

        Vector3 currentEdge = edgeToAlign switch
        {
            Edge.Min => bounds.min,
            Edge.Max => bounds.max,
            Edge.Center => bounds.center,
            _ => bounds.min,
        };

        Vector3 offset = targetPosition - currentEdge;

        Vector3 finalPosition = transform.position + offset;
        Vector3 startPosition = transform.position;

        isMoving = true;

        // Падающий старт: объект "лежит"
        transform.rotation = Quaternion.Euler(90f, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);

        // Встать вертикально + переместиться
        Sequence seq = DOTween.Sequence();

        // Поднимаемся над землей как бы
        seq.Append(transform.DOMoveY(startPosition.y + riseHeight, 0.3f));

        // Одновременно перемещаемся и поворачиваемся
        seq.Append(
            DOTween.Sequence()
                .Append(transform.DOMove(finalPosition, moveDuration))
                .Join(transform.DORotate(Vector3.zero, rotationDuration))
        );

        // Возвращаем контроль после окончания
        seq.OnComplete(() => isMoving = false);
    }
}