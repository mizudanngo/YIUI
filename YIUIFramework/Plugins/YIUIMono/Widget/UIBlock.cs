using UnityEngine;
using UnityEngine.UI;

namespace YIUIFramework
{
    /// <summary>
    /// 不可见的一个图，用来阻挡UI的投射。
    /// TIP: 在每个Panel中，放置在底部，用来防止当前Panel接收到的投射穿透到后面的Panel
    /// </summary>
    public class UIBlock : Graphic, ICanvasRaycastFilter
    {
        public override bool raycastTarget
        {
            get => true;
            set { }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "StyleCop.CSharp.NamingRules",
            "SA1300:ElementMustBeginWithUpperCaseLetter",
            Justification = "Reviewed. Suppression is OK here.")]
        public override Texture mainTexture
        {
            get { return null; }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "StyleCop.CSharp.NamingRules",
            "SA1300:ElementMustBeginWithUpperCaseLetter",
            Justification = "Reviewed. Suppression is OK here.")]
        public override Material materialForRendering
        {
            get { return null; }
        }

        public bool IsRaycastLocationValid(
            Vector2 screenPoint, Camera eventCamera)
        {
            return true;
        }

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();
        }

        public override void SetAllDirty()
        {
        }

        public override void SetLayoutDirty()
        {
        }

        public override void SetVerticesDirty()
        {
        }

        public override void SetMaterialDirty()
        {
        }
    }
}