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
    /// 一个管理Unity场景中ARcore会话的组件
    /// </summary>
    [HelpURL("https://developers.google.com/ar/reference/unity/class/GoogleARCore/ARCoreSession")]
    public class ARCoreSession : MonoBehaviour
    {
        /// <summary>
        /// A scriptable object specifying the ARCore session configuration.
        ///  一个可编辑的对象来指定tango设备连接设置
        /// </summary>
        [Tooltip("A scriptable object specifying the ARCore session configuration.")]
        public ARCoreSessionConfig SessionConfig;

        private OnChooseCameraConfigurationDelegate m_OnChooseCameraConfiguration;

        /// <summary>
        /// Selects a camera configuration for the ARCore session being resumed.
        /// 选取正在恢复ARcore会话的相机配置
        /// </summary>
        /// <param name="supportedConfigurations">
        /// A list of supported camera configurations. Currently it contains 3 camera configs.
        /// 支持的相机配置列表。目前它包含3个相机配置。
        /// The GPU texture resolutions are the same in all three configs.
        /// 在所有的三个配置中，GPU纹理分辨率是一样的
        /// Currently, most devices provide GPU texture resolution of 1920 x 1080,
        /// 目前，大多数设备提供的GPU纹理分辨率为1920×1080，
        /// but devices might provide higher or lower resolution textures, 
        /// 但是，设备可以提供更高或更低的分辨率纹理，
        /// depending on device capabilities. The CPU image resolutions returned are VGA, 720p,
        /// 取决于设备的能力。返回的CPU图像分辨率为VGA，720P，
        /// and a resolution matching the GPU texture.
        /// 且分辨率与GPU纹理匹配 </param>
        /// <returns>The index of the camera configuration in <c>supportedConfigurations</c> to be used for the ARCore session.
        /// supportedConfigurations列表里的相机配置索引被用于ARCore会话中
        ///  If the return value is not a valid index (e.g. the value -1), then no camera
        ///  如果返回值不是有效的索引，表示没有相机配置被设置且ARcore会话将使用先前选定的相机配置
        /// configuration will be set and the ARCore session will use the previously selected camera configuration
        ///  or a default configuration if no previous selection exists.
        /// 或者当先前的选定不存在就使用默认的配置
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
        /// 注册允许从一个有效配置列表中选择相机配置的回调。
        /// The callback will be invoked each time the ARCore session is resumed 
        /// 每次恢复B会话时都会调用回调，
        /// which can happen when the ARCoreSession
        /// component becomes enabled or the Android application moves from 'paused' to 'resumed' state.
        /// 这可能发生在ARCoreSession组件被启用或者Android应用程序从“暂停”状态移动到“恢复”状态时。
        /// </summary>
        /// <param name="onChooseCameraConfiguration">The callback to register for selecting a camera configuration.回调以选择相机配置。</param>
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
