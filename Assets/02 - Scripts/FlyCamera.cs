using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyCamera : MonoBehaviour {

    public bool isFrenchKeyboard = false;
    public float moveSpeed = 1.0f;
    public float rotateSpeed = 1.0f;
    
    private void OnEnglishKeyboard()
    {
        Vector2 dir = Vector2.zero;
        if (Input.GetKey(KeyCode.UpArrow))
        {
            dir += Vector2.up;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            dir += Vector2.down;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            dir += Vector2.left;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            dir += Vector2.right;
        }
        Vector2 offset = dir * moveSpeed;
        transform.position = new Vector3(transform.position.x + offset.x, transform.position.y, transform.position.z + offset.y);

        // rotating
        float angleAroundX = 0;
        float angleAroundY = 0;
        float angleAroundZ = 0;
        if (Input.GetKey(KeyCode.A))
        {
            angleAroundY += rotateSpeed;
        }
        if (Input.GetKey(KeyCode.D))
        {
            angleAroundY -= rotateSpeed;
        }
        if (Input.GetKey(KeyCode.W))
        {
            angleAroundX -= rotateSpeed;
        }
        if (Input.GetKey(KeyCode.S))
        {
            angleAroundX += rotateSpeed;
        }
        if (Input.GetKey(KeyCode.Q))
        {
            angleAroundZ += rotateSpeed;
        }
        if (Input.GetKey(KeyCode.E))
        {
            angleAroundZ -= rotateSpeed;
        }

        if (Mathf.Abs(angleAroundX) >= 1)
        {
            transform.rotation *= Quaternion.AngleAxis(angleAroundX, Vector3.right);
        }

        if (Mathf.Abs(angleAroundY) >= 1)
        {
            transform.rotation *= Quaternion.AngleAxis(angleAroundY, Vector3.up);
        }

        if (Mathf.Abs(angleAroundZ) >= 1)
        {
            transform.rotation *= Quaternion.AngleAxis(angleAroundZ, Vector3.back);
        }

        if (Input.GetKey(KeyCode.Z))
        {
            // go higher
            transform.position += Vector3.up * moveSpeed;
        }

        if (Input.GetKey(KeyCode.X))
        {
            // go down
            transform.position += Vector3.down * moveSpeed;
        }
    }

    private void OnFrenchKeyboard()
    {
        Vector2 dir = Vector2.zero;
        if (Input.GetKey(KeyCode.UpArrow))
        {
            dir += Vector2.up;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            dir += Vector2.down;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            dir += Vector2.left;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            dir += Vector2.right;
        }
        Vector2 offset = dir * moveSpeed;
        transform.position = new Vector3(transform.position.x + offset.x, transform.position.y, transform.position.z + offset.y);

        // rotating
        float angleAroundX = 0;
        float angleAroundY = 0;
        float angleAroundZ = 0;
        if (Input.GetKey(KeyCode.Q))
        {
            angleAroundY += rotateSpeed;
        }
        if (Input.GetKey(KeyCode.D))
        {
            angleAroundY -= rotateSpeed;
        }
        if (Input.GetKey(KeyCode.Z))
        {
            angleAroundX -= rotateSpeed;
        }
        if (Input.GetKey(KeyCode.S))
        {
            angleAroundX += rotateSpeed;
        }
        if (Input.GetKey(KeyCode.A))
        {
            angleAroundZ += rotateSpeed;
        }
        if (Input.GetKey(KeyCode.E))
        {
            angleAroundZ -= rotateSpeed;
        }

        if (Mathf.Abs(angleAroundX) >= 1)
        {
            transform.rotation *= Quaternion.AngleAxis(angleAroundX, Vector3.right);
        }

        if (Mathf.Abs(angleAroundY) >= 1)
        {
            transform.rotation *= Quaternion.AngleAxis(angleAroundY, Vector3.up);
        }

        if (Mathf.Abs(angleAroundZ) >= 1)
        {
            transform.rotation *= Quaternion.AngleAxis(angleAroundZ, Vector3.back);
        }

        if (Input.GetKey(KeyCode.W))
        {
            // go higher
            transform.position += Vector3.up * moveSpeed;
        }

        if (Input.GetKey(KeyCode.X))
        {
            // go down
            transform.position += Vector3.down * moveSpeed;
        }
    }
    void Update () {
        // moving
        if (isFrenchKeyboard)
        {
            OnFrenchKeyboard();
        }
        else
        {
            OnEnglishKeyboard();
        }
    }
}
