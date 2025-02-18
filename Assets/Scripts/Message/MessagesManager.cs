using StateMachine;
using StateMachine.states;
using System.Collections.Generic;
using UnityEngine;

public class MessagesManager : MonoBehaviour
{
    private Dictionary<DictKey, string> billboardMessages = new Dictionary<DictKey, string>()
    {
        { new DictKey(States.Wind, Variant.First), "The wind is picking up" },
        { new DictKey(States.Wind, Variant.Second), "The wind is picking up" },
        { new DictKey(States.Anvil, Variant.First), "Look up!" },
        { new DictKey(States.Anvil, Variant.Second), "Look up!" },
        { new DictKey(States.Storm, Variant.First), "Storm is coming... it will be slippery" },
        { new DictKey(States.Storm, Variant.Second), "Thunder!" },
        { new DictKey(States.Ufo, Variant.First), "Mother ship incoming" },
        { new DictKey(States.Ufo, Variant.Second), "Is that a cow?" }
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
