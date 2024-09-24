using UnityEngine;

public class ClockHandDragger : MonoBehaviour
{
    [SerializeField] private AnalogClockBehaviour _analogClockBehaviour;

    private bool _isDragging = false;
    private float _moveSpeed = 2f;

    private void Update()
    {
        if (_isDragging)
            DragHand();
    }

    private void OnMouseDown()
    {
        _isDragging = true;
    }

    private void OnMouseUp()
    {
        _isDragging = false;
    }

    private void DragHand()
    {
        Plane plane = new Plane(Vector3.up, _analogClockBehaviour.transform.position);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (plane.Raycast(ray, out float hitdist))
        {
            Vector3 targetPoint = ray.GetPoint(hitdist);
            Vector3 direction = targetPoint - transform.position;

            direction.y = 0;

            if (direction.sqrMagnitude > 0)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                targetRotation = Quaternion.Euler(0, targetRotation.eulerAngles.y, 0); 
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _moveSpeed * Time.deltaTime);
            }
        }
    }
}    