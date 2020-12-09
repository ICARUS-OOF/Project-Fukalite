using System.Collections.Generic;
using UnityEngine;
namespace ProjectFukalite.Data.Serializables
{
    [System.Serializable]
    public class DialogueData
    {
        public string entityName;
        [TextArea]
        public List<string> sentences = new List<string>();
    }
}