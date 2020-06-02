﻿﻿using System.Collections.Generic;
using System;
using JetBrains.Annotations;
using UnityEngine;

 public class EventManager : Singleton<EventManager> {
    private readonly Dictionary<UIEvent, List<BaseEvent>> mEventDictionary = new Dictionary<UIEvent, List<BaseEvent>>();

    public void Listen(UIEvent uiEvent, Action<object> listenerAction, Action callerAction = null) {
        var newUIEvent = new BaseEvent {
            EventName = uiEvent,
            ListenerAction = listenerAction,
            CallerAction = callerAction
        };
        if (!mEventDictionary.ContainsKey(uiEvent)) {
            var newBaseTimerList = new List<BaseEvent>();
            mEventDictionary.Add(uiEvent, newBaseTimerList);
        }
        mEventDictionary[uiEvent].Add(newUIEvent);
    }

    public void Fire(UIEvent uiEvent, object obj = null) {
        if (!mEventDictionary.ContainsKey(uiEvent)) {
            Debug.LogError(uiEvent + "not exist!");
            return;
        }
        foreach (var @event in mEventDictionary[uiEvent]) {
            @event.CallerAction?.Invoke();
            @event.ListenerAction(obj);
        }
    }
}
 
 public class BaseEvent {
     public UIEvent EventName { get; set; }
     public Action<object> ListenerAction { get; set; }
     [CanBeNull]
     public Action CallerAction { get; set; }
 }

 public enum UIEvent {
     ON_ENTER_GAME,
     SPAWN_SHAPE,
     CREASE_SCORE,
     END_GAME,
     DISCONNECT,
     RECONNECT
 }
