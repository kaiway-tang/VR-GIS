using System.Collections.Generic;
using UnityEngine;

public class GestureDetector : MonoBehaviour
{
    [SerializeField] Transform mapTrfm, orienterTrfm, separationReferenceTrfm;
    [SerializeField] Transform leftHandTrfm, rightHandTrfm;

    [SerializeField] int mode;
    const int DEFAULT = 0, ZOOM = 1, ROTATE = 2, LAYERS = 3;

    public static bool inSeparationMode;

    public TextDisplay mapDisplay, layersMapDisplay, LHandDisplay, RHandDisplay;

    public static GestureDetector self;

    private void Start()
    {
        orienterTrfm.parent = null;
        layerMovementMode = TO_ANCHORS;

        self = this;
    }

    bool shiftShortcutEnabled;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && shiftShortcutEnabled)
        {
            if (!inSeparationMode)
            {
                SeparateLayers();
            }
            else
            {
                CollapseLayers();
            }
        }
    }

    #region handle gestures

    public static HandState rightHandState;
    public static HandState leftHandState;

    [SerializeField] HandState _rightHandState;
    [SerializeField] HandState _leftHandState;

    public enum HandState
    {
        neutral,
        pinch,
        grab,
        point
    }

    public void RightPinch()
    {
        rightHandState = HandState.pinch;

        if (leftHandState == HandState.pinch)
        {
            EnterZoomMode();
        }
        else
        {
            EnterRotationMode();
        }

        for (int i = 0; i < layerScripts.Length; i++)
        {
            layerScripts[i].OnPinch(false);
        }
    }

    public void LeftPinch()
    {
        leftHandState = HandState.pinch;

        if (rightHandState == HandState.pinch)
        {
            EnterZoomMode();
        }
        else
        {
            EnterRotationMode();
        }

        for (int i = 0; i < layerScripts.Length; i++)
        {
            layerScripts[i].OnPinch(true);
        }
    }

    public void RightPinchRelease()
    {
        rightHandState = HandState.neutral;

        ExitZoomMode();

        if (mode == ROTATE && trackedHand == rightHandTrfm)
        {
            ExitRotationMode();
        }
    }

    public void LeftPinchRelease()
    {
        leftHandState = HandState.neutral;

        ExitZoomMode();

        if (mode == ROTATE && trackedHand == leftHandTrfm)
        {
            ExitRotationMode();
        }
    }

    bool preferredHand;
    const bool RIGHT = false, LEFT = true;
    public void RightGrab()
    {
        if (rightHandState == HandState.neutral)
        {
            CollapseLayers();
        }

        rightHandState = HandState.grab;
    }

    public void LeftGrab()
    {
        if (leftHandState == HandState.neutral)
        {
            CollapseLayers();
        }

        if (doSeparateText)
        {
            LHandDisplay.FadeText("2) Open your hand");
            mapDisplay.FadeText("");
            doSeparateText = false;
            preferredHand = LEFT;
        }

        leftHandState = HandState.grab;
    }

    public void RightGrabRelease()
    {
        if (rightHandState == HandState.grab)
        {
            SeparateLayers();
        }

        if (doSeparateText)
        {
            RHandDisplay.FadeText("2) Open your hand");
            mapDisplay.FadeText("");
            doSeparateText = false;
            preferredHand = RIGHT;
        }

        rightHandState = HandState.neutral;
    }

    public void LeftGrabRelease()
    {
        if (leftHandState == HandState.grab)
        {
            SeparateLayers();
        }

        leftHandState = HandState.neutral;
    }

    bool usingPoint = true;
    public void RightPoint()
    {
        if (!usingPoint) { return; }

        Debug.Log("pointing");
        rightHandState = HandState.point;
    }

    public void LeftPoint()
    {
        if (!usingPoint) { return; }

        Debug.Log("pointing");
        leftHandState = HandState.point;
    }

    #endregion

    #region handle zoom

    [SerializeField] float initDistance;
    Vector3 mapInitScale;
    void EnterZoomMode()
    {
        //if (mode != DEFAULT) { return; }

        mode = ZOOM;
        mapInitScale = mapTrfm.localScale;
        initDistance = Vector3.Distance(leftHandTrfm.position, rightHandTrfm.position);
    }

    void ExitZoomMode()
    {
        if (mode == ZOOM)
        {
            mode = DEFAULT;
        }
    }

    #endregion

    #region handle rotation


    Transform trackedHand;
    void EnterRotationMode()
    {
        if (mode != DEFAULT) { return; }

        mode = ROTATE;

        if (leftHandState == HandState.pinch) { trackedHand = leftHandTrfm; }
        if (rightHandState == HandState.pinch) { trackedHand = rightHandTrfm; }

        mapTrfm.parent = null;
        orienterTrfm.forward = trackedHand.position - orienterTrfm.position;
        mapTrfm.parent = orienterTrfm;
    }

    void ExitRotationMode()
    {
        if (mode == ROTATE)
        {
            mapTrfm.parent = null;
            mode = DEFAULT;
        }
    }

    #endregion

    #region handle separation

    [SerializeField] Transform[] layers;
    [SerializeField] LayerInteraction[] layerScripts;

    [SerializeField] Transform[] anchors;
    [SerializeField] Transform[] separate;
    [SerializeField] Transform[] disable;

    Quaternion saveMapRotation, referenceRotation;
    Vector3 saveMapPosition, referencePosition;

    int layerMovementMode;
    const int TO_ANCHORS = 1, TO_SEPARATE = 2;

    int layerGestureCooldown;

    void SeparateLayers()
    {
        if (layerMovementMode != TO_ANCHORS || layerGestureCooldown > 0) { return; }
        layerGestureCooldown = 50;

        layerMovementMode = TO_SEPARATE;

        saveMapPosition = mapTrfm.position;
        saveMapRotation = mapTrfm.rotation;
        inSeparationMode = true;

        referencePosition = separationReferenceTrfm.position;
        referenceRotation = separationReferenceTrfm.rotation;

        //mapTrfm.localScale = new Vector3(1.6f, 1.6f, 1.6f);
    }

    void CollapseLayers()
    {
        if (layerMovementMode != TO_SEPARATE || layerGestureCooldown > 0) { return; }
        layerGestureCooldown = 100;

        layerMovementMode = TO_ANCHORS;

        //mapTrfm.position = saveMapPosition;
        //mapTrfm.rotation = saveMapRotation;
        inSeparationMode = false;

        LHandDisplay.FadeText("");
        RHandDisplay.FadeText("");
        layersMapDisplay.FadeText("");
        timeInSeparate = 0;
    }

    #endregion

    float flatScale = 0;
    int timeInRotate, timeInZoom, timeInSeparate, timeInEnabling;
    public static bool firstSeparate, doSeparateText, firstLayerDisable, firstLayerHover, firstCollapse;
    private void FixedUpdate()
    {
        if (mode == ZOOM)
        {
            mapTrfm.localScale = mapInitScale * ((Vector3.Distance(leftHandTrfm.position, rightHandTrfm.position)+flatScale)/(initDistance+flatScale));

            timeInZoom++;
            if (timeInZoom == 200)
            {
                mapDisplay.FadeText("To enter Layer View:\n1) create a fist");
                doSeparateText = true;
                timeInZoom++;
            }
        }

        if (mode == ROTATE)
        {
            orienterTrfm.forward = trackedHand.position - orienterTrfm.position;

            timeInRotate++;
            if (timeInRotate == 200)
            {
                mapDisplay.FadeText("Pinch with two hands to zoom");
                timeInRotate++;
            }
        }

        if (layerMovementMode == TO_ANCHORS)
        {
            for (int i = 0; i < layers.Length; i++)
            {
                layers[i].position += (anchors[i].position - layers[i].position) * .1f;
            }
        }
        else if (layerMovementMode == TO_SEPARATE)
        {
            for (int i = 0; i < layers.Length; i++)
            {
                if (layerScripts[i].isEnabled) { layers[i].position += (separate[i].position - layers[i].position) * .1f; }
                else
                {
                    layers[i].position += (disable[i].position - layers[i].position) * .1f;

                    if (!firstLayerDisable)
                    {
                        LHandDisplay.FadeText("");
                        RHandDisplay.FadeText("");
                        firstLayerDisable = true;
                        timeInEnabling = 1;
                    }
                }
            }

            if (!firstSeparate)
            {
                firstSeparate = true;

                layersMapDisplay.FadeText("Hover over layers for info");
                RHandDisplay.FadeText("");
                LHandDisplay.FadeText("");
                timeInSeparate = 1;
            }

            if (timeInSeparate > 0)
            {
                timeInSeparate++;

                if (timeInSeparate == 300)
                {
                    layersMapDisplay.FadeText("Pinch layers to enable/disable them");
                }
            }

            if (timeInEnabling > 0)
            {
                timeInEnabling++;

                if (timeInEnabling == 300)
                {
                    layersMapDisplay.FadeText("Make a fist to return to Map View");
                }
            }

            //mapTrfm.rotation = Quaternion.RotateTowards(mapTrfm.rotation, referenceRotation, 2);
            //mapTrfm.position += (referencePosition - mapTrfm.position) * .1f;

            //mapTrfm.rotation = Quaternion.RotateTowards(mapTrfm.rotation, separationReferenceTrfm.rotation, 2);
            //mapTrfm.position += (separationReferenceTrfm.position - mapTrfm.position) * .1f;

            //mapTrfm.localScale = new Vector3(1.6f, 1.6f, 1.6f);
        }

        if (layerGestureCooldown > 0)
        {
            layerGestureCooldown--;
        }



        _rightHandState = rightHandState;
        _leftHandState = leftHandState; 
    }
}
