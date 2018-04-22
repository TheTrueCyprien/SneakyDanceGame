using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public interface IRythmMessageTarget : IEventSystemHandler {

    // functions that can be called via the messaging system
    void OnBeat(int index);
    void SongStarted(float secPerBeat);
}
