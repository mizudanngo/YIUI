using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace YIUIBind
{
	[Serializable]
	[LabelText("改变颜色")]
	[RequireComponent(typeof(Graphic))]
	[AddComponentMenu("YIUIBind/Data/颜色 【Color Toggle Group】 UIDataBindGraphicGroup")]
	public class UIDataBindGraphicGroup : UIDataBindBool
	{
		[SerializeField]
		[LabelText("条件成立")]
		private Color m_FocusColor = Color.white;
		
		[SerializeField]
		[LabelText("条件不成立")]
		private Color m_NormalColor = Color.white;
		
		[SerializeField]
		[LabelText("控制的目标")]
		[Required("必须有此组件")]
		private Graphic[] m_Targets;

		protected override void OnValueChanged()
		{
			if (m_Targets == null) return;
			
			bool result = GetResult();
			Color color = result ? m_FocusColor : m_NormalColor;
			foreach (var value in m_Targets)
			{
				value.color = color;
			}
		}
	}
}