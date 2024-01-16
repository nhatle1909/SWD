using EXE.Interface;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Rest.Verify.V2.Service;
namespace EXE.Service
{
    public class OTPService : IOTPService
    {
        private readonly IConfiguration _configuration;

        public OTPService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        public async Task<MessageResource> SendOTP(string phoneNumber)
        {
            var SID = _configuration["Twilio:SID"];
            var Token = _configuration["Twilio:Token"];
            var serviceSid = _configuration["Twilio:ServiceSID"];
            TwilioClient.Init(SID, Token);
            var message = MessageResource.Create(
            body: "Join Earth's mightiest heroes. Like Kevin Bacon.",
            from: new Twilio.Types.PhoneNumber("cc"),
            to: new Twilio.Types.PhoneNumber(phoneNumber)
        );
        

            return message;


        }

        public Task<string> VerifyOTP(string phoneNumber, string otp)
        {
            throw new NotImplementedException();
        }
    }
}
