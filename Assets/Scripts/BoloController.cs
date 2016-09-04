using UnityEngine;
using System.Collections;

/// <summary>
/// Controla que al ser derribado active la funcion correspondiente del GameManager: BoloDerribado(). 
/// Tambien reinicia el bolo cuando se lo requiera.
/// </summary>
public class BoloController : MonoBehaviour 
{
	// Necesario para que llame la funcion BoloDerribado del GameManagerController una sola vez.
	public bool caido; 

	// Guarda la posicion y la rotacion inicial del bolo, para que cuando reinicie se situe en el lugar correspondiente.
	Vector3 InitialPosition;
	Quaternion InitialRotation;
	
	void Start () 
	{
		caido = false;
		InitialRotation = transform.rotation;
		InitialPosition = transform.position;
	}

	void OnCollisionEnter(Collision collision)
	{
		// Si el bolo aun no ha caido pero su rotacion en el eje x o en el z es superior a los 45º, se puede confirmar que el bolo va a caer.
		if (!caido &&
		    ((transform.rotation.eulerAngles.x > -45 && transform.rotation.eulerAngles.x < -135) || 
		 	(transform.rotation.eulerAngles.z > 45 && transform.rotation.eulerAngles.z < 315)))
		{
			caido = true;
			GameManagerController.Instance.BoloDerribado();
		}
	}

	/// <summary>
	/// Vuelve a poner al bolo en su posicion inicial.
	/// </summary>
	public void Reiniciar()
	{
		transform.position = InitialPosition;
		transform.rotation = InitialRotation;
		// Se elimina cualquier velocidad que el Rigidbody tenga en el momento de reiniciar.
		GetComponent<Rigidbody>().velocity = Vector3.zero;
		GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
		caido = false;
	}
}
