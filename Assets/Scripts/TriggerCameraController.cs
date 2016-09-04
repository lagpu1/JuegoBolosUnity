using UnityEngine;
using System.Collections;

/// <summary>
/// Controla que al colisionar con la bola del jugador, la camara secundaria se active.
/// Ademas activa una cuenta atras del GameManager para que al acabar el tiempo active el recogedor.
/// </summary>
public class TriggerCameraController : MonoBehaviour 
{
	void OnTriggerEnter (Collider other)
	{
		if (other.tag == "Player")    
		{
			GameManagerController.Instance.AparecerCamara();
			// Activa la corrutina CuentaAtras() del GameManager
            StartCoroutine(GameManagerController.Instance.CuentaAtras());
        }
	}
}
