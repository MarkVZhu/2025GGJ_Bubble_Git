using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class BubbleMovement : MonoBehaviour
{
    [Header("Sound Detect Settings")]
    public AudioLoudnessDetection detector;
    public float loudnessSensibility = 100;

    [Header("Bubble Size Settings")]
    public float inflateRate = 0.6f;
    public float shrinkRate = 0.1f;
    public float minSize = 0.3f;
    public float maxSize = 1.5f;
    public float warningThreshold = 0.7f;

    [Header("Bubble Movement Settings")]
    public float baseSpeed = 5f;
    public float sizeFactor = 3f;
    public float minSpeed = 1f;
    public float maxSpeed = 5f;

    [Header("Blow Force Curve")]
    public AnimationCurve loudnessToForceCurve = AnimationCurve.EaseInOut(0f, 0f, 5f, 125f);

    [Header("Additional Settings")]
    public float inertiaDampening = 0.99f;
    public float maxVelocity = 6f;

    [Header("Bubble Wobble Settings")]
    public float wobbleFrequency = 1f;
    public float wobbleAmplitude = 0.3f;

    [Header("Bubble Inflate Settings")]
    public float inflateRadius = 0.5f;

    [Header("Debug Info")]
    [SerializeField] private float currentSpeed;
    [SerializeField] private float currentSize;
    [SerializeField] private float currentBlowForce;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private bool hasPopped = false;
    private float wobbleTime;
    private Vector2 previousDirection;

    [Header("Rotation Correction")]
    public float rotationReturnFactor = 2f;
    public float angularDragWhileReturning = 0.2f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        rb.gravityScale = 0f;

        rb.drag = 1f;
        rb.angularDrag = 0f;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameManager.Instance.PauseGame();
        }

        if (GameManager.Instance.IsGameRunning && !hasPopped)
        {
            HandleSizeChange();
            HandleColorAndPopCheck();
            HandleBlowInput();
        }
    }

    private void FixedUpdate()
    {
        if (GameManager.Instance.IsGameRunning && !hasPopped)
        {
            HandleMovementPhysics();
        }
    }

    #region Bubble Size

    private void HandleSizeChange()
    {
        bool mouseOver = IsMouseOverCollider();

        if (mouseOver && Input.GetKey(KeyCode.Space) && Input.GetMouseButton(0))
        {
            transform.localScale += Vector3.one * inflateRate * Time.deltaTime;
        }
        else
        {
            transform.localScale -= Vector3.one * shrinkRate * Time.deltaTime;
        }

        float clampedSize = Mathf.Clamp(transform.localScale.x, minSize, maxSize);
        transform.localScale = Vector3.one * clampedSize;
        currentSize = clampedSize;
    }

    private void HandleColorAndPopCheck()
    {
        float scale = transform.localScale.x;
        float warningOffset = (maxSize - minSize) * (1f - warningThreshold);

        if (scale >= maxSize)
        {
            ChangeBubbleColor(Color.red);
            if (!hasPopped)
            {
                PopBubble("Bubble popped due to exceeding max size.");
            }
        }
        else if (scale <= minSize)
        {
            ChangeBubbleColor(Color.red);
            if (!hasPopped)
            {
                PopBubble("Bubble popped due to falling below min size.");
            }
        }
        else if (scale >= (maxSize - warningOffset) || scale <= (minSize + warningOffset))
        {
            ChangeBubbleColor(Color.yellow);
            hasPopped = false;
        }
        else
        {
            ChangeBubbleColor(Color.white);
            hasPopped = false;
        }
    }

    private void PopBubble(string reason)
    {
        Debug.Log(reason);
        hasPopped = true;
        GameManager.Instance.EndGame();
    }

    #endregion

    #region Blow Input

    private float cachedBlowForce = 0f;

    private void HandleBlowInput()
    {
        if (Input.GetKey(KeyCode.Space) && Input.GetMouseButton(0) && !IsMouseOverCollider())
        {
            float loudness = 0f;
            if (detector != null)
            {
                loudness = detector.GetLoudnessFromMicrophone() * loudnessSensibility;
            }
            loudness = Mathf.Clamp(loudness, 0f, 10f);

            // cachedBlowForce = loudnessToForceCurve.Evaluate(loudness);
            cachedBlowForce = 50f;
        }
        else
        {
            cachedBlowForce = 0f;
        }

        currentBlowForce = cachedBlowForce;
    }

    #endregion

    #region Movement Physics

    private void HandleMovementPhysics()
    {
        float scale = transform.localScale.x;
        currentSpeed = baseSpeed + (5f / scale) * sizeFactor;
        currentSpeed = Mathf.Clamp(currentSpeed, minSpeed, maxSpeed);

        rb.AddForce(Vector2.up * currentSpeed * Time.fixedDeltaTime, ForceMode2D.Force);

        if (cachedBlowForce > 0f)
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPos.z = 0f;
            Vector2 directionAway = (transform.position - mouseWorldPos).normalized;

            previousDirection = directionAway;

            rb.AddForce(directionAway * cachedBlowForce * Time.fixedDeltaTime, ForceMode2D.Force);

            float torqueFactor = 0.2f;
            float crossZ = Vector3.Cross(Vector2.up, directionAway).z;
            float torque = crossZ * cachedBlowForce * torqueFactor;

            rb.AddTorque(torque * Time.fixedDeltaTime, ForceMode2D.Force);

            rb.angularDrag = 0.1f;
        }
        else
        {

            rb.angularDrag = angularDragWhileReturning;


            float currentAngle = rb.rotation;



            float angleDiff = Mathf.DeltaAngle(currentAngle, 0f);


            float torqueToApply = angleDiff * rotationReturnFactor;


            rb.AddTorque(-torqueToApply * Time.fixedDeltaTime, ForceMode2D.Force);


        }


        if (rb.velocity.magnitude > maxVelocity)
        {
            rb.velocity = rb.velocity.normalized * maxVelocity;
        }
    }

    #endregion

    #region Helpers

    private void ChangeBubbleColor(Color color)
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = color;
        }
    }

    private bool IsMouseOverCollider()
    {
        Collider2D collider = GetComponent<Collider2D>();
        if (collider == null) return false;

        Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Check if any Collider2D within the radius overlaps
        Collider2D[] colliders = Physics2D.OverlapCircleAll(mouseWorldPos, inflateRadius);

        foreach (Collider2D col in colliders)
        {
            if (col == collider) return true;
        }

        return false;
    }

    public void SetHasPopped(bool p)
    {
        hasPopped = p;
    }

    #endregion
}
