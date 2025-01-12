namespace CurrencyLink.Application.Queries.Coindesk
{
    public class CoindeskResponse
    {
        public List<CoindeskInfo>? CoindeskInfos { get; set; }
        /// <summary>
        /// 回傳資料總數
        /// </summary>
        public int Total { get; set; }
        public class CoindeskInfo 
        { 
            /// <summary>
            /// 流水號，供編輯時選定單筆Id
            /// </summary>
            public int Id { get; set; }
            /// <summary>
            /// 介接時對方API給的Code
            /// </summary>
            public string Code { get; set; } = string.Empty;
            /// <summary>
            /// 使用者可更新的中文名字
            /// </summary>
            public string CodeName { get; set; } = string.Empty;
            /// <summary>
            /// 介接回來的幣別代表符號
            /// </summary>
            public string Symbol { get; set; } = string.Empty;
            /// <summary>
            /// 介接回來的幣別描述
            /// </summary>
            public string Description { get; set; } = string.Empty;
            /// <summary>
            /// 介接回來的幣別匯率(float)
            /// </summary>
            public decimal RateFloat { get; set; } = decimal.Zero;
            /// <summary>
            /// 介接回來的幣別匯率(文字)
            /// </summary>
            public string Rate { get; set; } = string.Empty;
            //public DateTime? DeleteDate { get; set; }
            public string CurrencyCode { get; set; } = string.Empty;
            /// <summary>
            /// 介接更新時間
            /// </summary>
            public DateTime UpdateDate { get; set; }
        }
    }
}
