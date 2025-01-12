using CurrencyLink.Application.Commands.CoindeskAPIClient;
using CurrencyLink.Common;
using DBUtility;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace CurrencyLink.Controllers
{

    [ApiLogging]
    [APIError]
    [ApiResult]
    [ApiController]
    [Route("v1/CoindeskAPIClient")]
    public class CoindeskAPIClientController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CoindeskAPIClientController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// 第三方介接取得匯率服務
        /// </summary>
        /// <remarks>
        /// 呼叫第三方介接程式，呼叫成功後反序列化回傳的資料，若無異常則對DB進行寫入
        /// <code>
        /// <br/>
        /// 透過 ReturnCode 判斷狀態:<br/>
        /// IntegrationHTTP(9) API呼叫失敗<br/>
        /// IntegrationException(8) API呼叫異常，進入Catch<br/>
        /// JsonParseError(10) API呼叫成功，但JsonParse失敗<br/>
        /// OperationFailed(4) API呼叫成功，但JsonParse異常，進入Catch<br/>
        /// DBConnectError(3) API呼叫和JsonParse成功，寫入DB時連線失敗<br/>
        /// </code>
        /// </remarks>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            CoindeskAPIClientCommand coindeskCommand = new CoindeskAPIClientCommand();
            var result = await _mediator.Send(coindeskCommand);

            if (result == ErrorCode.KEventHTTP)
            {
                throw new APIError.IntegrationHTTP();
            }
            else if (result == ErrorCode.KErrIntegrationException)
            {
                throw new APIError.IntegrationException();
            }
            else if (result == ErrorCode.KErrJsonParseError)
            {
                throw new APIError.JsonParseError();
            }
            else if (result == ErrorCode.KErrErrorStatus)
            {
                throw new APIError.OperationFailed();
            }
            else if (result == ErrorCode.KErrDBError)
            {
                throw new APIError.DBConnectError();
            }
            return Ok();
        }
    }
}
