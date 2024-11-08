using UnityEngine;


namespace Assets.Entities
{
    [System.Serializable]
    public partial class Class
    {
        public string name { get; set; }

        public string description { get; set; }

        public Function[] functions { get; set; }

    }
}
