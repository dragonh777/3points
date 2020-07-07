using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Siege_AcidPotBullet : MonoBehaviour
{
    Animator _animator;
    Rigidbody2D _rigid;
    CapsuleCollider2D _capColl;
    GameObject _parent;
    private bool isHit = false;

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();    
        _rigid = GetComponent<Rigidbody2D>();
        _capColl = GetComponent<CapsuleCollider2D>();
        _parent = gameObject.transform.parent.gameObject;

        //if(gameObject.tag == "Siege_EnemyBullet") {

        //    if(Siege_EAcidPot.attackPosition) {
        //        _rigid.AddForce(new Vector2(12, 7), ForceMode2D.Impulse);
        //    }
        //    else if(!Siege_EAcidPot.attackPosition) {
        //        _rigid.AddForce(new Vector2(-12, 7), ForceMode2D.Impulse);
        //    }

        //}
        //else {

        //    if(Siege_PAcidPot.attackPosition) {
        //        _rigid.AddForce(new Vector2(10, 5), ForceMode2D.Impulse);
        //    }
        //    else if(!Siege_PAcidPot.attackPosition) {
        //        _rigid.AddForce(new Vector2(-10, 5), ForceMode2D.Impulse);
        //    }

        //}

        if(_parent.name == "Player") {
            if (_parent.GetComponent<Siege_EAcidPot>().attackPosition) {
                _rigid.AddForce(new Vector2(12, 7), ForceMode2D.Impulse);
            }
            else if (!_parent.GetComponent<Siege_EAcidPot>().attackPosition) {
                _rigid.AddForce(new Vector2(-12, 7), ForceMode2D.Impulse);
            }
        }
        else if(_parent.name == "EnemyAcid") {
            if (_parent.GetComponent<Siege_EAcidPot>().attackPosition) {
                _rigid.AddForce(new Vector2(10, 5), ForceMode2D.Impulse);
            }
            else if (!_parent.GetComponent<Siege_EAcidPot>().attackPosition) {
                _rigid.AddForce(new Vector2(-10, 5), ForceMode2D.Impulse);
            }
        }
        else {
            if (Siege_PAcidPot.attackPosition) {
                _rigid.AddForce(new Vector2(10, 5), ForceMode2D.Impulse);
            }
            else if (!Siege_PAcidPot.attackPosition) {
                _rigid.AddForce(new Vector2(-10, 5), ForceMode2D.Impulse);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(!isHit) {
            //if(_parent.name == "Player" || _parent.name == "EnemyAcid") {
            //    if (_parent.GetComponent<Siege_EAcidPot>().attackPosition) {
            //        transform.Rotate(0f, 0f, -0.5f);
            //    }
            //    else if (!_parent.GetComponent<Siege_EAcidPot>().attackPosition) {
            //        transform.Rotate(0f, 0f, 0.5f);
            //    }
            //}
            //else {
            //    if (Siege_PAcidPot.attackPosition) {
            //        transform.Rotate(0f, 0f, -0.5f);
            //    }
            //    else if (!Siege_PAcidPot.attackPosition) {
            //        transform.Rotate(0f, 0f, 0.5f);
            //    }
            //}
            transform.Rotate(0f, 0f, 0.5f);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(gameObject.tag == "Siege_EnemyBullet" && (other.gameObject.tag == "Floor" || other.gameObject.tag == "Siege_Player")) {
            _capColl.enabled = false;
            _rigid.constraints = RigidbodyConstraints2D.FreezeAll;
            isHit = true;
            _animator.Play("hit");
        }
        else if(gameObject.tag == "Siege_Bullet" && (other.gameObject.tag == "Floor" || other.gameObject.tag == "Siege_Enemy")) {
            _capColl.enabled = false;
            _rigid.constraints = RigidbodyConstraints2D.FreezeAll;
            isHit = true;
            _animator.Play("hit");
        }
    }

    void EndEffect()
    {
        Destroy(gameObject);
    }
}
