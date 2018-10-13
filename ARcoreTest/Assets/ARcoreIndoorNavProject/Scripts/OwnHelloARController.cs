namespace GoogleARCore.Examples.HelloAR
{
    using System.Collections.Generic;
    using GoogleARCore;
    using GoogleARCore.Examples.Common;
    using UnityEngine;
    using UnityEngine.UI;
#if UNITY_EDITOR
    using Input = InstantPreviewInput;
#endif
    public class OwnHelloARController: MonoBehaviour
    {
        public GameObject FirstPersonCamera;
        public GameObject cameraTarget;
        public Text camPoseText;
        private Vector3 m_prevARPosePosition;
        private bool trackingStarted = false;
        private Trackable trackable;
        public void Start()
        {
            m_prevARPosePosition = Vector3.zero;
        }
        private bool m_IsQuitting = false;  
        public void Update()
        {
            _UpdateApplicationLifecycle();
            // Only allow the screen to sleep when not tracking.
            if (Session.Status != SessionStatus.Tracking)
            {
                
                trackingStarted = false;
                camPoseText.text = "Lost tracking, wait ....";
                const int lostTrackingSleepTimeout = 15;
                Screen.sleepTimeout = lostTrackingSleepTimeout;
                return;
            }
            else
            {
                camPoseText.text = "";
                Screen.sleepTimeout = SleepTimeout.NeverSleep;
            }
            Vector3 currentARPosition = Frame.Pose.position;
            if (!trackingStarted)
            {
                trackingStarted = true;
                m_prevARPosePosition = Frame.Pose.position;
            }
            Vector3 deltaPosition =   m_prevARPosePosition- currentARPosition;
            m_prevARPosePosition = currentARPosition;
            if (cameraTarget != null)
            {
                cameraTarget.transform.Translate(deltaPosition.x, 0.0f, deltaPosition.z);
                //TODO: FollowTarget是稍后要挂在相机的自定义类
                FirstPersonCamera.GetComponent<FollowTarget>().targetRot = Frame.Pose.rotation;
            }   
        }

    
        private void _UpdateApplicationLifecycle()
        {
            // Exit the app when the 'back' button is pressed.
            if (Input.GetKey(KeyCode.Escape))
            {
                Application.Quit();
            }

            if (m_IsQuitting)
            {
                return;
            }

            // Quit if ARCore was unable to connect and give Unity some time for the toast to appear.
            if (Session.Status == SessionStatus.ErrorPermissionNotGranted)
            {
              
                m_IsQuitting = true;
                Invoke("_DoQuit", 0.5f);
            }
            else if (Session.Status.IsError())
            {
              
                m_IsQuitting = true;
                Invoke("_DoQuit", 0.5f);
            }
        }

        /// <summary>
        /// Actually quit the application.
        /// </summary>
        private void _DoQuit()
        {
            Application.Quit();
        }
    }
}
