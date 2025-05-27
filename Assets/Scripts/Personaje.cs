using System.Collections;
using UnityEngine;

public class Personaje : MonoBehaviour
{
    [SerializeField] float velocidadMovimineto;
    [SerializeField] float velocidadRotacion;
    [SerializeField] float frecuenciaPisadas;

    private CharacterController characterController;
    private Camera camara;
    private AudioSource audioSource;

    private Vector3 movimiento;
    private float rotacionY;
    private Coroutine coroutine;
    private bool caminando;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        camara = Camera.main;
        audioSource = GetComponent<AudioSource>();
    }

    private void FixedUpdate()
    {
        Movimiento();
        MovimientoCamara();
    }

    void Movimiento()
    {
        float movX = Input.GetAxis("Horizontal");
        float movZ = Input.GetAxis("Vertical");

        movimiento = transform.right * movX + transform.forward * movZ;
        characterController.SimpleMove(movimiento * velocidadMovimineto);

        if (movimiento != Vector3.zero && caminando == false)
        {
            caminando = true;
            coroutine = StartCoroutine(FrecuenciaPisadas());
        }
        else if (movimiento == Vector3.zero && coroutine != null)
        {
            caminando = false;
            StopCoroutine(coroutine);
        }
    }

    void MovimientoCamara()
    {
        float ratonX = Input.GetAxis("Mouse X") * velocidadRotacion;
        float ratonY = Input.GetAxis("Mouse Y") * velocidadRotacion;

        rotacionY -= ratonY;
        rotacionY = Mathf.Clamp(rotacionY, -90, 90);

        camara.transform.localRotation = Quaternion.Euler(rotacionY, 0, 0);
        transform.Rotate(Vector3.up * ratonX);
    }

    IEnumerator FrecuenciaPisadas()
    {
        while (true)
        {
            yield return new WaitForSeconds(frecuenciaPisadas);
            audioSource.Play();
        }
    }
}
