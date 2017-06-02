using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BehaviourBase {

    public enum BehaviourState {
        BehaviourNormal,
        BehaviourPause,
        BehaviourRemove,
    }

    public virtual BehaviourState state { get; set; }

    public virtual void Awake()
    {

    }

    public virtual void OnEnable() { 
        
    }

    public virtual void Start()
    {

    }

    public virtual void OnDisable()
    {

    }

    public virtual void OnDestroy()
    {

    }

    public virtual void Update()
    {

    }

    public virtual void LateUpdate()
    {

    }
}


public class BehaviourUpdateManager : MonoBehaviour {

    protected List<BehaviourBase> behaviourCollection = new List<BehaviourBase>();

    protected List<int> markRemoveBehaviourCollection = new List<int>();

    protected int Count = 0;

    public void RegisterBehaviour(BehaviourBase behaviour,bool asFirst) {
        behaviourCollection.Add(behaviour);
        Count++;
        if (asFirst && Count > 1)
        {
            BehaviourBase temp = behaviour;
            behaviourCollection[0] = behaviourCollection[Count - 1];
            behaviourCollection[Count - 1] = temp;
        }
    }

    public void UpdateBehaviourIndex(BehaviourBase behaviour, int index)
    {
        if (index >= 0 && index < Count) {
            if (!behaviourCollection.Remove(behaviour))
            {
                Count++;
            }
            behaviourCollection.Insert(index, behaviour); 
        }
        
    }

    public void UnRegisterBehaviour(BehaviourBase behaviour)
    {
        if (behaviourCollection.Remove(behaviour)) Count--;
    }

    public void UnRegisterBehaviour(int behaviourIndex)
    {
        if (behaviourIndex >= 0 && behaviourIndex < Count)
        {
            behaviourCollection.RemoveAt(behaviourIndex);
            Count--;
        }
    }

    public void Clear() {
        foreach (int index in markRemoveBehaviourCollection)
        {
            UnRegisterBehaviour(index);
        }
        markRemoveBehaviourCollection.Clear();
    }

    public void Awake()
    {
        for (int index = 0; index < Count; index++)
        {
            if (behaviourCollection[index] != null)
            {
                if (behaviourCollection[index].state == BehaviourBase.BehaviourState.BehaviourNormal) behaviourCollection[index].Awake();
                else if (behaviourCollection[index].state == BehaviourBase.BehaviourState.BehaviourRemove)
                    markRemoveBehaviourCollection.Add(index);
            }
            else markRemoveBehaviourCollection.Add(index);
        }

        Clear();
    }

    public void OnEnable()
    {
        for (int index = 0; index < Count; index++)
        {
            behaviourCollection[index].OnEnable();
        }
    }

    public void Start()
    {
        for (int index = 0; index < Count; index++)
        {
            behaviourCollection[index].Start();
        }
        Clear();
    }

    public void OnDisable()
    {
        for (int index = 0; index < Count; index++)
        {
            behaviourCollection[index].OnDisable();
        }
        Clear();
    }

    public void OnDestroy()
    {
        for (int index = 0; index < Count; index++)
        {
            behaviourCollection[index].OnDestroy();
        }
        Clear();
    }

    public void Update()
    {
        for (int index = 0; index < Count; index++)
        {
            behaviourCollection[index].Update();
        }
        Clear();
    }

    public void LateUpdate()
    {
        for (int index = 0; index < Count; index++)
        {
            behaviourCollection[index].LateUpdate();
        }
        Clear();
    }

}
