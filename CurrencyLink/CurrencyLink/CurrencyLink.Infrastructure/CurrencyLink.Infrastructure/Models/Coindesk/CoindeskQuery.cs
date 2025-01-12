using Azure;
using System.ComponentModel.DataAnnotations;

namespace CurrencyLink.Infrastructure.Models.Coindesk
{
    public class CoindeskQuery
    {

        /// <summary>
        /// 
        /// </summary>
        public enum SortColumn
        {
            Id = 1,
            Code = 2,
            CodeName = 3,
            Symbol = 4,
            Description = 5,
            Rate_float = 6,
            Rate = 7,
            UpdateDate = 8
        }

        public enum SortOrderBy
        {
            /// <summary>
            /// 正序排列
            /// </summary>
            Asc = 1,
            /// <summary>
            /// 倒序排列
            /// </summary>
            Desc = 2
        }

        public class CoindeskQueryParameter
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
            public string SortColumn { get; set; } = string.Empty ;
            /// <summary>
            /// ASC OR DESC
            /// </summary>
            public string? SortOrderBy { get; set; }

            public int StartIndex { get { return (Page - 1) * PageLimit + 1; } }
            public int EndIndex { get { return Page * PageLimit; } }

            public string SortBy
            {
                get
                {
                    if (string.IsNullOrEmpty(SortColumn) ||
                    !Enum.TryParse(SortColumn.Trim(), true, out CoindeskQuery.SortColumn intSortBy) ||
                    !Enum.IsDefined(typeof(CoindeskQuery.SortColumn), intSortBy))
                    {
                        SortColumn = "Code";
                    }
                    else
                    {
                        SortColumn = Enum.GetName(typeof(CoindeskQuery.SortColumn), intSortBy);
                    }

                    if (string.IsNullOrEmpty(SortOrderBy) ||
                        !Enum.TryParse(SortOrderBy.Trim(), true, out CoindeskQuery.SortOrderBy intSortorderBy) ||
                        !Enum.IsDefined(typeof(CoindeskQuery.SortOrderBy), intSortorderBy))
                    {
                        SortOrderBy = Enum.GetName(typeof(CoindeskQuery.SortOrderBy), CoindeskQuery.SortOrderBy.Asc);
                    }
                    else
                    {
                        SortOrderBy = Enum.GetName(typeof(CoindeskQuery.SortOrderBy), intSortorderBy);
                    }

                    return SortColumn + " " + SortOrderBy;
                }
            }
        }

        public class CoindeskDTO
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
            /// <summary>
            /// 更新時間
            /// </summary>
            public DateTime UpdateDate { get; set; }
            /// <summary>
            /// 更新時間
            /// </summary>
            public int TotalItem { get; set; }
        }
    }
}
