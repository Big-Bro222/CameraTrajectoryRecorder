using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TimerFactory : MonoSingleton<TimerFactory>
{
    private void Update()
    {
        UpdateTimerStatus();
    }

    private List<Timer> _timerPool = new List<Timer>();
    private List<Timer> _activateHandlers = new List<Timer>();

    private long currentTime
    {
        get { return (long)(Time.time * 1000); }
    }
    private void UpdateTimerStatus()
    {
        //update each timer
        for (int i = 0; i < _activateHandlers.Count; i++)
        {
            Timer handler = _activateHandlers[i];
            //Detect if the handler is triggered
            if (!handler.isTriggered) continue;

            long t = currentTime;
            if (t >= handler.currentTime)
            {
                Action<Timer> method = handler.callBackHandler;
                while (t >= handler.currentTime)
                {
                    handler.currentTime += handler.delay;
                    handler.executedTime += handler.delay;
                    method.Invoke(handler);
                }
            }
        }
    }

    /// <summary>
    /// Create a timer
    /// </summary>
    /// <param name="interval">timer interval by milliseconds</param>
    /// <param name="callBackAction"></param>
    /// <returns></returns>
    public Timer CreateLoopTimer( int interval, Action<Timer> callBackAction, bool executeOnStart=false)
    {
        if (callBackAction == null)
        {
            return null;
        }
        Timer handler;
        //select timer from timer pool
        if (_timerPool.Count > 0)
        {
            handler = _timerPool[_timerPool.Count - 1];
            _timerPool.Remove(handler);
        }
        else
        {
            handler = new Timer();
        }
        handler.delay = interval;
        handler.callBackHandler = callBackAction;
        handler.currentTime =executeOnStart? currentTime:currentTime+interval;
        _activateHandlers.Add(handler);
        return handler;
    }

    public void ClearTimer(Timer timerHandler)
    {
        Clear(timerHandler);
    }

    private void Clear(Timer timerHandler)
    {
        Timer handler = _activateHandlers.FirstOrDefault(t => t == timerHandler);
        if (handler != null)
        {
            //remove from active timer list and add to timer pool
            _activateHandlers.Remove(handler);
            handler.Clear();
            _timerPool.Add(handler);
        }
    }

    public void ClearAllTimer()
    {
        foreach (Timer handler in _activateHandlers)
        {
            Clear(handler);
            ClearAllTimer();
            return;
        }
    }

    public class Timer
    {
        public int delay;
        public long currentTime;
        public long executedTime;
        public Action<Timer> callBackHandler;
        public bool isTriggered;
        public void Trigger()
        {
            isTriggered = true;
        }
        public void Pause()
        {
            isTriggered = false;
        }
        public void Clear()
        {
            isTriggered = false;
            callBackHandler = null;
        }
    }
}
