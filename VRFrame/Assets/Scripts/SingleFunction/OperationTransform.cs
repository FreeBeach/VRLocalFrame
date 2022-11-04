using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace VRFrame
{
    /// <summary>
    /// 挂载在单个部件上，或者单元上
    /// 涉及到 位置，角度，变色
    /// </summary>
    public class PartOperation : MonoBehaviour
    {
        /// <summary>
        /// 唯一标记
        /// </summary>
        public int singleSign { protected set; get; }
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="sign"></param>
        /// <param name="pos"></param>
        /// <param name="euler"></param>
        public void Init(int sign,Vector3 pos,Vector3 euler)
        {

        }
        /// <summary>
        /// 显示在场景中
        /// </summary>
        public void Show()
        {

        }
        /// <summary>
        /// 场景中隐藏
        /// </summary>
        public void Hide()
        {

        }
        /// <summary>
        /// 销毁调用
        /// </summary>
        public void OnDestroy()
        {
            
        }
    }
}


