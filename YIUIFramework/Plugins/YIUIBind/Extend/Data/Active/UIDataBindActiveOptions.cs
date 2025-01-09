using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace YIUIBind
{
	[Serializable]
	[LabelText("GameObject列表的显隐切换")]
	[AddComponentMenu("YIUIBind/Data/显隐 【Active Options】 UIDataBindActiveOptions")]
	public sealed class UIDataBindActiveOptions : UIDataBindSelectBase
	{
		[SerializeField]
		[LabelText("控制的目标")]
		[Required("必须有此组件")]
		private GameObject[] m_OptionTargets;

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
				m_OptionTargets[i].SetActive(i == dataValue);
			}
		}
	}
}