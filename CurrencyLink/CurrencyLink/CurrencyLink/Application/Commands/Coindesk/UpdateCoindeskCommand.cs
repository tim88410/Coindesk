using MediatR;

namespace CurrencyLink.Application.Commands.Coindesk
{
    public class UpdateCoindeskCommand : IRequest<int>
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
        public string CurrencyCode { get; set; } = string.Empty;
    }
}
