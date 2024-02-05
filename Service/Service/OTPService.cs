using Microsoft.Extensions.Configuration;
using Service.Interface;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
namespace Service.Service
{
    public class OTPService : IOTPService
    {
        private readonly IConfiguration _configuration;

        public OTPService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<string> SendSMS(string phoneNumber, string Message)
        {
            var SID = _configuration["Twilio:SID"];
            var Token = _configuration["Twilio:Token"];
            var serviceSid = _configuration["Twilio:ServiceSID"];
            var senderNumber = _configuration["Twilio:PhoneNumber"];
            TwilioClient.Init(SID, Token);
            var message = await MessageResource.CreateAsync(
            body: Message,
            from: new Twilio.Types.PhoneNumber(senderNumber),
            to: new Twilio.Types.PhoneNumber(phoneNumber)
        );
            return Message;
        }

        public Task<bool> VerifyOTP(string phoneNumber, string otp)
        {
            throw new Exception();
        }
    }
}
