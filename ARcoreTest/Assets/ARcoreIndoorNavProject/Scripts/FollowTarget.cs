using UnityEngine;
using System.Collections;
// Attach this script to the camera that you want to follow the target
public class FollowTarget : MonoBehaviour
{
    public Transform targetToFollow;
    [HideInInspector]
    public Quaternion targetRot;                      // The rotation of the device camera from Frame.Pose.rotation     来自Frame.Pose.rotation的设备相机旋转值
    public float distanceToTargetXZ = 10.0f;    // The distance in the XZ plane to the target    相机相对XZ平面的距离比例
    public float heightOverTarget = 5.0f;         
    public float heightSmoothingSpeed = 2.0f;
    public float rotationSmoothingSpeed = 2.0f;
    // Use lateUpdate to assure that the camera is updated after the target has been updated.
    // 使用lateUpdat确保在目标更新后更新摄像头
    //先Update可以是targetRot赋上值
    void LateUpdate()
    {
        if (!targetToFollow)
            return;
        Vector3 targetEulerAngles = targetRot.eulerAngles;
        // Calculate the current rotation angle around the Y axis we want to apply to the camera.
        //计算我们要应用于相机的Y轴周围的当前旋转角
        // We add 180 degrees as the device camera points to the negative Z direction
        //我们添加180度因为设备相机指向负Z方向
        float rotationToApplyAroundY = targetEulerAngles.y + 180.0f;
        float heightToApply = targetToFollow.position.y + heightOverTarget;
        // Smooth interpolation between current camera rotation angle and the rotation angle we want to apply.
        // 当前相机旋转角度和我们想要应用的旋转角度之间的平滑插值
        // Use LerpAngle to handle correctly when angles > 360
        //当角度>360时，使用LerpAngle正确处理
        float newCamRotAngleY = Mathf.LerpAngle(transform.eulerAngles.y, rotationToApplyAroundY, rotationSmoothingSpeed * Time.deltaTime);
        float newCamHeight = Mathf.Lerp(transform.position.y, heightToApply, heightSmoothingSpeed * Time.deltaTime);
        Quaternion newCamRotYQuat = Quaternion.Euler(0, newCamRotAngleY, 0);
        // Set camera position the same as the target position
        // 将摄像机位置设置与目标位置相同
        transform.position = targetToFollow.position;
        // Move the camera back in the direction defined by newCamRotYQuat and the amount defined by distanceToTargetXZ
        // 按照newCamRotYQuat定义的方向向后移动摄像机，并由distancToTargetXZ定义数量
        transform.position -= newCamRotYQuat * Vector3.forward * distanceToTargetXZ;
        // Finally set the camera height
        //最后设置相机高度
        transform.position = new Vector3(transform.position.x, newCamHeight, transform.position.z);
        // Keep the camera looking to the target to apply rotation around X axis
        //让相机注视目标以围绕X轴应用旋转
        transform.LookAt(targetToFollow);
    }
}