using StateMachine;
using StateMachine.states;
using System.Collections.Generic;
using UnityEngine;

public class MessagesManager : MonoBehaviour
{
    private Dictionary<DictKey, string> billboardMessages = new Dictionary<DictKey, string>()
    {
        { new DictKey(States.StartState, Variant.First), "" },
        { new DictKey(States.StartState, Variant.Second), "" },
        { new DictKey(States.Wind, Variant.First), "The wind is picking up..." },
        { new DictKey(States.Wind, Variant.Second), "The wind is picking up" },
        { new DictKey(States.Anvil, Variant.First), "Look up!" },
        { new DictKey(States.Anvil, Variant.Second), "Look up!" },
        { new DictKey(States.Storm, Variant.First), "Thunder!" },
        { new DictKey(States.Storm, Variant.Second), "Storm is coming... it will be slippery!" },
        { new DictKey(States.Ufo, Variant.First), "Mother ship incoming!" },
        { new DictKey(States.Ufo, Variant.Second), "Is that a cow?" },
        { new DictKey(States.Beaver, Variant.First), "Kick the beaver!" },
        { new DictKey(States.Beaver, Variant.Second), "Portal Pig strikes again!" },
        { new DictKey(States.Fruit, Variant.First), "Fruit Thursday!" },
        { new DictKey(States.Fruit, Variant.Second), "Watch out for coconuts!" },
        { new DictKey(States.Bird, Variant.First), "Watch out for birds!" },
        { new DictKey(States.Bird, Variant.Second), "Watch out for birds!" },
        { new DictKey(States.SharkState, Variant.First), "Fishy, fishy!" },
        { new DictKey(States.SharkState, Variant.Second), "Is that a sharknado?" },
        { new DictKey(States.PlayerDeath, Variant.First), "Mr Potato!" }
    };

    public string GetMessage(States state, Variant variant)
    {
        DictKey key = new DictKey(state, variant);

        if (billboardMessages.TryGetValue(key, out string message))
        {
            return message;
        }
        return "";
    }
}
