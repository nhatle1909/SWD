using Twilio.Rest.Api.V2010.Account;

namespace EXE.Interface
{
    public interface IOTPService
    {
        public Task<MessageResource> SendOTP(string phoneNumber);
        public Task<string> VerifyOTP(string phoneNumber,string otp);
    }
}
