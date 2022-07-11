using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boundaries : MonoBehaviour
{
    private Vector2 screenBounds;
    private float objectWidth;
    private float objectHeight;
    [SerializeField] bool destroyOnExit;

    // Start is called before the first frame update
    void Start()
    {
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
        objectWidth = GetComponent<SpriteRenderer>().bounds.size.x / 2;
        objectHeight = GetComponent<SpriteRenderer>().bounds.size.y / 2;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 viewPos = transform.position;
        if (destroyOnExit)
        {
            if (viewPos.x > screenBounds.x + objectWidth || viewPos.x < screenBounds.x * -1 - objectWidth
             || viewPos.y > screenBounds.y + objectHeight || viewPos.y < screenBounds.x * -1 - objectHeight)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            viewPos.x = Mathf.Clamp(viewPos.x, screenBounds.x*-1 + objectWidth, screenBounds.x - objectWidth);
            viewPos.y = Mathf.Clamp(viewPos.y, screenBounds.y * -1 + objectHeight, screenBounds.y - objectHeight);
            transform.position = viewPos;
        }
    }

}
