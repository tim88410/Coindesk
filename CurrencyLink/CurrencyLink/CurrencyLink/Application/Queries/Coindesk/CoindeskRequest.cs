using MediatR;
using System.ComponentModel.DataAnnotations;

namespace CurrencyLink.Application.Queries.Coindesk
{
    public class CoindeskRequest : IRequest<CoindeskResponse>
    {
        /// <summary>
        /// 介接進來的Code
        /// </summary>
        public string Code { get; set; } = string.Empty;
        /// <summary>
        /// 使用者更新的中文名字
        /// </summary>
        public string CodeName { get; set; } = string.Empty;
        /// <summary>
        /// 第幾頁
        /// </summary>
        [Required]
        [Range(1, 10000)]
        public int Page { get; set; }
        /// <summary>
        /// 每頁幾筆
        /// </summary>
        [Required]
        [Range(1, int.MaxValue)]
        public int PageLimit { get; set; }
        /// <summary>
        /// 前端排序選項
        /// </summary>
        public string SortColumn { get; set; } = string.Empty;
        /// <summary>
        /// ASC OR DESC
        /// </summary>
        public string SortOrderBy { get; set; } = string.Empty;
    }
}
