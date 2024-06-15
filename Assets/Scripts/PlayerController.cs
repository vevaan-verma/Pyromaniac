using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

    [Header("References")]
    [SerializeField] private SpriteRenderer scarf;
    [SerializeField] private Transform leftFoot;
    [SerializeField] private Transform rightFoot;
    private Rigidbody2D rb;
    private Animator anim;
    private ActionBar actionBar;

    [Header("Mechanics")]
    private Dictionary<MechanicType, bool> mechanicStatuses;

    [Header("Movement")]
    [SerializeField] private float walkSpeed;
    [SerializeField] private float sprintSpeed;
    private float horizontalInput;
    private bool isFacingRight;

    [Header("Jumping")]
    [SerializeField] private float jumpForce;

    [Header("Actions")]
    [SerializeField] private List<ActionProperties> actions;

    [Header("Elements")]
    [SerializeField] private Element[] elements;
    [SerializeField] private float elementFadeDuration;
    private int currElementIndex;

    [Header("Wand")]
    [SerializeField] private WandSlot wandSlot;
    [SerializeField] private Wand wand;
    private bool isWandEquipped;
    private Coroutine wandCoroutine;

    [Header("Wall")]
    [SerializeField] private WallPreview wallPreviewPrefab;
    [SerializeField] private GameObject fireWallPrefab; /* MUST HAVE ONE SECOND RISE ANIMATION */
    private WallPreview wallPreview;

    [Header("Ground Check")]
    [SerializeField] private float groundCheckRadius;
    [SerializeField] private LayerMask environmentMask;
    private bool isGrounded;

    private void Awake() {

        // set up mechanic statuses early so scripts can change them earlier too
        mechanicStatuses = new Dictionary<MechanicType, bool>();
        Array mechanics = Enum.GetValues(typeof(MechanicType)); // get all mechanic type values

        // add all mechanic types to dictionary
        foreach (MechanicType mechanicType in mechanics)
            mechanicStatuses.Add(mechanicType, true); // set all mechanics to true by default

    }

    private void Start() {

        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        actionBar = FindObjectOfType<ActionBar>();

        isFacingRight = true; // player faces right by default

        foreach (ActionProperties action in actions)
            actionBar.AddItem(action);

        scarf.DOColor(elements[currElementIndex].GetColor(), elementFadeDuration); // change scarf color

        actionBar.SelectSlot(0); // select first slot by default

        wallPreview = Instantiate(wallPreviewPrefab, Vector2.zero, Quaternion.identity); // instantiate wall preview

    }

    private void Update() {

        /* GROUND CHECK */
        isGrounded = Physics2D.OverlapCircle(leftFoot.position, groundCheckRadius, environmentMask) || Physics2D.OverlapCircle(rightFoot.position, groundCheckRadius, environmentMask); // check both feet for ground check

        /* MOVEMENT */
        horizontalInput = Input.GetAxisRaw("Horizontal");

        if (Input.GetKey(KeyCode.Space) && isGrounded)
            Jump();

        /* FLIPPING */
        CheckFlip();

        /* ACTION BAR SLOTS */
        // first number key is reserved for wand
        if (Input.GetKeyDown(KeyCode.Alpha1))
            wandSlot.SetSelected(!isWandEquipped); // toggle selection
        else if (Input.GetKeyDown(KeyCode.Alpha2))
            actionBar.SelectSlot(0);
        else if (Input.GetKeyDown(KeyCode.Alpha3))
            actionBar.SelectSlot(1);
        else if (Input.GetKeyDown(KeyCode.Alpha4))
            actionBar.SelectSlot(2);
        else if (Input.GetKeyDown(KeyCode.Alpha5))
            actionBar.SelectSlot(3);
        else if (Input.GetKeyDown(KeyCode.Alpha6))
            actionBar.SelectSlot(4);
        else if (Input.GetKeyDown(KeyCode.Alpha7))
            actionBar.SelectSlot(5);
        else if (Input.GetKeyDown(KeyCode.Alpha8))
            actionBar.SelectSlot(6);
        else if (Input.GetKeyDown(KeyCode.Alpha9))
            actionBar.SelectSlot(7);

        /* SPELL CASTING / SUMMONING */
        if (Input.GetMouseButtonDown(0))
            if (isWandEquipped && wandCoroutine == null) // make sure wand is equipped and not playing animation
                wand.CastSpell(elements[currElementIndex].GetGradient()); // cast spell
            else if (wallPreview.CanPlaceWall())
                StartCoroutine(PlaceWall());

    }

    private void FixedUpdate() {

        if (mechanicStatuses[MechanicType.Movement]) {

            rb.velocity = new Vector2(horizontalInput * (isWandEquipped ? walkSpeed : sprintSpeed), rb.velocity.y); // adjust input based on rotation (if wand is out, player walks, else sprint)
            anim.SetBool("isMoving", horizontalInput != 0f && isGrounded); // player is moving on ground

        } else {

            rb.velocity = new Vector2(0f, rb.velocity.y); // stop player horizontal movement
            anim.SetBool("isMoving", false); // player is not moving

        }
    }

    private void CheckFlip() {

        if (isFacingRight && horizontalInput < 0f || !isFacingRight && horizontalInput > 0f) {

            transform.Rotate(0f, 180f, 0f);
            isFacingRight = !isFacingRight;

        }
    }

    private void Jump() {

        if (!mechanicStatuses[MechanicType.Jump]) return;

        rb.velocity = transform.up * new Vector2(rb.velocity.x, jumpForce);

    }

    public void ToggleWand() {

        if (wandCoroutine != null || !mechanicStatuses[MechanicType.Wand]) return; // wand is still playing animation

        anim.SetBool("isWandEquipped", !isWandEquipped); // bool hasn't been flipped yet, used opposite
        wandCoroutine = StartCoroutine(PlayWandAnim());

    }

    private IEnumerator PlayWandAnim() {

        if (!isWandEquipped) anim.SetTrigger("equipWand"); // equip wand
        else anim.SetTrigger("unequipWand"); // unequip wand

        yield return null; // wait for animation to play
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length); // wait for animation to finish
        isWandEquipped = !isWandEquipped;

        wandCoroutine = null;

    }

    private IEnumerator PlaceWall() {

        DisableAllMechanics(); // disable all mechanics
        anim.SetTrigger("summon"); // play summon animation

        yield return null; // wait for animation to play

        Animator wallAnim = Instantiate(fireWallPrefab, wallPreview.transform.position, Quaternion.identity).GetComponent<Animator>(); // summon fire wall
        wallAnim.speed = anim.GetCurrentAnimatorStateInfo(0).speed; // set wall rise speed to match player animation speed

        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length); // wait for animations to finish

        EnableAllMechanics(); // enable all mechanics

    }

    public void CycleElement() {

        currElementIndex = (currElementIndex + 1) % elements.Length; // cycle through elements
        scarf.DOColor(elements[currElementIndex].GetColor(), elementFadeDuration); // change scarf color

    }

    public void CycleElement(ActionType actionType) {

        for (int i = 0; i < actions.Count; i++)
            if (actions[i].GetActionType() == actionType)
                currElementIndex = i; // cycle to target element

        scarf.DOColor(elements[currElementIndex].GetColor(), elementFadeDuration); // change scarf color

    }

    public void EnableAllMechanics() {

        // enable all mechanics
        foreach (MechanicType mechanicType in mechanicStatuses.Keys.ToList())
            mechanicStatuses[mechanicType] = true;

    }

    public void DisableAllMechanics() {

        if (wandCoroutine != null) StopCoroutine(wandCoroutine); // stop wand coroutine
        if (isWandEquipped) ToggleWand(); // unequip wand

        // disable all mechanics
        foreach (MechanicType mechanicType in mechanicStatuses.Keys.ToList())
            mechanicStatuses[mechanicType] = false;

        // send to idle animation
        anim.SetBool("isMoving", false); // stop moving animation
        anim.SetBool("isWandEquipped", false); // unequip wand

    }
}
