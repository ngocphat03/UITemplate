namespace AXitUnityTemplate.Utilities
{
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    class CardinalDirection
    {
        public static readonly Vector2 Up        = new Vector2(0, 1);
        public static readonly Vector2 Down      = new Vector2(0, -1);
        public static readonly Vector2 Right     = new Vector2(1, 0);
        public static readonly Vector2 Left      = new Vector2(-1, 0);
        public static readonly Vector2 UpRight   = new Vector2(1, 1);
        public static readonly Vector2 UpLeft    = new Vector2(-1, 1);
        public static readonly Vector2 DownRight = new Vector2(1, -1);
        public static readonly Vector2 DownLeft  = new Vector2(-1, -1);
    }

    public enum Swipe
    {
        None,
        Up,
        Down,
        Left,
        Right,
        UpLeft,
        UpRight,
        DownLeft,
        DownRight
    };

    public class SwipeManager : MonoBehaviour
    {
        #region Inspector Variables

        [Tooltip("Min swipe distance (inches) to register as swipe")]
        [SerializeField] float minSwipeLength = 0.5f;

        [Tooltip("If true, a swipe is counted when the min swipe length is reached. If false, a swipe is counted when the touch/click ends.")]
        [SerializeField] bool triggerSwipeAtMinLength = false;

        [Tooltip("Whether to detect eight or four cardinal directions")]
        [SerializeField] bool useEightDirections = false;

        #endregion

        const float eightDirAngle = 0.906f;
        const float fourDirAngle  = 0.5f;
        const float defaultDPI    = 72f;
        const float dpcmFactor    = 2.54f;

        static Dictionary<Swipe, Vector2> cardinalDirections = new Dictionary<Swipe, Vector2>()
        {
            { Swipe.Up,         CardinalDirection.Up                 },
            { Swipe.Down,         CardinalDirection.Down             },
            { Swipe.Right,         CardinalDirection.Right             },
            { Swipe.Left,         CardinalDirection.Left             },
            { Swipe.UpRight,     CardinalDirection.UpRight             },
            { Swipe.UpLeft,     CardinalDirection.UpLeft             },
            { Swipe.DownRight,     CardinalDirection.DownRight         },
            { Swipe.DownLeft,     CardinalDirection.DownLeft         }
        };

        public delegate void OnSwipeDetectedHandler(Swipe swipeDirection, Vector2 swipeVelocity);

        static OnSwipeDetectedHandler _OnSwipeDetected;
        public static event OnSwipeDetectedHandler OnSwipeDetected
        {
            add
            {
                SwipeManager._OnSwipeDetected += value;
                SwipeManager.autoDetectSwipes =  true;
            }
            remove
            {
                SwipeManager._OnSwipeDetected -= value;
            }
        }

        public static Vector2 swipeVelocity;

        static float        dpcm;
        static float        swipeStartTime;
        static float        swipeEndTime;
        static bool         autoDetectSwipes;
        static bool         swipeEnded;
        static Swipe        swipeDirection;
        static Vector2      firstPressPos;
        static Vector2      secondPressPos;
        static SwipeManager instance;


        void Awake()
        {
            SwipeManager.instance = this;
            float dpi = (Screen.dpi == 0) ? SwipeManager.defaultDPI : Screen.dpi;
            SwipeManager.dpcm = dpi / SwipeManager.dpcmFactor;
        }

        void Update()
        {
            if (SwipeManager.autoDetectSwipes)
            {
                SwipeManager.DetectSwipe();
            }
        }

        /// <summary>
        /// Attempts to detect the current swipe direction.
        /// Should be called over multiple frames in an Update-like loop.
        /// </summary>
        static void DetectSwipe()
        {
            if (SwipeManager.GetTouchInput() || SwipeManager.GetMouseInput())
            {
                // Swipe already ended, don't detect until a new swipe has begun
                if (SwipeManager.swipeEnded)
                {
                    return;
                }

                Vector2 currentSwipe = SwipeManager.secondPressPos - SwipeManager.firstPressPos;
                float   swipeCm      = currentSwipe.magnitude / SwipeManager.dpcm;

                // Check the swipe is long enough to count as a swipe (not a touch, etc)
                if (swipeCm < SwipeManager.instance.minSwipeLength)
                {
                    // Swipe was not long enough, abort
                    if (SwipeManager.instance.triggerSwipeAtMinLength) return;
                
                    // if (Application.isEditor)
                    // {
                    //     Debug.Log("[SwipeManager] Swipe was not long enough.");
                    // }

                    SwipeManager.swipeDirection = Swipe.None;

                    return;
                }

                SwipeManager.swipeEndTime   = Time.time;
                SwipeManager.swipeVelocity  = currentSwipe * (SwipeManager.swipeEndTime - SwipeManager.swipeStartTime);
                SwipeManager.swipeDirection = SwipeManager.GetSwipeDirByTouch(currentSwipe);
                SwipeManager.swipeEnded     = true;

                if (SwipeManager._OnSwipeDetected != null)
                {
                    SwipeManager._OnSwipeDetected(SwipeManager.swipeDirection, SwipeManager.swipeVelocity);
                }
            }
            else
            {
                SwipeManager.swipeDirection = Swipe.None;
            }
        }

        public static bool IsSwiping()          { return SwipeManager.swipeDirection != Swipe.None; }
        public static bool IsSwipingRight()     { return SwipeManager.IsSwipingDirection(Swipe.Right); }
        public static bool IsSwipingLeft()      { return SwipeManager.IsSwipingDirection(Swipe.Left); }
        public static bool IsSwipingUp()        { return SwipeManager.IsSwipingDirection(Swipe.Up); }
        public static bool IsSwipingDown()      { return SwipeManager.IsSwipingDirection(Swipe.Down); }
        public static bool IsSwipingDownLeft()  { return SwipeManager.IsSwipingDirection(Swipe.DownLeft); }
        public static bool IsSwipingDownRight() { return SwipeManager.IsSwipingDirection(Swipe.DownRight); }
        public static bool IsSwipingUpLeft()    { return SwipeManager.IsSwipingDirection(Swipe.UpLeft); }
        public static bool IsSwipingUpRight()   { return SwipeManager.IsSwipingDirection(Swipe.UpRight); }

        #region Helper Functions

        static bool GetTouchInput()
        {
            if (Input.touches.Length > 0)
            {
                Touch t = Input.GetTouch(0);

                // Swipe/Touch started
                if (t.phase == TouchPhase.Began)
                {
                    SwipeManager.firstPressPos  = t.position;
                    SwipeManager.swipeStartTime = Time.time;
                    SwipeManager.swipeEnded     = false;
                    // Swipe/Touch ended
                }
                else if (t.phase == TouchPhase.Ended)
                {
                    SwipeManager.secondPressPos = t.position;
                    return true;
                    // Still swiping/touching
                }
                else
                {
                    // Could count as a swipe if length is long enough
                    if (SwipeManager.instance.triggerSwipeAtMinLength)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        static bool GetMouseInput()
        {
            // Swipe/Click started
            if (Input.GetMouseButtonDown(0))
            {
                SwipeManager.firstPressPos  = (Vector2)Input.mousePosition;
                SwipeManager.swipeStartTime = Time.time;
                SwipeManager.swipeEnded     = false;
                // Swipe/Click ended
            }
            else if (Input.GetMouseButtonUp(0))
            {
                SwipeManager.secondPressPos = (Vector2)Input.mousePosition;
                return true;
                // Still swiping/clicking
            }
            else
            {
                // Could count as a swipe if length is long enough
                if (SwipeManager.instance.triggerSwipeAtMinLength)
                {
                    return true;
                }
            }

            return false;
        }

        static bool IsDirection(Vector2 direction, Vector2 cardinalDirection)
        {
            var angle = SwipeManager.instance.useEightDirections ? SwipeManager.eightDirAngle : SwipeManager.fourDirAngle;
            return Vector2.Dot(direction, cardinalDirection) > angle;
        }

        static Swipe GetSwipeDirByTouch(Vector2 currentSwipe)
        {
            currentSwipe.Normalize();
            var swipeDir = SwipeManager.cardinalDirections.FirstOrDefault(dir => SwipeManager.IsDirection(currentSwipe, dir.Value));
            return swipeDir.Key;
        }

        static bool IsSwipingDirection(Swipe swipeDir)
        {
            SwipeManager.DetectSwipe();
            return SwipeManager.swipeDirection == swipeDir;
        }

        #endregion
    }
}