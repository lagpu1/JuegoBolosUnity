using UnityEngine;
using System.Collections;

/// <summary>
/// Hace desaparecer la pelota y los bolos que hayan sido derribados y llama al GameManager para que cambie el turno.
/// </summary>
public class RecogedorController : MonoBehaviour 
{
	/// <summary>
	/// Ejecuta el trigger para que se produzca la animacion correspondiente y remueva los objetos.
	/// </summary>
	public void Recoger()
	{
		GetComponent<Animator>().SetTrigger("Recoger");
	}

	/// <summary>
	/// Se ejecuta al final de la animacion y llama a la funcion del mismoo nombre del GameManager.
	/// </summary>
	void cambiarTurno()
	{
		GameManagerController.Instance.cambiarTurno();
	}
	
	void OnTriggerEnter (Collider other)
	{
		if (other.tag == "Player")
			other.gameObject.SetActive(false);
		// Solo los bolos que sabemos que han caido van a desparecer.
		else if (other.tag == "Bolo" && other.GetComponent<BoloController>().caido)
			other.gameObject.SetActive(false);
	}
}
