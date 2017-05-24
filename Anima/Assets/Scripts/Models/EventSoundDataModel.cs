using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventSoundDataModel : MonoBehaviour {

    public AudioSource[] EventSoundList;
    private static Dictionary<string, AudioSource> _eventSoundList;

    void Start()
    {
        EventSoundList = this.gameObject.GetComponents<AudioSource>();

        _eventSoundList = new Dictionary<string, AudioSource>()
        {
            { EventObjectModel.Rain, EventSoundList[0] },
            { EventObjectModel.Food, EventSoundList[1] },
            { EventObjectModel.Population, EventSoundList[2]},
            { EventObjectModel.Tree, EventSoundList[3]},
            { EventObjectModel.Wildfire, EventSoundList[4] },
            { EventObjectModel.flood, EventSoundList[5] },
            { EventObjectModel.GroundCollapse, EventSoundList[6] },
            { EventObjectModel.Desolation, EventSoundList[7] },
            { EventObjectModel.Earthquake, EventSoundList[8] },
            { EventObjectModel.Typhoon, EventSoundList[9] },
            { EventObjectModel.Nothing, EventSoundList[10] }
        };
    }

    public AudioSource getEventSound(string eventName)
    {
        return _eventSoundList[eventName];
    }
}
