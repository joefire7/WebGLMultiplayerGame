using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    [SerializeField] private InputReader inputReader;
    // Start is called before the first frame update
    void Start()
    {
        // subscribe
        inputReader.MoveEvent += HandleMove;
    }

    private void OnDestroy()
    {
        // unsubcribe
        inputReader.MoveEvent -= HandleMove;
    }

    private void HandleMove(Vector2 movement)
    {
        //Debug.Log(movement);
    }
}
