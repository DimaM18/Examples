using UnityEngine;

public class SimplePhysicsExample : MonoBehaviour
{
    private Rigidbody _rigidBody;

    private void Start()
    {
        // Получаем компонент Rigidbody, можно и через SerializeField
        _rigidBody = GetComponent<Rigidbody>();

        if (_rigidBody == null)
        {
            Debug.LogError("Rigidbody не найден на объекте!");
        }
    }

    private void Update()
    {
        // При нажатии пробела применяем силу вперед
        if (Input.GetKeyDown(KeyCode.Space))
        {
            var force = transform.forward * 500f;
            _rigidBody.AddForce(force);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Выводим имя объекта, с которым столкнулись
        Debug.Log("Столкнулись с " + collision.gameObject.name);
    }
}