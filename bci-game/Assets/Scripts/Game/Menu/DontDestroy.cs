namespace Game.Menu
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class DontDestroy : MonoBehaviour
    {
        [SerializeField] public GameObject NotionTester;
        
        void Start()
        {
            DontDestroyOnLoad(NotionTester);
        }
    }
}
