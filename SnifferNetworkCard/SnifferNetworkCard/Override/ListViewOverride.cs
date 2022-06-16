namespace SnifferNetworkCard.Override
{
    public class ListViewFix : System.Windows.Forms.ListView
    {
        /// <summary>
        /// 覆寫ListViewItem 的控制項 : 目的是為了避免更新時畫面閃爍太頻繁，給予良好的使用者體驗
        /// </summary>
        public ListViewFix()
        {
            //使用雙緩衝
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.EnableNotifyMessage, true);
        }

        protected override void OnNotifyMessage(Message m)
        {
            //Filter out the WM_ERASEBKGND message
            if (m.Msg != 0x14)
            {
                base.OnNotifyMessage(m);
            }
        }
    }
}
