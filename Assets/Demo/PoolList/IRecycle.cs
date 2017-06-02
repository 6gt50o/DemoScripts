using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRecycle {
    int index { get; set; }
    bool isRecycle { get; set; }
    void AddNew();
    void Recycle();
    void CopyFrom(IRecycle from,bool remainIndex);
    void Clear(bool remainIndex);
}
