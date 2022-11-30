using System;

using Unity.XR.CoreUtils;

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.Serialization;

namespace Sula {

    [AddComponentMenu("XR/Tracked Pose Driver (Input System)")]
    [Serializable]
    public class TrackedRotationDriver : MonoBehaviour, ISerializationCallbackReceiver {
        /// <summary>
        /// Options for which <see cref="Transform"/> properties to update.
        /// </summary>
        /// <seealso cref="axis"/>
        public enum Axis {
            x, y, z
        }

        [SerializeField]
        private Axis _axisSelected;

        public Axis axis {
            get => _axisSelected;
            set => _axisSelected = value;
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

        private Quaternion m_CurrentRotation = Quaternion.identity;
        private bool m_RotationBound;

        private void BindActions() {
            BindRotation();
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
            UnbindRotation();
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
            if (m_XROrigin == null)
                m_XROrigin = FindObjectOfType<XROrigin>();

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

        protected virtual void SetLocalTransform(Quaternion newRotation) {
            transform.localRotation = newRotation;
        }

        protected virtual void SetLocalRotation(Quaternion newRotation, Axis rotationAxis) {
            Vector3 newVector;

            switch (rotationAxis) {
                case Axis.x:
                    newVector = new Vector3(newRotation.eulerAngles.x + xrOrigin.transform.rotation.eulerAngles.x, 0, 0);
                    if (this.transform.rotation.eulerAngles != newVector) {
                        this.transform.eulerAngles = newVector;
                    }
                    break;
                case Axis.y:
                    newVector = new Vector3(0, newRotation.eulerAngles.y + xrOrigin.transform.rotation.eulerAngles.y, 0);
                    if (this.transform.rotation.eulerAngles != newVector) {
                        this.transform.eulerAngles = newVector;
                    }
                    break;
                case Axis.z:
                    newVector = new Vector3(0, 0, newRotation.eulerAngles.z + xrOrigin.transform.rotation.eulerAngles.z);
                    if (this.transform.rotation.eulerAngles != newVector) {
                        this.transform.eulerAngles = newVector;
                    }
                    break;
            }
        }

        private bool HasStereoCamera() {
            Camera cameraComponent = GetComponent<Camera>();
            return cameraComponent != null && cameraComponent.stereoEnabled;
        }

        protected virtual void PerformUpdate() {
            SetLocalRotation(m_CurrentRotation, _axisSelected);
        }

        #region DEPRECATED

        // Disable warnings that these fields are never assigned to. They are set during Unity deserialization and migrated.
        // ReSharper disable UnassignedField.Local
#pragma warning disable 0649


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
            m_RotationInput = new InputActionProperty(m_RotationAction);
            m_HasMigratedActions = true;
#pragma warning restore 0612
        }

        #endregion
    }
}


