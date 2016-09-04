using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// Controla a los objetos de la escena y el juego en si.
/// </summary>
public class GameManagerController : MonoBehaviour 
{
	// Solo queremos que haya un GameManager en el juego que pueda ser accedido gobalmente.
	static GameManagerController instance;

	/// <summary>
	/// Devuelve la referencia a esta instancia, por lo que proporciona un acceso global.
	/// </summary>
	/// <value>La instancia</value>
	public static GameManagerController Instance
	{
		get { return instance; }
	}

	// Es el que controla en que tirada del turno esta actualmente.
	enum GameThrow { tiro1, tiro2, tiro3 }
	GameThrow tiradaTurno;
	
	public Camera secondaryCamera;

	// Los scripts (que estan enlazados con el objeto en si) se añaden desde la interfaz de Unity.
	public BoloController[] bolos;
	public RecogedorController recogedor;
	public PlayerController jugador;
	public CanvasController canvas;

	// Puntuacion de cada turno, la puntuacion total y el numero del turno en el que esta el juego actualmente
	int[] puntuacionTurno = new int[10];
	int puntuacionTotal;
	int turno;

	// Tiempo antes de que el recogedor se active
	float tiempoEspera;

	// Garantiza que solo haya una instancia
	void Awake()
	{
		if (instance == null)
			instance = this;
		else
			Destroy(this);
	}

	// Prepara el inicio
	void Start () 
	{
		secondaryCamera.enabled = false;
		tiempoEspera = 8f;

		jugador.gameObject.SetActive (false);

		for (int i=0; i < puntuacionTurno.Length; i++)
			bolos[i].gameObject.SetActive(false);
	}

	/// <summary>
	/// Tras la caida de un bolo se suma 1 al la puntuacion de ese turno y se muestra en pantalla el cambio.
	/// </summary>
	public void BoloDerribado()
	{
		puntuacionTurno[turno]++;
		canvas.cambiarTextoPuntuacion(puntuacionTurno[turno]);
	}

	/// <summary>
	/// Corrutina que espera una cantidad de tiempo determinada antes de ejecutar al recogedor su determinada accion.
	/// </summary>
    public IEnumerator CuentaAtras()
    {
        yield return new WaitForSeconds(tiempoEspera);
		
		recogedor.Recoger ();
        yield break;
    }

	/// <summary>
	/// Aparece la camara secundaria
	/// </summary>
	public void AparecerCamara()
	{
		secondaryCamera.enabled = true;
	}

	/// <summary>
	/// Desaparece la camara secundaria
	/// </summary>
	public void DesaparecerCamara()
	{
		secondaryCamera.enabled = false;
	}

	/// <summary>
	/// Hace desaparecer todo lo relacionado con la interfaz del jugador y lanza la bola del jugador
	/// </summary>
	public void Lanzar()
	{
		jugador.Lanzar ();
		canvas.desactivarUIJugador ();
	}

	/// <summary>
	/// Salir del juego
	/// </summary>
	public void Salir()
	{
		Application.Quit ();
	}

	/// <summary>
	/// Se prepara todo para que el juega pueda dar comienzo
	/// </summary>
	public void Jugar()
	{
		jugador.gameObject.SetActive (true);

		for (int i=0; i<bolos.Length; i++)
		{
			bolos [i].gameObject.SetActive (true);
			puntuacionTurno[i] = 0;
		}

		turno = 0;
		puntuacionTotal = 0;
		tiradaTurno = GameThrow.tiro1;
		cambiarTextoTiros();
		canvas.cambiarTextoPuntuacion(puntuacionTurno[turno]);
		canvas.cambiarTextoTurno(turno);
	}

	/// <summary>
	/// En funcion de la fase en que este tiradaTurno, se actualiza el texto Tiros con el numero correspondiente
	/// </summary>
	public void cambiarTextoTiros()
	{
		int tiro = 0;

		switch (tiradaTurno)
		{
			case GameThrow.tiro1:
			{
				tiro = 1;
				break;
			}
			case GameThrow.tiro2:
			{
				tiro = 2;
				break;
			}
			case GameThrow.tiro3:
			{
				tiro = 3;
				break;
			}
		}

		canvas.cambiarTextoTiros (tiro);
	}

	/// <summary>
	/// Controla los cambios de turnos y cuando se debe ejecutar el fin del juego
	/// </summary>
	public void cambiarTurno()
	{
		// Primera tirada del turno
		if (tiradaTurno == GameThrow.tiro1)
		{
			jugador.Reiniciar();
			jugador.gameObject.SetActive (true);
			DesaparecerCamara();
			canvas.activarUIJugador();

			// Si el tiro no ha llegado a 10 (pleno)
			if (puntuacionTurno[turno] < 10)
			{
				tiradaTurno = GameThrow.tiro2;
				cambiarTextoTiros();
			}

			// Si hay pleno
			else
			{
				// Reinicio de los bolos
				for (int i = 0; i<bolos.Length ;i++)
				{
					bolos[i].Reiniciar();
					bolos[i].gameObject.SetActive(true);
				}

				// Si el turno es distinto de 9 (turno 10 en la realidad), se debe cambiar el turno
				if (turno != 9)
				{
					tiradaTurno = GameThrow.tiro1;
					cambiarTextoTiros();

					puntuacionTotal += puntuacionTurno[turno];

					turno++;
					canvas.cambiarTextoPuntuacion(puntuacionTurno[turno]);
					canvas.cambiarTextoTurno(turno);
				}
				else
					tiradaTurno = GameThrow.tiro2;
			}
		}

		// Segunda tirada del turno
		else if (tiradaTurno == GameThrow.tiro2)
		{
			DesaparecerCamara();
			jugador.Reiniciar();

			// Si todavia no es el turno 9 (turno 10 real), se cambia el turno
			if (turno < 9)
			{
				tiradaTurno = GameThrow.tiro1;
				cambiarTextoTiros();

				jugador.gameObject.SetActive (true);
				canvas.activarUIJugador();
				
				for (int i = 0; i<bolos.Length ;i++)
				{
					bolos[i].gameObject.SetActive(true);
					bolos [i].Reiniciar();
				}

				puntuacionTotal += puntuacionTurno[turno];

				turno++;
				canvas.cambiarTextoPuntuacion(puntuacionTurno[turno]);
				canvas.cambiarTextoTurno(turno);
			}

			// Actual turno 10
			else if (turno == 9) 
			{
				// No llega a hacer semipleno o pleno en el turno: Fin del juego
				if (puntuacionTurno[turno] < 10)
				{
					for (int i = 0; i<bolos.Length ;i++)
					{
						bolos[i].Reiniciar();
						bolos[i].gameObject.SetActive(false);
					}

					puntuacionTotal += puntuacionTurno[turno];
					canvas.finDelJuego(puntuacionTotal);
				}

				// Puede haber realizado un pleno, semipleno o dos plenos
				else
				{
					jugador.gameObject.SetActive (true);
					canvas.activarUIJugador();

					// Puede tirar una vez mas
					tiradaTurno = GameThrow.tiro3;
					cambiarTextoTiros();

					// Comprueba si todos los bolos estan caidos. Si es asi, se reinician y se hacen aparecer
					int contador;
					for (contador=0; contador<bolos.Length && bolos[contador].caido; contador++);
					
					if (contador == bolos.Length)
					{
						for (int i = 0; i<bolos.Length ;i++)
						{
							bolos[i].gameObject.SetActive(true);
							bolos [i].Reiniciar();
						}
					}
				}
			}
		}

		// Tercera tirada: Fin del juego
		else if (tiradaTurno == GameThrow.tiro3)
		{
			DesaparecerCamara();
			jugador.Reiniciar();

			for (int i = 0; i<bolos.Length ;i++)
			{
				bolos [i].Reiniciar();
				bolos[i].gameObject.SetActive(false);
			}

			puntuacionTotal += puntuacionTurno[turno];
			canvas.finDelJuego(puntuacionTotal);
		}
	}
	
}
