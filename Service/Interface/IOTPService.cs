namespace Service.Interface
{
    public interface IOTPService
    {
        public Task<string> SendSMS(string phoneNumber, string Message);
        public Task<bool> VerifyOTP(string phoneNumber, string otp);
    }
}
