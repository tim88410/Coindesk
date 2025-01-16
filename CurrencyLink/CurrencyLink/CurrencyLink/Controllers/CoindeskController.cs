using CurrencyLink.Application.Commands.Coindesk;
using CurrencyLink.Application.Queries.Coindesk;
using CurrencyLink.Common;
using DBUtility;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace CurrencyLink.Controllers
{
    [ApiController]
    public class CoindeskController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CoindeskController(IMediator mediator)
        {
            _mediator = mediator;
        }


        /// <summary>
        /// 依據Filter對Coindesk進行查詢，取得List資料
        /// </summary>
        /// <remarks>
        /// <code>
        /// <br/>
        /// 透過 ReturnCode 判斷狀態:<br/>
        /// ParamError(2) 查詢參數錯誤<br/>
        /// DBConnectError(3) 查詢入DB時連線失敗<br/>
        /// OperationSuccessful(5) 查詢成功<br/>
        /// DataNotFound(6) API呼叫成功，但沒有任何資料回傳<br/>
        /// </code>
        /// </remarks>
        [ApiLogging]
        [ApiResult]
        [APIError]
        [HttpGet]
        [Route("v1/Coindesk")]
        public async Task<IActionResult> Get([FromQuery] CoindeskRequest request)
        {
            if (!ModelState.IsValid)
            {
                throw new APIError.ParamError();
            }
            var result = await _mediator.Send(request);

            if (result == null)
            {
                throw new APIError.DBConnectError();
            }
            else if (result.Total == 0)
            {
                throw new APIError.DataNotFound();
            }
            return Ok(result);
        }

        /// <summary>
        /// 傳入Coindesk 主Key [Id]進行查詢，取得單一資料，通常用於單一筆資料編輯時，Load的情況
        /// </summary>
        /// <remarks>
        /// <code>
        /// <br/>
        /// 透過 ReturnCode 判斷狀態:<br/>
        /// ParamError(2) 查詢參數錯誤<br/>
        /// DBConnectError(3) 查詢入DB時連線失敗<br/>
        /// OperationSuccessful(5) 查詢成功<br/>
        /// DataNotFound(6) API呼叫成功，但沒有任何資料回傳<br/>
        /// </code>
        /// </remarks>
        [ApiLogging]
        [ApiResult]
        [APIError]
        [HttpGet]
        [Route("v1/Coindesk/{Id}")]
        public async Task<IActionResult> GetOne(int Id)
        {
            if (!ModelState.IsValid)
            {
                throw new APIError.ParamError();
            }
            var result = await _mediator.Send(new CoindeskGetOneRequest { Id = Id });

            if (result == null)
            {
                throw new APIError.DBConnectError();
            }
            else if (!result.Any())
            {
                throw new APIError.DataNotFound();
            }
            return Ok(result.FirstOrDefault());
        }

        /// <summary>
        /// 新增單筆資料進Coindesk，須注意新增時Id必須為0，若Id大於0則判定為修改，會回傳參數錯誤
        /// </summary>
        /// <remarks>
        /// <code>
        /// Id:流水號，供編輯時選定單筆Id
        /// Code:介接時對方API給的Code
        /// CodeName:使用者可更新的中文名字
        /// Symbol:介接回來的幣別代表符號
        /// Description:介接回來的幣別描述
        /// RateFloat:介接回來的幣別匯率(float)
        /// Rate:介接回來的幣別匯率(文字)
        /// CurrencyCode:介接回來bpi的Key
        /// <br/>
        /// 透過 ReturnCode 判斷狀態:<br/>
        /// ParamError(2) 參數錯誤<br/>
        /// DBConnectError(3) DB連線失敗<br/>
        /// OperationSuccessful(5) 查詢成功<br/>
        /// </code>
        /// </remarks>
        [ApiLogging]
        [ApiResult]
        [APIError]
        [HttpPost]
        [Route("v1/Coindesk")]
        public async Task<IActionResult> Post([FromBody] UpdateCoindeskCommand request)
        {
            if (!ModelState.IsValid || request.Id > 0)
            {
                throw new APIError.ParamError();
            }
            var result = await _mediator.Send(request);

            if (result == ErrorCode.KErrDBError)
            {
                throw new APIError.DBConnectError();
            }
            return Ok();
        }

        /// <summary>
        /// 針對某筆資料進行修改，須注意修改時Id必不得為0，若Id為0則判定為新增，會回傳參數錯誤
        /// </summary>
        /// <remarks>
        /// <code>
        /// Id:流水號，供編輯時選定單筆Id
        /// Code:介接時對方API給的Code
        /// CodeName:使用者可更新的中文名字
        /// Symbol:介接回來的幣別代表符號
        /// Description:介接回來的幣別描述
        /// RateFloat:介接回來的幣別匯率(float)
        /// Rate:介接回來的幣別匯率(文字)
        /// CurrencyCode:介接回來bpi的Key
        /// <br/>
        /// 透過 ReturnCode 判斷狀態:<br/>
        /// ParamError(2) 參數錯誤<br/>
        /// DBConnectError(3) DB連線失敗<br/>
        /// OperationSuccessful(5) 修改成功<br/>
        /// </code>
        /// </remarks>
        [ApiLogging]
        [ApiResult]
        [APIError]
        [HttpPut]
        [Route("v1/Coindesk")]
        public async Task<IActionResult> Put([FromBody] UpdateCoindeskCommand request)
        {
            if (!ModelState.IsValid || request.Id == 0)
            {
                throw new APIError.ParamError();
            }
            var result = await _mediator.Send(request);

            if (result == ErrorCode.KErrDBError)
            {
                throw new APIError.DBConnectError();
            }
            return Ok();
        }

        /// <summary>
        /// 刪除Coindesk內某筆資料，須注意刪除時Id必不得為0，若Id為0則判定為參數錯誤
        /// </summary>
        /// <remarks>
        /// <code>
        /// <br/>
        /// 透過 ReturnCode 判斷狀態:<br/>
        /// ParamError(2) 參數錯誤<br/>
        /// DBConnectError(3) DB連線失敗<br/>
        /// OperationSuccessful(5) 刪除成功<br/>
        /// </code>
        /// </remarks>
        [ApiLogging]
        [ApiResult]
        [APIError]
        [HttpDelete]
        [Route("v1/Coindesk")]
        public async Task<IActionResult> Delete(int Id)
        {
            if (!ModelState.IsValid || Id == 0)
            {
                throw new APIError.ParamError();
            }
            var result = await _mediator.Send(new DeleteCoindeskCommand { Id = Id });

            if (result == ErrorCode.KErrDBError)
            {
                throw new APIError.DBConnectError();
            }
            return Ok();
        }

        /// <summary>
        /// Export Coindesk內所有資料為Csv檔
        /// </summary>
        /// <remarks>
        /// <code>
        /// StatusCode 500 匯出錯誤
        /// </code>
        /// </remarks>
        [HttpGet]
        [Route("v1/CoindeskExport")]
        public async Task<object?> Export([FromQuery] CoindeskExportRequest request)
        {
            var result = await _mediator.Send<CoindeskExportResponse?>(request);
            if (result == null)
            {
                return StatusCode(500, "An error occurred while DBError.");
            }

            var csvContent = new StringBuilder();
            csvContent.AppendLine("Id,Code,CodeName,Symbol,Description,RateFloat,Rate,CurrencyCode,UpdateDate");
            if (result.CoindeskInfo.Any())
            { 
                foreach (var item in result.CoindeskInfo)
                {
                    List<string> itemInfo = new List<string>() { 
                        AddQuotes(item.Id.ToString()),
                        AddQuotes(item.Code),
                        AddQuotes(item.CodeName),
                        AddQuotes(item.Symbol),
                        AddQuotes(item.Description),
                        AddQuotes(item.RateFloat.ToString()),
                        AddQuotes(item.Rate),
                        AddQuotes(item.CurrencyCode),
                        AddQuotes(item.UpdateDate.ToString())
                    };
                    csvContent.AppendLine(string.Join(",", itemInfo));
                }
            }

            var byteArray = Encoding.UTF8.GetBytes(csvContent.ToString());
            return File(byteArray, "text/csv", "data.csv");
        }

        private string AddQuotes(string str)
        {
            return $"\"{str}\""; ;
        }
    }
}
