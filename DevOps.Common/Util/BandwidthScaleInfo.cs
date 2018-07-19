namespace DevOps.Common.Util
{
    /// <summary>
    /// 带宽范围信息
    /// </summary>
    public struct BandwidthScaleInfo
    {
        /// <summary>
        /// 原始带宽
        /// </summary>
        public float value;
        /// <summary>
        /// 单位
        /// </summary>
        public string unitName;
        /// <summary>
        /// 扩展带宽
        /// </summary>
        public long scale;

        /// <summary>
        /// 带宽范围信息
        /// </summary>
        /// <param name="value">原始带宽</param>
        /// <param name="unitName">单位</param>
        /// <param name="unit">扩展带宽</param>
        public BandwidthScaleInfo(float value, string unitName, long scale)
        {
            this.value = value;
            this.unitName = unitName;
            this.scale = scale;
        }
    }
}