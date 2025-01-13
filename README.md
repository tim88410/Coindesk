1.印出所有 API 被呼叫以及呼叫外部 API 的 request and response body log 在CurrencyLink\CurrencyLink\CurrencyLink\Common\ApiLoggingAttribute.cs建立了記錄呼叫API的Attribute 並在每個Controller上使用(API 被呼叫) 預期會輸出Log檔在CurrencyLink\CurrencyLink\CurrencyLink\logs 底下 呼叫外部 API的部分請參考 CurrencyLink\CurrencyLink\CurrencyLink.Infrastructure\CurrencyLink.Infrastructure\Service\CoindeskAPIClientService.cs 在執行呼叫API以後，紀錄ResponseUri，StatusCode，Content(回傳的結果)
2.Error handling 處理 API response 各個Controller上都有建立APIError的Attribute 並且抽象化各種API錯誤的類型與其代碼 請參考CurrencyLink\CurrencyLink\CurrencyLink\Common\APIError.cs
3.swagger-ui 有補上API Controller註解以及Return Code代表意義
4.加解密技術應用 (AES/RSA…etc.) appsetting內的連線字串已經過Aes加密 解密的過程在Program.cs內 AES的實作請參考CurrencyLink\CurrencyLink\CurrencyLink\Common\Helper\AESHelper.cs
