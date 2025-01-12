using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyLink.Infrastructure.Models.Coindesk
{
    public class CoindeskCommand
    {
        public class CoindeskParameter
        {
            /// <summary>
            /// 流水號
            /// </summary>
            public int Id { get; set; }
            /// <summary>
            /// 介接進來的幣別
            /// </summary>
            public string Code { get; set; } = string.Empty;
            /// <summary>
            /// 使用者更新的幣別中文名稱
            /// </summary>
            public string CodeName { get; set; } = string.Empty;
            /// <summary>
            /// 幣別符號
            /// </summary>
            public string Symbol { get; set; } = string.Empty;
            /// <summary>
            /// 介接進來的幣別描述
            /// </summary>
            public string Description { get; set; } = string.Empty;
            /// <summary>
            /// 介接進來的幣匯率，可至小數點第四位
            /// </summary>
            public decimal Rate_float { get; set; }
            /// <summary>
            /// 介接進來的幣匯率文字
            /// </summary>
            public string Rate { get; set; } = string.Empty;
            /// <summary>
            /// 介接進來的幣別代碼
            /// </summary>
            public string CurrencyCode { get; set; } = string.Empty;
        }

        public class CoindeskDeleteParameter
        {
            /// <summary>
            /// 流水號
            /// </summary>
            public int Id { get; set; }
        }
    }
}
