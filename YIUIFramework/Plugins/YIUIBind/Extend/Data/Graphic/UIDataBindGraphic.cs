using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using UnityEngine.UI;

namespace YIUIBind
{
	[Serializable]
	[LabelText("改变颜色")]
	[RequireComponent(typeof(Graphic))]
	[AddComponentMenu("YIUIBind/Data/颜色 【Color】 UIDataBindGraphic")]
	public class UIDataBindGraphic : UIDataBindSelectBase
	{
		[SerializeField]
		[LabelText("所有结果逻辑")]
#if UNITY_EDITOR
		[EnableIf(nameof(Enable))]
#endif
		private UIBooleanLogic m_BooleanLogic = UIBooleanLogic.And;

		[OdinSerialize]
		[LabelText("所有计算结果的变量")]
		[ListDrawerSettings(IsReadOnly = true)]
#if UNITY_EDITOR
		[EnableIf(nameof(Enable))]
#endif
		private List<UIDataBoolRef> m_Datas = new List<UIDataBoolRef>();
		
		[SerializeField]
        [LabelText("Normal Color")]
        private Color m_NormalColor = Color.white;
        
		[SerializeField]
		[LabelText("Focus Color")]
		private Color m_FocusColor = Color.white;
		
		
		protected override int Mask()
		{
			return 1 << (int)EUIBindDataType.Bool |
			       1 << (int)EUIBindDataType.Int |
			       1 << (int)EUIBindDataType.Float |
			       1 << (int)EUIBindDataType.String;
		}

		protected override int SelectMax()
		{
			return 1;
		}

		[SerializeField]
		[ReadOnly]
		[Required("必须有此组件")]
		[LabelText("图")]
		private Graphic m_Graphic;

		protected override void OnRefreshData()
		{
			base.OnRefreshData();
			m_Graphic ??= GetComponent<Graphic>();
		}

		protected override void OnValueChanged()
		{
			var dataValue = GetFirstValue<Color>();

			
			if (m_Graphic != null)
			{
				m_Graphic.color = dataValue;
			}
		}
	}
}