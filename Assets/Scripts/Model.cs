using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class Model : MonoBehaviour
{
    public GameObject[] prefabs;
    public GameObject containBlocks;
    public GameObject containSpawn;
    public GameObject[,] mapBlocks;

    public List<GameObject> spawnShapes;
    public int maxSpawnShape = 3;

    public int columnMatrix;

    public int rowMatrix;

    public GameObject shadowShape;

    public Sprite spriteShadow;
    // Start is called before the first frame update
    void Start()
    {
        mapBlocks = new GameObject[columnMatrix, rowMatrix];
    }

    public void OnEnterGame()
    {
        SpawnShapes();
    }

    public void SpawnShapes()
    {
        spawnShapes = new List<GameObject>();
        List<int> rands = new List<int>();
        int cursorPos = 0;
        for (int i = 0; i < maxSpawnShape; i++)
        {
            int rand = Random.Range(0, prefabs.Length);
            while (rands.Contains(rand))
            {
                rand = Random.Range(0, prefabs.Length);
            }
            rands.Add(rand);
            GameObject shape = Instantiate(prefabs[rand]);
            shape.transform.position = new Vector2(containSpawn.transform.position.x + cursorPos, containSpawn.transform.position.y);
            shape.transform.parent = containSpawn.transform;
            shape.SetActive(true);
            spawnShapes.Add(shape);
            cursorPos += shape.GetComponent<Shape>().widthBlocks + 1;
            shape.GetComponent<Shape>().RandomColor();
        }

        //CheckAndWarningShape();
        /*int ofsContain = (Screen.width - cursorPos) / 2;
        Debug.Log("ofscontain " + ofsContain);*/
    }

    public bool PlaceShape(Shape shape)
    {
        DestroyShadow();
        if (IsShapeOnBoundary(shape))
        {
            GameObject[] blocks = shape.blocks;
            for (int i = 0; i < blocks.Length; i++)
            {
                int column = Mathf.RoundToInt(blocks[i].transform.position.x);
                int row = Mathf.RoundToInt(blocks[i].transform.position.y);
                mapBlocks[column, row] = blocks[i];
                blocks[i].transform.parent = containBlocks.transform;
                blocks[i].transform.position = new Vector2(column, row);
                blocks[i].transform.name = "[" + column + ";" + row + "]";
            }
            spawnShapes.Remove(shape.gameObject);
            Destroy(shape.gameObject);
            if (spawnShapes.Count <= 0)
            {
                spawnShapes.Clear();
                Controller.Instance.eventManager.Fire(UIEvent.SPAWN_SHAPE);
            }
            else
            {
                CheckAndWarningShape();   
            }
            CheckAndDestroyAchieveBlocks();
            CheckLose();
            return true;
        }

        return false;
    }

    public bool IsShapeOnBoundary(Shape shape)
    {
        GameObject[] blocks = shape.blocks;
        for (int i = 0; i < blocks.Length; i++)
        {
            Vector2 posBlock = blocks[i].transform.position;
            if (!IsPositionOnBoundary(posBlock)) return false;
        }

        return true;
    }

    public bool IsPositionOnBoundary(Vector2 position)
    {
        int x = Mathf.RoundToInt(position.x);
        int y = Mathf.RoundToInt(position.y);
        return x >= 0 
               && x < columnMatrix 
               && y >= 0 
               && y < rowMatrix
                && mapBlocks[x, y] == null;
    }

    public List<GameObject> GetAchievesBlocks()
    {
        List<GameObject> achieves = new List<GameObject>();
        for (int i = 0; i < rowMatrix; i++)
        {
            List<GameObject> achievesRow = new List<GameObject>();
            for (int j = 0; j < columnMatrix; j++)
            {
                if (mapBlocks[j, i] != null)
                {
                    achievesRow.Add(mapBlocks[j, i]);
                }
                else
                {
                    achievesRow.Clear();
                    break;
                }
            }

            if (achievesRow.Count > 0)
            {
                achieves = achieves.Union(achievesRow).ToList();
            }
        }
        
        for (int i = 0; i < columnMatrix; i++)
        {
            List<GameObject> achievesColumn = new List<GameObject>();
            for (int j = 0; j < rowMatrix; j++)
            {
                if (mapBlocks[i, j] != null)
                {
                    achievesColumn.Add(mapBlocks[i, j]);
                }
                else
                {
                    achievesColumn.Clear();
                    break;
                }
            }

            if (achievesColumn.Count > 0)
            {
                achieves = achieves.Union(achievesColumn).ToList();
            }
        }
        
        return achieves;
    }

    public void CheckAndDestroyAchieveBlocks()
    {
        List<GameObject> achieves = GetAchievesBlocks();
        Controller.Instance.eventManager.Fire(UIEvent.CREASE_SCORE, achieves.Count);
        for (int i = 0; i < achieves.Count; i++)
        {
            DestroyBlock(achieves[i]);
        }
    }

    public void DestroyBlock(GameObject obj)
    {
        int x = Mathf.RoundToInt(obj.transform.position.x);
        int y = Mathf.RoundToInt(obj.transform.position.y);
        mapBlocks[x, y] = null;
        Sequence mySequence = DOTween.Sequence();
        mySequence.PrependInterval(.02f * x + .02f * y);
        mySequence.Append(obj.transform.DOScale(1.2f, 0.3f));
        mySequence.Append(obj.transform.DOScale(0, 0.3f).OnComplete(() =>
        {
            Destroy(obj);
        }));
    }

    public void CheckLose()
    {
        bool isLose = true;
        for (int i = 0; i < spawnShapes.Count; i++)
        {
            if (spawnShapes[i].GetComponent<Shape>().isCanPlaceInMap())
            {
                isLose = false;
                break;
            }
        }

        if (isLose)
        {
            Debug.Log("end game !");
            Controller.Instance.eventManager.Fire(UIEvent.END_GAME);
        }
    }

    public void CheckAndWarningShape()
    {
        for (int i = 0; i < spawnShapes.Count; i++)
        {
            Shape shape = spawnShapes[i].GetComponent<Shape>();
            shape.DOPause();
            if (shape.isCanPlaceInMap())
            {
                //shape.RunAnimationWarning();   
            }
        }
    }

    public void UpdateShadowShape(GameObject shape)
    {
        if (shadowShape == null)
        {
            shadowShape = Instantiate(shape);
            shadowShape.transform.parent = containBlocks.transform;
            shadowShape.GetComponent<Shape>().UpdateSkinShadow(spriteShadow);
        }
        shadowShape.SetActive(false);
        if (IsShapeOnBoundary(shape.GetComponent<Shape>()))
        {
            shadowShape.SetActive(true);
            Shape shapeS = shape.GetComponent<Shape>();
            Shape shapeSh = shadowShape.GetComponent<Shape>();
            for (int i = 0; i < shapeS.blocks.Length; i++)
            {
                shapeSh.blocks[i].transform.position = new Vector2(Mathf.RoundToInt(shapeS.blocks[i].transform.position.x), Mathf.RoundToInt(shapeS.blocks[i].transform.position.y));
            }
        }
    }

    public void DestroyShadow()
    {
        if (shadowShape != null)
        {
            Destroy(shadowShape);
            shadowShape = null;
        }
    }

}
