using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PAnim : MonoBehaviour
{

    private Animator _animator;
    // Start is called before the first frame update
    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var xx = Input.GetAxisRaw("Horizontal");

        _animator.SetFloat("Move", Mathf.Abs(xx));

        if (Input.GetKeyDown(KeyCode.Space))
            _animator.SetTrigger("Attack");

    }
}
