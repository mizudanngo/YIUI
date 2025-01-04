using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace YIUIFramework
{
    /// <summary>
    /// 面板拆分数据
    /// 主要做分块加载
    /// </summary>
    [HideReferenceObjectPicker]
    [HideLabel]
    public sealed partial class UIPanelSplitData
    {
        [HideInInspector]
        public GameObject Panel;

        [BoxGroup("通用界面", centerLabel: true)]
        [LabelText("所有子界面父对象")]
        [ReadOnly]
        public RectTransform AllViewParent;

        [BoxGroup("通用界面", centerLabel: true)]
        [LabelText("所有通用界面(默认存在的)")]
        public List<RectTransform> AllCommonView = new List<RectTransform>();

        [BoxGroup("通用界面", centerLabel: true)]
        [LabelText("所有需要[动态创建]的界面")]
        public List<RectTransform> AllCreateView = new List<RectTransform>();

        [BoxGroup("弹窗界面", centerLabel: true)]
        [LabelText("所有弹出界面父级")]
        [ReadOnly]
        public RectTransform AllPopupViewParent;

        [BoxGroup("弹窗界面", centerLabel: true)]
        [LabelText("所有弹出界面[动态创建]")]
        public List<RectTransform> AllPopupView = new List<RectTransform>();
    }
}