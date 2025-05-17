namespace PropertyControl.Commons.Schemas
{
    public class JwtData
    {
        public int UserId { set; get; }
        public int RoleId { get; set; }
        public bool IsAdmin => RoleId == 1;
        public string Username { set; get; }
        public string Aud { set; get; }
        public string Iss { set; get; }
        public string Secret { set; get; }
    }
}