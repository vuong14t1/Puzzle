using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class Shape : MonoBehaviour
{
    public GameObject[] blocks;

    public Vector2 initPosition;

    public bool isDrag = false;

    public float deltaX;

    public float deltaY;

    public GameObject pivot;

    public int widthBlocks;

    public Color[] colors1;

    private void Awake()
    {
        colors1 = new Color[5];
        colors1[0] = Color.cyan;
        colors1[1] = Color.green;
        colors1[2] = Color.white;
        colors1[3] = Color.red;
        colors1[4] = Color.yellow;
    }

    public void UpdateSkinShadow(Sprite sprite)
    {
        for (int i = 0; i < blocks.Length; i++)
        {
            SpriteRenderer spriteRenderer = blocks[i].GetComponent<SpriteRenderer>(); 
            spriteRenderer.sprite = sprite;
            spriteRenderer.color = Color.white;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        initPosition = transform.position;
        
    }

    public void RandomColor()
    {
        int rand = Random.Range(0, colors1.Length);
        for (int i = 0; i < blocks.Length; i++)
        {
            blocks[i].GetComponent<SpriteRenderer>().color = colors1[rand];
        }
    }

    private void OnMouseDown()
    {
        if (!isDrag && GameManager.Instance.stateGame == StateGame.Playing)
        {
            deltaX = Camera.main.ScreenToWorldPoint(Input.mousePosition).x - transform.position.x;
            deltaY = Camera.main.ScreenToWorldPoint(Input.mousePosition).y - transform.position.y;
            isDrag = true;
            Controller.Instance.model.UpdateShadowShape(gameObject);
        }
    }

    private void OnMouseDrag()
    {
        if (isDrag)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector2(mousePos.x - deltaX, mousePos.y - deltaY);
            Controller.Instance.model.UpdateShadowShape(gameObject);
        }
    }

    private void OnMouseUp()
    {
        isDrag = false;
        PlaceShape();
    }

    public void PlaceShape()
    {
        if (!Controller.Instance.model.PlaceShape(this))
        {
            transform.DOMove(initPosition, 0.5f);
        }
        
    }

    public bool isCanPlaceInMap()
    {
        int columnMatrix = Controller.Instance.model.columnMatrix;
        int rowMatrix = Controller.Instance.model.rowMatrix;
        for (int i = 0; i < rowMatrix; i++)
        {
            for (int j = 0; j < columnMatrix; j++)
            {
                bool isTrue = true;
                for (int k = 0; k < blocks.Length; k++)
                {
                    int xB = Mathf.RoundToInt(blocks[k].transform.localPosition.x);
                    int yB = Mathf.RoundToInt(blocks[k].transform.localPosition.y);
                    Vector2 pos = new Vector2(j + xB, i + yB);
                    if (!Controller.Instance.model.IsPositionOnBoundary(pos))
                    {
                        isTrue = false;
                        break;
                    }
                }

                if (isTrue)
                {
                    return true;
                }
            }
        }

        return false;
    }

    public void RunAnimationWarning()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOScale(1f, 0.5f));
        sequence.Append(transform.DOScale(0.9f, 0.5f));
        sequence.SetLoops(-1);
    }
}
