using System;

using Unity.XR.CoreUtils;

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.Serialization;

namespace Sula {
    /// <summary>
    /// The <see cref="FilteredTrackedPoseDriver"/> component applies the current pose value of a tracked device
    /// to the <see cref="Transform"/> of the <see cref="GameObject"/>.
    /// <see cref="FilteredTrackedPoseDriver"/> can track multiple types of devices including XR HMDs, controllers, and remotes.
    /// </summary>
    /// <remarks>
    /// For <see cref="positionInput"/> and <see cref="rotationInput"/>, if an action is directly defined
    /// in the <see cref="InputActionProperty"/>, as opposed to a reference to an action externally defined
    /// in an <see cref="InputActionAsset"/>, the action will automatically be enabled and disabled by this
    /// behavior during <see cref="OnEnable"/> and <see cref="OnDisable"/>. The enabled state for actions
    /// externally defined must be managed externally from this behavior.
    /// </remarks>
    [Serializable]
    [AddComponentMenu("XR/Filtered Tracked Pose Driver (Input System)")]
    public class FilteredTrackedPoseDriver : MonoBehaviour, ISerializationCallbackReceiver {

        #region Axis    Filtering
        [System.Flags]
        public enum Axis {
            NONE = 0,
            X = 1,
            Y = 2,
            Z = 4,

        }

        [SerializeField]
        private Axis _rotationAxisSelected;

        public Axis rotationAxis {
            get => _rotationAxisSelected;
            set => _rotationAxisSelected = value;
        }
        [SerializeField]
        private Axis _positionAxisSelected;

        public Axis positionAxis {
            get => _positionAxisSelected;
            set => _positionAxisSelected = value;
        }

        [SerializeField, FormerlySerializedAs("m_XRRig")]
        [Tooltip("The XR Origin object to provide access control to.")]
        private XROrigin m_XROrigin;
        /// <summary>
        /// The XR Origin object to provide access control to.
        /// </summary>
        public XROrigin xrOrigin {
            get => m_XROrigin;
            set => m_XROrigin = value;
        }

        #endregion

        /// <summary>
        /// Options for which <see cref="Transform"/> properties to update.
        /// </summary>
        /// <seealso cref="trackingType"/>
        public enum TrackingType {
            /// <summary>
            /// Update both rotation and position.
            /// </summary>
            RotationAndPosition,

            /// <summary>
            /// Update rotation only.
            /// </summary>
            RotationOnly,

            /// <summary>
            /// Update position only.
            /// </summary>
            PositionOnly,
            /// <summary>
            /// Doesn't update
            /// </summary>
            Nothing,
        }

        [SerializeField]
        private TrackingType m_TrackingType;
        /// <summary>
        /// The tracking type being used by the Tracked Pose Driver
        /// to control which <see cref="Transform"/> properties to update.
        /// </summary>
        /// <seealso cref="TrackingType"/>
        public TrackingType trackingType {
            get => m_TrackingType;
            set => m_TrackingType = value;
        }

        /// <summary>
        /// Options for which phases of the player loop will update <see cref="Transform"/> properties.
        /// </summary>
        /// <seealso cref="updateType"/>
        public enum UpdateType {
            /// <summary>
            /// Update after the Input System has completed an update and right before rendering.
            /// </summary>
            /// <seealso cref="InputUpdateType.Dynamic"/>
            /// <seealso cref="InputUpdateType.BeforeRender"/>
            UpdateAndBeforeRender,

            /// <summary>
            /// Update after the Input System has completed an update.
            /// </summary>
            /// <seealso cref="InputUpdateType.Dynamic"/>
            Update,

            /// <summary>
            /// Update right before rendering.
            /// </summary>
            /// <seealso cref="InputUpdateType.BeforeRender"/>
            BeforeRender,
        }

        [SerializeField]
        private UpdateType m_UpdateType = UpdateType.UpdateAndBeforeRender;
        /// <summary>
        /// The update type being used by the Tracked Pose Driver
        /// to control which phases of the player loop will update <see cref="Transform"/> properties.
        /// </summary>
        /// <seealso cref="UpdateType"/>
        public UpdateType updateType {
            get => m_UpdateType;
            set => m_UpdateType = value;
        }

        [SerializeField]
        private InputActionProperty m_PositionInput;
        /// <summary>
        /// The action to read the position value of a tracked device.
        /// Must support reading a value of type <see cref="Vector3"/>.
        /// </summary>
        public InputActionProperty positionInput {
            get => m_PositionInput;
            set {
                if (Application.isPlaying)
                    UnbindPosition();

                m_PositionInput = value;

                if (Application.isPlaying && isActiveAndEnabled)
                    BindPosition();
            }
        }

        [SerializeField]
        private InputActionProperty m_RotationInput;
        /// <summary>
        /// The action to read the rotation value of a tracked device.
        /// Must support reading a value of type <see cref="Quaternion"/>.
        /// </summary>
        public InputActionProperty rotationInput {
            get => m_RotationInput;
            set {
                if (Application.isPlaying)
                    UnbindRotation();

                m_RotationInput = value;

                if (Application.isPlaying && isActiveAndEnabled)
                    BindRotation();
            }
        }

        private Vector3 m_CurrentPosition = Vector3.zero;
        private Quaternion m_CurrentRotation = Quaternion.identity;
        private bool m_RotationBound;
        private bool m_PositionBound;

        private void BindActions() {
            BindPosition();
            BindRotation();
        }

        private void BindPosition() {
            if (m_PositionBound)
                return;

            InputAction action = m_PositionInput.action;
            if (action == null)
                return;

            action.performed += OnPositionPerformed;
            action.canceled += OnPositionCanceled;
            m_PositionBound = true;

            if (m_PositionInput.reference == null) {
                action.Rename($"{gameObject.name} - TPD - Position");
                action.Enable();
            }
        }

        private void BindRotation() {
            if (m_RotationBound)
                return;

            InputAction action = m_RotationInput.action;
            if (action == null)
                return;

            action.performed += OnRotationPerformed;
            action.canceled += OnRotationCanceled;
            m_RotationBound = true;

            if (m_RotationInput.reference == null) {
                action.Rename($"{gameObject.name} - TPD - Rotation");
                action.Enable();
            }
        }

        private void UnbindActions() {
            UnbindPosition();
            UnbindRotation();
        }

        private void UnbindPosition() {
            if (!m_PositionBound)
                return;

            InputAction action = m_PositionInput.action;
            if (action == null)
                return;

            if (m_PositionInput.reference == null)
                action.Disable();

            action.performed -= OnPositionPerformed;
            action.canceled -= OnPositionCanceled;
            m_PositionBound = false;
        }

        private void UnbindRotation() {
            if (!m_RotationBound)
                return;

            InputAction action = m_RotationInput.action;
            if (action == null)
                return;

            if (m_RotationInput.reference == null)
                action.Disable();

            action.performed -= OnRotationPerformed;
            action.canceled -= OnRotationCanceled;
            m_RotationBound = false;
        }

        private void OnPositionPerformed(InputAction.CallbackContext context) {
            Debug.Assert(m_PositionBound, this);
            m_CurrentPosition = context.ReadValue<Vector3>();
        }

        private void OnPositionCanceled(InputAction.CallbackContext context) {
            Debug.Assert(m_PositionBound, this);
            m_CurrentPosition = Vector3.zero;
        }

        private void OnRotationPerformed(InputAction.CallbackContext context) {
            Debug.Assert(m_RotationBound, this);
            m_CurrentRotation = context.ReadValue<Quaternion>();
        }

        private void OnRotationCanceled(InputAction.CallbackContext context) {
            Debug.Assert(m_RotationBound, this);
            m_CurrentRotation = Quaternion.identity;
        }

        /// <summary>
        /// This function is called when the script instance is being loaded.
        /// </summary>
        protected virtual void Awake() {
#if UNITY_INPUT_SYSTEM_ENABLE_VR && ENABLE_VR
            if (HasStereoCamera())
            {
                UnityEngine.XR.XRDevice.DisableAutoXRCameraTracking(GetComponent<Camera>(), true);
            }
#endif
        }

        /// <summary>
        /// This function is called when the object becomes enabled and active.
        /// </summary>
        protected void OnEnable() {
            InputSystem.onAfterUpdate += UpdateCallback;
            BindActions();
        }

        /// <summary>
        /// This function is called when the object becomes disabled or inactive.
        /// </summary>
        protected void OnDisable() {
            UnbindActions();
            InputSystem.onAfterUpdate -= UpdateCallback;
        }

        /// <summary>
        /// This function is called when the <see cref="MonoBehaviour"/> will be destroyed.
        /// </summary>
        protected virtual void OnDestroy() {
#if UNITY_INPUT_SYSTEM_ENABLE_VR && ENABLE_VR
            if (HasStereoCamera())
            {
                UnityEngine.XR.XRDevice.DisableAutoXRCameraTracking(GetComponent<Camera>(), false);
            }
#endif
        }

        protected void UpdateCallback() {
            if (InputState.currentUpdateType == InputUpdateType.BeforeRender)
                OnBeforeRender();
            else
                OnUpdate();
        }

        protected virtual void OnUpdate() {
            if (m_UpdateType == UpdateType.Update ||
                m_UpdateType == UpdateType.UpdateAndBeforeRender) {
                PerformUpdate();
            }
        }

        protected virtual void OnBeforeRender() {
            if (m_UpdateType == UpdateType.BeforeRender ||
                m_UpdateType == UpdateType.UpdateAndBeforeRender) {
                PerformUpdate();
            }
        }

        protected virtual void SetLocalTransform(Vector3 newPosition, Quaternion newRotation) {
            if (m_TrackingType == TrackingType.Nothing) {
                return;
            }
            if (m_TrackingType == TrackingType.RotationAndPosition ||
                m_TrackingType == TrackingType.RotationOnly) {
                SetLocalRotation(m_CurrentRotation, _rotationAxisSelected);
            }

            if (m_TrackingType == TrackingType.RotationAndPosition ||
                m_TrackingType == TrackingType.PositionOnly) {
                SetLocalPosition(m_CurrentPosition, _positionAxisSelected);
            }
        }

        protected virtual void SetLocalPosition(Vector3 newPosition, Axis positionAxis) {

            {
                Vector3 localPosition = transform.localPosition;
                if ((positionAxis == Axis.NONE)) {
                    //Update the tracking type
                    if (m_TrackingType == TrackingType.RotationAndPosition) {
                        m_TrackingType = TrackingType.RotationOnly;

                    }
                    else {
                        m_TrackingType = TrackingType.Nothing;
                    }
                    return;
                }
                if (positionAxis.HasFlag(Axis.X)) {
                    localPosition.x = newPosition.x;
                }
                if (positionAxis.HasFlag(Axis.Y)) {
                    localPosition.y = newPosition.y;
                }
                if (positionAxis.HasFlag(Axis.Z)) {
                    localPosition.z = newPosition.z;
                }

                transform.localPosition = localPosition;
            }
        }
        protected virtual void SetLocalRotation(Quaternion newRotation, Axis rotationAxis) {

            {
                Vector3 localRotation = transform.rotation.eulerAngles;

                if ((rotationAxis == Axis.NONE)) {
                    //Update the tracking type
                    if (m_TrackingType == TrackingType.RotationAndPosition) {
                        m_TrackingType = TrackingType.PositionOnly;

                    }
                    else {
                        m_TrackingType = TrackingType.Nothing;
                    }
                    return;
                }
                if (rotationAxis.HasFlag(Axis.X)) {
                    //I'm adding the rotation of the XR origin to the rotation of the object, with this I ensure the correct rotation if the player rotates
                    localRotation.x = newRotation.eulerAngles.x + xrOrigin.transform.rotation.eulerAngles.x;
                }
                if (rotationAxis.HasFlag(Axis.Y)) {
                    localRotation.y = newRotation.eulerAngles.y + xrOrigin.transform.rotation.eulerAngles.y;
                }
                if (rotationAxis.HasFlag(Axis.Z)) {
                    localRotation.z = newRotation.eulerAngles.z + xrOrigin.transform.rotation.eulerAngles.z;
                }

                this.transform.eulerAngles = localRotation;
            }
        }

        //protected virtual void SetLocalRotation(Quaternion newRotation, Axis rotationAxis) {
        //    Vector3 newVector;

        //    switch (rotationAxis) {
        //        case Axis.X:
        //            newVector = new Vector3(newRotation.eulerAngles.x + xrOrigin.transform.rotation.eulerAngles.x, 0, 0);
        //            if (this.transform.rotation.eulerAngles != newVector) {
        //                this.transform.eulerAngles = newVector;
        //            }
        //            break;
        //        case Axis.Y:
        //            newVector = new Vector3(0, newRotation.eulerAngles.y + xrOrigin.transform.rotation.eulerAngles.y, 0);
        //            if (this.transform.rotation.eulerAngles != newVector) {
        //                this.transform.eulerAngles = newVector;
        //            }
        //            break;
        //        case Axis.Z:
        //            newVector = new Vector3(0, 0, newRotation.eulerAngles.z + xrOrigin.transform.rotation.eulerAngles.z);
        //            if (this.transform.rotation.eulerAngles != newVector) {
        //                this.transform.eulerAngles = newVector;
        //            }
        //            break;
        //    }

        //}

        private bool HasStereoCamera() {
            Camera cameraComponent = GetComponent<Camera>();
            return cameraComponent != null && cameraComponent.stereoEnabled;
        }

        protected virtual void PerformUpdate() {
            SetLocalTransform(m_CurrentPosition, m_CurrentRotation);
        }

        #region DEPRECATED

        // Disable warnings that these fields are never assigned to. They are set during Unity deserialization and migrated.
        // ReSharper disable UnassignedField.Local
#pragma warning disable 0649
        [Obsolete]
        [SerializeField, HideInInspector]
        private InputAction m_PositionAction;
        public InputAction positionAction {
            get => m_PositionInput.action;
            set => positionInput = new InputActionProperty(value);
        }

        [Obsolete]
        [SerializeField, HideInInspector]
        private InputAction m_RotationAction;
        public InputAction rotationAction {
            get => m_RotationInput.action;
            set => rotationInput = new InputActionProperty(value);
        }
#pragma warning restore 0649
        // ReSharper restore UnassignedField.Local

        /// <summary>
        /// Stores whether the fields of type <see cref="InputAction"/> have been migrated to fields of type <see cref="InputActionProperty"/>.
        /// </summary>
        [SerializeField, HideInInspector]
        private bool m_HasMigratedActions;

        /// <summary>
        /// This function is called when the user hits the Reset button in the Inspector's context menu
        /// or when adding the component the first time. This function is only called in editor mode.
        /// </summary>
        protected void Reset() {
            m_HasMigratedActions = true;
        }

        /// <inheritdoc />
        void ISerializationCallbackReceiver.OnBeforeSerialize() {
        }

        /// <inheritdoc />
        void ISerializationCallbackReceiver.OnAfterDeserialize() {
            if (m_HasMigratedActions)
                return;

#pragma warning disable 0612
            m_PositionInput = new InputActionProperty(m_PositionAction);
            m_RotationInput = new InputActionProperty(m_RotationAction);
            m_HasMigratedActions = true;
#pragma warning restore 0612
        }

        #endregion
    }
}
