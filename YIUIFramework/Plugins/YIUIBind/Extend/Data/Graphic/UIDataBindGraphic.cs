using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace YIUIBind
{
	[Serializable]
	[LabelText("改变颜色")]
	[RequireComponent(typeof(Graphic))]
	[AddComponentMenu("YIUIBind/Data/颜色 【Color Toggle】 UIDataBindGraphic")]
	public class UIDataBindGraphic : UIDataBindBool
	{
		[SerializeField]
		[LabelText("条件成立")]
		private Color m_FocusColor = Color.white;
		
		[SerializeField]
		[LabelText("条件不成立")]
		private Color m_NormalColor = Color.white;

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
			if (m_Graphic == null) return;
			
			var result = GetResult();
			m_Graphic.color = result ? m_FocusColor : m_NormalColor;
		}
	}
}