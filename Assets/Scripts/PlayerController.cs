using UnityEngine;
using System.Collections;

/// <summary>
/// Permite al jugador mover, girar y lanzar la pelota hacia los bolos.
/// Tambien se reinicia a su posicion inicial cuando es necesario.
/// </summary>
public class PlayerController : MonoBehaviour 
{
	Rigidbody rb;

	// Guarda la posicion y rotacion inicial de la bola, para que vuelva a ese mismo estado en el momento necesario.
	Vector3 InitialPosition;
	Quaternion InitialRotation;

	// Impide que el jugador puede dar a la bola mas de una vez en el mismo lanzamiento.
	bool lanzar;

	// Tope del movimiento lateral y la rotacion que el jugador le puede dar.
	int movimientoMax;
	int rotacionMax;

	// Objeto de ayuda al jugador saber en que direccion va a ir la pelota.
	public GameObject bloqueApuntar;
	
	void Start () 
	{
		rb = GetComponent<Rigidbody> ();
		InitialRotation = transform.rotation;
		InitialPosition = transform.position;
		lanzar = true;
		movimientoMax = 15; 
		rotacionMax = 15;
	}

	void OnMouseDown()
	{
		if (lanzar)
		{
			// Llama al GameManagerController para que aparte de que lance la pelota tambien realice otras operaciones.
			GameManagerController.Instance.Lanzar();
		}
	}

	/// <summary>
	/// Aplica fuerza a la bola y hace desaparecer el objeto que ayuda a apuntado.
	/// </summary>
	public void Lanzar()
	{
		rb.AddRelativeForce (new Vector3 (-25000, 0, 0));
		bloqueApuntar.SetActive (false);
		lanzar = false;
	}

	/// <summary>
	/// Vuelve la pelota a su posicion inicial, y la prepara para que se pueda lanzar de nuevo.
	/// </summary>
	public void Reiniciar()
	{
		transform.position = InitialPosition;
		transform.rotation = InitialRotation;
		rb.velocity = Vector3.zero;
		rb.angularVelocity = Vector3.zero;
		lanzar = true;
		bloqueApuntar.SetActive (true);
	}

	/// <summary>
	/// Permite mover la pelota lateralmente (hasta un limite) antes de lanzarla.
	/// </summary>
	/// <param name="numero"> Numero modificador del desplazamiento <ej</param>
	public void Mover(float numero)
	{
		transform.position = new Vector3 (transform.position.x, transform.position.y, InitialPosition.z + numero * movimientoMax);
	}

	/// <summary>
	/// Permite rotar la pelota hasta un angulo determinado.
	/// </summary>
	/// <param name="numero">Numero modificador de la rotacion</param>
	public void Rotar(float numero)
	{
		transform.rotation = Quaternion.Euler(0, numero * rotacionMax, 0);
	}
}
