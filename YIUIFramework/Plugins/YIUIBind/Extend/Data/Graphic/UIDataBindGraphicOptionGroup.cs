using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace YIUIBind
{
	[Serializable]
	[LabelText("改变颜色")]
	[AddComponentMenu("YIUIBind/Data/颜色 【Color OptionGroup Toggle】 UIDataBindGraphicOptionGroup")]
	public class UIDataBindGraphicOptionGroup : UIDataBindSelectBase
	{
		[Serializable]
		private struct InnerOption
		{
			public Graphic[] Graphics;
		}
		
		[SerializeField]
		[LabelText("条件成立")]
		private Color m_FocusColor = Color.white;
		
		[SerializeField]
		[LabelText("条件不成立")]
		private Color m_NormalColor = Color.white;
		
		[SerializeField]
		[LabelText("控制的目标")]
		[Required("必须有此组件")]
		private InnerOption[] m_OptionTargets;

		protected override int Mask()
		{
			return 1 << (int)EUIBindDataType.Int;	
		}
		
		protected override int SelectMax()
		{
			return 1;
		}

		protected override void OnValueChanged()
		{
			if (m_OptionTargets == null) return;
			
			var dataValue = GetFirstValue<int>();
			for (int i = 0; i < m_OptionTargets.Length; i++)
			{
				var color = i == dataValue? m_FocusColor : m_NormalColor;
				foreach (var graphic in m_OptionTargets[i].Graphics)
				{
					graphic.color = color;
				}
			}
		}
	}
}