using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] PlayerController player;



    // Start is called before the first frame update
    void Start()
    {
        player.Init();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
