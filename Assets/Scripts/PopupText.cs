using UnityEngine;

public class PopupText : MonoBehaviour
{
  float life_time = 1.0f;
  float up_speed = 20.0f;

  void Update()
  {
    var dt = Time.deltaTime;
    life_time -= dt;
    var current_position = transform.position;
    current_position.y += dt * up_speed;
    transform.position = current_position;

    if(life_time < 0)
      GameObject.Destroy(gameObject);
  }
}
