using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControls : MonoBehaviour
{
  public InputController inputs;
  public float rotationSpeed;
  public Camera cam;

  private float scrollWeight = 5f;


  private void Update()
  {
    HandleMovement();
  }


  private void HandleMovement()
  {
    if(inputs.movement != Vector2.zero)
    {
      //Rotate the camera parent object and the camera will follow
      transform.Rotate(new Vector3(0f, -1f * inputs.movement.x, inputs.movement.y), rotationSpeed * Time.deltaTime);
    }

    if (inputs.scroll != 0f)
    {
      cam.transform.position += (Vector3.zero - cam.transform.position).normalized * inputs.scroll * Time.deltaTime * scrollWeight;
    }
  }




}
