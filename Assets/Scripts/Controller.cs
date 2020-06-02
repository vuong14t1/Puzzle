using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : Singleton<Controller>
{
    public Model model;

    public View view;
    public EventManager eventManager;

    protected override void Awake()
    {
        base.Awake();
    }

    // Start is called before the first frame update
    void Start()
    {
        model = FindObjectOfType<Model>();
        view = FindObjectOfType<View>();
        eventManager = FindObjectOfType<EventManager>();
        RegisterEventListener();
        eventManager.Fire(UIEvent.ON_ENTER_GAME);
    }

    public void RegisterEventListener()
    {
        eventManager.Listen(UIEvent.ON_ENTER_GAME, OnEnterGame);
        eventManager.Listen(UIEvent.SPAWN_SHAPE, SpawnShape);
        eventManager.Listen(UIEvent.CREASE_SCORE, CreaseScore);
        eventManager.Listen(UIEvent.END_GAME, EndGame);
    }

    public void EndGame(object obj)
    {
        GameManager.Instance.OnEndGame();
        GUIManager.Instance.ShowGUIOver();
    }

    private void CreaseScore(object obj)
    {
        GameManager.Instance.CreaseScore((int)obj);
        view.UpdateScore();
    }

    private void SpawnShape(object obj)
    {
        model.SpawnShapes();
    }

    public void OnEnterGame(object obj)
    {
        GameManager.Instance.OnEnterGame();
        view.OnEnterGame();
        model.OnEnterGame();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
