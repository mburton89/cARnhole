using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

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
    public Vector3 initialSpawnPoint;
    public Vector3 initialAngles;

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
        initialSpawnPoint = transform.position;
        initialAngles = transform.eulerAngles;
        InvokeRepeating(nameof(CheckMomentum), 0, .5f);
    }

    public void Throw()
    {
        _hasThrown = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        SoundManager.Instance.PlayBagHitSound();

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
            transform.position = initialSpawnPoint;
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

            if (_pointsToGive == 3)
            {
                gameObject.tag = "ThreePoints";
            }
        }
    }

    public void MoveToSpawnPoint()
    {
        StartCoroutine(MoveToSpawnPointCo());
    }

    private IEnumerator MoveToSpawnPointCo()
    {
        GetComponent<Collider>().enabled = false;
        GetComponent<Rigidbody>().isKinematic = true;
        transform.DOMove(initialSpawnPoint, 3f, false).SetEase(Ease.InOutQuad);
        transform.DORotate(initialAngles, 3f, RotateMode.Fast).SetEase(Ease.InOutQuad);
        yield return new WaitForSeconds(3.01f);
        transform.position = initialSpawnPoint;
        transform.eulerAngles = initialAngles;
        GetComponent<Collider>().enabled = true;
        GetComponent<Rigidbody>().isKinematic = false;
        yield return new WaitForSeconds(.1f);
        _hasThrown = false;
        _hasHitBoard = false;
        _hasHitHole = false;
        _hasHitNoPoints = false;
        _gavePointsToPlayer = false;
        _pointsToGive = 0;
        gameObject.tag = "Untagged";
    }
}
