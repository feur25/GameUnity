using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Animation _animation;

    public float walkSpeed = 3;
    public float runSpeed = 6;
    public float turnSpeed = 200;

    public string inputForward;
    public string inputBack;
    public string inputLeft;
    public string inputRight;

    public Vector3 jumpUp;
    CapsuleCollider playerCollider;
    Rigidbody rigidbody;
    public bool sprinting;

    void Awake() {
        _animation = gameObject.GetComponent<Animation>();
        playerCollider = gameObject.GetComponent<CapsuleCollider>();
        rigidbody = gameObject.GetComponent<Rigidbody>();
    }
    bool IsGrounded() {
        return Physics.CheckCapsule(playerCollider.bounds.center, new Vector3(playerCollider.bounds.center.x, playerCollider.bounds.min.y - 0.1f, playerCollider.bounds.center.z), 0.09f);
    }

    void Update() {

        bool forward = Input.GetKey(inputForward);
        bool back = Input.GetKey(inputBack);
        bool right = Input.GetKey(inputRight);
        bool left = Input.GetKey(inputLeft);
        bool sprint = Input.GetKey(KeyCode.LeftShift);
        bool jumping = Input.GetKeyDown(KeyCode.Space);

        float movement = right ? 1f : left ? -1f : 0f;
        float rotation = forward ? 1f : back ? -0.5f : 0f;

        float finalSpeed = sprint ? runSpeed : walkSpeed;

        transform.Rotate(0, rotation * turnSpeed * Time.deltaTime, 0);


        bool newSprinting = sprint && movement != 0f;
        if (newSprinting && !sprinting) {
            StartCoroutine(AnimationRun());
            _animation.Play("run");
        }else if (!newSprinting && sprinting) {
            _animation.Play("idle");
        }
        sprinting = newSprinting;

        if (movement != 0f) {
            _animation.Play("walk");
            transform.Translate(Vector3.forward * movement * finalSpeed * Time.deltaTime);
        }

        if (jumping && IsGrounded()){
            rigidbody.AddForce(Vector3.up * jumpUp.y, ForceMode.Impulse);
        }
    }

    IEnumerator AnimationRun() {
        if(Input.GetKey(inputBack)){
            transform.Translate(0,0, -(runSpeed/(float)2.5) * Time.deltaTime);
        }else if(Input.GetKey(inputForward)){
            float newRunSpeed = runSpeed -(float)1;
            Debug.Log(newRunSpeed);
            transform.Translate(0,0, newRunSpeed * Time.deltaTime);
        }
        yield return new WaitForSeconds(1);
        if(Input.GetKey(inputBack)){
            transform.Translate(0,0, -(runSpeed/2) * Time.deltaTime);
        }else if(Input.GetKey(inputForward)){
            transform.Translate(0,0, runSpeed * Time.deltaTime);
        }
    }
}