using UnityEngine;

namespace Assets.Entities
{
    [System.Serializable]
    public partial class File
    {
        public string name { get; set; }

        public string overview { get; set; }

        public Function[] functions { get; set; }

        public Class[] classes { get; set; }
    }
}
