using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// Hace aparecer o desaparecer los objetos relacionados con la interfaz de usuario
/// </summary>
public class CanvasController : MonoBehaviour 
{
	// Los objetos del canvas se pasan por la interfaz de Unity
	public GameObject panel;
	public GameObject logo;
	public GameObject titulo;
	public GameObject botonEmpezar;
	public GameObject botonSalir;
	public GameObject botonVolverAlMenu;
	public GameObject moverPersonaje;
	public GameObject girarPersonaje;
	public GameObject textoGirarMover;

	// Textos que se pasan por la interfaz de Unity.
	public Text textoTiros;
	public Text puntuacionFinal;
	public Text textoTurno;
	public Text puntuacion;
	
	void Start () 
	{
		panel.SetActive (true);
		logo.SetActive (true);
		titulo.SetActive (true);
		botonEmpezar.SetActive (true);
		botonSalir.SetActive (true);
	}

	/// <summary>
	/// Desaparece el menu inicial para dar paso al juego en si
	/// </summary>
	public void Jugar()
	{
		GameManagerController.Instance.Jugar();

		panel.SetActive (false);
		logo.SetActive (false);
		titulo.SetActive (false);
		botonEmpezar.SetActive (false);
		botonSalir.SetActive (false);
		activarUIJugador ();
		textoTiros.gameObject.SetActive (true);
		textoTurno.gameObject.SetActive (true);
		puntuacion.gameObject.SetActive (true);
	}

	/// <summary>
	/// Desaparece todo lo relacionado con la interfaz del jugador
	/// </summary>
	public void desactivarUIJugador()
	{
		moverPersonaje.SetActive (false);
		girarPersonaje.SetActive (false);
		textoGirarMover.SetActive (false);
	}

	/// <summary>
	/// Aparece todo lo relacionado con la interfaz del jugador
	/// </summary>
	public void activarUIJugador()
	{
		moverPersonaje.SetActive (true);
		girarPersonaje.SetActive (true);
		textoGirarMover.SetActive (true);
	}

	/// <summary>
	/// Salir del juego
	/// </summary>
	public void Salir()
	{
		GameManagerController.Instance.Salir();	
	}

	/// <summary>
	/// Cambia el texto Tiros en funcion del numero que le llegue
	/// </summary>
	/// <param name="numero">Numero del tiro actual</param>
	public void cambiarTextoTiros(int numero)
	{
		textoTiros.text = "Tiro nº " + numero;
	}

	/// <summary>
	/// Cambia el texto Puntuacion en funcion del numero que le llegue
	/// </summary>
	/// <param name="numero">Numero actual de bolos tirados</param>
	public void cambiarTextoPuntuacion(int numero)
	{
		puntuacion.text = "" + numero;
	}

	/// <summary>
	/// Cambia el texto TextoTurno em funcion del numero que le llegue.
	/// </summary>
	/// <param name="numero">Numero actual del turno</param>
	public void cambiarTextoTurno(int numero)
	{
		textoTurno.text = "Turno " + (numero+1);
	}

	/// <summary>
	/// Desaparece la pantalla de fin de juego para mostrar de nuevo el menu inicial
	/// </summary>
	public void volverAlMenu()
	{
		logo.SetActive (true);
		titulo.SetActive (true);
		botonEmpezar.SetActive (true);
		botonSalir.SetActive (true);
		puntuacionFinal.gameObject.SetActive (false);
		botonVolverAlMenu.SetActive (false);	
	}

	/// <summary>
	/// Muestra la pantalla de fin del juego
	/// </summary>
	/// <param name="puntos">Puntos totales de la partida</param>
	public void finDelJuego(int puntos)
	{
		puntuacion.gameObject.SetActive (false);
		panel.SetActive (true);
		textoTiros.gameObject.SetActive (false);
		puntuacionFinal.gameObject.SetActive (true);
		botonVolverAlMenu.SetActive (true);
		textoTurno.gameObject.SetActive (false);

		puntuacionFinal.text = "¡Se terminó la partida! \nTu puntuación es de " + puntos + " puntos";
	}
}
