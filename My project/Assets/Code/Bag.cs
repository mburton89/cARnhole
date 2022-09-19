using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bag : MonoBehaviour
{
    private bool _hasThrown;
    private bool _hasHitBoard;
    private bool _hasHitHole;
    private bool _hasHitNoPoints;
    private bool _gavePointsToPlayer;
    private int _pointsToGive;
    public bool isPlayer1;
    private Rigidbody _rigidbody;
    public Transform initialSpawnPoint;

    private void OnDestroy()
    {
        print("OnDestroy");
    }

    private void Start()
    {
        _hasThrown = false;
        _hasHitBoard = false;
        _hasHitHole = false;
        _hasHitNoPoints = false;
        _gavePointsToPlayer = false;
        _pointsToGive = 0;
        _rigidbody = GetComponent<Rigidbody>();
        InvokeRepeating(nameof(CheckMomentum), 0, 1f);
    }

    public void Throw()
    {
        _hasThrown = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        SoundManager.Instance.bagHitSound.Play();

        if (collision.gameObject.CompareTag("OnePoint") && !_hasHitBoard)
        {
            _hasHitBoard = true;
            _pointsToGive += 1;
        }
        else if (collision.gameObject.CompareTag("ThreePoints") && !_hasHitHole)
        {
            _hasHitHole = true;
            if (_hasHitBoard)
            {
                _pointsToGive += 2;
                if (_gavePointsToPlayer)
                {
                    CarnholeManager.Instance.AwardPointsForRound(isPlayer1, 2, false);
                }
            }
            else
            {
                _pointsToGive += 3;
            }
        }
        else if (collision.gameObject.CompareTag("NoPoints"))
        {
            if (!_hasHitNoPoints && _hasHitBoard && !_hasHitHole)
            {
                _hasHitNoPoints = true;
                _pointsToGive -= 1;
                if (_gavePointsToPlayer)
                {
                    CarnholeManager.Instance.AwardPointsForRound(isPlayer1, -1, false);
                }
            }
        }
        else if (collision.gameObject.CompareTag("ResetThrow"))
        {
            _gavePointsToPlayer = false;
            _hasThrown = false;
        }
        else if (collision.gameObject.CompareTag("OutOfBounds"))
        {
            _rigidbody.velocity = Vector3.zero;
            transform.position = initialSpawnPoint.position;
            transform.eulerAngles = Vector3.zero;
            _gavePointsToPlayer = false;
            _hasThrown = false;
        }
    }

    void CheckMomentum()
    {
        if (_hasThrown && _rigidbody.velocity.magnitude < 0.1f && !_gavePointsToPlayer)
        {
            _gavePointsToPlayer = true;
            CarnholeManager.Instance.AwardPointsForRound(isPlayer1, _pointsToGive, true);

            if (_pointsToGive == 1)
            {
                gameObject.tag = "OnePoint";
            }
        }
    }
}
