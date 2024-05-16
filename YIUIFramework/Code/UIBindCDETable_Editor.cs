﻿#if UNITY_EDITOR
using Sirenix.OdinInspector;
using YIUIBind;
using UnityEngine;
using UnityEditor;

using YIUIFramework.Editor;

namespace YIUIFramework
{
    //Editor
    public sealed partial class UIBindCDETable
    {
        #region 界面参数
        [LabelText("组件类型")]
        [OnValueChanged(nameof(OnValueChangedEUICodeType))]
        [ReadOnly]
        public EUICodeType UICodeType = EUICodeType.Component;

        [BoxGroup("配置", true, true)]
        [HideIf(nameof(UICodeType), EUICodeType.Component)]
        [LabelText("窗口选项")]
        [GUIColor(0, 1, 1)]
        [EnableIf(nameof(Enable))]
        public EWindowOption WindowOption = EWindowOption.None;

        [ShowIf(nameof(UICodeType), EUICodeType.Panel)]
        [BoxGroup("配置", true, true)]
        [OnValueChanged(nameof(OnValueChangedEPanelLayer))]
        [GUIColor(0, 1, 1)]
        [EnableIf(nameof(Enable))]
        public EPanelLayer PanelLayer = EPanelLayer.Panel;

        [ShowIf(nameof(UICodeType), EUICodeType.Panel)]
        [BoxGroup("配置", true, true)]
        [GUIColor(0, 1, 1)]
        [EnableIf(nameof(Enable))]
        public EPanelOption PanelOption = EPanelOption.None;

        [ShowIf(nameof(UICodeType), EUICodeType.Panel)]
        [BoxGroup("配置", true, true)]
        [GUIColor(0, 1, 1)]
        [EnableIf(nameof(Enable))]
        public EPanelStackOption PanelStackOption = EPanelStackOption.VisibleTween;

        [ShowIf(nameof(UICodeType), EUICodeType.View)]
        [BoxGroup("配置", true, true)]
        [GUIColor(0, 1, 1)]
        [EnableIf(nameof(Enable))]
        public EViewWindowType ViewWindowType = EViewWindowType.View;

        [ShowIf(nameof(UICodeType), EUICodeType.View)]
        [BoxGroup("配置", true, true)]
        [GUIColor(0, 1, 1)]
        [EnableIf(nameof(Enable))]
        public EViewStackOption ViewStackOption = EViewStackOption.VisibleTween;

        [ShowIf(nameof(ShowCachePanelTime), EUICodeType.Panel)]
        [BoxGroup("配置", true, true)]
        [GUIColor(0, 1, 1)]
        [LabelText("缓存时间")]
        [EnableIf(nameof(Enable))]
        public float CachePanelTime = 10;

        private bool ShowCachePanelTime => PanelOption.HasFlag(EPanelOption.TimeCache);

        [LabelText("同层级时 优先级高的在前面")] //相同时后开的在前
        [ShowIf(nameof(UICodeType), EUICodeType.Panel)]
        [BoxGroup("配置", true, true)]
        [GUIColor(0, 1, 1)]
        [EnableIf(nameof(Enable))]
        public int Priority = 0;

        private void OnValueChangedEUICodeType()
        {
            if (name.EndsWith(UIStaticHelper.UIPanelName) || name.EndsWith(UIStaticHelper.UIPanelSourceName))
            {
                if (UICodeType != EUICodeType.Panel)
                {
                    Debug.LogWarning($"{name} 结尾{UIStaticHelper.UIPanelName} 必须设定为{UIStaticHelper.UIPanelName}类型");
                }

                UICodeType = EUICodeType.Panel;
            }
            else if (name.EndsWith(UIStaticHelper.UIViewName))
            {
                if (UICodeType != EUICodeType.View)
                {
                    Debug.LogWarning($"{name} 结尾{UIStaticHelper.UIViewName} 必须设定为{UIStaticHelper.UIViewName}类型");
                }

                UICodeType = EUICodeType.View;
            }
            else
            {
                if (UICodeType != EUICodeType.Component)
                {
                    Debug.LogWarning($"{name} 想设定为其他类型 请按照规则设定 请勿强行修改");
                }

                UICodeType = EUICodeType.Component;
            }
        }

        private void OnValueChangedEPanelLayer()
        {
            if (PanelLayer >= EPanelLayer.Cache)
            {
                Debug.LogError($" {name} 层级类型 选择错误 请重新选择");
                PanelLayer = EPanelLayer.Panel;
            }
        }

        #endregion

        private bool ShowAutoCheckBtn()
        {
            if (!UIOperationHelper.CheckUIOperation(false)) return false;
            return true;
        }

        [GUIColor(1, 1, 0)]
        [Button("自动检查所有", 30)]
        [PropertyOrder(-100)]
        [ShowIf(nameof(ShowAutoCheckBtn))]
        private void AutoCheckBtn()
        {
            AutoCheck();
        }

        internal bool AutoCheck()
        {
            if (!UIOperationHelper.CheckUIOperation(this)) return false;
            if (!UICreateModule.InitVoName(this)) return false;
            OnValueChangedEUICodeType();
            OnValueChangedEPanelLayer();
            if (UICodeType == EUICodeType.Panel && IsSplitData)
            {
                PanelSplitData.Panel = gameObject;
                if (!PanelSplitData.AutoCheck()) return false;
            }

            UICreateModule.RefreshChildCdeTable(this);
            ComponentTable?.AutoCheck();
            DataTable?.AutoCheck();
            EventTable?.AutoCheck();
            return true;
        }

        private bool ShowCreateBtnByHierarchy()
        {
            if (string.IsNullOrEmpty(PkgName) || string.IsNullOrEmpty(ResName)) return false;
            if (ResName.Contains("Source")) return false;
            if (!UIOperationHelper.CheckUIOperation(this, false)) return false;
            return !PrefabUtility.IsPartOfPrefabAsset(this);
        }

        [GUIColor(0f, 0.5f, 1f)]
        [Button("生成", 50)]
        [ShowIf(nameof(ShowCreateBtnByHierarchy))]
        internal void CreateUICodeByHierarchy()
        {
            if (!ShowCreateBtnByHierarchy()) return;

            var prefabStage = UnityEditor.SceneManagement.PrefabStageUtility.GetCurrentPrefabStage();
            if (prefabStage == null)
            {
                Debug.LogError($"当前不在预制体编辑器模式下");
                return;
            }

            var path = prefabStage.assetPath;
            var root = prefabStage.prefabContentsRoot;
            PrefabUtility.SaveAsPrefabAsset(root, path, out var success);
            if (!success)
            {
                Debug.LogError("快捷保存失败 请检查");
                return;
            }

            prefabStage.ClearDirtiness();

            var cdeTable = AssetDatabase.LoadAssetAtPath<UIBindCDETable>(path);
            if (cdeTable == null) return;
            cdeTable.CreateUICode();

            AssetDatabase.OpenAsset(cdeTable);
        }

        private bool ShowCreateBtn()
        {
            if (IsSplitData) return false;
            if (!UIOperationHelper.CheckUIOperationAll(this, false)) return false;
            return true;
        }

        [GUIColor(0.7f, 0.4f, 0.8f)]
        [Button("生成", 50)]
        [ShowIf(nameof(ShowCreateBtn))]
        internal void CreateUICode()
        {
            if (!UIOperationHelper.CheckUIOperation(this)) return;

            CreateUICode(true, true);
        }

        private bool ShowPanelSourceSplit()
        {
            if (!UIOperationHelper.CheckUIOperationAll(this, false)) return false;
            return IsSplitData;
        }

        [GUIColor(0f, 0.4f, 0.8f)]
        [Button("源数据拆分", 50)]
        [ShowIf(nameof(ShowPanelSourceSplit))]
        internal void PanelSourceSplit()
        {
            if (!UIOperationHelper.CheckUIOperation(this)) return;

            if (IsSplitData)
            {
                if (AutoCheck())
                {
                    UIPanelSourceSplit.Do(this);
                }
            }
            else
            {
                UnityTipsHelper.ShowError($"{name} 当前数据不是源数据 无法进行拆分 请检查数据");
            }
        }

        internal void CreateUICode(bool refresh, bool tips)
        {
            UICreateModule.Create(this, refresh, tips);
        }

        private void OnValidate()
        {
            ComponentTable ??= GetComponent<UIBindComponentTable>();
            DataTable      ??= GetComponent<UIBindDataTable>();
            EventTable     ??= GetComponent<UIBindEventTable>();
        }

        private void AddComponentTable()
        {
            if (!UIOperationHelper.CheckUIOperation()) return;

            ComponentTable = gameObject.GetOrAddComponent<UIBindComponentTable>();
        }

        private void AddDataTable()
        {
            if (!UIOperationHelper.CheckUIOperation()) return;

            DataTable = gameObject.GetOrAddComponent<UIBindDataTable>();
        }

        private void AddEventTable()
        {
            if (!UIOperationHelper.CheckUIOperation()) return;

            EventTable = gameObject.GetOrAddComponent<UIBindEventTable>();
        }
    }
}
#endif