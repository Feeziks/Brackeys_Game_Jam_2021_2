using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControls : MonoBehaviour
{
  public InputController inputs;
  public float rotationSpeed;





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
  }




}
