using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public RawImage Blood;
    public GameObject WastedText;
    public float Speed = 2;
    public AudioSource HitSound;
    public AudioSource Walking;
    public AudioSource Wasted;

    private float _health = 100;
    private bool _isDead;

    void Update()
    {
        if (_isDead)
        {
            WastedText.transform.localScale *= 1 + Time.deltaTime/2;
            return;
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            Speed = 2.5f;
        }
        else
        {
            Speed = 2;
        }

        if (!Walking.isPlaying)
        {
            Walking.Play();
        }
        if (Input.GetKey(KeyCode.W))
        {
            transform.position += transform.forward * Speed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            transform.position -= transform.forward * Speed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            transform.position -= transform.right * Speed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.position += transform.right * Speed * Time.deltaTime;
        }
        else
        {
            Walking.Stop();
        }

        _health = Mathf.Clamp(_health + Time.deltaTime, 0, 100);
        Color color = Blood.color;
        color.a = (100 - _health) / 100;
        Blood.color = color;
    }

    void OnCollisionStay(Collision collision)
    {
        if (_isDead)
        {
            return;
        }

        Car car = collision.collider.GetComponent<Car>();
        if (car != null)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                car.EnterCar(gameObject);
            }
        }
    }

    public void Hit(int amount = 30)
    {
        if (_isDead)
        {
            return;
        }

        _health -= amount;
        HitSound.Play();

        if (_health <= 0)
        {
            _isDead = true;

            Color color = Blood.color;
            color.a = 1;
            Blood.color = color;

            WastedText.SetActive(true);
            Wasted.Play();
            StartCoroutine(ResetLevel());
        }
    }

    private IEnumerator ResetLevel()
    {
        yield return new WaitForSeconds(2);

        SceneManager.LoadScene(0);
    }
}
