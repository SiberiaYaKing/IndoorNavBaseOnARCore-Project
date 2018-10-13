//-----------------------------------------------------------------------
// <copyright file="ARCoreSession.cs" company="Google">
//
// Copyright 2017 Google Inc. All Rights Reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
// </copyright>
//-----------------------------------------------------------------------

namespace GoogleARCore
{
    using System.Collections.Generic;
    using GoogleARCoreInternal;
    using UnityEngine;

    /// <summary>
    /// A component that manages the ARCore Session in a Unity scene.
    /// һ������Unity������ARcore�Ự�����
    /// </summary>
    [HelpURL("https://developers.google.com/ar/reference/unity/class/GoogleARCore/ARCoreSession")]
    public class ARCoreSession : MonoBehaviour
    {
        /// <summary>
        /// A scriptable object specifying the ARCore session configuration.
        ///  һ���ɱ༭�Ķ�����ָ��tango�豸��������
        /// </summary>
        [Tooltip("A scriptable object specifying the ARCore session configuration.")]
        public ARCoreSessionConfig SessionConfig;

        private OnChooseCameraConfigurationDelegate m_OnChooseCameraConfiguration;

        /// <summary>
        /// Selects a camera configuration for the ARCore session being resumed.
        /// ѡȡ���ڻָ�ARcore�Ự���������
        /// </summary>
        /// <param name="supportedConfigurations">
        /// A list of supported camera configurations. Currently it contains 3 camera configs.
        /// ֧�ֵ���������б�Ŀǰ������3��������á�
        /// The GPU texture resolutions are the same in all three configs.
        /// �����е����������У�GPU����ֱ�����һ����
        /// Currently, most devices provide GPU texture resolution of 1920 x 1080,
        /// Ŀǰ��������豸�ṩ��GPU����ֱ���Ϊ1920��1080��
        /// but devices might provide higher or lower resolution textures, 
        /// ���ǣ��豸�����ṩ���߻���͵ķֱ�������
        /// depending on device capabilities. The CPU image resolutions returned are VGA, 720p,
        /// ȡ�����豸�����������ص�CPUͼ��ֱ���ΪVGA��720P��
        /// and a resolution matching the GPU texture.
        /// �ҷֱ�����GPU����ƥ�� </param>
        /// <returns>The index of the camera configuration in <c>supportedConfigurations</c> to be used for the ARCore session.
        /// supportedConfigurations�б���������������������ARCore�Ự��
        ///  If the return value is not a valid index (e.g. the value -1), then no camera
        ///  �������ֵ������Ч����������ʾû��������ñ�������ARcore�Ự��ʹ����ǰѡ�����������
        /// configuration will be set and the ARCore session will use the previously selected camera configuration
        ///  or a default configuration if no previous selection exists.
        /// ���ߵ���ǰ��ѡ�������ھ�ʹ��Ĭ�ϵ�����
        /// </returns>
        public delegate int OnChooseCameraConfigurationDelegate(List<CameraConfig> supportedConfigurations);

        /// <summary>
        /// Unity Awake.
        /// </summary>
        [SuppressMemoryAllocationError(Reason = "Could create new LifecycleManager")]
        public void Awake()
        {
            LifecycleManager.Instance.CreateSession(this);
        }

        /// <summary>
        /// Unity OnDestroy.
        /// </summary>
        [SuppressMemoryAllocationError(IsWarning = true, Reason = "Requires further investigation.")]
        public void OnDestroy()
        {
            LifecycleManager.Instance.ResetSession();
        }

        /// <summary>
        /// Unity OnEnable.
        /// </summary>
        [SuppressMemoryAllocationError(Reason = "Enabling session creates a new ARSessionConfiguration")]
        public void OnEnable()
        {
            LifecycleManager.Instance.EnableSession();
        }

        /// <summary>
        /// Unity OnDisable.
        /// </summary>
        [SuppressMemoryAllocationError(IsWarning = true, Reason = "Requires further investigation.")]
        public void OnDisable()
        {
            LifecycleManager.Instance.DisableSession();
        }

        /// <summary>
        /// Registers a callback that allows a camera configuration to be selected from a list of valid configurations.
        /// ע�������һ����Ч�����б���ѡ��������õĻص���
        /// The callback will be invoked each time the ARCore session is resumed 
        /// ÿ�λָ�B�Ựʱ������ûص���
        /// which can happen when the ARCoreSession
        /// component becomes enabled or the Android application moves from 'paused' to 'resumed' state.
        /// ����ܷ�����ARCoreSession��������û���AndroidӦ�ó���ӡ���ͣ��״̬�ƶ������ָ���״̬ʱ��
        /// </summary>
        /// <param name="onChooseCameraConfiguration">The callback to register for selecting a camera configuration.�ص���ѡ��������á�</param>
        public void RegisterChooseCameraConfigurationCallback(OnChooseCameraConfigurationDelegate onChooseCameraConfiguration)
        {
            m_OnChooseCameraConfiguration = onChooseCameraConfiguration;
        }

        internal OnChooseCameraConfigurationDelegate GetChooseCameraConfigurationCallback()
        {
            return m_OnChooseCameraConfiguration;
        }
    }
}
