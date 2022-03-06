using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerThrowPlunger : MonoBehaviour
{
    private Transform localTransform;
    private Vector3 VectorLerp;
    private Vector3 savedDirection;
    private float desatascadorCooldown;
    private float timerPoint;
    private bool createMaterial;
    private bool desatascador;
    private GameObject desatascadorInstance;
    [SerializeField] private GameObject desatascadorPrefab;
    private int desatascadorBaseCooldown = 20;

    public void Desatascador(QuadControlSystem controls, PlayerVehicleScript player,  bool plungerEnabled, int playerNum)
    {
        RaycastHit hit;
        if (Physics.SphereCast(transform.position, 10, transform.TransformDirection(Vector3.forward), out hit, 10))
        {
            if ((hit.transform.tag.Contains("Player") || hit.transform.tag.Contains("Tree") || hit.transform.tag.Contains("Valla")) && (hit.transform.position - transform.position).magnitude > 5 && hit.transform != transform)
                localTransform = hit.transform;
        }

        if (localTransform != null)
            savedDirection = (localTransform.position - transform.position).normalized;

        LineRenderer line = GetComponent<LineRenderer>();

        line.SetPosition(0, Vector3.zero);
        line.SetPosition(1, Vector3.zero);

        bool isCorrect = transform.InverseTransformDirection(savedDirection).z > 0.75;

        if (savedDirection != Vector3.zero && desatascadorCooldown <= 0)
        {
            Vector3 sum = (transform.position + savedDirection * 3);

            if (isCorrect)
            {
                line.material.color = Color.green;
                timerPoint = 2;
                line.SetPosition(0, transform.position);
                line.SetPosition(1, sum);
            }
            else if (timerPoint > 0)
            {
                line.material.color = Color.red;
                line.SetPosition(0, transform.position);
                line.SetPosition(1, sum);
                timerPoint -= Time.deltaTime;
            }
            else
            {
                savedDirection = Vector3.zero;
            }
        }

        if ((controls.Quad.plunger || plungerEnabled) && !desatascador && desatascadorCooldown <= 0 && desatascadorInstance == null)
        {
            if (!createMaterial)
            {
                line.material = new Material(line.material);
                createMaterial = true;
            }

            desatascadorInstance = Instantiate(desatascadorPrefab, this.transform.position, this.transform.rotation);
            Physics.IgnoreCollision(desatascadorInstance.transform.GetChild(0).GetComponent<BoxCollider>(), transform.GetChild(0).GetComponent<BoxCollider>());
            desatascadorInstance.GetComponent<plungerInstance>().playerShotPlunger = this.gameObject;
            desatascadorInstance.GetComponent<plungerInstance>().playerNum = playerNum;
            desatascadorInstance.GetComponent<plungerInstance>().normalDir = savedDirection;
            desatascador = true;
            desatascadorCooldown = desatascadorBaseCooldown;
        }
        else
            plungerEnabled = false;

        if (desatascadorCooldown > 0)
            desatascadorCooldown -= Time.deltaTime;

        if (desatascador)
        {
            if (desatascadorCooldown <= desatascadorBaseCooldown / 2 && desatascadorInstance != null)
            {
                savedDirection = Vector3.zero;
                player.vehicleMaxSpeed = player.savedMaxSpeed;
                desatascadorInstance.GetComponent<plungerInstance>().destroyPlunger = true;
                desatascadorInstance = null;
                desatascador = false;
                plungerEnabled = false;
            }
            else if (desatascadorInstance == null)
            {
                savedDirection = Vector3.zero;
                player.vehicleMaxSpeed = player.savedMaxSpeed;
                desatascador = false;
            }
            if (player.vehicleMaxSpeed > player.savedMaxSpeed)
            {
                player.vehicleMaxSpeed -= 0.5f;
            }
        }
    }
}
